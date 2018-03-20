using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Junkbot.Game.World.Actors.Animation
{
    internal class AnimationServer
    {
        private ActorAnimation ActiveAnimation;

        private AnimationStore AnimationStore;


        public event EventHandler SpecialFrameEntered
        {
            add { ActiveAnimation.SpecialFrameEntered += value; }
            remove { ActiveAnimation.SpecialFrameEntered -= value; }
        }


        public AnimationServer(AnimationStore store)
        {
            AnimationStore = store;
        }


        public ActorAnimationFrame GetCurrentFrame()
        {
            return ActiveAnimation?.CurrentFrame;
        }

        public void GoToAndPlay(string animName)
        {
            if (animName == ActiveAnimation?.Name)
                ActiveAnimation.Restart();
            else
            {
                ActiveAnimation = AnimationStore.GetAnimation(animName);
                ActiveAnimation.Play();
            }
        }

        public void GoToAndStop(string animName)
        {
            if (animName == ActiveAnimation?.Name)
                ActiveAnimation.Stop();
            else
                ActiveAnimation = AnimationStore.GetAnimation(animName);
        }

        public void Progress()
        {
            ActiveAnimation?.Step();
        }
    }
}
