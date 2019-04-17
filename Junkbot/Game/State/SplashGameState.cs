using Oddmatics.Rzxe.Game;
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


        public override void RenderFrame(IGraphicsController graphics)
        {
            var sb = graphics.CreateSpriteBatch("menu-atlas");

            graphics.ClearViewport(Color.CornflowerBlue);

            sb.Draw(
                "neo_title",
                new Rectangle(
                    Point.Empty,
                    graphics.TargetResolution
                    )
                );

            sb.Finish();
        }
    }
}
