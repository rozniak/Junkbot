using Junkbot.Game.World.Actors.Animation;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Junkbot.Game.UI.Controls
{
    internal abstract class Control
    {
        public AnimationServer Animation { get; private set; }

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


        event EventHandler LocationChanged;

        event EventHandler SizeChanged;
    }
}
