using Oddmatics.Rzxe.Game.Interface;
using Oddmatics.Rzxe.Windowing.Graphics;
using System;
using System.Drawing;

namespace Junkbot.Game.Interface
{
    public class JunkbotUxButton : UxComponent
    {
        public JunkbotUxButton()
        {
            Location = new Point(64, 64);
            Size     = new Size(100, 64);
        }


        public override void Render(ISpriteBatch sb)
        {
            sb.DrawBorderBox(
                "button_s_inactive",
                new System.Drawing.Rectangle(
                    Location,
                    Size
                )
            );
            
            sb.DrawString(
                "play",
                "font_default",
                new Point(
                    Location.X + 4,
                    Location.Y + 16
                ),
                4
            );
        }
    }
}
