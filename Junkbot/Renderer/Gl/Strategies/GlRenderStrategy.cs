using Junkbot.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Junkbot.Renderer.Gl.Strategies
{
    internal abstract class GlRenderStrategy : IRenderStrategy
    {
        public GlResourceCache Resources
        {
            get { return _Resources; }

            set
            {
                if (_Resources != null)
                    throw new InvalidOperationException("GlRenderStrategy: Attempt to set resource cache after one has already been set.");

                _Resources = value;
            }
        }
        private GlResourceCache _Resources;


        public abstract void Dispose();

        public abstract bool Initialize(JunkbotGame gameReference);

        public abstract void RenderFrame();
    }
}
