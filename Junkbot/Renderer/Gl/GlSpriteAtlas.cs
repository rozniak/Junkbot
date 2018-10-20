using Pencil.Gaming.Graphics;
using Pencil.Gaming.MathUtils;
using System;
using System.Collections.Generic;

namespace Junkbot.Renderer.Gl
{
    /// <summary>
    /// Represents a sprite atlas.
    /// </summary>
    internal sealed class GlSpriteAtlas : IDisposable
    {
        /// <summary>
        /// Gets the ID of the atlas texture in OpenGL.
        /// </summary>
        public int GlTextureId { get; private set; }

        /// <summary>
        /// Gets the size of the atlas texture.
        /// </summary>
        public Vector2 Size { get; private set; }


        /// <summary>
        /// The internal sprite to UV rectangle mappings.
        /// </summary>
        private Dictionary<string, Rectanglei> AtlasMap;


        /// <summary>
        /// Initializes a new instance of the <see cref="GlSpriteAtlas"/> class.
        /// </summary>
        /// <param name="size">The size of the atlas.</param>
        /// <param name="glTextureId">The ID of the atlas texture in OpenGL.</param>
        /// <param name="map">The sprite to UV rectangle mappings.</param>
        public GlSpriteAtlas(Vector2 size, int glTextureId, Dictionary<string, Rectanglei> map)
        {
            AtlasMap = map;
            GlTextureId = glTextureId;
            Size = size;
        }


        /// <summary>
        /// Releases all resources used by this <see cref="GlSpriteAtlas"/>.
        /// </summary>
        public void Dispose()
        {
            GL.DeleteTexture(GlTextureId);
        }

        /// <summary>
        /// Gets the UV rectangle for a sprite on this atlas.
        /// </summary>
        /// <param name="spriteName">The sprite name.</param>
        /// <returns>
        /// The <see cref="Rectanglei"/> that represents the UV blitting information
        /// for the sprite.
        /// </returns>
        public Rectanglei GetSpriteUV(string spriteName)
        {
            return AtlasMap[spriteName];
        }
    }
}
