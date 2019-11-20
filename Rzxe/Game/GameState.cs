using Oddmatics.Rzxe.Input;
using Oddmatics.Rzxe.Windowing.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Oddmatics.Rzxe.Game
{
    public abstract class GameState : IEquatable<GameState>
    {
        public abstract InputFocalMode FocalMode { get; }

        public abstract string Name { get; }


        public bool Equals(GameState other)
        {
            return this.Name == other.Name;
        }


        public abstract void RenderFrame(IGraphicsController graphics);

        public abstract void Update(TimeSpan deltaTime, InputEvents inputs);
    }
}
