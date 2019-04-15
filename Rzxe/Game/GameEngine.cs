using Oddmatics.Rzxe.Input;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Oddmatics.Rzxe.Game
{
    public abstract class GameEngine
    {
        public abstract Size DefaultClientWindowSize { get; }


        protected List<GameState> StateStack { get; set; }


        public virtual void Begin()
        {
            StateStack = new List<GameState>();
        }

        public virtual void Update(TimeSpan deltaTime, InputEvents inputs)
        {
            //
            // Nothing yet
            //
        }
    }
}
