/**
 * JunkbotUxText.cs - Junkbot Text String UI Component
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
    /// Represents a Junkbot themed text string user interface component.
    /// </summary>
    public sealed class JunkbotUxText : UxComponent
    {
        /// <summary>
        /// Gets or sets the color of the text.
        /// </summary>
        public Color Color
        {
            get { return _Color; }
            set
            {
                _Color = value;
                Invalidate();
            }
        }
        private Color _Color;
        
        /// <summary>
        /// Gets or sets the font size of the text.
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
        public override Size Size
        {
            get
            {
                if (Font == null)
                {
                    return Size.Empty;
                }

                return Font.MeasureString(Text).Size;
            }
            
            set
            {
                throw new NotSupportedException(
                    "Setting exact size is impossible for text, use " +
                    $"{nameof(FontSize)} instead."
                );
            }
        }
        
        /// <summary>
        /// Gets or sets the text of the component.
        /// </summary>
        public string Text
        {
            get { return _Text; }
            set
            {
                _Text = value.ToLower();
                Invalidate();
            }
        }
        private string _Text;
        
        
        /// <summary>
        /// The font resource used for the text.
        /// </summary>
        private IFont Font { get; set; }

        /// <summary>
        /// The target sprite batch used for drawing.
        /// </summary>
        private ISpriteBatch TargetSpriteBatch { get; set; }

        /// <summary>
        /// The drawing instruction for the text.
        /// </summary>
        private IStringDrawInstruction TextDrawInstruction { get; set; }
        
        
        /// <summary>
        /// Initializes a new instance of the <see cref="JunkbotUxText"/> class.
        /// </summary>
        public JunkbotUxText()
        {
            Dirty = true;
            FontSize = 1;
        }
        
        
        /// <inheritdoc />
        public override void Dispose()
        {
            base.Dispose();
            
            if (TargetSpriteBatch != null)
            {
                TargetSpriteBatch.Dispose();
            }
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
                
                Font = TargetSpriteBatch.Atlas.GetSpriteFont("default", FontSize);
                TextDrawInstruction =
                    TargetSpriteBatch.DrawString(
                        string.Empty,
                        Font,
                        Point.Empty,
                        Color.Transparent
                    );
            }
            
            if (Dirty)
            {
                Font = TargetSpriteBatch.Atlas.GetSpriteFont("default", FontSize);

                TextDrawInstruction.Color = Color;
                TextDrawInstruction.Font = Font;
                TextDrawInstruction.Location = ActualLocation;
                TextDrawInstruction.Text = Text;

                Dirty = false;
            }
        }
    }
}
