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


        private List<GameState> StateStack { get; set; }


        protected void PushState(GameState state)
        {
            if (StateStack.Contains(state))
            {
                throw new InvalidOperationException(
                    "Can't push state as it is already present."
                    );
            }

            StateStack.Add(state);

            //
            // TODO: Handle adding input events based on InputFocalMode
            //
        }

        protected void PopState()
        {
            if (StateStack.Count == 0)
            {
                throw new InvalidOperationException(
                    "No states to pop."
                    );
            }

            StateStack.RemoveAt(StateStack.Count - 1);

            //
            // TODO: Handle removing input events based on InputFocalMode
            //
        }


        public virtual void Begin()
        {
            StateStack = new List<GameState>();
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
            //
            // Nothing yet
            //
        }
    }
}
