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
    public class JunkbotUxMoveCounter : UxComponent
    {
        private static readonly Size CounterBoxWidthSize =
            new Size(68, 0);

        private static readonly Size PaddingOffsetSize =
            new Size(-2, 3);


        /// <inheritdoc />
        public override Point Location
        {
            get { return _Location; }
            set
            {
                _Location = value;
                Invalidate();
            }
        }
        private Point _Location;


        /// <summary>
        /// The game scene.
        /// </summary>
        private Scene Scene;


        #region Drawing Related

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

        #endregion


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
            Scene = scene;

            Scene.PlayedMove += Scene_PlayedMove;
        }


        /// <inheritdoc />
        public override void Dispose()
        {
            AssertNotDisposed();

            Disposing = true;

            if (TargetSpriteBatch != null)
            {
                TargetSpriteBatch.Instructions.Remove(CounterInstruction);
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

                // Immediately invalidate
                //
                Invalidate();
            }
        }


        /// <summary>
        /// Invalidates the control.
        /// </summary>
        private void Invalidate()
        {
            if (TargetSpriteBatch == null)
            {
                return;
            }

            string movesStr = Scene.Moves.ToString();
            Size   size     = Font.MeasureString(movesStr).Size;

            CounterInstruction.Location = Location.Add(CounterBoxWidthSize)
                                                  .Add(PaddingOffsetSize)
                                                  .Subtract(
                                                      new Size(size.Width, 0)
                                                  );
            CounterInstruction.Text     = movesStr;
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
