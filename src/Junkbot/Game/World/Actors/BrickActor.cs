using Junkbot.Game.World.Actors.Animation;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Junkbot.Game.World.Actors
{
    internal class BrickActor : IActor
    {
        public static IList<Color> ValidColors = new List<Color>(new Color[]
        {
            Color.Red, Color.Yellow, Color.White, Color.Green, Color.Blue
        }).AsReadOnly();


        public AnimationServer Animation { get; private set; }

        public IReadOnlyList<Rectangle> BoundingBoxes { get { return this._BoundingBoxes; } }
        private IReadOnlyList<Rectangle> _BoundingBoxes;

        public Color Color
        {
            get { return _Color; }
            set
            {
                _Color = value;
                UpdateBrickAnim();
            }
        }
        private Color _Color;

        public Size GridSize { get { return _GridSize; } }
        private Size _GridSize;

        public bool IsImmobile
        {
            get { return _Color.Name == "Gray"; }
        }

        public Point Location
        {
            get { return _Location; }
            set
            {
                Point oldLocation = _Location;
                _Location = value;

                LocationChanged?.Invoke(this, new LocationChangedEventArgs(oldLocation, value));
            }
        }
        private Point _Location;

        public BrickSize Size
        {
            get { return _Size; }
            set
            {
                _Size = value;
                _BoundingBoxes = new List<Rectangle>(new Rectangle[] {
                    new Rectangle(0, 0, (int)value, 1)
                }).AsReadOnly();
                _GridSize = new Size((int)value, 1);
                UpdateBrickAnim();
            }
        }
        private BrickSize _Size;


        public event LocationChangedEventHandler LocationChanged;


        public BrickActor(AnimationStore store, Point location, Color color, BrickSize size)
        {
            Animation = new AnimationServer(store);
            _BoundingBoxes = new List<Rectangle>().AsReadOnly();
            _Color = color;
            _GridSize = new Size((int)size, 1);
            Location = location;
            _Size = size;

            UpdateBrickAnim();
        }


        public void Update()
        {
            Animation.Progress();
        }


        private void UpdateBrickAnim()
        {
            string brickSize = ((int)Size).ToString();

            if (IsImmobile)
                Animation.GoToAndStop("legopart-brick-immobile-" + brickSize);
            else
            {
                if (!ValidColors.Contains(Color))
                    return;

                Animation.GoToAndStop("legopart-brick-" + Color.Name.ToLower() + "-" + brickSize);
            }
        }
    }
}
