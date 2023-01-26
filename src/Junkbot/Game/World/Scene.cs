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
using Junkbot.Game.World.Logic;
using Oddmatics.Rzxe.Extensions;
using Oddmatics.Rzxe.Game.Animation;
using Oddmatics.Rzxe.Input;
using Oddmatics.Rzxe.Windowing.Graphics;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Junkbot.Game
{
    /// <summary>
    /// Represents the main Junkbot game scene.
    /// </summary>
    public sealed class Scene : IDisposable
    {
        /// <summary>
        /// The offset on the y-axis for the actual level itself within the scene.
        /// </summary>
        public const int LevelYOffset = 24;
    
    
        /// <summary>
        /// Gets the actors in the scene.
        /// </summary>
        public IList<JunkbotActorBase> Actors
        {
            get { return _Actors.AsReadOnly(); }
        }
        private List<JunkbotActorBase> _Actors;
        
        /// <summary>
        /// Gets the brick picker controller.
        /// </summary>
        public BrickPicker BrickPicker { get; private set; }

        /// <summary>
        /// Gets the size of a single cell in the grid.
        /// </summary>
        public Size CellSize { get; private set; }

        /// <summary>
        /// Gets the number of moves made by the player.
        /// </summary>
        public int Moves { get; private set; }

        /// <summary>
        /// Gets the mobile actors in the scene.
        /// </summary>
        public IList<JunkbotActorBase> MobileActors
        {
            get { return _MobileActors.AsReadOnly(); }
        }
        private List<JunkbotActorBase> _MobileActors;

        /// <summary>
        /// Gets the size of the scene.
        /// </summary>
        public Size Size { get; private set; }


        /// <summary>
        /// The backing animation store.
        /// </summary>
        private SpriteAnimationStore AnimationStore;

        /// <summary>
        /// The value that indicates whether the scene has been disposed or is
        /// currently being disposed.
        /// </summary>
        private bool Disposing;
        
        /// <summary>
        /// The number of flags that Junkbot has collected.
        /// </summary>
        private int FlagsCollected;
        
        /// <summary>
        /// The number of flags that Junkbot must collect to win the level.
        /// </summary>
        private int FlagsRequired;

        /// <summary>
        /// The parsed level data for the scene.
        /// </summary>
        private JunkbotLevelData LevelData;

        /// <summary>
        /// The playfield grid.
        /// </summary>
        private JunkbotActorBase[,] PlayField;


        #region Drawing Related
        
        /// <summary>
        /// The sprite batch for drawing the level background.
        /// </summary>
        private ISpriteBatch BackgroundSpriteBatch;

        #endregion


        /// <summary>
        /// Occurs when gameplay has ended.
        /// </summary>
        public EventHandler GameplayEnded;

        /// <summary>
        /// Occurs when a move has been made.
        /// </summary>
        public EventHandler PlayedMove;


        /// <summary>
        /// Initializes a new instance of the <see cref="Scene"/> class.
        /// </summary>
        /// <param name="level">
        /// The level.
        /// </param>
        /// <param name="store">
        /// The animation store.
        /// </param>
        public Scene(
            JunkbotLevel         level,
            SpriteAnimationStore store = null
        )
        {
            AnimationStore = store;
            LevelData = level.ParseLevel();

            GoToInitialState();
        }


        /// <summary>
        /// Adds an actor to the scene.
        /// </summary>
        /// <param name="actor">
        /// The actor.
        /// </param>
        /// <param name="location">
        /// The location at which to insert the actor.
        /// </param>
        /// <returns>
        /// True if the actor was inserted.
        /// </returns>
        public bool AddActor(
            JunkbotActorBase actor,
            Point            location
        )
        {
            actor.Location = location;
            UpdateActorGridPosition(actor, location);

            actor.LocationChanged += Actor_LocationChanged;

            _Actors.Add(actor);
            
            if (actor.IsMobile)
            {
                _MobileActors.Add(actor);
            }

            return true;
        }

        /// <summary>
        /// Checks that a region of the grid should be considered free by the
        /// specified actor.
        /// </summary>
        /// <param name="actor">
        /// The actor requesting the check.
        /// </param>
        /// <param name="region">
        /// The region to check.
        /// </param>
        /// <param name="foundActor">
        /// (Output) The actor blocking the region, if any.
        /// </param>
        /// <returns>
        /// True if the region is free.
        /// </returns>
        public bool CheckGridRegionFreeForActor(
            JunkbotActorBase     actor,
            Rectangle            region,
            out JunkbotActorBase foundActor
        )
        {
            Point[] cellsToCheck = RectToGridCells(region);

            foundActor = null;

            foreach (Point cell in cellsToCheck)
            {
                if (
                    cell.X < 0 || cell.X >= PlayField.GetLength(0) ||
                    cell.Y < 0 || cell.Y >= PlayField.GetLength(1)
                )
                {
                    return false;
                }

                if (
                    PlayField[cell.X, cell.Y] != null &&
                    PlayField[cell.X, cell.Y] != actor
                )
                {
                    foundActor = PlayField[cell.X, cell.Y];
                    return false;
                }
            }

            return true;
        }
        
        /// <summary>
        /// Clips a rectangle into the play field.
        /// </summary>
        /// <param name="rect">
        /// The rectangle.
        /// </param>
        /// <returns>
        /// The clipped rectangle.
        /// </returns>
        public Rectangle ClipRect(
            Rectangle rect
        )
        {
            return rect.ClipInside(
                new Rectangle(
                    Point.Empty,
                    Size
                )
            );
        }

        /// <inheritdoc />
        public void Dispose()
        {
            // TODO: Probably shift 'AssertNotDisposed' to an abstract class in
            //       rzxe?
            //
            if (Disposing)
            {
                throw new ObjectDisposedException(nameof(Scene));
            }

            Disposing = true;

            if (BackgroundSpriteBatch != null)
            {
                BackgroundSpriteBatch.Dispose();
            }
        }

        /// <summary>
        /// Gets the actor at the specified cell.
        /// </summary>
        /// <param name="cell">
        /// The cell.
        /// </param>
        /// <returns>
        /// The actor that occupies the cell, null if the cell is free or out of
        /// bounds.
        /// </returns>
        public JunkbotActorBase GetActorAtCell(
            Point cell
        )
        {
            return GetActorAtCell(cell.X, cell.Y);
        }
        
        /// <summary>
        /// Gets the actor at the specified cell, that is of the specified type.
        /// </summary>
        /// <typeparam name="T">
        /// The type to look for.
        /// </typeparam>
        /// <param name="x">
        /// The x-coordinate of the cell.
        /// </param>
        /// <param name="y">
        /// The y-coordinate of the cell.
        /// </param>
        /// <returns>
        /// The actor that occupies the cell, null if the cell is free, out of bounds,
        /// or the actor is not of the specified type.
        /// </returns>
        public T GetActorAtCell<T>(
            int  x,
            int  y
        )
        where T : JunkbotActorBase
        {
            JunkbotActorBase actor = GetActorAtCell(x, y);
            
            if (actor == null || actor.GetType() != typeof(T))
            {
                return null;
            }

            return (T) actor;
        }

        /// <summary>
        /// Gets the actor at the specified cell.
        /// </summary>
        /// <param name="x">
        /// The x-coordinate of the cell.
        /// </param>
        /// <param name="y">
        /// The y-coordinate of the cell.
        /// </param>
        /// <returns>
        /// The actor that occupies the cell, null if the cell is free or out of
        /// bounds.
        /// </returns>
        public JunkbotActorBase GetActorAtCell(
            int x,
            int y
        )
        {
            if (
                x < 0 || x >= PlayField.GetLength(0) ||
                y < 0 || y >= PlayField.GetLength(1)
            )
            {
                return null;
            }
            
            return PlayField[x, y];
        }
        
        /// <summary>
        /// Gets all actors that are within the region.
        /// </summary>
        /// <param name="rect">
        /// The region of cells.
        /// </param>
        /// <returns>
        /// The actors in the region as an <see cref="IList{T}"/> collection.
        /// </returns>
        public IList<JunkbotActorBase> GetActorsInRegion(
            Rectangle rect
        )
        {
            Point[] cells = RectToGridCells(ClipRect(rect));
            var     list  = new List<JunkbotActorBase>();
            
            foreach (Point cell in cells)
            {
                JunkbotActorBase actor = GetActorAtCell(cell);

                if (
                    actor != null &&
                    !list.Contains(actor)
                )
                {
                    list.Add(actor);
                }
            }

            return list.AsReadOnly();
        }
        
        /// <summary>
        /// Gets actors of the specified type that are within the region.
        /// </summary>
        /// <typeparam name="T">
        /// The type to look for.
        /// </typeparam>
        /// <param name="rect">
        /// The region of cells.
        /// </param>
        /// <returns>
        /// The actors in region as an <see cref="IList{T}"/> collection.
        /// </returns>
        public IList<T> GetActorsInRegion<T>(
            Rectangle rect
        )
        where T : JunkbotActorBase
        {
            IList<JunkbotActorBase> actors = GetActorsInRegion(rect);
            
            Type           tType    = typeof(T);
            IEnumerable<T> filtered =
                actors.Where(
                    (actor) => actor.GetType() == tType
                ).Cast<T>();

            return new List<T>(filtered).AsReadOnly();
        }

        /// <summary>
        /// Gets all mobile actors of the specified type that are in the playfield.
        /// </summary>
        /// <typeparam name="T">
        /// The type to look for.
        /// </typeparam>
        /// <returns>
        /// All mobile actors of the specified type as an <see cref="IList{T}"/>
        /// collection.
        /// </returns>
        public IList<T> GetActorsOfType<T>()
        where T : JunkbotActorBase
        {
            Type           tType  = typeof(T);
            IEnumerable<T> actors =
                Actors.Where(
                    (actor) => actor.GetType() == tType
                ).Cast<T>();

            return new List<T>(actors).AsReadOnly();
        }
        
        /// <summary>
        /// Sets the scene to its initial state, 
        /// </summary>
        public void GoToInitialState()
        {
            _Actors        = new List<JunkbotActorBase>();
            _MobileActors  = new List<JunkbotActorBase>();
            PlayField      = new JunkbotActorBase[
                                 LevelData.Size.Width,
                                 LevelData.Size.Height
                             ];
            CellSize       = LevelData.Spacing;
            Size           = LevelData.Size;

            // Read part/actor data in
            //
            foreach (JunkbotPartData part in LevelData.Parts)
            {
                JunkbotActorBase actor    = null;
                Color            color    = Color.FromName(
                                                LevelData.Colors[part.ColorIndex]
                                            );
                Point            location = part.Location;
                string           partName = LevelData.Types[part.TypeIndex];

                switch (partName)
                {
                    case "brick_01":
                    case "brick_02":
                    case "brick_03":
                    case "brick_04":
                    case "brick_06":
                    case "brick_08":
                        int brickSize = Convert.ToInt32(partName.Substring(6));
                        
                        actor = 
                            new BrickActor(
                                this,
                                location,
                                color,
                                (BrickSize) brickSize,
                                AnimationStore
                            );
                        
                        break;

                    case "flag":
                        actor =
                            new FlagActor(
                                this,
                                location,
                                AnimationStore
                            );

                        FlagsRequired++;
                            
                        break;

                    case "minifig":
                        actor =
                            new JunkbotActor(
                                this,
                                location,
                                part.AnimationName == "walk_l" ?
                                  FacingDirection.Left :
                                  FacingDirection.Right,
                                AnimationStore
                            );
                            
                        ((JunkbotActor) actor).FlagCollected += Junkbot_FlagCollected;
                            
                        break;

                    default:
                        throw new ArgumentException(
                            $"Unknown actor '{partName}' in level data."
                        );
                }

                // Shift location offset to get true location
                //
                AddActor(
                    actor,
                    location.Subtract(new Point(1, actor.GridSize.Height))
                );
            }
            
            // Set up brick picker
            //
            if (BrickPicker != null)
            {
                BrickPicker.PickedUpBricks -= BrickPicker_PickedUpBricks;
            }

            BrickPicker = new BrickPicker(this);

            BrickPicker.PickedUpBricks += BrickPicker_PickedUpBricks;
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
        /// Determines whether the region of cells contains only bricks or empty
        /// spaces.
        /// </summary>
        /// <param name="rect">
        /// The region of cells.
        /// </param>
        /// <returns>
        /// True if the region only contains bricks or empty spaces.
        /// </returns>
        public bool RegionContainsBricksOrNull(
            Rectangle rect
        )
        {
            IList<JunkbotActorBase> actors = GetActorsInRegion(rect);
            
            if (!actors.Any())
            {
                return true;
            }

            foreach (JunkbotActorBase actor in actors)
            {
                if (actor.GetType() != typeof(BrickActor))
                {
                    return false;
                }
            }

            return true;
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
        /// Removes an actor from the scene.
        /// </summary>
        /// <param name="actor">
        /// The actors.
        /// </param>
        /// <returns>
        /// True if the actor was removed.
        /// </returns>
        public bool RemoveActor(
            JunkbotActorBase actor
        )
        {
            JunkbotActorBase toRemove = GetActorAtCell(actor.Location);
            
            if (actor != toRemove)
            {
                throw new InvalidOperationException(
                    "Actor or grid out of sync."
                );
            }

            actor.LocationChanged -= Actor_LocationChanged;

            ClearGridCells(
                RectToGridCells(actor.BoundingBox)
            );

            _Actors.Remove(actor);
            
            if (actor.IsMobile)
            {
                _MobileActors.Remove(actor);
            }

            return true;
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
            RenderBackground(graphics);

            using (
                ISpriteBatch sb =
                    graphics.CreateSpriteBatch(
                        graphics.GetSpriteAtlas("actors"),
                        SpriteBatchUsageHint.Stream
                    )
            )
            {
                for (int y = PlayField.GetLength(1) - 1; y >= 0; y--)
                {
                    for (int x = 0; x < PlayField.GetLength(0); x++)
                    {
                        JunkbotActorBase actor = PlayField[x, y];

                        if (actor == null)
                        {
                            continue;
                        }

                        // Acquire sprite data from actor, if applicable...
                        //
                        SpriteAnimationSpriteData spriteData =
                            actor.GetSpriteAtCell(x, y);

                        if (spriteData == null)
                        {
                            continue;
                        }

                        // Calculate where exactly to draw the sprite, taking into
                        // account the isometric view
                        //
                        ISprite sprite   = sb.Atlas.Sprites[spriteData.SpriteName];
                        Point   cellDiff =
                            new Point(
                                0,
                                CellSize.Height - sprite.Size.Height + LevelYOffset
                            );
                        
                        sb.Draw(
                            sprite,
                            (new Point(x, y)).Product(CellSize)
                                             .Add(cellDiff)
                                             .Add(spriteData.Offset),
                            Color.Transparent
                        );
                    }
                }
                
                BrickPicker.RenderFrame(sb);

                sb.Finish();
            }
        }

        /// <summary>
        /// Updates the scene.
        /// </summary>
        /// <param name="game">
        /// The running Junkbot game instance.
        /// </param>
        /// <param name="deltaTime">
        /// The time difference since the last update.
        /// </param>
        /// <param name="inputs">
        /// The latest state of inputs, null if no inputs are to be processed.
        /// </param>
        public void Update(
            JunkbotGame game,
            TimeSpan    deltaTime,
            InputEvents inputs = null
        )
        {
            foreach (JunkbotActorBase actor in MobileActors)
            {
                actor.Update(deltaTime);
            }
            
            if (inputs != null)
            {
                BrickPicker.Update(
                    game,
                    deltaTime,
                    inputs
                );
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
        /// Renders the scene background.
        /// </summary>
        /// <param name="graphics">
        /// The graphics interface for the renderer.
        /// </param>
        private void RenderBackground(
            IGraphicsController graphics
        )
        {
            if (BackgroundSpriteBatch == null)
            {
                ISpriteAtlas atlas = graphics.GetSpriteAtlas("bg");

                BackgroundSpriteBatch =
                    graphics.CreateSpriteBatch(
                        atlas,
                        SpriteBatchUsageHint.Static
                    );

                // Render the background itself
                //
                BackgroundSpriteBatch.Draw(
                    atlas.Sprites[LevelData.Backdrop],
                    Point.Empty,
                    Color.Transparent
                );

                // Render decals
                //
                // The positioning is because their origin is in the centre rather
                // than the top left
                //
                foreach (JunkbotDecalData decal in LevelData.Decals)
                {
                    ISprite sprite = atlas.Sprites[decal.SpriteName];

                    BackgroundSpriteBatch.Draw(
                        sprite,
                        decal.Location.Subtract(sprite.Size.Reduce(2)),
                        Color.Transparent
                    );
                }
            }

            BackgroundSpriteBatch.Finish();
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
                var oldBounds = new Rectangle(
                                    (Point) oldPos,
                                    actor.GridSize
                                );
                var oldCells  = RectToGridCells(oldBounds);

                AssertGridInSync(actor, oldCells);
                ClearGridCells(oldCells);
            }

            // Update new cells
            //
            AssignGridCells(
                actor,
                RectToGridCells(actor.BoundingBox)
            );
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
        /// (Event) Handles when the player has picked up bricks.
        /// </summary>
        private void BrickPicker_PickedUpBricks(
            object    sender,
            EventArgs e
        )
        {
            Moves++;

            PlayedMove?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// (Event) Handles when Junkbot has collected a flag.
        /// </summary>
        private void Junkbot_FlagCollected(
            object    sender,
            EventArgs e
        )
        {
            FlagsCollected++;
            
            if (FlagsCollected >= FlagsRequired)
            {
                // TODO: This is extremely basic, we will need to expand this event
                //       to describe why the game ended (win or loss), and handle
                //       Update() here to freeze the actors (except when Junkbot dies, in
                //       which case freeze all actors except the dying Junkbot)
                //
                GameplayEnded?.Invoke(sender, e);
            }
        }
    }
}
