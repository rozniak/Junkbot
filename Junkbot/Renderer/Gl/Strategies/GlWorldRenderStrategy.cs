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


        private Vector2 SpriteAtlasDimensions;

        private Dictionary<string, Rectanglei> SpriteAtlasMap;

        private int SpriteAtlasTextureId;
        

        public void Dispose()
        {
            GL.DeleteTexture(SpriteAtlasTextureId);
        }

        public bool Initialize(JunkbotGame gameReference)
        {
            Game = gameReference;

            if (!File.Exists(Environment.CurrentDirectory + @"\Content\Atlas\actors-atlas.json") ||
                !File.Exists(Environment.CurrentDirectory + @"\Content\Atlas\actors-atlas.png"))
            {
                Console.WriteLine("Atlas is missing!");
                return false;
            }

            var atlasBmp = (Bitmap)Image.FromFile(Environment.CurrentDirectory + @"\Content\Atlas\actors-atlas.png");
            var atlasJson = File.ReadAllText(Environment.CurrentDirectory + @"\Content\Atlas\actors-atlas.json");
            var atlasNodeArray = JArray.Parse(atlasJson);

            SpriteAtlasMap = new Dictionary<string, Rectanglei>();
            SpriteAtlasDimensions = new Vector2(atlasBmp.Width, atlasBmp.Height);

            foreach (JToken token in atlasNodeArray)
            {
                string key = token.Value<string>("Name");
                string boundsCsv = token.Value<string>("Bounds");
                List<int> rectangleComponents = new List<int>();

                foreach (string boundComponent in boundsCsv.Split(','))
                {
                    rectangleComponents.Add(Convert.ToInt32(boundComponent));
                }

                SpriteAtlasMap.Add(
                    key,
                    new Rectanglei(rectangleComponents[0], rectangleComponents[1], rectangleComponents[2], rectangleComponents[3])
                    );
            }

            // Load the bitmap into OpenGL
            //
            SpriteAtlasTextureId = GlUtil.LoadBitmapTexture(atlasBmp);

            // Dispose the atlas resource
            //
            atlasBmp.Dispose();

            // Set up GL props
            //

            return true;
        }

        public void RenderFrame()
        {
            GL.Clear(ClearBufferMask.ColorBufferBit);
        }
    }
}
