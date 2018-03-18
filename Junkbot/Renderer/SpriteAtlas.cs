using Pencil.Gaming.Graphics;
using Pencil.Gaming.MathUtils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Junkbot.Renderer
{
    internal class SpriteAtlas : IDisposable
    {
        public int GlTextureId { get; private set; }

        public Vector2 Size { get; private set; }


        private Dictionary<string, Rectanglei> AtlasMap;


        public SpriteAtlas(Vector2 size, int glTextureId, Dictionary<string, Rectanglei> map)
        {
            AtlasMap = map;
            GlTextureId = glTextureId;
            Size = size;
        }


        public void Dispose()
        {
            GL.DeleteTexture(GlTextureId);
        }

        public Rectanglei GetSpriteUV(string spriteName)
        {
            return AtlasMap[spriteName];
        }
    }
}
