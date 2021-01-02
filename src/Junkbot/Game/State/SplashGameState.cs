using Junkbot.Game.Interface;
using Oddmatics.Rzxe.Game;
using Oddmatics.Rzxe.Game.Interface;
using Oddmatics.Rzxe.Input;
using Oddmatics.Rzxe.Windowing.Graphics;
using System;
using System.Drawing;
using System.IO;

namespace Junkbot.Game.State
{
    /// <summary>
    /// Represents the main menu game state.
    /// </summary>
    internal class SplashGameState : GameState
    {
        public override InputFocalMode FocalMode
        {
            get { return InputFocalMode.Always; }
        }

        public override string Name
        {
            get { return "SplashScreen"; }
        }
        
        
        private Scene DemoScene { get; set; }
        
        private UxShell Shell { get; set; }
        
        
        public SplashGameState(
            JunkbotGame junkbot
        )
        {
            DemoScene = Scene.FromLevel(
                            junkbot.Levels.GetLevelSource(0, 0),
                            junkbot.Animations
                        );
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


        public override void RenderFrame(IGraphicsController graphics)
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
                DrawMode.Stretch
            );
            
            sb.Finish();
            
            // Render the UI on top of everything
            //
            Shell.RenderFrame(graphics);
        }

        public override void Update(TimeSpan deltaTime, InputEvents inputs)
        {
            if (inputs != null)
            {
                Shell.HandleMouseInputs(inputs);
            }

            DemoScene.UpdateActors(deltaTime);
        }
    }
}
