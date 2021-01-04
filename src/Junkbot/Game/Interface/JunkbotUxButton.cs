/**
 * JunkbotUxButton.cs - Junkbot Button UI Component
 *
 * This source-code is part of a clean-room recreation of Lego Junkbot by Oddmatics:
 * <<https://www.oddmatics.uk>>
 *
 * Author(s): Rory Fewell <roryf@oddmatics.uk>
 */

using Oddmatics.Rzxe.Game.Interface;
using Oddmatics.Rzxe.Windowing.Graphics;
using System;
using System.Drawing;

namespace Junkbot.Game.Interface
{
    /// <summary>
    /// Represents a Junkbot themed button user interface component.
    /// </summary>
    public class JunkbotUxButton : UxComponent
    {
        /// <summary>
        /// The border metrics for the content area.
        /// </summary>
        /// <remarks>
        /// These metrics differ slightly from the slices used to draw the border
        /// itself as the border contents a small bit of the content area due to the
        /// shape of it.
        /// </remarks>
        private static readonly EdgeMetrics ContentAreaBorder =
            new EdgeMetrics(3, 8, 5, 1);
        
        
        /// <inheritdoc />
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
        
        /// <inheritdoc />
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
        
        /// <inheritdoc />
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
        
        
        /// <summary>
        /// The value that indicates whether the state of the button is dirty and
        /// the sprite batches must be regenerated on the next render call.
        /// </summary>
        private bool Dirty { get; set; }
        
        /// <summary>
        /// The font resource used for the button text.
        /// </summary>
        private IFont Font { get; set; }
        
        /// <summary>
        /// The sub-batch used to draw the button in its hover state.
        /// </summary>
        private ISubSpriteBatch StateHoverSpriteBatch { get; set; }
        
        /// <summary>
        /// The sub-batch used to draw the button in its normal state.
        /// </summary>
        private ISubSpriteBatch StateNormalSpriteBatch { get; set; }
        
        
        /// <summary>
        /// Initializes a new instance of the <see cref="JunkbotUxButton"/> class.
        /// </summary>
        public JunkbotUxButton()
        {
            _Location = new Point(64, 64);
            _Size     = new Size(100, 64);
            _Text     = string.Empty;

            Dirty = true;
        }
        
        
        /// <inheritdoc />
        public override void Render(
            ISpriteBatch sb
        )
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
