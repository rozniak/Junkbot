using Junkbot.Game;
using Pencil.Gaming.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Junkbot.Renderer.Gl.Strategies
{
    internal class GlMenuRenderStrategy : IRenderStrategy
    {
        private JunkbotGame Game;


        public void Dispose()
        {
        }

        public bool Initialize(JunkbotGame gameReference)
        {
            Game = gameReference;



            return true;
        }

        public void RenderFrame()
        {
            GL.Clear(ClearBufferMask.ColorBufferBit);
        }
    }
}
