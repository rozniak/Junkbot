using Oddmatics.Rzxe.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Oddmatics.Rzxe.Game
{
    public interface IGameEngine
    {
        void Begin();

        void Update(TimeSpan deltaTime, InputEvents inputs);
    }
}
