/**
 * JunkbotProfile.cs - Junkbot User Profile
 *
 * This source-code is part of a clean-room recreation of Lego Junkbot by Oddmatics:
 * <<https://www.oddmatics.uk>>
 *
 * Author(s): Rory Fewell <roryf@oddmatics.uk>
 */

using Junkbot.Game.World.Level;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace Junkbot.Game.Profile
{
    /// <summary>
    /// Represents a user profile for a set of Junkbot levels.
    /// </summary>
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class JunkbotProfile
    {
        /// <summary>
        /// The storage path for profile data.
        /// </summary>
        private static readonly string StoragePath =
            Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "Junkbot"
            );


        /// <summary>
        /// The recorded level progress.
        /// </summary>
        [JsonProperty(PropertyName = "building-progress")]
        private List<List<LevelCompletionRecord>> LevelProgress { get; set; }
        
        /// <summary>
        /// The level set that the profile belongs to.
        /// </summary>
        private JunkbotLevelStore LevelSet { get; set; }
        
        
        /// <summary>
        /// Initializes a new instance of the <see cref="JunkbotProfile"/> class.
        /// </summary>
        public JunkbotProfile()
        {
            LevelProgress = new List<List<LevelCompletionRecord>>();
        }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="JunkbotProfile"/> class for
        /// the specified level set.
        /// </summary>
        /// <param name="levelSet">
        /// The level set.
        /// </param>
        public JunkbotProfile(
            JunkbotLevelStore levelSet
        ) : this()
        {
            LevelSet = levelSet;
            
            for (int i = 0; i < LevelSet.Buildings; i++)
            {
                var levelList = new List<LevelCompletionRecord>();
                
                for (int j = 0; j < LevelSet.LevelsPerBuilding; j++)
                {
                    levelList.Add(new LevelCompletionRecord());
                }

                LevelProgress.Add(levelList);
            }
        }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="JunkbotProfile"/> from a
        /// profile JSON.
        /// </summary>
        /// <param name="levelSet">
        /// The level set.
        /// </param>
        /// <param name="jsonSrc">
        /// The JSON source for the profile.
        /// </param>
        private JunkbotProfile(
            JunkbotLevelStore levelSet,
            string            jsonSrc
        )
        {
            LevelSet = levelSet;

            JsonConvert.PopulateObject(jsonSrc, this);
        }


        /// <summary>
        /// Gets the completion record for the specified level.
        /// </summary>
        /// <param name="building">
        /// The building number.
        /// </param>
        /// <param name="level">
        /// The level number.
        /// </param>
        /// <returns>
        /// The level completion record for the level.
        /// </returns>
        public LevelCompletionRecord GetLevelCompletionInfo(
            int building,
            int level
        )
        {
            // Buildings are offset by 1, since we don't record progress for the
            // splash screen level
            //
            return LevelProgress[building - 1][level];
        }

        /// <summary>
        /// Save this <see cref="JunkbotProfile"/> to disk.
        /// </summary>
        public void Save()
        {
            EnsureStoragePathExists();

            File.WriteAllText(
                GetFilename(LevelSet),
                JsonConvert.SerializeObject(this)
            );
        }
        
        
        /// <summary>
        /// Creates a <see cref="JunkbotProfile"/> for the specified level set, if a
        /// profile does not exist, one will be created.
        /// </summary>
        /// <param name="levelSet">
        /// The level set.
        /// </param>
        /// <returns>
        /// The <see cref="JunkbotProfile"/> this method creates.
        /// </returns>
        public static JunkbotProfile FromLevelSet(
            JunkbotLevelStore levelSet
        )
        {
            string filename = GetFilename(levelSet);
            
            if (File.Exists(filename))
            {
                return new JunkbotProfile(
                    levelSet,
                    File.ReadAllText(filename)
                );
            }

            // Create a new profile now
            //
            var profile = new JunkbotProfile(levelSet);

            profile.Save();

            return profile;
        }
        
        
        /// <summary>
        /// Ensures that the profile storage path exists.
        /// </summary>
        private static void EnsureStoragePathExists()
        {
            if (!Directory.Exists(StoragePath))
            {
                Directory.CreateDirectory(StoragePath);
            }
        }

        /// <summary>
        /// Gets the filename for the profile belonging to a level set.
        /// </summary>
        /// <param name="levelSet">
        /// The level set.
        /// </param>
        /// <returns>
        /// The filename of the profile.
        /// </returns>
        private static string GetFilename(
            JunkbotLevelStore levelSet
        )
        {
            return Path.Combine(
                StoragePath,
                $"progress-{levelSet.LevelSetName.ToLower()}.json"
            );
        }
    }
}