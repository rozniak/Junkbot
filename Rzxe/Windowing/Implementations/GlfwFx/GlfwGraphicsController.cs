using Oddmatics.Rzxe.Windowing.Graphics;
using Pencil.Gaming.Graphics;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oddmatics.Rzxe.Windowing.Implementations.GlfwFx
{
    internal sealed class GlfwGraphicsController : IGraphicsController
    {
        private GlfwResourceCache ResourceCache { get; set; }


        public GlfwGraphicsController(GlfwResourceCache resourceCache)
        {
            ResourceCache = resourceCache;
        }


        public void ClearViewport(Color color)
        {
            GL.ClearColor(
                (float) color.R / 255,
                (float) color.G / 255,
                (float) color.B / 255,
                1.0f
                );

            GL.Clear(ClearBufferMask.ColorBufferBit);
        }

        public ISpriteBatch CreateSpriteBatch(string atlasName)
        {
            return new GlfwSpriteBatch(
                atlasName,
                ResourceCache
                );
        }
    }
}
