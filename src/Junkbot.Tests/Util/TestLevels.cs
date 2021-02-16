/**
 * TestLevels.cs - Junkbot Test Level Loading Utility
 *
 * This source-code is part of a clean-room recreation of Lego Junkbot by Oddmatics:
 * <<https://www.oddmatics.uk>>
 *
 * Author(s): Rory Fewell <roryf@oddmatics.uk>
 */

using System;
using System.IO;

namespace Junkbot.Tests.Util
{
    /// <summary>
    /// Provides utility methods for the test level content.
    /// </summary>
    public static class TestLevels
    {
        /// <summary>
        /// Gets the filepath for the level when running the specified test case.
        /// </summary>
        /// <param name="testCase">
        /// The test case.
        /// </param>
        /// <returns>
        /// The filepath for the level when running the specified test case.
        /// </returns>
        public static string GetLevelPath(
            string testCase
        )
        {
            return Path.Combine(
                Environment.CurrentDirectory,
                "Content",
                "Levels",
                $"map_test_{testCase}.txt"
            );
        }
    }
}
