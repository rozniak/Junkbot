using Junkbot.Game;
using Junkbot.Game.Input;
using Junkbot.Game.State;
using Junkbot.Renderer.Gl.Strategies;
using Pencil.Gaming;
using Pencil.Gaming.Graphics;
using Pencil.Gaming.MathUtils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Junkbot.Renderer.Gl
{
    internal class GlRenderer : IRenderer
    {
        public static readonly Vector2 JUNKBOT_VIEWPORT = new Vector2(650, 420);


        public bool IsOpen { get; private set; }


        private List<GlRenderStrategy> ActiveRenderStrategies;
        private InputEvents CurrentInputState;
        private JunkbotGame Game;
        private int GlVaoId;
        private GlResourceCache ResourceCache;
        private GlfwWindowPtr WindowPtr;


        public GlRenderer()
        {
            ActiveRenderStrategies = new List<GlRenderStrategy>();
            ResourceCache = new GlResourceCache();
        }


        public void Dispose()
        {
            if (IsOpen)
                throw new InvalidOperationException("GlRenderer: Cannot dispose renderer whilst it is running.");

            for (int i = ActiveRenderStrategies.Count - 1; i >= 0; i--)
            {
                ActiveRenderStrategies[i].Dispose();
                ActiveRenderStrategies.RemoveAt(i);
            }

            ResourceCache.Dispose();

            Glfw.Terminate();
        }

        public InputEvents GetInputEvents()
        {
            var thisUpdate = CurrentInputState;

            thisUpdate.FinalizeForReporting();

            CurrentInputState = new InputEvents(thisUpdate.DownedInputs, thisUpdate.MousePosition);

            return thisUpdate;
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
            CurrentInputState = new InputEvents();

            // Setup GLFW parameters and create window
            //
            Glfw.Init();

            Glfw.SetErrorCallback(OnError);

            Glfw.WindowHint(WindowHint.ContextVersionMajor, 3);
            Glfw.WindowHint(WindowHint.ContextVersionMinor, 2);
            Glfw.WindowHint(WindowHint.OpenGLForwardCompat, 1);
            Glfw.WindowHint(WindowHint.OpenGLProfile, (int)OpenGLProfile.Core);

            WindowPtr = Glfw.CreateWindow(650, 420, "Junkbot (OpenGL 3.2)", GlfwMonitorPtr.Null, GlfwWindowPtr.Null);

            IsOpen = true;

            Glfw.MakeContextCurrent(WindowPtr);

            // Set GL pixel storage parameter
            //
            GL.PixelStore(PixelStoreParameter.UnpackAlignment, 1);

            // Set up VAO
            //
            GlVaoId = GL.GenVertexArray();
            GL.BindVertexArray(GlVaoId);

            // Set up enabled features
            //
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
            
            // Set up viewport defaults
            //
            GL.ClearColor(0.39f, 0.58f, 0.93f, 1.0f); // Approx. cornflower blue
            GL.Viewport(0, 0, (int)JUNKBOT_VIEWPORT.X, (int)JUNKBOT_VIEWPORT.Y);

            // Set up input callbacks
            //
            Glfw.SetCharCallback(WindowPtr, OnChar);
            Glfw.SetCursorPosCallback(WindowPtr, OnCursorPos);
            Glfw.SetKeyCallback(WindowPtr, OnKey);
            Glfw.SetMouseButtonCallback(WindowPtr, OnMouseButton);
            Glfw.SetWindowSizeCallback(WindowPtr, OnWindowSize);

            // Now attach the game state event to update our strategies
            //
            Game.ChangeState += Game_ChangeState;
        }

        public void Stop()
        {
            IsOpen = false;
            Dispose();
        }
        

        private void Game_ChangeState(object sender, EventArgs e)
        {
            var game = (JunkbotGame)sender;

            switch (game.GameState.Identifier)
            {
                case JunkbotGameState.Menu:
                    var menuStrategy = new GlMenuRenderStrategy(ResourceCache);

                    menuStrategy.Resources = ResourceCache;
                    menuStrategy.Initialize(game);

                    ActiveRenderStrategies.Add(menuStrategy);

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

        private void OnChar(GlfwWindowPtr wnd, char ch)
        {
            CurrentInputState.ReportConsoleInput(ch);
        }

        private void OnCursorPos(GlfwWindowPtr wnd, double x, double y)
        {
            CurrentInputState.ReportMouseMovement((float) x, (float) y);
        }

        private void OnError(GlfwError code, string desc)
        {
            Console.WriteLine(desc);
        }

        private void OnKey(GlfwWindowPtr wnd, Key key, int scanCode, KeyAction action, KeyModifiers mods)
        {
            string inputString = "vk." + key.ToString();

            if (action == KeyAction.Press)
                CurrentInputState.ReportPress(inputString);
            else if (action == KeyAction.Release)
                CurrentInputState.ReportRelease(inputString);
        }

        private void OnMouseButton(GlfwWindowPtr wnd, MouseButton btn, KeyAction action)
        {
            string inputString = String.Empty;

            switch (btn)
            {
                case MouseButton.LeftButton:
                    inputString = "mb.left";
                    break;

                case MouseButton.MiddleButton:
                    inputString = "mb.middle";
                    break;

                case MouseButton.RightButton:
                    inputString = "mb.right";
                    break;
            }

            if (action == KeyAction.Press)
                CurrentInputState.ReportPress(inputString);
            else if (action == KeyAction.Release)
                CurrentInputState.ReportRelease(inputString);
        }

        private void OnWindowSize(GlfwWindowPtr wnd, int width, int height)
        {
            GL.Viewport(0, 0, width, height);
        }
    }
}
