//
// TODO:
//     Not sure if this class is needed... decals could simply be rendered as part of
//     the level within Scene.cs.
//
//     Don't *think* we need an entire actor for decals seeing as they're just
//     background stuff and logically are not actors.
//


using Oddmatics.Rzxe.Game.Actors.Animation;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Junkbot.Game.World.Actors
{
    internal class DecalActor
    {
        public static IList<string> ValidDecals = new List<string>(new string[]
        {
            "arrowd", "arrowl", "arrowld", "arrowlu", "arrowr", "arrowrd", "arrowru", "arrowu", "arrowul", "arrowur", "computer_matrix", "computer2dial", "door",
            "fusebox", "fusebox_pipes_d", "fusebox_pipes_l", "fusebox_pipes_r", "lite_number_0", "lite_number_1", "lite_number_2", "lite_number_3", "lite_number_4",
            "lite_number_5", "lite_number_6", "lite_number_7", "lite_number_8", "lite_number_9", "lite_word_level", "number_0", "number_1", "number_2",
            "number_3", "number_4", "number_5", "number_6", "number_7", "number_8", "number_9", "number_dash", "safetystrip_horiz", "safetystrip_vert", "sign_acid",
            "sign_keepout", "sign_no_access", "sign_skull", "sign_voltage", "terminal_1dial", "terminal_chart", "terminal_circuit", "window", "word_danger", "word_level"
        }).AsReadOnly();
//All valid decal strings, as read directly from the level file.
        public AnimationServer Animation { get; private set; }
        public string Decal
        {
            get { return _Decal; }
            set
            {
                _Decal = value;
                UpdateDecalAnim();
            }
        }
        private string _Decal;

        public Point Location
        {
            get { return _Location; }
            set
            {
                Point oldLocation = _Location;
                _Location = value;
            }
        }
        private Point _Location;

        

        public DecalActor(AnimationStore store, Point location)
        {
            Animation = new AnimationServer(store);
            Location = location;
        }


        public void Update(
            TimeSpan deltaTime
        )
        {
            Animation.Progress(deltaTime);
        }


        private void UpdateDecalAnim()
        {
        //Here, we have a very long switch sequence that determines what we want to update the decal to render.
        //I haven't figured out how decals will be read into the game and rendered, but I'm sticking with the names used in the level files for ease of use.
        }
    }
}

