using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oddmatics.Rzxe.Game
{
    public interface IGameEngineParameters
    {
        Size DefaultClientWindowSize { get; }

        string GameContentRoot { get; }
    }
}
