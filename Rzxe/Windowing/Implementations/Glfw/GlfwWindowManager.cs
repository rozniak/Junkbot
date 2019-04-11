using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Oddmatics.Rzxe.Game;
using Oddmatics.Rzxe.Input;

namespace Oddmatics.Rzxe.Windowing.Implementations.Glfw
{
    internal class GlfwWindowManager : IWindowManager
    {
        public bool IsOpen { get; private set; }

        public bool Ready { get; private set; }

        private IGameEngine _RenderedGameEngine;
        public IGameEngine RenderedGameEngine
        {
            get { return _RenderedGameEngine; }
            set
            {
                if (Locked)
                {
                    throw new InvalidOperationException(
                        "Window manager state has been locked."
                        );
                }
                else
                {
                    _RenderedGameEngine = value;
                }
            }
        }

        
        private bool Disposing { get; set; }

        private bool Locked { get; set; }


        public void Dispose()
        {
            if (Disposing)
            {
                throw new ObjectDisposedException(
                    "The window manager has already been disposed."
                    );
            }

            //
            // TODO: Handle any disposing necessary
            //
        }

        public void Initialize()
        {
            //
            // TODO: Handle any initialization necessary
            //

            Locked = true;
        }

        public InputEvents ReadInputEvents()
        {
            throw new NotImplementedException();
        }

        public void RenderFrame()
        {
            throw new NotImplementedException();
        }
    }
}
