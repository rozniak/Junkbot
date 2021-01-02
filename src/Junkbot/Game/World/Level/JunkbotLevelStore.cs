using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Junkbot.Game.World.Level
{
    internal sealed class JunkbotLevelStore
    {
        private JunkbotGame Junkbot { get; set; }
        
        private string LevelRoot { get; set; }

        private IList<IList<string>> Levels { get; set; }
        
        
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
        
        
        public IList<string> GetLevelList(
            byte building
        )
        {
            return Levels[building];
        }
        
        public string[] GetLevelSource(
            byte building,
            byte levelIndex
        )
        {
            string level         = Levels[building][levelIndex];
            string levelJsonPath = Path.Combine(LevelRoot, $"{level}.txt");

            return File.ReadLines(levelJsonPath).ToArray();
        }


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
