using Oddmatics.Rzxe.Input;
using Oddmatics.Rzxe.Windowing.Graphics;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Oddmatics.Rzxe.Game
{
    public abstract class GameEngine
    {
        public abstract IGameEngineParameters Parameters { get; }

        
        private List<GameState> InputWatchingStates { get; set; }

        private List<GameState> StateStack { get; set; }
        

        public virtual void Begin()
        {
            InputWatchingStates = new List<GameState>();
            StateStack = new List<GameState>();
        }

        public void PopState()
        {
            if (StateStack.Count == 0)
            {
                throw new InvalidOperationException(
                    "No states to pop."
                    );
            }

            int stackTop    = StateStack.Count - 1;
            GameState state = StateStack[stackTop];

            StateStack.RemoveAt(stackTop);

            if (state.FocalMode == InputFocalMode.Always)
            {
                InputWatchingStates.Remove(state);
            }
        }

        public void PushState(GameState state)
        {
            if (StateStack.Contains(state))
            {
                throw new InvalidOperationException(
                    "Can't push state as it is already present."
                    );
            }

            StateStack.Add(state);

            if (state.FocalMode == InputFocalMode.Always)
            {
                InputWatchingStates.Add(state);
            }
        }

        public virtual void RenderFrame(IGraphicsController graphics)
        {
            // Default - just clear with default colour
            //
            graphics.ClearViewport(Color.CornflowerBlue);

            // Render all states, top to bottom
            //
            for (int i = StateStack.Count - 1; i >= 0; i--)
            {
                StateStack[i].RenderFrame(graphics);
            }
        }

        public virtual void Update(TimeSpan deltaTime, InputEvents inputs)
        {
            int stackTop = StateStack.Count - 1;

            for (int i = stackTop; i >= 0; i--)
            {
                GameState state = StateStack[i];

                if (
                    state.FocalMode == InputFocalMode.Always ||
                    (i == stackTop && state.FocalMode == InputFocalMode.WhenCurrentStateOnly)
                )
                {
                    state.Update(deltaTime, inputs);
                }
                else
                {
                    state.Update(deltaTime, null);
                }
            }
        }
    }
}
