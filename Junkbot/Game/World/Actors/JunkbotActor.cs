using Pencil.Gaming.MathUtils;
using Junkbot.Game.World.Actors.Animation;
using Junkbot.Helpers;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Junkbot.Game.World.Actors
{
    internal class JunkbotActor : IThinker
    {
        public AnimationServer Animation { get; private set; }

        public IReadOnlyList<System.Drawing.Rectangle> BoundingBoxes { get { return this._BoundingBoxes; } }
        private IReadOnlyList<System.Drawing.Rectangle> _BoundingBoxes = new List<System.Drawing.Rectangle>(new System.Drawing.Rectangle[]
        {
            new System.Drawing.Rectangle(0, 0, 2, 3),
            new System.Drawing.Rectangle(0, 3, 1, 1)
        }).AsReadOnly();

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

        public Size GridSize { get { return _GridSize; } }
        private static readonly Size _GridSize = new Size(2, 4);


        private FacingDirection FacingDirection;

        private Scene Scene;


        public event LocationChangedEventHandler LocationChanged;


        public JunkbotActor(AnimationStore store, Scene scene, Point location, FacingDirection initialDirection)
        {
            Animation = new AnimationServer(store);
            Location = location;
            SetWalkingDirection(initialDirection);
            Scene = scene;
        }


        public void Update()
        {
            Animation.Progress();
        }


        private void SetWalkingDirection(FacingDirection direction)
        {
            FacingDirection = direction;

            // Detach event if necessary
            //
            try
            {
                Animation.SpecialFrameEntered -= Animation_SpecialFrameEntered;
            }
            catch (Exception ex) { }

            switch (direction)
            {
                case FacingDirection.Left:
                    Animation.GoToAndPlay("junkbot-walk-left");
                    break;

                case FacingDirection.Right:
                    Animation.GoToAndPlay("junkbot-walk-right");
                    break;

                default:
                    throw new Exception("JunkbotActor.SetWalkingDirection: Invalid direction provided.");
            }

            Animation.SpecialFrameEntered += Animation_SpecialFrameEntered;
        }


        private void Animation_SpecialFrameEntered(object sender, EventArgs e)
        {
            // Each tile is 15x18
            int dx = FacingDirection == FacingDirection.Left ? -1 : 1;

            // Check if we should turn around now
            //
            System.Drawing.Rectangle checkBounds = new System.Drawing.Rectangle(
                Location.Add(new Point(dx * GridSize.Width, 0)),
                new Size(1, 3)
                );

            if (!Scene.CheckGridRegionFree(checkBounds))
            {
                Location = Location.Add(new Point(dx, 0));

                SetWalkingDirection(FacingDirection == FacingDirection.Left ? FacingDirection.Right : FacingDirection.Left);
                return;
            }

            // Space is free, now check whether we need an elevation change, prioritize upwards changes
            //
            System.Drawing.Rectangle floorUpCheckBounds = new System.Drawing.Rectangle(
                Location.Add(new Point(dx, GridSize.Height - 1)),
                new Size(1, 1)
                );

            if (!Scene.CheckGridRegionFree(floorUpCheckBounds))
            {
                // Elevate up
                //
                Location = Location.Add(new Point(dx, -1));
                return;
            }

            // Now check downwards
            //
            System.Drawing.Rectangle floorMissingCheckBounds = new System.Drawing.Rectangle(
                Location.Add(new Point(dx, GridSize.Height)),
                new Size(1, 1)
                );

            System.Drawing.Rectangle floorDownCheckBounds = new System.Drawing.Rectangle(
                Location.Add(new Point(dx, GridSize.Height + 1)),
                new Size(1, 1)
                );

            if (Scene.CheckGridRegionFree(floorMissingCheckBounds) && !Scene.CheckGridRegionFree(floorDownCheckBounds))
            {
                // Lower junkbot
                //
                Location = Location.Add(new Point(dx, 1));
                return;
            }

            Location = Location.Add(new Point(dx, 0));
        }
        //public Vector2 Location { get; set; }


        //public JunkbotActor()
        //{
        //    Location = Vector2.Zero;
        //}


        public void Think(TimeSpan deltaTime)
        {

        }
    }
}
