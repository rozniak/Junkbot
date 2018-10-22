using Newtonsoft.Json.Linq;
using Pencil.Gaming.Graphics;
using Pencil.Gaming.MathUtils;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

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
        private GlSpriteAtlas(Vector2 size, int glTextureId, Dictionary<string, Rectanglei> map)
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


        /// <summary>
        /// Loads a sprite atlas from supplied files on disk.
        /// </summary>
        /// <param name="pathNoExt">
        /// The full path and filename of the atlas bitmap and UV mapping JSON
        /// document, any extension will be ignored.
        /// </param>
        /// <returns>
        /// A <see cref="GlSpriteAtlas"/> created fromthe supplied data.
        /// </returns>
        public static GlSpriteAtlas FromFileSet(string pathNoExt)
        {
            // Read texture atlas information and bitmap data
            //
            string atlasPath = Path.GetDirectoryName(pathNoExt);
            string atlasNoExt = Path.GetFileNameWithoutExtension(pathNoExt);

            var atlasBmp = (Bitmap)Image.FromFile(atlasPath + @"\" + atlasNoExt + ".png");
            var atlasJson = File.ReadAllText(atlasPath + @"\" + atlasNoExt + ".json");
            var atlasNodeArray = JArray.Parse(atlasJson);

            var atlasMap = new Dictionary<string, Rectanglei>();

            foreach (JToken token in atlasNodeArray)
            {
                string key = token.Value<string>("Name").ToLower();
                string boundsCsv = token.Value<string>("Bounds");
                var rectangleComponents = new List<int>();

                foreach (string boundComponent in boundsCsv.Split(','))
                {
                    rectangleComponents.Add(Convert.ToInt32(boundComponent));
                }

                atlasMap.Add(
                    key,
                    new Rectanglei(
                        rectangleComponents[0],
                        rectangleComponents[1],
                        rectangleComponents[2],
                        rectangleComponents[3]
                        )
                    );
            }

            // Read out atlas dimensions
            //
            Vector2 atlasDimensions = new Vector2(
                atlasBmp.Width,
                atlasBmp.Height
                );

            // Load the bitmap into OpenGL
            //
            int glTextureId = GlUtil.LoadBitmapTexture(atlasBmp);

            // Dispose the atlas resource
            //
            atlasBmp.Dispose();

            return new GlSpriteAtlas(atlasDimensions, glTextureId, atlasMap);
        }
    }
}
