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
using System.Linq;

namespace Junkbot.Game.World.Level
{
    /// <summary>
    /// Represents a store for Junkbot levels.
    /// </summary>
    public sealed class JunkbotLevelStore
    {
        /// <summary>
        /// The running Junkbot game instance.
        /// </summary>
        private JunkbotGame Junkbot { get; set; }
        
        /// <summary>
        /// The root directory for levels on disk.
        /// </summary>
        private string LevelRoot { get; set; }
        
        /// <summary>
        /// Gets or sets the levels.
        /// </summary>
        private IList<IList<string>> Levels { get; set; }
        
        
        /// <summary>
        /// Initializes a new instance of the <see cref="JunkbotLevelStore class."/>
        /// </summary>
        /// <param name="junkbot">
        /// The running Junkbot game instance.
        /// </param>
        public JunkbotLevelStore(
            JunkbotGame junkbot
        )
        {
            Junkbot = junkbot;

            // TODO: One day we need to support picking a game, right now just load
            //       Junkbot
            //
            LoadLevelList("Junkbot");
        }
        
        
        /// <summary>
        /// Gets the level list for the specified building.
        /// </summary>
        /// <param name="building">
        /// The building number.
        /// </param>
        /// <returns>
        /// The level list for the building.
        /// </returns>
        public IList<string> GetLevelList(
            byte building
        )
        {
            return Levels[building];
        }
        
        /// <summary>
        /// Gets the source for a specific level.
        /// </summary>
        /// <param name="building">
        /// The building number.
        /// </param>
        /// <param name="levelIndex">
        /// The level number.
        /// </param>
        /// <returns>
        /// The source data for the level.
        /// </returns>
        public string[] GetLevelSource(
            byte building,
            byte levelIndex
        )
        {
            string level         = Levels[building][levelIndex];
            string levelJsonPath = Path.Combine(LevelRoot, $"{level}.txt");

            return File.ReadLines(levelJsonPath).ToArray();
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
            int numBuildings   = (int) Math.Ceiling(listing.Levels.Length / 15f);

            // Add demo/splash level as 'building 0'
            //
            levels.Add(
                new List<string>()
                {
                    listing.SplashLevel
                }.AsReadOnly()
            );
            
            // Add buildings now - start from building 1
            //
            for (int i = 1; i <= numBuildings; i++)
            {
                var nextLevels = new List<string>();
                
                // 15 levels per building - there could be some left over in the last
                // building though (as we use Math.Ceiling earlier)
                //
                for (int j = 0; j < 15; j++)
                {
                    int nextLevel = (i * 15) + j; // Offset in array
                    
                    if (nextLevel >= listing.Levels.Length)
                    {
                        break; // No more levels left to add to the building!
                    }
                    
                    nextLevels.Add(listing.Levels[nextLevel]);
                }

                levels.Add(nextLevels.AsReadOnly());
            }

            Levels = levels.AsReadOnly();
        }
    }
}
