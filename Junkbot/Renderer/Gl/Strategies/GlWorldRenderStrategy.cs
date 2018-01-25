using Junkbot.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Junkbot.Renderer.Gl.Strategies
{
    internal class GlWorldRenderStrategy : IRenderStrategy
    {
        private JunkbotGame Game;

        

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public bool Initialize(JunkbotGame gameReference)
        {
            Game = gameReference;

            return true;
        }

        public void RenderFrame()
        {
            throw new NotImplementedException();
        }
    }
}
