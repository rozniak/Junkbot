using Oddmatics.Rzxe.Game;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Junkbot.Game
{
    internal sealed class JunkbotEngineParameters : IGameEngineParameters
    {
        public Size DefaultClientWindowSize
        {
            get { return new Size(650, 420); }
        }

        public string GameContentRoot
        {
            get
            {
                return Environment.CurrentDirectory + "\\Content";
            }
        }
    }
}
