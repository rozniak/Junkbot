using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Oddmatics.Rzxe.Windowing
{
    public interface IWindowManager
    {
        bool Ready { get; }


        void Initialize();
    }
}
