using Oddmatics.Rzxe.Windowing.Graphics;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oddmatics.Rzxe.Windowing.Implementations.GlfwFx
{
    internal sealed class GlfwSpriteBatch : ISpriteBatch
    {



        public GlfwSpriteBatch(
            string atlasName,
            GlfwResourceCache resourceCache
            )
        {

        }


        public void Draw(string spriteName, Rectangle rect)
        {
            
        }

        public void Finish()
        {
            throw new NotImplementedException();
        }
    }
}
