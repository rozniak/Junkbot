using Junkbot.Game;
using Junkbot.Game.Input;
using Junkbot.Game.State;
using Junkbot.Renderer.Gl.Strategies;
using Pencil.Gaming;
using Pencil.Gaming.Graphics;
using Pencil.Gaming.MathUtils;
using System;
using System.Collections.Generic;

namespace Junkbot.Renderer.Gl
{
    /// <summary>
    /// Renders Junkbot using the OpenGL API.
    /// </summary>
    internal sealed class GlRenderer : IRenderer
    {
        /// <summary>
        /// The viewport dimensions of Junkbot.
        /// </summary>
        public static readonly Vector2 JUNKBOT_VIEWPORT = new Vector2(650, 420);


        /// <summary>
        /// Gets the value that indicates whether the renderer window is still open.
        /// </summary>
        public bool IsOpen { get; private set; }


        /// <summary>
        /// The active render strategies.
        /// </summary>
        private List<GlRenderStrategy> ActiveRenderStrategies;

        /// <summary>
        /// The current input events recorded by the renderer window.
        /// </summary>
        private InputEvents CurrentInputState;

        /// <summary>
        /// The Junkbot game engine.
        /// </summary>
        private JunkbotGame Game;

        /// <summary>
        /// The ID of the vertex array object used by this renderer.
        /// </summary>
        private int GlVaoId;

        /// <summary>
        /// The resource cache for OpenGL objects.
        /// </summary>
        private GlResourceCache ResourceCache;

        /// <summary>
        /// The pointer to the active GLFW window.
        /// </summary>
        private GlfwWindowPtr WindowPtr;


        /// <summary>
        /// Initializes a new instance of the <see cref="GlRenderer"/> class.
        /// </summary>
        public GlRenderer()
        {
            ActiveRenderStrategies = new List<GlRenderStrategy>();
            ResourceCache = new GlResourceCache();
        }


        /// <summary>
        /// Releases all resources used by this <see cref="GlRenderer"/>.
        /// </summary>
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

        /// <summary>
        /// Retrieves the input events from the renderer window.
        /// </summary>
        /// <returns>
        /// An <see cref="InputEvents"/> instance containing input data pulled from
        /// the renderer window.
        /// </returns>
        public InputEvents GetInputEvents()
        {
            var thisUpdate = CurrentInputState;

            thisUpdate.FinalizeForReporting();

            CurrentInputState = new InputEvents(thisUpdate.DownedInputs, thisUpdate.MousePosition);

            return thisUpdate;
        }

        /// <summary>
        /// Renders the next frame.
        /// </summary>
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

        /// <summary>
        /// Starts this renderer.
        /// </summary>
        /// <param name="gameInstance">
        /// A reference to the Junkbot game engine instance.
        /// </param>
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
            //Game.ChangeState += Game_ChangeState;
        }

        /// <summary>
        /// Stops this renderer.
        /// </summary>
        public void Stop()
        {
            IsOpen = false;
            Dispose();
        }
        

        /// <summary>
        /// (Event) Occurs when the Junkbot game engine changes state.
        /// </summary>
        private void Game_ChangeState(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// (Callback) Handles character inputs from the GLFW window.
        /// </summary>
        /// <param name="wnd">A pointer to the GLFW window.</param>
        /// <param name="ch">The character inputted.</param>
        private void OnChar(GlfwWindowPtr wnd, char ch)
        {
            CurrentInputState.ReportConsoleInput(ch);
        }

        /// <summary>
        /// (Callback) Handles cursor movement inputs from the GLFW window.
        /// </summary>
        /// <param name="wnd">A pointer to the GLFW window.</param>
        /// <param name="x">The x-coordiante of the cursor.</param>
        /// <param name="y">The y-coordinate of the cursor.</param>
        private void OnCursorPos(GlfwWindowPtr wnd, double x, double y)
        {
            CurrentInputState.ReportMouseMovement((float) x, (float) y);
        }

        /// <summary>
        /// (Callback) Handles errors that occur within the GLFW library.
        /// </summary>
        /// <param name="code">The error code.</param>
        /// <param name="desc">A description of the error that occurred.</param>
        private void OnError(GlfwError code, string desc)
        {
            Console.WriteLine(desc);
        }

        /// <summary>
        /// (Callback) Handles keyboard inputs from the GLFW window. 
        /// </summary>
        /// <param name="wnd">A pointer to the GLFW window.</param>
        /// <param name="key">The key that has had its state changed.</param>
        /// <param name="scanCode">The scancode of the key.</param>
        /// <param name="action">The action that occurred.</param>
        /// <param name="mods">Modifier keys pressed at the same time.</param>
        private void OnKey(GlfwWindowPtr wnd, Key key, int scanCode, KeyAction action, KeyModifiers mods)
        {
            string inputString = "vk." + key.ToString();

            if (action == KeyAction.Press)
                CurrentInputState.ReportPress(inputString);
            else if (action == KeyAction.Release)
                CurrentInputState.ReportRelease(inputString);
        }

        /// <summary>
        /// (Callback) Handles mouse inputs from the GLFW window.
        /// </summary>
        /// <param name="wnd">A pointer to the GLFW window.</param>
        /// <param name="btn">The mouse button that has had its state changed.</param>
        /// <param name="action">The action that occurred.</param>
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

        /// <summary>
        /// (Callback) Handles window resize events that occurred in the GLFW window.
        /// </summary>
        /// <param name="wnd">A pointer to the GLFW window.</param>
        /// <param name="width">The width of the window in pixels.</param>
        /// <param name="height">The height of the window in pixels.</param>
        private void OnWindowSize(GlfwWindowPtr wnd, int width, int height)
        {
            GL.Viewport(0, 0, width, height);
        }
    }
}
