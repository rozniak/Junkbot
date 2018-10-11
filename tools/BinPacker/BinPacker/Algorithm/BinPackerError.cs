using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oddmatics.Tools.BinPacker.Algorithm
{
    /// <summary>
    /// Specifies constants that define the error that occurred during the bin packing
    /// process.
    /// </summary>
    internal enum BinPackerError
    {
        /// <summary>
        /// Represents that bin packing was successful.
        /// </summary>
        None,

        /// <summary>
        /// Represents that the atlas did not have enough space to fit all the sprites.
        /// </summary>
        OutOfRoom,

        /// <summary>
        /// Represents that a source file was missing.
        /// </summary>
        MissingFile,

        /// <summary>
        /// Represents that there was a problem reading one of the source files.
        /// </summary>
        UnreadableFile
    }
}