/**
 * JunkbotLevel.cs - Junkbot Level
 *
 * This source-code is part of a clean-room recreation of Lego Junkbot by Oddmatics:
 * <<https://www.oddmatics.uk>>
 *
 * Author(s): Rory Fewell <roryf@oddmatics.uk>
 */

using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

namespace Junkbot.Game.World.Level
{
    /// <summary>
    /// Represents a Junkbot level.
    /// </summary>
    public class JunkbotLevel
    {
        /// <summary>
        /// Gets the index of the building that the level belongs to.
        /// </summary>
        public int BuildingIndex { get; private set; }
        
        /// <summary>
        /// Gets the index of the level within the building.
        /// </summary>
        public int LevelIndex { get; private set; }
        
        /// <summary>
        /// Gets the name of the level.
        /// </summary>
        public string Name { get; private set; }
        
        
        /// <summary>
        /// The path to the level data on disk.
        /// </summary>
        private string LevelFilePath { get; set; }

        /// <summary>
        /// The store of Junkbot levels.
        /// </summary>
        private JunkbotLevelStore LevelStore { get; set; }
        
        /// <summary>
        /// The parsed level data.
        /// </summary>
        private JunkbotLevelData ParsedLevelData { get; set; }


        /// <summary>
        /// Initializes a new instance of the <see cref="JunkbotLevel"/> class by
        /// retrieving the level data from a store.
        /// </summary>
        /// <param name="levelStore">
        /// The store of Junkbot levels.
        /// </param>
        /// <param name="buildingIndex">
        /// The index of the building the level belongs to.
        /// </param>
        /// <param name="levelIndex">
        /// The index of the level within the building.
        /// </param>
        public JunkbotLevel(
            JunkbotLevelStore levelStore,
            int               buildingIndex,
            int               levelIndex
        )
        {
            BuildingIndex = buildingIndex;
            LevelIndex    = levelIndex;
            LevelStore    = levelStore;

            Name = LevelStore.GetLevelList(BuildingIndex)[LevelIndex];
            
            LevelFilePath =
                Path.Combine(
                    LevelStore.LevelRoot,
                    $"{Name}.txt"
                );
        }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="JunkbotLevel"/> class by
        /// specifying an exact path to the level data on disk.
        /// </summary>
        /// <param name="path">
        /// The path to the level data on disk.
        /// </param>
        public JunkbotLevel(
            string path
        )
        {
            LevelFilePath = path;
            Name          = string.Empty;
        }


        /// <summary>
        /// Gets the next level, if there is one.
        /// </summary>
        /// <returns>
        /// The next level, if one exists, null otherwise.
        /// </returns>
        public JunkbotLevel GetNextLevel()
        {
            if (LevelStore == null)
            {
                throw new InvalidOperationException(
                    "Level was loaded individually, there is no next level."
                );
            }

            int nextLevel = LevelIndex + 1;
            
            if (nextLevel >= LevelStore.LevelsPerBuilding)
            {
                int nextBuilding = BuildingIndex + 1;
                
                if (nextBuilding >= LevelStore.Buildings)
                {
                    return null;
                }

                return LevelStore.GetLevel(nextBuilding, 0);
            }

            return LevelStore.GetLevel(BuildingIndex, nextLevel);
        }
        
        /// <summary>
        /// Parses the data for the level.
        /// </summary>
        /// <returns>
        /// The parsed level data.
        /// </returns>
        public JunkbotLevelData ParseLevel()
        {
            if (ParsedLevelData != null)
            {
                return ParsedLevelData;
            }

            var decals    = new List<JunkbotDecalData>();
            var levelData = new JunkbotLevelData();
            var parts     = new List<JunkbotPartData>();

            string[] lvlFile = GetLevelSource();

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

            ParsedLevelData = levelData;

            return ParsedLevelData;
        }
        
        
        /// <summary>
        /// Gets the source for the level.
        /// </summary>
        private string[] GetLevelSource()
        {
            return File.ReadLines(LevelFilePath).ToArray();
        }
    }
}
