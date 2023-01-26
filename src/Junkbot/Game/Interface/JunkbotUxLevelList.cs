/**
 * JunkbotUxLevelList.cs - Junkbot Level List UI Component
 *
 * This source-code is part of a clean-room recreation of Lego Junkbot by Oddmatics:
 * <<https://www.oddmatics.uk>>
 *
 * Author(s): Rory Fewell <roryf@oddmatics.uk>
 */

using Junkbot.Game.Profile;
using Junkbot.Game.World.Level;
using Oddmatics.Rzxe.Extensions;
using Oddmatics.Rzxe.Game.Interface;
using Oddmatics.Rzxe.Input;
using Oddmatics.Rzxe.Util.Shapes;
using Oddmatics.Rzxe.Windowing.Graphics;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace Junkbot.Game.Interface
{
    

    /// <summary>
    /// Represents a Junkbot level list user interface component.
    /// </summary>
    public sealed class JunkbotUxLevelList : UxComponent
    {
        private sealed class LevelListDrawData
        {
            public ISpriteDrawInstruction CheckBox { get; private set; }
            
            public IStringDrawInstruction LevelNumber { get; private set; }

            public IStringDrawInstruction MoveCount { get; private set; }
            
            public IStringDrawInstruction Name { get; private set; }


            public LevelListDrawData(
                ISpriteBatch sb,
                IFont        font
            )
            {
                LevelNumber =
                    sb.DrawString(
                        string.Empty,
                        font,
                        Point.Empty,
                        Color.Black
                    );
                CheckBox =
                    sb.Draw(
                        sb.Atlas.Sprites["checkbox_off"],
                        Point.Empty,
                        Color.Transparent
                    );
                Name =
                    sb.DrawString(
                        string.Empty,
                        font,
                        Point.Empty,
                        Color.Black
                    );
                MoveCount =
                    sb.DrawString(
                        string.Empty,
                        font,
                        Point.Empty,
                        Color.Black
                    );
            }
        }


        /// <summary>
        /// The position offset of the checkbox for displaying level completion state.
        /// </summary>
        private static readonly Size CheckOffset = new Size(36, 7);
        
        /// <summary>
        /// The height of each individual item in the list.
        /// </summary>
        private const int ItemHeight = 21;
        
        /// <summary>
        /// The width of each individual item in the list.
        /// </summary>
        private const int ItemWidth = 448;
        
        /// <summary>
        /// The position offset of the numerals for each level in the list.
        /// </summary>
        private static readonly Size NumberOffset = new Size(3, 6);
        
        /// <summary>
        /// The position offset of the move counters for each level in the list.
        /// </summary>
        private static readonly Size MovesOffset = new Size(3, 6);
        
        /// <summary>
        /// The position offset of the title of each level in the list.
        /// </summary>
        private static readonly Size TitleOffset = new Size(64, 6);
        
        
        /// <summary>
        /// Gets or sets the building that has been selected.
        /// </summary>
        public byte SelectedBuilding
        {
            get { return _SelectedBuilding; }
            set
            {
                if (_SelectedBuilding == value)
                {
                    return;
                }

                _SelectedBuilding = value;
                InvalidateLevelListData();
            }
        }
        private byte _SelectedBuilding;
        
        /// <inheritdoc />
        public override Size Size
        {
            get
            {
                return new Size(
                    ItemWidth,
                    ItemHeight * Game.Levels.LevelsPerBuilding
                );
            }
            
            set
            {
                throw new InvalidOperationException(
                    "Cannot set the size of level lists."
                );
            }
        }
        
        
        /// <summary>
        /// The value that indicates whether the state of the level list is dirty and the
        /// draw instructions must be updated on the next render call.
        /// </summary>
        private bool DirtyLevelList { get; set; }

        /// <summary>
        /// The font resource used for the list item text.
        /// </summary>
        private IFont Font { get; set; }
        
        /// <summary>
        /// The running Junkbot game instance.
        /// </summary>
        private JunkbotGame Game { get; set; }
        
        /// <summary>
        /// The selection highlight instruction.
        /// </summary>
        private IShapeDrawInstruction HighlightInstruction { get; set; }
        
        /// <summary>
        /// The instructions for drawing the level list.
        /// </summary>
        private List<LevelListDrawData> ListInstructions { get; set; }

        /// <summary>
        /// The target sprite batch for drawing.
        /// </summary>
        private ISpriteBatch TargetSpriteBatch { get; set; }
        
        
        /// <summary>
        /// Occurs when a level has been selected.
        /// </summary>
        public event LevelSelectedEventHandler LevelSelected;


        /// <summary>
        /// Initializes a new instance of the <see cref="JunkbotUxLevelList"/> class.
        /// </summary>
        /// <param name="game">
        /// The running Junkbot game instance.
        /// </param>
        public JunkbotUxLevelList(
            JunkbotGame game
        )
        {
            Dirty            = true;
            DirtyLevelList   = true;
            Game             = game;
            ListInstructions = new List<LevelListDrawData>();
            SelectedBuilding = 1;
        }
        
        
        /// <inheritdoc />
        public override void Dispose()
        {
            base.Dispose();
            
            if (TargetSpriteBatch != null)
            {
                TargetSpriteBatch.Dispose();
            }
        }
        
        /// <inheritdoc />
        public override void OnClick(
            ControlInput mouseButton,
            Point        mouseLocation
        )
        {
            int levelIndex = HitTestLevelList(PointToClient(mouseLocation));
            
            if (
                mouseButton == ControlInput.MouseButtonLeft &&
                levelIndex  >= 0
            )
            {
                LevelSelected?.Invoke(
                    this,
                    new LevelSelectedEventArgs(SelectedBuilding, (byte) levelIndex)
                );
            }
        }

        /// <inheritdoc />
        public override void OnMouseLeave()
        {
            UpdateHighlight(-1);
        }

        /// <inheritdoc />
        public override void OnMouseMove(
            Point mouseLocation
        )
        {
            UpdateHighlight(
                HitTestLevelList(PointToClient(mouseLocation))
            );
        }
        
        /// <inheritdoc />
        public override void Render(
            ISpriteBatch sb
        )
        {
            AssertNotDisposed();
            
            if (TargetSpriteBatch == null)
            {
                TargetSpriteBatch = sb;
                
                if (Font == null)
                {
                    Font = sb.Atlas.GetSpriteFont("default", 2);
                }
                
                // Initialize instructions
                //
                HighlightInstruction =
                    sb.Draw(
                        new Polygon(
                            new Point(0, 0),
                            new Point(ItemWidth, 0),
                            new Point(ItemWidth, ItemHeight),
                            new Point(0, ItemHeight)
                        ),
                        Point.Empty,
                        Color.Transparent
                    );
                
                for (int i = 0; i < Game.Levels.LevelsPerBuilding; i++)
                {
                    ListInstructions.Add(
                        new LevelListDrawData(TargetSpriteBatch, Font)
                    );
                }
            }
            
            if (Dirty)
            {
                for (int i = 0; i < Game.Levels.LevelsPerBuilding; i++)
                {
                    var rowOffset = new Point(0, ItemHeight * i);
                    
                    ListInstructions[i].LevelNumber.Location =
                        ActualLocation.Add(NumberOffset).Add(rowOffset);
                    ListInstructions[i].CheckBox.Location =
                        ActualLocation.Add(CheckOffset).Add(rowOffset);
                    ListInstructions[i].Name.Location =
                        ActualLocation.Add(TitleOffset).Add(rowOffset);

                    // Right-align the moves counter
                    //
                    Size textSize =
                        Font.MeasureString(
                            ListInstructions[i].MoveCount.Text
                        ).Size;

                    int finalX =
                        ActualLocation.X + ItemWidth - MovesOffset.Width - textSize.Width;
                    int finalY =
                        ActualLocation.Y + rowOffset.Y + MovesOffset.Height;

                    ListInstructions[i].MoveCount.Location =
                        new Point(finalX, finalY);
                }

                Dirty = false;
            }
            
            if (DirtyLevelList)
            {
                // Store references we'll use later
                //
                ISprite checkboxOff = TargetSpriteBatch.Atlas.Sprites["checkbox_off"];
                ISprite checkboxOn  = TargetSpriteBatch.Atlas.Sprites["check_light"];

                IList<string> levelNames =
                    Game.Levels.GetLevelList(SelectedBuilding);

                for (int i = 0; i < Game.Levels.LevelsPerBuilding; i++)
                {
                    var progress =
                        Game.UserProfile.GetLevelCompletionInfo(
                            SelectedBuilding,
                            i
                        );

                    ListInstructions[i].CheckBox.Sprite =
                        progress.Done ? checkboxOn : checkboxOff;

                    ListInstructions[i].Name.Text = levelNames[i].ToLower();

                    // Right-align the moves counter
                    //
                    string moveText = progress.Done ?
                                          progress.Moves.ToString() :
                                          string.Empty;
                    Size   textSize = Font.MeasureString(moveText).Size;

                    int finalX =
                        ActualLocation.X + ItemWidth - MovesOffset.Width - textSize.Width;
                    int finalY =
                        ActualLocation.Y + (ItemHeight * i) + MovesOffset.Height;

                    ListInstructions[i].MoveCount.Location =
                        new Point(finalX, finalY);
                    ListInstructions[i].MoveCount.Text = moveText;
                }

                DirtyLevelList = false;
            }
        }
        
        
        /// <summary>
        /// Performs a hit test and determines the currently highlighted level.
        /// </summary>
        /// <param name="p">
        /// The <see cref="Point"/> to hit test.
        /// </param>
        /// <returns>
        /// The zero-based index of the highlighted level in the list, -1 if no level
        /// is highlighted.
        /// </returns>
        private int HitTestLevelList(
            Point p
        )
        {
            int itemIndex = (int) Math.Floor((float) p.Y / ItemHeight);
                
            if (itemIndex >= 0 && itemIndex < Game.Levels.LevelsPerBuilding)
            {
                return itemIndex;
            }

            return -1;
        }

        /// <summary>
        /// Invalidates the currently drawn level list data.
        /// </summary>
        private void InvalidateLevelListData()
        {
            DirtyLevelList = true;
        }
        
        /// <summary>
        /// Updates the selection highlight.
        /// </summary>
        /// <param name="highlightedIndex">
        /// The index of the level that is currently highlighted.
        /// </param>
        private void UpdateHighlight(
            int highlightedIndex
        )
        {
            if (HighlightInstruction == null)
            {
                return;
            }
            
            if (highlightedIndex > -1)
            {
                HighlightInstruction.Color    =
                        Color.FromArgb(191, 192, 191);
                HighlightInstruction.Location =
                        new Point(
                            Location.X,
                            Location.Y + highlightedIndex * ItemHeight
                        );

                return;
            }
            else
            {
                HighlightInstruction.Color = Color.Transparent;
            }
        }
    }
}
