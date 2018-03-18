using Junkbot.Game;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Pencil.Gaming.Graphics;
using Pencil.Gaming.MathUtils;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Junkbot.Renderer.Gl.Strategies
{
    internal class GlWorldRenderStrategy : GlRenderStrategy
    {
        private SpriteAtlas ActorAtlas;

        private JunkbotGame Game;
        

        public override void Dispose()
        {
            ActorAtlas?.Dispose();
        }

        public override bool Initialize(JunkbotGame gameReference)
        {
            Game = gameReference;

            ActorAtlas = GlUtil.LoadAtlas(Environment.CurrentDirectory + @"\Content\Atlas\actors-atlas");

            return true;
        }

        public override void RenderFrame()
        {
            GL.Clear(ClearBufferMask.ColorBufferBit);
        }
    }
}
