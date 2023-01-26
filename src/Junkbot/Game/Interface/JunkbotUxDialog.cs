/**
 * JunkbotUxDialog.cs - Junkbot Dialog UI Component
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
    /// Represents a Junkbot themed dialog user interface component.
    /// </summary>
    public class JunkbotUxDialog : UxContainer
    {
        /// <inheritdoc />
        public override UxContainer Owner
        {
            get { return _Owner; }
            set
            {
                _Owner?.ReleaseInputRouting(this);

                _Owner = value;
                _Owner?.StealInputRouting(this);

                Invalidate();
            }
        }


        /// <summary>
        /// The drawing instrution for the dialog body.
        /// </summary>
        private IBorderBoxDrawInstruction DialogDrawInstruction { get; set; }


        /// <summary>
        /// Initializes a new instance of the <see cref="JunkbotUxDialog"/> class.
        /// </summary>
        public JunkbotUxDialog()
        {
            Dirty = true;
        }
        
        
        /// <inheritdoc />
        public override void Dispose()
        {
            base.Dispose();

            Owner?.ReleaseInputRouting(this);
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

                DialogDrawInstruction =
                    TargetSpriteBatch.DrawBorderBox(
                        sb.Atlas.BorderBoxes["dialog"],
                        new Rectangle(0, 0, 0, 0),
                        Color.Transparent
                    );
            }
            
            if (Dirty)
            {
                DialogDrawInstruction.Location = ActualLocation;
                DialogDrawInstruction.Size = Size;

                Dirty = false;
            }

            RenderContainer(sb);
        }
    }
}
