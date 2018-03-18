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

        private SpriteAtlas MenuAtlas;


        public void Dispose()
        {
            MenuAtlas?.Dispose();
        }

        public bool Initialize(JunkbotGame gameReference)
        {
            Game = gameReference;

            MenuAtlas = GlUtil.LoadAtlas(Environment.CurrentDirectory + @"\Content\Atlas\menu-atlas");

            return true;
        }

        public void RenderFrame()
        {
            GL.Clear(ClearBufferMask.ColorBufferBit);
        }
    }
}
