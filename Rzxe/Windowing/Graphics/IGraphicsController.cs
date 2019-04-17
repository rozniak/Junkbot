using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Oddmatics.Rzxe.Windowing.Graphics
{
    public interface IGraphicsController
    {
        Size TargetResolution { get; }


        void ClearViewport(Color color);

        ISpriteBatch CreateSpriteBatch(string atlasName);
    }
}
