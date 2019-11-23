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

namespace Oddmatics.Rzxe.Windowing.Implementations.GlfwFx
{
    internal sealed class GLSpriteAtlas : IDisposable
    {
        public int GlTextureId { get; private set; }

        public string Name { get; private set; }

        public Vector2 Size { get; private set; }


        private Dictionary<string, Rectanglei> SpriteMap { get; set; }

        private bool Disposing { get; set; }


        private GLSpriteAtlas(
            string name,
            Vector2 size,
            int glTextureId,
            Dictionary<string, Rectanglei> map
            )
        {
            Name = name;
            Size = size;
            GlTextureId = glTextureId;
            SpriteMap = map;
        }


        public void Dispose()
        {
            if (Disposing)
            {
                throw new ObjectDisposedException(Name);
            }

            Disposing = true;

            GL.DeleteTexture(GlTextureId);
        }

        public Rectanglei GetSpriteUV(string spriteName)
        {
            return SpriteMap[spriteName];
        }


        internal static GLSpriteAtlas FromFileSet(string pathNoExt)
        {
            // Read texture atlas information and bitmap data
            //
            string atlasPath = Path.GetDirectoryName(pathNoExt);
            string atlasNoExt = Path.GetFileNameWithoutExtension(pathNoExt);

            string atlasBmpSrc  = atlasPath + Path.DirectorySeparatorChar +
                                  atlasNoExt + ".png";
            string atlasJsonSrc = atlasPath + Path.DirectorySeparatorChar +
                                  atlasNoExt + ".json";

            var atlasBmp = (Bitmap)Image.FromFile(atlasBmpSrc);
            var atlasJson = File.ReadAllText(atlasJsonSrc);
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
            int glTextureId = GLUtility.LoadBitmapTexture(atlasBmp);

            // Dispose the atlas resource bitmap
            //
            atlasBmp.Dispose();
            
            return new GLSpriteAtlas(
                atlasNoExt,
                atlasDimensions,
                glTextureId,
                atlasMap
                );
        }
    }
}
