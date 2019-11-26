using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Oddmatics.Rzxe.Game;
using Oddmatics.Rzxe.Input;
using Pencil.Gaming;
using Pencil.Gaming.Graphics;

namespace Oddmatics.Rzxe.Windowing.Implementations.GlfwFx
{
    internal sealed class GlfwWindowManager : IWindowManager
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


        private InputEvents CurrentInputState { get; set; }
        
        private bool Disposing { get; set; }

        private bool Locked { get; set; }


        #region GLFW Bits and Bobs

        private int GlVaoId { get; set; }

        private GLResourceCache ResourceCache { get; set; }

        private GlfwWindowPtr WindowPtr { get; set; }

        #endregion


        public void Dispose()
        {
            if (Disposing)
            {
                throw new ObjectDisposedException(
                    "The window manager has already been disposed."
                    );
            }

            Glfw.Terminate();
            IsOpen = false;
        }

        public void Initialize()
        {
            CurrentInputState = new InputEvents();
            ResourceCache = new GLResourceCache(RenderedGameEngine.Parameters);

            // Set up GLFW parameters and create the window
            //
            Glfw.Init();

            Glfw.SetErrorCallback(OnError);

            Glfw.WindowHint(WindowHint.ContextVersionMajor, 3);
            Glfw.WindowHint(WindowHint.ContextVersionMinor, 2);
            Glfw.WindowHint(WindowHint.OpenGLForwardCompat, 1);
            Glfw.WindowHint(WindowHint.OpenGLProfile, (int)OpenGLProfile.Core);

            WindowPtr = Glfw.CreateWindow(
                RenderedGameEngine.Parameters.DefaultClientWindowSize.Width,
                RenderedGameEngine.Parameters.DefaultClientWindowSize.Height,
                "Junkbot (OpenGL 3.2)",
                GlfwMonitorPtr.Null,
                GlfwWindowPtr.Null
                );

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
            GL.Viewport(
                0,
                0,
                RenderedGameEngine.Parameters.DefaultClientWindowSize.Width,
                RenderedGameEngine.Parameters.DefaultClientWindowSize.Height
                );

            // Set up input callbacks
            //
            Glfw.SetCharCallback(WindowPtr, OnChar);
            Glfw.SetCursorPosCallback(WindowPtr, OnCursorPos);
            Glfw.SetKeyCallback(WindowPtr, OnKey);
            Glfw.SetMouseButtonCallback(WindowPtr, OnMouseButton);
            Glfw.SetWindowSizeCallback(WindowPtr, OnWindowSize);

            Locked = true;
            Ready = true;
        }
        
        public InputEvents ReadInputEvents()
        {
            var lastUpdate = CurrentInputState;

            lastUpdate.FinalizeForReporting();

            CurrentInputState = new InputEvents(
                lastUpdate.DownedInputs,
                lastUpdate.MousePosition
                );

            return lastUpdate;
        }

        public void RenderFrame()
        {
            if (Glfw.WindowShouldClose(WindowPtr))
            {
                Dispose();
                return;
            }
            
            RenderedGameEngine.RenderFrame(
                new GLGraphicsController(
                    ResourceCache,
                    RenderedGameEngine.Parameters.DefaultClientWindowSize
                    )
                );

            Glfw.SwapBuffers(WindowPtr);
            Glfw.PollEvents();
        }


        #region GLFW Callbacks

        private void OnChar(GlfwWindowPtr wnd, char ch)
        {
            CurrentInputState.ReportConsoleInput(ch);
        }

        private void OnCursorPos(GlfwWindowPtr wnd, double x, double y)
        {
            CurrentInputState.ReportMouseMovement(
                (float) x, (float) y
                );
        }

        private void OnError(GlfwError code, string desc)
        {
            Console.WriteLine(desc);
        }

        private void OnKey(GlfwWindowPtr wnd, Key key, int scanCode, KeyAction action, KeyModifiers mods)
        {
            string inputString = "vk." + key.ToString();

            switch (action)
            {
                case KeyAction.Press:
                    CurrentInputState.ReportPress(inputString);
                    break;

                case KeyAction.Release:
                    CurrentInputState.ReportRelease(inputString);
                    break;
            }
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

            switch (action)
            {
                case KeyAction.Press:
                    CurrentInputState.ReportPress(inputString);
                    break;

                case KeyAction.Release:
                    CurrentInputState.ReportRelease(inputString);
                    break;
            }
        }

        private void OnWindowSize(GlfwWindowPtr wnd, int width, int height)
        {
            GL.Viewport(0, 0, width, height);
        }

        #endregion
    }
}
