/**
 * JunkbotUxMoveCounter.cs - Junkbot Move Counter UI Component
 *
 * This source-code is part of a clean-room recreation of Lego Junkbot by Oddmatics:
 * <<https://www.oddmatics.uk>>
 *
 * Author(s): Rory Fewell <roryf@oddmatics.uk>
 */

using Oddmatics.Rzxe.Extensions;
using Oddmatics.Rzxe.Game.Interface;
using Oddmatics.Rzxe.Windowing.Graphics;
using System;
using System.Drawing;

namespace Junkbot.Game.Interface
{
    /// <summary>
    /// Represents a Junkbot move counter user interface component.
    /// </summary>
    public sealed class JunkbotUxMoveCounter : UxComponent
    {
        /// <summary>
        /// The width of the move counter box, as a <see cref="Size"/>.
        /// </summary>
        private static readonly Size CounterBoxWidthSize =
            new Size(68, 0);

        /// <summary>
        /// The top-right padding in the move counter box.
        /// </summary>
        private static readonly Size PaddingOffsetSize =
            new Size(-2, 3);
            
            
        /// <inheritdoc />
        public override Size Size
        {
            get
            {
                if (Font == null)
                {
                    return Size.Empty;
                }

                return Font.MeasureString(Scene.Moves.ToString()).Size;
            }
            
            set
            {
                throw new InvalidOperationException(
                    "Cannot set the size of move counters."
                );
            }
        }
            

        /// <summary>
        /// The font resource used for the counter.
        /// </summary>
        private IFont Font { get; set; }

        /// <summary>
        /// The drawing instruction for the counter.
        /// </summary>
        private IStringDrawInstruction CounterInstruction { get; set; }

        /// <summary>
        /// The target sprite batch for drawing.
        /// </summary>
        private ISpriteBatch TargetSpriteBatch { get; set; }

        /// <summary>
        /// The game scene.
        /// </summary>
        private Scene Scene;


        /// <summary>
        /// Initializes a new instance of <see cref="JunkbotUxMoveCounter"/>.
        /// </summary>
        /// <param name="scene">
        /// The game scene to track the moves for.
        /// </param>
        public JunkbotUxMoveCounter(
            Scene scene
        )
        {
            Dirty = true;
            Scene = scene;

            Scene.PlayedMove += Scene_PlayedMove;
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

                if (Font == null)
                {
                    Font = sb.Atlas.GetSpriteFont("default", 2);
                }

                CounterInstruction =
                    sb.DrawString(
                        string.Empty,
                        Font,
                        Point.Empty,
                        Color.FromArgb(204, 153, 0)
                    );
            }
            
            if (Dirty)
            {
                CounterInstruction.Location = ActualLocation.Add(CounterBoxWidthSize)
                                                            .Add(PaddingOffsetSize)
                                                            .Subtract(
                                                                new Size(Size.Width, 0)
                                                            );
                CounterInstruction.Text     = Scene.Moves.ToString();

                Dirty = false;
            }
        }


        /// <summary>
        /// (Event) Handles when a move has been made.
        /// </summary>
        private void Scene_PlayedMove(
            object    sender,
            EventArgs e
        )
        {
            Invalidate();
        }
    }
}
