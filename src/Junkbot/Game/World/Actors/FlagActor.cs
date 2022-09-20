/**
 * FlagActor.cs - Junkbot Flag Actor
 *
 * This source-code is part of a clean-room recreation of Lego Junkbot by Oddmatics:
 * <<https://www.oddmatics.uk>>
 *
 * Author(s): Rory Fewell <roryf@oddmatics.uk>
 */

using Oddmatics.Rzxe.Extensions;
using Oddmatics.Rzxe.Game.Animation;
using System;
using System.Drawing;

namespace Junkbot.Game.World.Actors
{
    /// <summary>
    /// Represents a flag in Junkbot.
    /// </summary>
    public sealed class FlagActor : JunkbotActorBase
    {
        /// <summary>
        /// The bounding box of the flag.
        /// </summary>
        private static readonly Rectangle FlagBoundingBox =
            new Rectangle(0, 0, 2, 3);
            
            
        /// <inheritdoc />
        public override Rectangle BoundingBox
        {
            get { return FlagBoundingBox.AddOffset(Location); }
        }
        
        /// <inheritdoc />
        public override Size GridSize
        {
            get { return FlagBoundingBox.Size; }
        }

        /// <inheritdoc />
        public override bool IsMobile
        {
            get { return false; }
        }


        /// <summary>
        /// The scene.
        /// </summary>
        private Scene Scene { get; set; }


        /// <summary>
        /// Initializes a new instance of the <see cref="FlagActor"/> class.
        /// </summary>
        /// <param name="scene">
        /// The scene.
        /// </param>
        /// <param name="location">
        /// The location.
        /// </param>
        /// <param name="store">
        /// The animation store.
        /// </param>
        public FlagActor(
            Scene                scene,
            Point                location,
            SpriteAnimationStore store = null
        )
        {
            Location = location;
            Scene    = scene;
            
            if (store != null)
            {
                Animation = new SpriteAnimationServer(store);

                Animation.GoToAndStop("flag-trashcan");
            }
        }


        /// <summary>
        /// Collects the flag.
        /// </summary>
        public void Collect()
        {
            Scene.RemoveActor(this);
        }
        
        
        /// <inheritdoc />
        public override void Update(
            TimeSpan deltaTime
        )
        {
            Animation.Progress(deltaTime);
        }
    }
}
