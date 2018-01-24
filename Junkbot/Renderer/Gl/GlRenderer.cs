using Junkbot.Game;
using Pencil.Gaming;
using Pencil.Gaming.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Junkbot.Renderer.Gl
{
    internal class GlRenderer : IRenderer
    {
        public bool IsOpen { get; private set; }


        private JunkbotGame Game;
        private int GlVaoId;
        private GlfwWindowPtr WindowPtr;


        public void RenderFrame()
        {
            if (Glfw.WindowShouldClose(WindowPtr))
            {
                Stop();
                return;
            }

            GL.Clear(ClearBufferMask.ColorBufferBit);

            Glfw.SwapBuffers(WindowPtr);
            Glfw.PollEvents();
        }

        public void Start(JunkbotGame gameInstance)
        {
            Game = gameInstance;

            // Setup GLFW parameters and create window
            //
            Glfw.Init();

            Glfw.SetErrorCallback(OnError);

            Glfw.WindowHint(WindowHint.ContextVersionMajor, 3);
            Glfw.WindowHint(WindowHint.ContextVersionMinor, 2);
            Glfw.WindowHint(WindowHint.OpenGLForwardCompat, 1);
            Glfw.WindowHint(WindowHint.OpenGLProfile, (int)OpenGLProfile.Core);

            WindowPtr = Glfw.CreateWindow(1366, 768, "Junkbot (OpenGL 3.2)", GlfwMonitorPtr.Null, GlfwWindowPtr.Null);

            IsOpen = true;

            Glfw.MakeContextCurrent(WindowPtr);

            // Set GL pixel storage parameter
            //
            GL.PixelStore(PixelStoreParameter.UnpackAlignment, 1);

            // Set up VAO
            //
            GlVaoId = GL.GenVertexArray();
            GL.BindVertexArray(GlVaoId);

            GL.ClearColor(0.0f, 0.0f, 0.0f, 1.0f);
        }
        
        public void Stop()
        {
            IsOpen = false;
            Glfw.Terminate();
        }


        private void OnError(GlfwError code, string desc)
        {
            Console.WriteLine(desc);
        }
    }
}
