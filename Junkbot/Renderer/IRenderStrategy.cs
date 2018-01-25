using Junkbot.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Junkbot.Renderer
{
    internal interface IRenderStrategy : IDisposable
    {
        bool Initialize(JunkbotGame gameReference);

        void RenderFrame();
    }
}
