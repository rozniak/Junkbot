/**
 * JunkbotUxButton.cs - Junkbot Button UI Component
 *
 * This source-code is part of a clean-room recreation of Lego Junkbot by Oddmatics:
 * <<https://www.oddmatics.uk>>
 *
 * Author(s): Rory Fewell <roryf@oddmatics.uk>
 */

using Oddmatics.Rzxe.Game.Interface;
using Oddmatics.Rzxe.Input;
using Oddmatics.Rzxe.Windowing.Graphics;
using System;
using System.Drawing;

namespace Junkbot.Game.Interface
{
    /// <summary>
    /// Represents a Junkbot themed button user interface component.
    /// </summary>
    public sealed class JunkbotUxButton : UxComponent
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
        
        
        /// <summary>
        /// Gets or sets the font size of the text on the button.
        /// </summary>
        public int FontSize
        {
            get { return _FontSize; }
            set
            {
                if (value <= 0)
                {
                    throw new ArgumentException("Invalid font size specified.");
                }

                _FontSize = value;
                Invalidate();
            }
        }
        private int _FontSize;

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
                Invalidate();
            }
        }
        private string _Text;
        
        
        /// <summary>
        /// The font resource used for the button text.
        /// </summary>
        private IFont Font { get; set; }


        #region Drawing Related

        /// <summary>
        /// The drawing instruction for the button body.
        /// </summary>
        private IBorderBoxDrawInstruction ButtonDrawInstruction { get; set; }

        /// <summary>
        /// The drawing instruction for the button text.
        /// </summary>
        private IStringDrawInstruction ButtonTextDrawInstruction { get; set; }
        
        /// <summary>
        /// The target sprite batch for drawing.
        /// </summary>
        private ISpriteBatch TargetSpriteBatch { get; set; }

        #endregion
        
        
        /// <summary>
        /// Occurs when the button has been clicked.
        /// </summary>
        public event MouseInputEventHandler Click;


        /// <summary>
        /// Initializes a new instance of the <see cref="JunkbotUxButton"/> class.
        /// </summary>
        public JunkbotUxButton()
        {
            Dirty = true;
            FontSize = 1;
        }
        
        
        /// <inheritdoc />
        public override void Dispose()
        {
            base.Dispose();

            TargetSpriteBatch?.Dispose();
        }
        
        /// <inheritdoc />
        public override void OnClick(
            ControlInput mouseButton,
            Point        mouseLocation
        )
        {
            Click?.Invoke(
                this,
                new MouseInputEventArgs(
                    mouseButton,
                    mouseLocation
                )
            );
        }

        /// <inheritdoc />
        public override void OnMouseEnter()
        {
            if (TargetSpriteBatch == null)
            {
                return;
            }

            ButtonDrawInstruction.BorderBox =
                TargetSpriteBatch.Atlas.BorderBoxes["button_s_active"];
        }
        
        /// <inheritdoc />
        public override void OnMouseLeave()
        {
            if (TargetSpriteBatch == null)
            {
                return;
            }
            
            ButtonDrawInstruction.BorderBox =
                TargetSpriteBatch.Atlas.BorderBoxes["button_s_inactive"];
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

                ButtonDrawInstruction =
                    TargetSpriteBatch.DrawBorderBox(
                        sb.Atlas.BorderBoxes["button_s_inactive"],
                        Rectangle.Empty,
                        Color.Transparent
                    );
                    
                Font = TargetSpriteBatch.Atlas.GetSpriteFont("default", FontSize);
                ButtonTextDrawInstruction =
                    TargetSpriteBatch.DrawString(
                        string.Empty,
                        Font,
                        Point.Empty,
                        Color.Black
                    );
            }
            
            if (Dirty)
            {
                Font = TargetSpriteBatch.Atlas.GetSpriteFont("default", FontSize);
            
                StringMetrics stringSize = Font.MeasureString(Text);

                int contentHeight =
                    Size.Height - ContentAreaBorder.Bottom - ContentAreaBorder.Top;
                int contentWidth =
                    Size.Width - ContentAreaBorder.Left - ContentAreaBorder.Right;

                int contentX = (contentWidth / 2) - (stringSize.Size.Width / 2);
                int contentY = (contentHeight / 2) - (stringSize.Size.Height / 2);

                int finalX = ActualLocation.X + ContentAreaBorder.Left + contentX;
                int finalY = ActualLocation.Y + ContentAreaBorder.Top + contentY;

                ButtonDrawInstruction.Location = ActualLocation;
                ButtonDrawInstruction.Size = Size;

                ButtonTextDrawInstruction.Location = new Point(finalX, finalY);
                ButtonTextDrawInstruction.Text = Text;

                Dirty = false;
            }
        }
    }
}
