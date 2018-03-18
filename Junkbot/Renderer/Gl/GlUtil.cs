using Newtonsoft.Json.Linq;
using Pencil.Gaming.Graphics;
using Pencil.Gaming.MathUtils;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Junkbot.Renderer.Gl
{
    internal static class GlUtil
    {
        public static Dictionary<string, Rectanglei> LoadAtlas(string filename, out Vector2i atlasDimensions, out int glTextureId)
        {
            glTextureId = -1; // Set texture ID output to -1 in case this routine fails

            // Read texture atlas information and bitmap data
            //
            string atlasPath = Path.GetFullPath(filename);
            string atlasNoExt = Path.GetFileNameWithoutExtension(filename);

            var atlasBmp = (Bitmap)Image.FromFile(atlasPath + @"\" + atlasNoExt + ".png");
            var atlasJson = File.ReadAllText(atlasPath + @"\" + atlasNoExt + ".json");
            var atlasNodeArray = JArray.Parse(atlasJson);

            var atlasMap = new Dictionary<string, Rectanglei>();
            
            foreach (JToken token in atlasNodeArray)
            {
                string key = token.Value<string>("Name");
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
            atlasDimensions = new Vector2i(
                atlasBmp.Width,
                atlasBmp.Height
                );

            // Load the bitmap into OpenGL
            //
            glTextureId = GlUtil.LoadBitmapTexture(atlasBmp);

            // Dispose the atlas resource
            //
            atlasBmp.Dispose();

            return atlasMap;
        }

        public static int LoadBitmapTexture(Bitmap bmp)
        {
            // Flip texture before processing
            //
            bmp.RotateFlip(RotateFlipType.RotateNoneFlipY);

            // Acquire bitmap raw data handle
            //
            BitmapData data = bmp.LockBits(
                new System.Drawing.Rectangle(Point.Empty, bmp.Size),
                ImageLockMode.ReadOnly,
                System.Drawing.Imaging.PixelFormat.Format32bppArgb
                );

            // Create the OpenGL texture
            //
            int textureId = GL.GenTexture();

            // Bind texture and load bitmap data
            //
            GL.BindTexture(TextureTarget.Texture2D, textureId);

            GL.TexImage2D(
                TextureTarget.Texture2D,
                0,
                PixelInternalFormat.Rgba8,
                bmp.Width,
                bmp.Height,
                0,
                Pencil.Gaming.Graphics.PixelFormat.Bgra,
                PixelType.UnsignedByte,
                data.Scan0
                );

            // Set texture parameters
            //
            GL.TexParameterI(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, new int[] { (int)TextureMagFilter.Linear });
            GL.TexParameterI(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, new int[] { (int)TextureMinFilter.Linear });

            // Unlock bitmap raw data
            //
            bmp.UnlockBits(data);

            return textureId;
        }
    }
}
