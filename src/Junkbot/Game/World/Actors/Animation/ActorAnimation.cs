using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Junkbot.Game.World.Actors.Animation
{
    internal class ActorAnimation
    {
        public ActorAnimationFrame CurrentFrame
        {
            get { return Frameset[CurrentFrameIndex]; }
        }

        public bool IsPlaying { get; private set; }

        public string Name { get; private set; }


        private int CurrentFrameIndex;

        private IList<ActorAnimationFrame> Frameset;

        private byte TickCount;


        public event EventHandler FinishedPlayback;

        public event EventHandler SpecialFrameEntered;


        public ActorAnimation(string animName, IList<ActorAnimationFrame> frameset)
        {
            CurrentFrameIndex = 0;
            Frameset = frameset;
            IsPlaying = false;
            Name = animName;
            TickCount = 0;
        }


        public void Pause()
        {
            IsPlaying = false;
        }

        public void Play()
        {
            IsPlaying = true;
        }

        public void Restart()
        {
            CurrentFrameIndex = 0;
            IsPlaying = true;
            TickCount = 0;
        }

        public void Step()
        {
            if (!IsPlaying)
                return;

            TickCount++;

            if (TickCount > CurrentFrame.Ticks)
            {
                if (CurrentFrameIndex + 1 >= Frameset.Count)
                    CurrentFrameIndex = 0;
                else
                    ++CurrentFrameIndex;

                TickCount = 0;

                // Check if the next frame should emit an event
                //
                if (CurrentFrame.ShouldEmitEvent)
                    SpecialFrameEntered?.Invoke(this, EventArgs.Empty);
            }
        }

        public void Stop()
        {
            CurrentFrameIndex = 0;
            IsPlaying = false;
            TickCount = 0;
        }
    }
}
