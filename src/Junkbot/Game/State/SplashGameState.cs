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
            Shell     = new UxShell();
            
            Shell.Components.Add(
                new JunkbotUxButton()
                {
                    Location = new Point(139, 148),
                    Size     = new Size(116, 45),
                    Text     = "play"
                }
            );
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
            
            // Render the splash next (FIXME: Separate into another method)
            //
            ISpriteAtlas atlas = graphics.GetSpriteAtlas("menu");
            ISpriteBatch sb    = graphics.CreateSpriteBatch(atlas);

            sb.Draw(
                atlas.Sprites["neo_title"],
                new Rectangle(
                    Point.Empty,
                    graphics.TargetResolution
                ),
                DrawMode.Stretch,
                Color.Transparent
            );
            
            sb.Finish();
            
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
    }
}
