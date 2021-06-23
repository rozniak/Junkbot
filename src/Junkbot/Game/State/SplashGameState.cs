/**
 * SplashGameState.cs - Splash Screen Game State
 *
 * This source-code is part of a clean-room recreation of Lego Junkbot by Oddmatics:
 * <<https://www.oddmatics.uk>>
 *
 * Author(s): Rory Fewell <roryf@oddmatics.uk>
 */

using Junkbot.Game.Interface;
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
    /// Represents the splash screen game state.
    /// </summary>
    internal class SplashGameState : GameState
    {
        /// <summary>
        /// The rectangle that defines the bounds of the visible portion of the scene
        /// in the viewport.
        /// </summary>
        private static Rectangle SceneRect =
            new Rectangle(35, 200, 414, 184);
        
        
        /// <inhertdoc />
        public override InputFocalMode FocalMode
        {
            get { return InputFocalMode.Always; }
        }
        
        /// <inhertdoc />
        public override string Name
        {
            get { return "SplashScreen"; }
        }
        
        
        /// <summary>
        /// The demo/loading level game scene.
        /// </summary>
        private Scene DemoScene { get; set; }
        
        /// <summary>
        /// The running Junkbot game instance.
        /// </summary>
        private JunkbotGame Game { get; set; }

        /// <summary>
        /// The user interface shell.
        /// </summary>
        private UxShell Shell { get; set; }


        #region Drawing Related
        
        /// <summary>
        /// The target sprite batch for rendering the title.
        /// </summary>
        private ISpriteBatch TitleSpriteBatch { get; set; }

        #endregion


        #region Shell Components
        
        /// <summary>
        /// The 'PLAY' button.
        /// </summary>
        private JunkbotUxButton PlayButton { get; set; }

        #endregion


        /// <summary>
        /// Initializes a new instance of the <see cref="SplashGameState"/> class.
        /// </summary>
        /// <param name="game">
        /// The running Junkbot game instance.
        /// </param>
        public SplashGameState(
            JunkbotGame game
        )
        {
            DemoScene = Scene.FromLevel(
                            game.Levels.GetLevelSource(0, 0),
                            game.Animations
                        );
            Game      = game;

            InitializeShell();
        }
        
        
        /// <inheritdoc />
        public override void Dispose()
        {
            Shell.Dispose();
            
            if (TitleSpriteBatch != null)
            {
                TitleSpriteBatch.Dispose();
            }
        }

        /// <inheritdoc />
        public override void RenderFrame(
            IGraphicsController graphics
        )
        {
            graphics.ClearViewport(Color.CornflowerBlue);
            
            // Render scene first, it's below everything
            //
            DemoScene.RenderFrame(graphics);

            // Render the splash next
            //
            RenderTitle(graphics);
            
            // Render the UI on top of everything
            //
            Shell.RenderFrame(graphics);
        }
        
        /// <inheritdoc />
        public override void Update(
            TimeSpan    deltaTime,
            InputEvents inputs
        )
        {
            if (inputs != null)
            {
                Shell.HandleMouseInputs(inputs);
            }

            // Check the mouse is within the scene
            //
            PointF mousePos =
                Game.EngineHost.Renderer.PointToViewport(
                    inputs.MousePosition
                );
            
            if (Collision.PointInRect(mousePos, SceneRect))
            {
                DemoScene.Update(
                    Game,
                    deltaTime,
                    inputs
                );
            }
            else
            {
                DemoScene.Update(
                    Game,
                    deltaTime
                );
            }
        }
        
        
        /// <summary>
        /// Initializes the shell for the splash screen.
        /// </summary>
        private void InitializeShell()
        {
            Shell = new UxShell();

            // PlayButton
            //
            PlayButton =
                new JunkbotUxButton()
                {
                    Location = new Point(139, 148),
                    Size     = new Size(116, 45),
                    Text     = "play"
                };
                
            PlayButton.Click += PlayButton_Click;
                
            Shell.Components.Add(PlayButton);
        }

        /// <summary>
        /// Renders the title screen graphics.
        /// </summary>
        /// <param name="graphics">
        /// The graphics interface for the renderer.
        /// </param>
        private void RenderTitle(
            IGraphicsController graphics
        )
        {
            if (TitleSpriteBatch == null)
            {
                ISpriteAtlas atlas = graphics.GetSpriteAtlas("menu");
                
                TitleSpriteBatch =
                    graphics.CreateSpriteBatch(
                        atlas,
                        SpriteBatchUsageHint.Static
                    );

                TitleSpriteBatch.Draw(
                    atlas.Sprites["neo_title"],
                    new Rectangle(
                        Point.Empty,
                        graphics.TargetResolution
                    ),
                    DrawMode.Stretch,
                    Color.Transparent
                );
            }

            TitleSpriteBatch.Finish();
        }
        
        
        /// <summary>
        /// (Event) Handles the 'Play' button being clicked.
        /// </summary>
        private void PlayButton_Click(
            object    sender,
            EventArgs e
        )
        {
            Game.CurrentGameState = new SelectLevelGameState(Game);

            Dispose();
        }
    }
}
