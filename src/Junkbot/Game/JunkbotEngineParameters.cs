using Oddmatics.Rzxe.Game;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
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
                return String.Format(
                     "{0}{1}Content",
                     Environment.CurrentDirectory,
                     Path.DirectorySeparatorChar
                );
            }
        }
    }
}
