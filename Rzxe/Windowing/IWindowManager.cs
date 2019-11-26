using Oddmatics.Rzxe.Game;
using Oddmatics.Rzxe.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Oddmatics.Rzxe.Windowing
{
    public interface IWindowManager : IDisposable
    {
        bool IsOpen { get; }

        bool Ready { get; }

        IGameEngine RenderedGameEngine { get; set; }


        void Initialize();

        InputEvents ReadInputEvents();

        void RenderFrame();
    }
}
