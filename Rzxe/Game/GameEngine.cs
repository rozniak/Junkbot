using Oddmatics.Rzxe.Input;
using Oddmatics.Rzxe.Windowing.Graphics;
using System;

namespace Oddmatics.Rzxe.Game
{
    public interface IGameEngine
    {
        GameState CurrentGameState { get; set; }

        IGameEngineParameters Parameters { get; }


        void Begin();

        void RenderFrame(IGraphicsController graphics);

        void Update(TimeSpan deltaTime, InputEvents inputs);
    }
}
