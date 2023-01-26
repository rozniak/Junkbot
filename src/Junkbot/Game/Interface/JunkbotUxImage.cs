/**
 * JunkbotUxImage.cs - Junkbot Image UI Component
 *
 * This source-code is part of a clean-room recreation of Lego Junkbot by Oddmatics:
 * <<https://www.oddmatics.uk>>
 *
 * Author(s): Rory Fewell <roryf@oddmatics.uk>
 */

using Oddmatics.Rzxe.Game.Interface;
using Oddmatics.Rzxe.Windowing.Graphics;
using System.Drawing;

namespace Junkbot.Game.Interface
{
    /// <summary>
    /// Represents a basic image component within Junkbot's user interface.
    /// </summary>
    public sealed class JunkbotUxImage : UxComponent
    {
        /// <summary>
        /// The default image to display.
        /// </summary>
        private const string DefaultImage = "check_light";
    
    
        /// <summary>
        /// Gets or sets the name of the image to display.
        /// </summary>
        public string ImageName
        {
            get { return _ImageName; }
            set
            {
                _ImageName = value;
                Invalidate();
            }
        }
        private string _ImageName;
        
        
        /// <summary>
        /// The drawing instruction for the image.
        /// </summary>
        private ISpriteDrawInstruction ImageDrawInstruction { get; set; }

        /// <summary>
        /// The target sprite batch used for drawing.
        /// </summary>
        private ISpriteBatch TargetSpriteBatch { get; set; }


        /// <summary>
        /// Initializes a new instance of the <see cref="JunkbotUxImage"/> class.
        /// </summary>
        public JunkbotUxImage()
        {
            Dirty     = true;
            ImageName = DefaultImage;
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

                ImageDrawInstruction =
                    TargetSpriteBatch.Draw(
                        TargetSpriteBatch.Atlas.Sprites[ImageName],
                        Point.Empty,
                        Color.Transparent
                    );
            }
            
            if (Dirty)
            {
                ISprite sprite = TargetSpriteBatch.Atlas.Sprites[ImageName];
                
                ImageDrawInstruction.Location = ActualLocation;
                ImageDrawInstruction.Size     = Size == Size.Empty ? sprite.Size : Size;
                ImageDrawInstruction.Sprite   = sprite;

                Dirty = false;
            }
        }
    }
}
