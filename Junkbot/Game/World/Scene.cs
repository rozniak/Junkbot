using Junkbot.Game.World.Actors;
using Junkbot.Game.World.Actors.Animation;
using Junkbot.Game.World.Level;
using Junkbot.Game.Helpers;
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

        public IList<IActor> MobileActors
        {
            get { return _MobileActors.AsReadOnly(); }
        }
        private List<IActor> _MobileActors;

        public IList<BrickActor> ImmobileBricks
        {
            get { return _ImmobileBricks.AsReadOnly(); }
        }
        private List<BrickActor> _ImmobileBricks;


        private AnimationStore AnimationStore;

        private IActor[,] PlayField;


        public Scene(JunkbotLevelData levelData, AnimationStore store)
        {
            _MobileActors = new List<IActor>();
            _ImmobileBricks = new List<BrickActor>();
            AnimationStore = store;
            PlayField = new IActor[levelData.Size.Width, levelData.Size.Height];
            CellSize = levelData.Spacing;

            foreach (JunkbotPartData part in levelData.Parts)
            {
                IActor actor = null;
                Color color = Color.FromName(levelData.Colors[part.ColorIndex]);
                Point location = part.Location; // Subtract one to get zero-indexed location

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

                if (actor is BrickActor)
                {
                    var brick = (BrickActor)actor;

                    _ImmobileBricks.Add((BrickActor)actor);
                }
                else
                    _MobileActors.Add(actor);
            }
        }

        private void Actor_LocationChanged(object sender, LocationChangedEventArgs e)
        {
            UpdateActorGridPosition((IActor)sender, e.NewLocation, e.OldLocation);
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

        public void UpdateActors()
        {
            foreach (IActor actor in _MobileActors)
            {
                actor.Update();
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


        public static Scene FromLevel(string[] lvlFile, AnimationStore store)
        {
            var levelData = new JunkbotLevelData();
            var parts = new List<JunkbotPartData>();

            foreach (string line in lvlFile)
            {
                // Try retrieving the data
                //
                string[] definition = line.Split('=');

                if (definition.Length != 2)
                    continue; // Not a definition

                // Retrieve key and value
                //
                string key = definition[0].ToLower();
                string value = definition[1];

                switch (key)
                {
                    case "colors":
                        levelData.Colors = value.ToLower().Split(',');
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
                            string[] partData = def.Split(';');

                            if (partData.Length != 7)
                            {
                                Console.WriteLine("Invalid part data encountered");
                                continue;
                            }

                            var part = new JunkbotPartData();

                            part.Location = new Point(
                                Convert.ToInt32(partData[0]),
                                Convert.ToInt32(partData[1])
                                );

                            part.TypeIndex = (byte)(Convert.ToByte(partData[2]) - 1); // Minus one to convert to zero-indexed index

                            part.ColorIndex = (byte)(Convert.ToByte(partData[3]) - 1); // Minus one to convert to zero-indexed index

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

                        levelData.Size = new Size(
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

                        levelData.Spacing = new Size(
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
                            types.AddRange(levelData.Types);

                        types.AddRange(value.ToLower().Split(','));

                        levelData.Types = types.ToArray();

                        break;
                    case "decals":
                        string[] decalsDef = value.Split(','); //Splits up each decal in a row.

                        foreach (string def in decalsDef)
                        {
                            string[] decalData = def.Split(';'); //first two define X and Y, then the Decal type
                            if (decalData.Length != 3)
                            {
                                Console.WriteLine("Invalid decal data encountered");
                                continue;
                            }
                            var decals = new JunkbotDecalData(); //a new struct for storing decal data: its sprite and position.
                            decals.Location = new Point(
                                Convert.ToInt32(decalData[0]),
                                Convert.ToInt32(decalData[1])
                                );
                            decals.Decal = decalData[2]; //If I'm not mistaken, this will pass the relevant information from the level to the decal entry.
                                                         //Now, all that remains is to get stuff sorted out.

                        }
                        break;
                    case "backdrop":
                        levelData.Backdrop = value;
                        break;
                }
            }

            levelData.Parts = parts.AsReadOnly();

            return new Scene(levelData, store);
        }
    }
}
