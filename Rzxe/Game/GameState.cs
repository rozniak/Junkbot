using Oddmatics.Rzxe.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Oddmatics.Rzxe.Game
{
    public abstract class GameState
    {
        public abstract InputFocalMode FocalMode { get; }


        protected List<UxComponent> UxComponents { get; set; }
    }
}
