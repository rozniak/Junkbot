/**
 * JunkbotLevelStore.cs - Junkbot Level Store
 *
 * This source-code is part of a clean-room recreation of Lego Junkbot by Oddmatics:
 * <<https://www.oddmatics.uk>>
 *
 * Author(s): Rory Fewell <roryf@oddmatics.uk>
 */

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace Junkbot.Game.World.Level
{
    /// <summary>
    /// Represents a store for Junkbot levels.
    /// </summary>
    public sealed class JunkbotLevelStore
    {
        /// <summary>
        /// Gets the number of buildings in the level set.
        /// </summary>
        public int Buildings { get; private set; }
        
        /// <summary>
        /// Gets the name of the set of levels that are currently loaded.
        /// </summary>
        public string LevelSetName { get; private set; }
        
        /// <summary>
        /// Gets the number of levels per building.
        /// </summary>
        public int LevelsPerBuilding { get; private set; }
        
        /// <summary>
        /// Gets the root directory for levels on disk.
        /// </summary>
        public string LevelRoot { get; private set; }


        /// <summary>
        /// The running Junkbot game instance.
        /// </summary>
        private JunkbotGame Junkbot { get; set; }
        
        /// <summary>
        /// Gets or sets the levels.
        /// </summary>
        private IList<IList<string>> Levels { get; set; }
        
        
        /// <summary>
        /// Initializes a new instance of the <see cref="JunkbotLevelStore"/> class.
        /// </summary>
        /// <param name="junkbot">
        /// The running Junkbot game instance.
        /// </param>
        /// <param name="levelSetName">
        /// The name of the level set.
        /// </param>
        public JunkbotLevelStore(
            JunkbotGame junkbot,
            string      levelSetName
        )
        {
            Junkbot = junkbot;
            
            LoadLevelList(levelSetName);
        }
        
        
        /// <summary>
        /// Gets a level from the store.
        /// </summary>
        /// <param name="buildingIndex">
        /// The index of the building the level belongs to.
        /// </param>
        /// <param name="levelIndex">
        /// The index of the level within the building.
        /// </param>
        /// <returns>
        /// The level.
        /// </returns>
        public JunkbotLevel GetLevel(
            int buildingIndex,
            int levelIndex
        )
        {
            return new JunkbotLevel(this, buildingIndex, levelIndex);
        }

        /// <summary>
        /// Gets the level list for the specified building.
        /// </summary>
        /// <param name="buildingIndex">
        /// The index of the building.
        /// </param>
        /// <returns>
        /// The level list for the building.
        /// </returns>
        public IList<string> GetLevelList(
            int buildingIndex
        )
        {
            return Levels[buildingIndex];
        }
        
        
        /// <summary>
        /// Loads the level list.
        /// </summary>
        /// <param name="game">
        /// The game to load levels for.
        /// </param>
        private void LoadLevelList(
            string game
        )
        {
            LevelRoot =
                Path.Combine(
                    Junkbot.Parameters.GameContentRoot,
                    "Levels",
                    game
                );

            // Read from disk
            //
            var levels         = new List<IList<string>>();
            var listingSrcPath = Path.Combine(LevelRoot, "levels.json");
            var listing        = JsonConvert.DeserializeObject<JunkbotLevelListing>(
                                     File.ReadAllText(listingSrcPath)
                                 );
            int perBuilding    = (int) Math.Ceiling(
                                     (float) listing.Levels.Length / listing.Buildings
                                 );

            // Add demo/splash level as 'building 0'
            //
            levels.Add(
                new List<string>()
                {
                    listing.SplashLevel
                }.AsReadOnly()
            );
            
            // Add buildings now
            //
            for (int i = 0; i < listing.Buildings; i++)
            {
                var nextLevels = new List<string>();
                
                for (int j = 0; j < perBuilding; j++)
                {
                    int nextLevel = (i * perBuilding) + j; // Offset in array
                    
                    if (nextLevel >= listing.Levels.Length)
                    {
                        break; // No more levels left to add to the building!
                    }
                    
                    nextLevels.Add(listing.Levels[nextLevel]);
                }

                levels.Add(nextLevels.AsReadOnly());
            }

            Buildings         = listing.Buildings;
            Levels            = levels.AsReadOnly();
            LevelSetName      = game;
            LevelsPerBuilding = perBuilding;
        }
    }
}
