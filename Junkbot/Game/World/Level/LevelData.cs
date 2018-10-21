using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Junkbot.Game.World.Level
{
    internal struct JunkbotLevelData
    {
        // [info]
        public string Title;
        public ushort Par;
        public string Hint;

        // [background]
        public string Backdrop;
        public string[] Decals;

        // [playfield]
        public Size Size;
        public Size Spacing; // Always 15,18
        public byte Scale; // Is this even used?

        // [partslist]
        public string[] Types;
        public string[] Colors;
        public IList<JunkbotPartData> Parts;
    }
}
