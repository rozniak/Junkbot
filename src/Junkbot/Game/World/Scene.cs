using Junkbot.Game.World.Actors;
using Junkbot.Game.World.Level;
using Junkbot.Helpers;
using Oddmatics.Rzxe.Game.Actors.Animation;
using Oddmatics.Rzxe.Windowing.Graphics;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Junkbot.Game
{
    internal class Scene
    {
        public Size CellSize { get; private set; }

        public IList<IActor> Actors { get; private set; }

        public IList<IActor> MobileActors { get; private set; }

        public Size Size { get; private set; }


        private AnimationStore AnimationStore;

        private IActor[,] PlayField;


        public Scene(JunkbotLevelData levelData, AnimationStore store)
        {
            AnimationStore  = store;
            PlayField       = new IActor[levelData.Size.Width, levelData.Size.Height];
            CellSize        = levelData.Spacing;
            Size            = levelData.Size;

            // Read part/actor data in
            //
            var actors       = new List<IActor>();
            var mobileActors = new List<IActor>();
            
            foreach (JunkbotPartData part in levelData.Parts)
            {
                IActor actor    = null;
                Color  color    = Color.FromName(levelData.Colors[part.ColorIndex]);
                Point  location = part.Location; // Subtract one to get zero-indexed location

                switch (levelData.Types[part.TypeIndex])
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
                        actor = new JunkbotActor(store, this, location, (part.AnimationName == "WALK_L" ? FacingDirection.Left : FacingDirection.Right));
                        break;

                    default:
                        Console.WriteLine("Unknown actor: " + levelData.Types[part.TypeIndex]);
                        continue;
                }
                
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
        

        public bool CheckGridRegionFree(Rectangle region)
        {
            Point[] cellsToCheck = region.ExpandToGridCoordinates();

            foreach (Point cell in cellsToCheck)
            {
                if (cell.X < 0 || cell.X >= PlayField.GetLength(0) || cell.Y < 0 || cell.Y >= PlayField.GetLength(1))
                    return false;

                if (PlayField[cell.X, cell.Y] != null)
                    return false;
            }

            return true;
        }
        
        public void RenderFrame(
            IGraphicsController graphics
        )
        {
            //
            // FIXME: The below code is preliminary and subject to the overlapping
            //        problem described in issue #15
            //
            
            ISpriteBatch sb =
                graphics.CreateSpriteBatch(
                    graphics.GetSpriteAtlas("actors")
                );
            
            for (int y = PlayField.GetLength(1) - 1; y >= 0; y--)
            {
                for (int x = 0; x < PlayField.GetLength(0); x++)
                {
                    IActor actor = PlayField[x, y];
                    
                    if (actor == null)
                    {
                        continue;
                    }
                    
                    if (
                        actor.Location.X != x ||
                        actor.Location.Y != y
                    )
                    {
                        continue;
                    }

                    // Draw now!
                    //
                    ActorAnimationFrame frame = actor.Animation.GetCurrentFrame();

                    sb.Draw(
                        sb.Atlas.Sprites[frame.SpriteName],
                        actor.Location.Product(CellSize).Add(frame.Offset)
                    );
                }
            }

            sb.Finish();
        }

        public void UpdateActors(
            TimeSpan deltaTime
        )
        {
            foreach (IActor actor in MobileActors)
            {
                actor.Update(deltaTime);
            }
        }


        private void AssertGridInSync(IActor actor, Point[] cellsToCheck)
        {
            foreach (Point cell in cellsToCheck)
            {
                if (PlayField[cell.X, cell.Y] != actor)
                    throw new Exception("Scene.VerifyGridInSync: Grid out of sync!! X:" + cell.X.ToString() + ", Y:" + cell.Y.ToString());
            }
        }

        private void AssignGridCells(IActor actor, Point[] cells)
        {
            foreach (Point cell in cells)
            {
                // Bomb out if the cell is not free (naughty actor!)
                //
                if (PlayField[cell.X, cell.Y] != null)
                    throw new Exception("Scene.AssignGridCells: Attempted to assign an occupied cell!! X:" + cell.X.ToString() + ", Y:" + cell.Y.ToString());

                PlayField[cell.X, cell.Y] = actor;
            }
        }

        private void ClearGridCells(Point[] cells)
        {
            foreach (Point cell in cells)
            {
                PlayField[cell.X, cell.Y] = null;
            }
        }

        private void UpdateActorGridPosition(IActor actor, Point newPos, Point? oldPos = null)
        {
            // If oldPos has been specified, verify and clear
            //
            if (oldPos != null)
            {
                var oldCells = new List<Point>();

                foreach (Rectangle rect in actor.BoundingBoxes)
                {
                    oldCells.AddRange((new Rectangle(((Point)oldPos).Add(rect.Location), rect.Size)).ExpandToGridCoordinates());
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
                newCells.AddRange((new Rectangle(newPos.Add(rect.Location), rect.Size)).ExpandToGridCoordinates());
            }

            AssignGridCells(actor, newCells.ToArray());
        }
        
        
        private void Actor_LocationChanged(object sender, LocationChangedEventArgs e)
        {
            UpdateActorGridPosition((IActor)sender, e.NewLocation, e.OldLocation);
        }


        public static Scene FromLevel(string[] lvlFile, AnimationStore store)
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
                            //
                            // DECAL FORMAT:
                            //     [0] - x position
                            //     [1] - y position
                            //     [2] - decal sprite name
                            //
                            string[] decalData = def.Split(';');
                            
                            if (decalData.Length != 3)
                            {
                                Console.WriteLine("Invalid decal data encountered");
                                continue;
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
                                Console.WriteLine("Invalid part data encountered");
                                continue;
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
                            Console.WriteLine("Invalid playfield size encountered");
                            continue;
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
                            Console.WriteLine("Invalid playfield spacing encountered");
                            continue;
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
