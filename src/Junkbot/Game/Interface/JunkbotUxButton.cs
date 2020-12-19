using Oddmatics.Rzxe.Game.Interface;
using Oddmatics.Rzxe.Windowing.Graphics;
using System;
using System.Drawing;

namespace Junkbot.Game.Interface
{
    public class JunkbotUxButton : UxComponent
    {
        private static readonly EdgeMetrics ContentAreaBorder =
            new EdgeMetrics(3, 8, 5, 1);
    
    
        public override Point Location
        {
            get { return _Location; }
            set
            {
                _Location = value;
                Dirty     = true;
            }
        }
        private Point _Location;
        
        public override Size Size
        {
            get { return _Size; }
            set
            {
                _Size = value;
                Dirty = true;
            }
        }
        private Size _Size;

        public string Text
        {
            get { return _Text; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentNullException(
                        $"Cannot set text to null, use {nameof(string.Empty)} instead."
                    );
                }

                _Text = value;
                Dirty = true;
            }
        }
        private string _Text;
        

        private bool Dirty { get; set; }
        
        private IFont Font { get; set; }
        
        private ISubSpriteBatch StateHoverSpriteBatch { get; set; }
        
        private ISubSpriteBatch StateNormalSpriteBatch { get; set; }


        public JunkbotUxButton()
        {
            _Location = new Point(64, 64);
            _Size     = new Size(100, 64);
            _Text     = string.Empty;

            Dirty = true;
        }
        
        
        public override void Render(ISpriteBatch sb)
        {
            if (Dirty)
            {
                if (Font == null)
                {
                    Font = sb.Atlas.GetSpriteFont("default", 4);
                }
                
                var           inactiveBox = sb.Atlas.BorderBoxes["button_s_inactive"];
                StringMetrics stringSize  = Font.MeasureString(Text);

                int contentHeight =
                    Size.Height - ContentAreaBorder.Bottom - ContentAreaBorder.Top;
                int contentWidth  =
                    Size.Width  - ContentAreaBorder.Left   - ContentAreaBorder.Right;

                int contentX = ((contentWidth  / 2) - (stringSize.Size.Width  / 2));
                int contentY = ((contentHeight / 2) - (stringSize.Size.Height / 2));

                int finalX = Location.X + ContentAreaBorder.Left + contentX;
                int finalY = Location.Y + ContentAreaBorder.Top  + contentY;

                ISubSpriteBatch subSb = sb.CreateSubBatch();

                subSb.DrawBorderBox(
                    inactiveBox,
                    new Rectangle(
                        Location,
                        Size
                    )
                );

                subSb.DrawString(
                    Text,
                    Font,
                    new Point(finalX, finalY)
                );

                StateNormalSpriteBatch = subSb;
            }

            sb.DrawSubBatch(StateNormalSpriteBatch);
        }
    }
}
