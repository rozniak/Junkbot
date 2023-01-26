/**
 * PlayingLevelGameState.cs - Playing Level Game State
 *
 * This source-code is part of a clean-room recreation of Lego Junkbot by Oddmatics:
 * <<https://www.oddmatics.uk>>
 *
 * Author(s): Rory Fewell <roryf@oddmatics.uk>
 */

using Junkbot.Game.Interface;
using Junkbot.Game.World.Level;
using Oddmatics.Rzxe.Game;
using Oddmatics.Rzxe.Game.Interface;
using Oddmatics.Rzxe.Input;
using Oddmatics.Rzxe.Logic;
using Oddmatics.Rzxe.Windowing.Graphics;
using System;
using System.Drawing;

namespace Junkbot.Game.State
{
    /// <summary>
    /// Represents the in-game level game state.
    /// </summary>
    internal class PlayingLevelGameState : GameState
    {
        /// <summary>
        /// Specifies constants defining the possible internal states for the in-game
        /// level game state.
        /// </summary>
        private enum PlayingLevelInternalState
        {
            /// <summary>
            /// Currently entering the level.
            /// </summary>
            Entering,
            
            /// <summary>
            /// The level is actively being played.
            /// </summary>
            Playing,
            
            /// <summary>
            /// The level has ended.
            /// </summary>
            Ended
        }
        
        
        /// <summary>
        /// The rectangle that defines the bounds of the scene in the viewport.
        /// </summary>
        private static Rectangle SceneRect =
            new Rectangle(0, 0, 530, 420);
    
    
        /// <inheritdoc />
        public override InputFocalMode FocalMode
        {
            get { return InputFocalMode.Always; }
        }
        
        /// <inheritdoc />
        public override string Name
        {
            get { return "PlayingLevel"; }
        }
        

        /// <summary>
        /// The running Junkbot game instance.
        /// </summary>
        private JunkbotGame Game { get; set; }
        
        /// <summary>
        /// The current internal state the game is in.
        /// </summary>
        private PlayingLevelInternalState InternalState { get; set; }
        
        /// <summary>
        /// The level being played.
        /// </summary>
        private JunkbotLevel Level { get; set; }

        /// <summary>
        /// The level game scene.
        /// </summary>
        private Scene LevelScene { get; set; }
        
        /// <summary>
        /// The user interface shell.
        /// </summary>
        private UxShell Shell { get; set; }


        #region Drawing Related

        /// <summary>
        /// The target sprite batch for rendering the shell sidebar.
        /// </summary>
        private ISpriteBatch SidebarSpriteBatch { get; set; }

        #endregion


        #region Shell Related
        
        /// <summary>
        /// The entering level dialog box.
        /// </summary>
        private JunkbotUxDialog EnteringDialog { get; set; }
        
        /// <summary>
        /// The time at which to destroy the entering level dialog box and begin
        /// gameplay.
        /// </summary>
        private DateTime EnteringDialogKillTime { get; set; }

        /// <summary>
        /// The move counter.
        /// </summary>
        private JunkbotUxMoveCounter MoveCounter { get; set; }

        #endregion


        /// <summary>
        /// Initializes a new instance of the <see cref="PlayingLevelGameState"/> class.
        /// </summary>
        /// <param name="game">
        /// The running Junkbot game instance.
        /// </param>
        /// <param name="level">
        /// The level to play.
        /// </param>
        public PlayingLevelGameState(
            JunkbotGame  game,
            JunkbotLevel level
        )
        {
            Game       = game;
            Level      = level;
            LevelScene = new Scene(level, game.Animations);
            Shell      = new UxShell();
                         
            LevelScene.GameplayEnded += LevelScene_GameplayEnded;
            
            // Prepare shell
            //
            MoveCounter =
                new JunkbotUxMoveCounter(LevelScene)
                {
                    Location = new Point(551, 222)
                };

            Shell.Components.Add(MoveCounter);
            
            // Init level
            //
            StartLevel();
        }


        /// <inheritdoc />
        public override void Dispose()
        {
            LevelScene.Dispose();
            Shell.Dispose();

            if (SidebarSpriteBatch != null)
            {
                SidebarSpriteBatch.Dispose();
            }
        }
        
        /// <inheritdoc />
        public override void RenderFrame(
            IGraphicsController graphics
        )
        {
            graphics.ClearViewport(Color.CornflowerBlue);

            LevelScene.RenderFrame(graphics);

            RenderSidebar(graphics);
            
            Shell.RenderFrame(graphics);
        }
        
        /// <inheritdoc />
        public override void Update(
            TimeSpan    deltaTime,
            InputEvents inputs
        )
        {
            Shell.Update(deltaTime, inputs);
            
            switch (InternalState)
            {
                case PlayingLevelInternalState.Entering:
                    if (DateTime.Now >= EnteringDialogKillTime)
                    {
                        Shell.Components.Remove(EnteringDialog);
                        EnteringDialog.Dispose();
                        EnteringDialog = null;

                        InternalState = PlayingLevelInternalState.Playing;
                    }
                    
                    break;

                case PlayingLevelInternalState.Playing:
                    PointF mousePos =
                        Game.EngineHost.Renderer.PointToViewport(
                            inputs.MousePosition
                        );

                    if (Collision.PointInRect(mousePos, SceneRect))
                    {
                        LevelScene.Update(
                            Game,
                            deltaTime,
                            inputs
                        );
                    }
                    else
                    {
                        LevelScene.Update(
                            Game,
                            deltaTime
                        );
                    }
                    
                    break;
            }
        }


        /// <summary>
        /// Renders the sidebar graphics.
        /// </summary>
        /// <param name="graphics">
        /// The graphics interface for the renderer.
        /// </param>
        private void RenderSidebar(
            IGraphicsController graphics
        )
        {
            if (SidebarSpriteBatch == null)
            {
                ISpriteAtlas atlas = graphics.GetSpriteAtlas("menu");

                SidebarSpriteBatch =
                    graphics.CreateSpriteBatch(
                        atlas,
                        SpriteBatchUsageHint.Static
                    );
                
                SidebarSpriteBatch.Draw(
                    atlas.Sprites["larger_bkg_test"],
                    new Rectangle(
                        new Point(527, 0),
                        new Size(123, 420)
                    ),
                    DrawMode.Stretch,
                    Color.Transparent
                );
            }

            SidebarSpriteBatch.Finish();
        }
        
        /// <summary>
        /// Starts or restarts the level.
        /// </summary>
        private void StartLevel()
        {
            // TODO: Implement start/restart functionality in Scene.cs and call it here

            //
            // We spawn the level name dialog here
            //
            // TODO: In future this dialog will animate, we directly spawn it in
            //       position here
            //

            // EnteringDialog
            //
            EnteringDialog = new JunkbotUxDialog()
            {
                Location = new Point(100, 130),
                Size = new Size(336, 164)
            };

            // buildingIcon
            //
            var buildingIcon = new JunkbotUxImage()
            {
                ImageName = $"building_icon_{Level.BuildingIndex}",
                Location = new Point(48, 35)
            };

            // buildingLogotype
            //
            var buildingLogotype = new JunkbotUxImage()
            {
                ImageName = $"building_tab_{Level.BuildingIndex}",
                Location = new Point(121, 52)
            };

            // levelName
            //
            int levelNum = ((Level.BuildingIndex - 1) * 15) + Level.LevelIndex + 1;
            
            var levelName = new JunkbotUxText()
            {
                Color = Color.White,
                FontSize = 2,
                Location = new Point(49, 83),
                Text = $"Level {levelNum}: {Level.Name}"
            };

            EnteringDialog.Components.Add(buildingIcon);
            EnteringDialog.Components.Add(buildingLogotype);
            EnteringDialog.Components.Add(levelName);
            Shell.Components.Add(EnteringDialog);

            EnteringDialogKillTime = DateTime.Now.AddSeconds(2);
            InternalState = PlayingLevelInternalState.Entering;
        }


        /// <summary>
        /// (Event) Handles when gameplay has ended in the level.
        /// </summary>
        private void LevelScene_GameplayEnded(
            object    sender,
            EventArgs e
        )
        {
            InternalState = PlayingLevelInternalState.Ended;

            var dialog = new JunkbotUxDialog()
            {
                Location = new Point(60, 80),
                Size     = new Size(410, 274)
            };

            var levelCompleteGfx = new JunkbotUxImage()
            {
                ImageName = "level_end_main"
            };

            var selectLvlButton = new JunkbotUxButton()
            {
                Location = new Point(286, 195),
                Size = new Size(96, 26),
                Text = "select level"
            };

            var nextLvlButton = new JunkbotUxButton()
            {
                Location = new Point(286, 223),
                Size     = new Size(96, 26),
                Text     = "next level"
            };
            
            nextLvlButton.Click += NextLvlButton_Click;
            selectLvlButton.Click += SelectLvlButton_Click;

            dialog.Components.Add(levelCompleteGfx);
            dialog.Components.Add(selectLvlButton);
            dialog.Components.Add(nextLvlButton);
            Shell.Components.Add(dialog);
        }
        
        
        /// <summary>
        /// (Event) Occurs when the 'Next Level' button is clicked.
        /// </summary>
        private void NextLvlButton_Click(
            object              sender,
            MouseInputEventArgs e
        )
        {
            Dispose();

            Game.CurrentGameState =
                new PlayingLevelGameState(
                    Game,
                    Level.GetNextLevel() // FIXME: This can be null!
                );
        }

        /// <summary>
        /// (Event) Occurs when the 'Select Level' button is clicked.
        /// </summary>
        private void SelectLvlButton_Click(
            object              sender,
            MouseInputEventArgs e
        )
        {
            Dispose();

            Game.CurrentGameState = new SelectLevelGameState(Game);
        }
    }
}
