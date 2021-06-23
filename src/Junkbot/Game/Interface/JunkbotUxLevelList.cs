/**
 * JunkbotUxLevelList.cs - Junkbot Level List UI Component
 *
 * This source-code is part of a clean-room recreation of Lego Junkbot by Oddmatics:
 * <<https://www.oddmatics.uk>>
 *
 * Author(s): Rory Fewell <roryf@oddmatics.uk>
 */

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
    public class JunkbotUxLevelList : UxComponent
    {
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
        
        
        /// <inheritdoc />
        public override Point Location
        {
            get { return _Location; }
            set
            {
                _Location = value;
                InvalidateLocation();
            }
        }
        private Point _Location;
        
        /// <summary>
        /// Gets or sets the building that has been selected.
        /// </summary>
        public int SelectedBuilding
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
        private int _SelectedBuilding;
        
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
        /// The font resource used for the list item text.
        /// </summary>
        private IFont Font { get; set; }
        
        /// <summary>
        /// The running Junkbot game instance.
        /// </summary>
        private JunkbotGame Game { get; set; }


        #region Drawing Related
        
        /// <summary>
        /// The selection highlight instruction.
        /// </summary>
        private IShapeDrawInstruction HighlightInstruction { get; set; }

        /// <summary>
        /// The list of drawing instructions for rendering the level completion
        /// checkboxes.
        /// </summary>
        private List<ISpriteDrawInstruction> LevelCheckInstructions { get; set; }
        
        /// <summary>
        /// The list of drawing instructions for rendering the level move counts.
        /// </summary>
        private List<IStringDrawInstruction> LevelMovesInstructions { get; set; }

        /// <summary>
        /// The list of drawing instructions for rendering the level names.
        /// </summary>
        private List<IStringDrawInstruction> LevelNameInstructions { get; set; }
        
        /// <summary>
        /// The list of drawing instructions for rendering the level numbers.
        /// </summary>
        private List<IStringDrawInstruction> LevelNumberInstructions { get; set; }

        /// <summary>
        /// The target sprite batch for drawing.
        /// </summary>
        private ISpriteBatch TargetSpriteBatch { get; set; }

        #endregion
        
        
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
            Game                    = game;
            LevelCheckInstructions  = new List<ISpriteDrawInstruction>();
            LevelMovesInstructions  = new List<IStringDrawInstruction>();
            LevelNameInstructions   = new List<IStringDrawInstruction>();
            LevelNumberInstructions = new List<IStringDrawInstruction>();
            SelectedBuilding        = 1;
        }
        
        
        /// <inheritdoc />
        public override void Dispose()
        {
            AssertNotDisposed();
            
            Disposing = true;
            
            if (TargetSpriteBatch != null)
            {
                var toRemove = new List<IDrawInstruction>();

                toRemove.Add(HighlightInstruction);
                toRemove.AddRange(LevelNumberInstructions);
                toRemove.AddRange(LevelCheckInstructions);
                toRemove.AddRange(LevelNameInstructions);
                toRemove.AddRange(LevelMovesInstructions);
                
                foreach (IDrawInstruction d in toRemove)
                {
                    TargetSpriteBatch.Instructions.Remove(d);
                }
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
                levelIndex  != -1
            )
            {
                LevelSelected?.Invoke(
                    this,
                    new LevelSelectedEventArgs(SelectedBuilding, levelIndex)
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
                    LevelNumberInstructions.Add(
                        TargetSpriteBatch.DrawString(
                            (i + 1).ToString(),
                            Font,
                            Point.Empty,
                            Color.Black
                        )
                    );
                    LevelCheckInstructions.Add(
                        TargetSpriteBatch.Draw(
                            TargetSpriteBatch.Atlas.Sprites["checkbox_off"],
                            Point.Empty,
                            Color.Transparent
                        )
                    );
                    LevelNameInstructions.Add(
                        TargetSpriteBatch.DrawString(
                            string.Empty,
                            Font,
                            Point.Empty,
                            Color.Black
                        )
                    );
                    LevelMovesInstructions.Add(
                        TargetSpriteBatch.DrawString(
                            string.Empty,
                            Font,
                            Point.Empty,
                            Color.Black
                        )
                    );
                }

                // Immediately invalidate
                //
                InvalidateLevelListData();
                InvalidateLocation();
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
            if (TargetSpriteBatch == null)
            {
                return;
            }
            
            // Store references we'll use later
            //
            ISprite checkboxOff = TargetSpriteBatch.Atlas.Sprites["checkbox_off"];
            ISprite checkboxOn  = TargetSpriteBatch.Atlas.Sprites["check_light"];

            IList<string> levelNames =
                Game.Levels.GetLevelList(
                    (byte) SelectedBuilding
                );
                
            for (int i = 0; i < Game.Levels.LevelsPerBuilding; i++)
            {
                var progress =
                    Game.UserProfile.GetLevelCompletionInfo(
                        SelectedBuilding,
                        i
                    );

                LevelCheckInstructions[i].Sprite =
                    progress.Done ? checkboxOn : checkboxOff;

                LevelNameInstructions[i].Text = levelNames[i].ToLower();

                // Right-align the moves counter
                //
                string moveText = progress.Done ?
                                      progress.Moves.ToString() :
                                      string.Empty;
                Size   textSize = Font.MeasureString(moveText).Size;
                
                LevelMovesInstructions[i].Location =
                    new Point(
                        Location.X + ItemWidth - MovesOffset.Width - textSize.Width,
                        Location.Y + (ItemHeight * i) + MovesOffset.Height
                    );
                LevelMovesInstructions[i].Text     = moveText;
            }
        }
        
        /// <summary>
        /// Invalidates the currently drawn control location.
        /// </summary>
        private void InvalidateLocation()
        {
            if (TargetSpriteBatch == null)
            {
                return;
            }
            
            for (int i = 0; i < Game.Levels.LevelsPerBuilding; i++)
            {
                var rowOffset = new Point(0, ItemHeight * i);

                LevelNumberInstructions[i].Location =
                    Location.Add(NumberOffset).Add(rowOffset);
                LevelCheckInstructions[i].Location =
                    Location.Add(CheckOffset).Add(rowOffset);
                LevelNameInstructions[i].Location =
                    Location.Add(TitleOffset).Add(rowOffset);
                    
                // Right-align the moves counter
                //
                Size textSize =
                    Font.MeasureString(
                        LevelMovesInstructions[i].Text
                    ).Size;
                
                LevelMovesInstructions[i].Location =
                    new Point(
                        Location.X + ItemWidth   - MovesOffset.Width - textSize.Width,
                        Location.Y + rowOffset.Y + MovesOffset.Height
                    );
            }
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
            if (TargetSpriteBatch == null)
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
