using Junkbot.Game.World.Actors.Animation;
using System;
using System.Drawing;

namespace Junkbot.Game.UI.Controls
{
    /// <summary>
    /// Represents a user interface control for a UI in the Junkbot game engine.
    /// </summary>
    internal abstract class Control
    {
        /// <summary>
        /// Gets the active animation of this control.
        /// </summary>
        public AnimationServer Animation { get; private set; }

        /// <summary>
        /// Gets or sets the location of this control on screen.
        /// </summary>
        public Point Location
        {
            get { return this._Location; }

            set
            {
                this._Location = value;

                LocationChanged?.Invoke(this, EventArgs.Empty);
            }
        }
        private Point _Location;
        
        /// <summary>
        /// Gets or sets the size of this control.
        /// </summary>
        public Size Size
        {
            get { return this._Size; }

            set
            {
                this._Size = value;

                SizeChanged?.Invoke(this, EventArgs.Empty);
            }
        }
        private Size _Size;


        /// <summary>
        /// Occurs when the location of this control has changed.
        /// </summary>
        event EventHandler LocationChanged;

        /// <summary>
        /// Occurs when the size of this control has changed.
        /// </summary>
        event EventHandler SizeChanged;
    }
}
