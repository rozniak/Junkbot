using Junkbot.Game;
using Junkbot.Game.State;
using Junkbot.Renderer.Gl.Strategies;
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
        private List<IRenderStrategy> ActiveRenderStrategies;
        private GlfwWindowPtr WindowPtr;


        public GlRenderer()
        {
            ActiveRenderStrategies = new List<IRenderStrategy>();
        }


        public void RenderFrame()
        {
            if (Glfw.WindowShouldClose(WindowPtr))
            {
                Stop();
                return;
            }

            GL.Clear(ClearBufferMask.ColorBufferBit);

            foreach (IRenderStrategy strategy in ActiveRenderStrategies)
            {
                strategy.RenderFrame();
            }

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
            
            GL.ClearColor(0.39f, 0.58f, 0.93f, 1.0f); // Approx. cornflower blue

            // Now attach the game state event to update our strategies
            //
            Game.ChangeState += Game_ChangeState;
        }

        public void Stop()
        {
            ActiveRenderStrategies.Clear();
            IsOpen = false;
            Glfw.Terminate();
        }


        private void Game_ChangeState(object sender, EventArgs e)
        {
            var game = (JunkbotGame)sender;

            switch (game.GameState.Identifier)
            {
                case JunkbotGameState.Menu:

                    break;

                case JunkbotGameState.Nothing:
                    ActiveRenderStrategies.Clear();
                    break;

                case JunkbotGameState.World:
                    var worldStrategy = new GlWorldRenderStrategy();

                    worldStrategy.Initialize(game);

                    ActiveRenderStrategies.Add(worldStrategy);

                    break;
            }
        }

        private void OnError(GlfwError code, string desc)
        {
            Console.WriteLine(desc);
        }
    }
}
