/**
 * Scene.cs - Junkbot Game Scene/Playfield
 *
 * This source-code is part of a clean-room recreation of Lego Junkbot by Oddmatics:
 * <<https://www.oddmatics.uk>>
 *
 * Author(s): Rory Fewell <roryf@oddmatics.uk>
 */

using Junkbot.Game.World.Actors;
using Junkbot.Game.World.Level;
using Junkbot.Helpers;
using Oddmatics.Rzxe.Game.Animation;
using Oddmatics.Rzxe.Windowing.Graphics;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace Junkbot.Game
{
    /// <summary>
    /// Represents the main Junkbot game scene.
    /// </summary>
    internal sealed class Scene
    {
        /// <summary>
        /// Gets the actors in the scene.
        /// </summary>
        public IList<JunkbotActorBase> Actors { get; private set; }
        
        /// <summary>
        /// Gets the size of a single cell in the grid.
        /// </summary>
        public Size CellSize { get; private set; }
        
        /// <summary>
        /// Gets the mobile actors in the scene.
        /// </summary>
        public IList<JunkbotActorBase> MobileActors { get; private set; }
        
        /// <summary>
        /// Gets the size of the scene.
        /// </summary>
        public Size Size { get; private set; }
        
        
        /// <summary>
        /// The backing animation store.
        /// </summary>
        private SpriteAnimationStore AnimationStore;
        
        /// <summary>
        /// The playfield grid.
        /// </summary>
        private JunkbotActorBase[,] PlayField;
        
        
        /// <summary>
        /// Initializes a new instance of the <see cref="Scene"/> class.
        /// </summary>
        /// <param name="levelData">
        /// The level data.
        /// </param>
        /// <param name="store">
        /// The animation store.
        /// </param>
        public Scene(
            JunkbotLevelData     levelData,
            SpriteAnimationStore store
        )
        {
            AnimationStore  = store;
            PlayField       = new JunkbotActorBase[
                                  levelData.Size.Width,
                                  levelData.Size.Height
                              ];
            CellSize        = levelData.Spacing;
            Size            = levelData.Size;

            // Read part/actor data in
            //
            var actors       = new List<JunkbotActorBase>();
            var mobileActors = new List<JunkbotActorBase>();
            
            foreach (JunkbotPartData part in levelData.Parts)
            {
                JunkbotActorBase actor    = null;
                Color            color    = Color.FromName(
                                                levelData.Colors[part.ColorIndex]
                                            );
                Point            location = part.Location;
                string           partName = levelData.Types[part.TypeIndex];

                switch (partName)
                {
                    case "brick_01":
                        actor = new BrickActor(store, location, color, BrickSize.One);
                        break;

                    case "brick_02":
                        actor = new BrickActor(store, location, color, BrickSize.Two);
                        break;

                    case "brick_03":
                        actor = new BrickActor(store, location, color, BrickSize.Three);
                        break;

                    case "brick_04":
                        actor = new BrickActor(store, location, color, BrickSize.Four);
                        break;

                    case "brick_06":
                        actor = new BrickActor(store, location, color, BrickSize.Six);
                        break;

                    case "brick_08":
                        actor = new BrickActor(store, location, color, BrickSize.Eight);
                        break;

                    case "minifig":
                        actor =
                            new JunkbotActor(
                                store,
                                this,
                                location,
                                part.AnimationName == "walk_l" ?
                                  FacingDirection.Left :
                                  FacingDirection.Right
                            );
                        break;

                    default:
                        throw new ArgumentException(
                            $"Unknown actor '{partName}' in level data."
                        );
                }
                
                // Shift location offset to get true location
                //
                actor.Location = location.Subtract(new Point(1, actor.GridSize.Height));
                UpdateActorGridPosition(actor, actor.Location);
                
                actor.LocationChanged += Actor_LocationChanged;

                actors.Add(actor);

                if (!(actor is BrickActor))
                {
                    mobileActors.Add(actor);
                }
            }

            Actors       = actors.AsReadOnly();
            MobileActors = mobileActors.AsReadOnly();
        }
        
        
        /// <summary>
        /// Checks that a region of the grid is free.
        /// </summary>
        /// <param name="region">
        /// The region to check.
        /// </param>
        /// <returns>
        /// True if the region is free.
        /// </returns>
        public bool CheckGridRegionFree(
            Rectangle region
        )
        {
            Point[] cellsToCheck = RectToGridCells(region);

            foreach (Point cell in cellsToCheck)
            {
                if (
                    cell.X < 0 || cell.X >= PlayField.GetLength(0) ||
                    cell.Y < 0 || cell.Y >= PlayField.GetLength(1)
                )
                {
                    return false;
                }

                if (PlayField[cell.X, cell.Y] != null)
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Computes the location of the specified point to grid coordinates.
        /// </summary>
        /// <param name="origin">
        /// The <see cref="Point"/> to convert.
        /// </param>
        /// <returns>
        /// A <see cref="Point"/> that represents the converted <see cref="Point"/>,
        /// in grid coordinates.
        /// </returns>
        public Point PointToGrid(
            Point origin
        )
        {
            return origin.Reduce(CellSize);
        }

        /// <summary>
        /// Computes the location and size of the specified rectangle to grid units.
        /// </summary>
        /// <param name="rect">
        /// The <see cref="Rectangle"/> to convert.
        /// </param>
        /// <returns>
        /// A <see cref="Rectangle"/> that represents the converted
        /// <see cref="Rectangle"/>, in grid cell units.
        /// </returns>
        public Rectangle PointToGrid(
            Rectangle rect
        )
        {
            return new Rectangle(
                PointToGrid(rect.Location),
                PointToGrid(rect.Size)
            );
        }

        /// <summary>
        /// Computes the size in grid units.
        /// </summary>
        /// <param name="subject">
        /// The <see cref="Size"/> to convert.
        /// </param>
        /// <returns>
        /// A <see cref="Size"/> that represents the converted <see cref="Size"/>, in
        /// in grid units.
        /// </returns>
        public Size PointToGrid(
            Size subject
        )
        {
            return subject.Reduce(CellSize);
        }

        /// <summary>
        /// Converts a <see cref="Rectangle"/> region into individual points on the
        /// grid that it covers.
        /// </summary>
        /// <param name="rect">
        /// The <see cref="Rectangle"/> to convert.
        /// </param>
        /// <returns>
        /// A <see cref="Point[]"/> array that contains a <see cref="Point"/> for each
        /// cell on the grid covered by the rectangle.
        /// </returns>
        public static Point[] RectToGridCells(
            Rectangle rect
        )
        {
            var coords = new List<Point>();

            for (int h = 0; h < rect.Height; h++)
            {
                for (int w = 0; w < rect.Width; w++)
                {
                    coords.Add(new Point(rect.X + w, rect.Y + h));
                }
            }

            return coords.ToArray();
        }

        /// <summary>
        /// Renders the scene.
        /// </summary>
        /// <param name="graphics">
        /// The graphics interface for the renderer.
        /// </param>
        public void RenderFrame(
            IGraphicsController graphics
        )
        {
            ISpriteBatch sb =
                graphics.CreateSpriteBatch(
                    graphics.GetSpriteAtlas("actors")
                );
            
            for (int y = PlayField.GetLength(1) - 1; y >= 0; y--)
            {
                for (int x = 0; x < PlayField.GetLength(0); x++)
                {
                    JunkbotActorBase actor = PlayField[x, y];
                    
                    if (actor == null)
                    {
                        continue;
                    }

                    // Draw now!
                    //
                    SpriteAnimationSpriteData spriteData =
                        actor.GetSpriteAtCell(x, y);
                    
                    if (spriteData == null)
                    {
                        continue;
                    }

                    sb.Draw(
                        sb.Atlas.Sprites[spriteData.SpriteName],
                        (new Point(x, y)).Product(CellSize)
                                         .Add(spriteData.Offset)
                    );
                }
            }

            sb.Finish();
        }
        
        /// <summary>
        /// Updates the actors.
        /// </summary>
        /// <param name="deltaTime">
        /// The time difference since the last update.
        /// </param>
        public void UpdateActors(
            TimeSpan deltaTime
        )
        {
            foreach (JunkbotActorBase actor in MobileActors)
            {
                actor.Update(deltaTime);
            }
        }
        
        
        /// <summary>
        /// Asserts that the grid cells are in sync with the actor.
        /// </summary>
        /// <param name="actor">
        /// The actor to verify.
        /// </param>
        /// <param name="cellsToCheck">
        /// The cells to verify.
        /// </param>
        private void AssertGridInSync(
            JunkbotActorBase actor,
            Point[]          cellsToCheck
        )
        {
            foreach (Point cell in cellsToCheck)
            {
                if (PlayField[cell.X, cell.Y] != actor)
                {
                    throw new Exception(
                        $"Grid out of sync. X:{cell.X}, Y:{cell.Y}"
                    );
                }
            }
        }
        
        /// <summary>
        /// Assigns the grid cells.
        /// </summary>
        /// <param name="actor">
        /// The actor.
        /// </param>
        /// <param name="cells">
        /// The cells to assign.
        /// </param>
        private void AssignGridCells(
            JunkbotActorBase actor,
            Point[]          cells
        )
        {
            foreach (Point cell in cells)
            {
                // Bomb out if the cell is not free (naughty actor!)
                //
                if (PlayField[cell.X, cell.Y] != null)
                {
                    throw new Exception(
                        $"Attempted to assign an occupied cell. X:{cell.X}, Y:{cell.Y}"
                    );
                }

                PlayField[cell.X, cell.Y] = actor;
            }
        }
        
        /// <summary>
        /// Clears the grid cells.
        /// </summary>
        /// <param name="cells">
        /// The cells to clear.
        /// </param>
        private void ClearGridCells(
            Point[] cells
        )
        {
            foreach (Point cell in cells)
            {
                PlayField[cell.X, cell.Y] = null;
            }
        }
        
        /// <summary>
        /// Updates the actor grid position.
        /// </summary>
        /// <param name="actor">
        /// The actor.
        /// </param>
        /// <param name="newPos">
        /// The new position.
        /// </param>
        /// <param name="oldPos">
        /// The old position.
        /// </param>
        private void UpdateActorGridPosition(
            JunkbotActorBase actor,
            Point            newPos,
            Point?           oldPos = null
        )
        {
            // If oldPos has been specified, verify and clear
            //
            if (oldPos != null)
            {
                var oldCells = new List<Point>();

                foreach (Rectangle rect in actor.BoundingBoxes)
                {
                    var oldRect =
                        new Rectangle(
                            ((Point) oldPos).Add(rect.Location),
                            rect.Size
                        );
                    
                    oldCells.AddRange(
                        RectToGridCells(oldRect)
                    );
                }

                var oldCellsArr = oldCells.ToArray();

                AssertGridInSync(actor, oldCellsArr);
                ClearGridCells(oldCellsArr);
            }

            // Update new cells
            //
            var newCells = new List<Point>();

            foreach (Rectangle rect in actor.BoundingBoxes)
            {
                var newRect =
                    new Rectangle(
                        newPos.Add(rect.Location),
                        rect.Size
                    );
                
                newCells.AddRange(
                    RectToGridCells(newRect)
                );
            }

            AssignGridCells(actor, newCells.ToArray());
        }
        
        
        /// <summary>
        /// (Event) Handles when an actor has changed its location.
        /// </summary>
        private void Actor_LocationChanged(
            object                   sender,
            LocationChangedEventArgs e
        )
        {
            UpdateActorGridPosition(
                (JunkbotActorBase) sender,
                e.NewLocation,
                e.OldLocation
            );
        }
        
        
        /// <summary>
        /// Creates a <see cref="Scene"/> from the specified file source.
        /// </summary>
        /// <param name="lvlFile">
        /// The lines of the level file.
        /// </param>
        /// <param name="store">
        /// The animation store.
        /// </param>
        /// <returns>
        /// The <see cref="Scene"/> this method creates.
        /// </returns>
        public static Scene FromLevel(
            string[]             lvlFile,
            SpriteAnimationStore store
        )
        {
            var decals    = new List<JunkbotDecalData>();
            var levelData = new JunkbotLevelData();
            var parts     = new List<JunkbotPartData>();

            foreach (string line in lvlFile)
            {
                // Try retrieving the data
                //
                string[] definition = line.Split('=');

                if (definition.Length != 2)
                {
                    continue; // Not a definition
                }

                // Retrieve key and value
                //
                string key   = definition[0].ToLower();
                string value = definition[1];

                switch (key)
                {
                    case "backdrop":
                        levelData.Backdrop = value;
                        break;
                
                    case "colors":
                        levelData.Colors = value.ToLower().Split(',');
                        break;
                        
                    case "decals":
                        string[] decalsDef = value.Split(',');

                        foreach (string def in decalsDef)
                        {
                            if (string.IsNullOrWhiteSpace(def))
                            {
                                continue;
                            }

                            //
                            // DECAL FORMAT:
                            //     [0] - x position
                            //     [1] - y position
                            //     [2] - decal sprite name
                            //
                            string[] decalData = def.Split(';');
                            
                            if (decalData.Length != 3)
                            {
                                throw new ArgumentException(
                                    "Invalid decal data."
                                );
                            }
                            
                            var decal = new JunkbotDecalData();
                            
                            decal.Location =
                                new Point(
                                    Convert.ToInt32(decalData[0]),
                                    Convert.ToInt32(decalData[1])
                                );
                                
                            decal.SpriteName = decalData[2];

                            decals.Add(decal);
                        }
                        
                        break;

                    case "hint":
                        levelData.Hint = value;
                        break;

                    case "par":
                        levelData.Par = Convert.ToUInt16(value);
                        break;

                    case "parts":
                        string[] partsDefs = value.ToLower().Split(',');

                        foreach (string def in partsDefs)
                        {
                            if (string.IsNullOrWhiteSpace(def))
                            {
                                continue;
                            }

                            //
                            // PART FORMAT:
                            //     [0] - x position
                            //     [1] - y position
                            //     [2] - type index
                            //     [3] - colour index
                            //     [4] - animation name
                            //     [5] - (UNUSED ATM) ??? possibly whether to animate
                            //
                            string[] partData = def.Split(';');

                            if (partData.Length != 7)
                            {
                                throw new ArgumentException(
                                    "Invalid part data encountered."
                                );
                            }

                            var part = new JunkbotPartData();

                            part.Location =
                                new Point(
                                    Convert.ToInt32(partData[0]),
                                    Convert.ToInt32(partData[1])
                                );
                                
                            // Minus one to convert to zero-indexed index
                            //
                            part.TypeIndex  = (byte)(Convert.ToByte(partData[2]) - 1);
                            part.ColorIndex = (byte)(Convert.ToByte(partData[3]) - 1);

                            part.AnimationName = partData[4].ToLower();

                            parts.Add(part);
                        }

                        break;

                    case "scale":
                        levelData.Scale = Convert.ToByte(value);
                        break;

                    case "size":
                        string[] sizeCsv = value.Split(',');

                        if (sizeCsv.Length != 2)
                        {
                            throw new ArgumentException(
                                "Invalid playfield size."
                            );
                        }

                        levelData.Size =
                            new Size(
                                Convert.ToInt32(sizeCsv[0]),
                                Convert.ToInt32(sizeCsv[1])
                            );

                        break;

                    case "spacing":
                        string[] spacingCsv = value.Split(',');

                        if (spacingCsv.Length != 2)
                        {
                            throw new ArgumentException(
                                "Invalid playfield spacing."
                            );
                        }

                        levelData.Spacing =
                            new Size(
                                Convert.ToInt32(spacingCsv[0]),
                                Convert.ToInt32(spacingCsv[1])
                            );

                        break;

                    case "title":
                        levelData.Title = value;
                        break;

                    case "types":
                        var types = new List<string>();

                        if (levelData.Types != null)
                        {
                            types.AddRange(levelData.Types);
                        }

                        types.AddRange(value.ToLower().Split(','));

                        levelData.Types = types.ToArray();

                        break;
                }
            }

            levelData.Decals = decals.AsReadOnly();
            levelData.Parts  = parts.AsReadOnly();
            
            return new Scene(levelData, store);
        }
    }
}
