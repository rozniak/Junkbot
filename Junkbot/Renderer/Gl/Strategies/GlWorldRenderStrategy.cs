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
    internal class GlWorldRenderStrategy : IRenderStrategy
    {
        private JunkbotGame Game;


        private Vector2i SpriteAtlasDimensions;

        private Dictionary<string, Rectanglei> SpriteAtlasMap;

        private int SpriteAtlasTextureId;
        

        public void Dispose()
        {
            GL.DeleteTexture(SpriteAtlasTextureId);
        }

        public bool Initialize(JunkbotGame gameReference)
        {
            Game = gameReference;

            SpriteAtlasMap = GlUtil.LoadAtlas(
                Environment.CurrentDirectory + @"\Content\Atlas\actors-atlas",
                out SpriteAtlasDimensions,
                out SpriteAtlasTextureId
                );

            return true;
        }

        public void RenderFrame()
        {
            GL.Clear(ClearBufferMask.ColorBufferBit);
        }
    }
}
