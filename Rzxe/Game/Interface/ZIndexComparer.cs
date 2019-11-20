using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oddmatics.Rzxe.Game.Interface
{
    internal sealed class ZIndexComparer : IComparer<UxComponent>
    {
        public int Compare(UxComponent x, UxComponent y)
        {
            if (x.ZIndex > y.ZIndex)
            {
                return 1;
            }
            else if (x.ZIndex < y.ZIndex)
            {
                return -1;
            }
            else
            {
                return 0;
            }
        }
    }
}
