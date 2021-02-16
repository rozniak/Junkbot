/**
 * BrickConnectionNode.cs - Junkbot Lego Brick Connection Map Node
 *
 * This source-code is part of a clean-room recreation of Lego Junkbot by Oddmatics:
 * <<https://www.oddmatics.uk>>
 *
 * Author(s): Rory Fewell <roryf@oddmatics.uk>
 */

using Junkbot.Game.World.Actors;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace Junkbot.Game.World.Logic
{
    /// <summary>
    /// Represents a node in a map of connected bricks.
    /// </summary>
    public class BrickConnectionNode
    {
        /// <summary>
        /// Gets the brick at the node.
        /// </summary>
        public BrickActor Brick { get; private set; }
        
        /// <summary>
        /// Gets the bricks that are connected to the topside.
        /// </summary>
        public IList<BrickActor> ConnectedBricksTopside { get; private set; }
        
        /// <summary>
        /// Gets the bricks that are connected to the underside.
        /// </summary>
        public IList<BrickActor> ConnectedBricksUnderside { get; private set; }
        
        /// <summary>
        /// Gets or sets a value indicating whether the brick has a direct connected
        /// path to a grey brick in either direction.
        /// </summary>
        public GreyBrickPathState GreyBrickPath { get; set; }


        /// <summary>
        /// Initializes a new insance of the <see cref="BrickConnectionNode"/> class.
        /// </summary>
        public BrickConnectionNode(
            Scene      scene,
            BrickActor brick
        )
        {
            Brick         = brick;
            GreyBrickPath = GreyBrickPathState.NoPath;

            CalculateConnections(scene);
        }
        
        
        /// <summary>
        /// Resolves the bricks that are connected to the node.
        /// </summary>
        /// <param name="scene">
        /// The game scene.
        /// </param>
        private void CalculateConnections(
            Scene scene
        )
        {
            var  connectedTop    = new List<BrickActor>();
            var  connectedUnder  = new List<BrickActor>();
            var  brickSize       = (int) Brick.Size;
            bool scanningTopside = true;

            // Register connections over and under
            //
            List<BrickActor> currentList = connectedTop;
            
            for (int y = Brick.Location.Y - 1; y <= Brick.Location.Y + 1; y += 2)
            {
                for (int x = Brick.Location.X; x < Brick.Location.X + brickSize; x++)
                {
                    var brick = scene.GetActorAtCell<BrickActor>(x, y);
                    
                    if (
                        brick == null ||
                        currentList.Contains(brick)
                    )
                    {
                        continue;
                    }
                    
                    // Was that a grey brick? If so, don't add it to the list but
                    // do record it in our state
                    //
                    if (brick.Color == Color.Gray)
                    {
                        if (scanningTopside)
                        {
                            GreyBrickPath = GreyBrickPathState.PathUpwards;
                        }
                        else
                        {
                            GreyBrickPath = GreyBrickPathState.PathDownwards;
                        }
                    }
                    else
                    {
                        currentList.Add(brick);
                    }
                }

                currentList     = connectedUnder;
                scanningTopside = false;
            }

            ConnectedBricksTopside   = connectedTop.AsReadOnly();
            ConnectedBricksUnderside = connectedUnder.AsReadOnly();
        }
    }
}
