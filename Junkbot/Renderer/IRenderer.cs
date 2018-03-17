using Junkbot.Game;
using Junkbot.Game.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Junkbot.Renderer
{
    internal interface IRenderer
    {
        bool IsOpen { get; }


        InputEvents GetInputEvents();

        void RenderFrame();

        void Start(JunkbotGame gameInstance);

        void Stop();
    }
}
