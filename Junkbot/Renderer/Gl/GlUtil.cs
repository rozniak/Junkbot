using Pencil.Gaming.Graphics;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Junkbot.Renderer.Gl
{
    internal static class GlUtil
    {
        public static int LoadBitmapTexture(Bitmap bmp)
        {
            // Flip texture before processing
            //
            bmp.RotateFlip(RotateFlipType.RotateNoneFlipY);

            // Acquire bitmap raw data handle
            //
            BitmapData data = bmp.LockBits(
                new Rectangle(Point.Empty, bmp.Size),
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
