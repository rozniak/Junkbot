using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Junkbot.Game.World.Actors.Animation
{
    internal class AnimationStore
    {
        private Dictionary<string, IList<ActorAnimationFrame>> Framesets;


        public AnimationStore()
        {
            Framesets = new Dictionary<string, IList<ActorAnimationFrame>>();

            // Add animation filepaths here
            //
            var anims = new string[]
            {
                Environment.CurrentDirectory + @"\Content\Animations\junkbot.json",
                Environment.CurrentDirectory + @"\Content\Animations\legoparts.json"
            };

            foreach (string animPath in anims)
            {
                LoadAnimationDefinitions(animPath);
            }
        }


        public ActorAnimation GetAnimation(string animName)
        {
            return new ActorAnimation(animName, Framesets[animName]);
        }


        private void LoadAnimationDefinitions(string filename)
        {
            string animJson = File.ReadAllText(filename);
            var framesets = JArray.Parse(animJson);

            foreach (JToken framesetDef in framesets)
            {
                var animName = framesetDef.Value<string>("name");
                var frameList = new List<ActorAnimationFrame>();
                var frames = (JArray)framesetDef.SelectToken("frames");

                foreach (JToken frameDef in frames)
                {
                    var offset = new Point(
                        frameDef["offset"].Value<int>("x"),
                        frameDef["offset"].Value<int>("y")
                        );
                    bool shouldEmitEvent = frameDef["emit_event"] != null && frameDef.Value<bool>("emit_event");
                    var spriteName = frameDef.Value<string>("sprite");
                    var ticks = frameDef.Value<byte>("ticks");

                    frameList.Add(new ActorAnimationFrame(shouldEmitEvent, offset, spriteName, ticks));
                }

                if (frameList.Count > 0) // Do not add empty animations
                    Framesets.Add(animName, frameList.AsReadOnly());
            }
        }
    }
}
