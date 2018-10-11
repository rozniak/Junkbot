using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oddmatics.Tools.BinPacker.Algorithm
{
    /// <summary>
    /// Represents a bin packer instance for generating an atlas bitmap.
    /// </summary>
    public sealed class BitmapBinPacker : IDisposable
    {
        /// <summary>
        /// Gets the generated atlas.
        /// </summary>
        public Bitmap Bitmap { get; private set; }

        /// <summary>
        /// Gets the value that indicates whether this <see cref="BitmapBinPacker"/>
        /// has been disposed or not.
        /// </summary>
        public bool Disposed { get; private set; }

        /// <summary>
        /// Gets the size of the atlas.
        /// </summary>
        public Size Size { get; private set; }


        /// <summary>
        /// The root node for the bin packing algorithm.
        /// </summary>
        private BinPackerNode RootNode;


        /// <summary>
        /// Initializes a new instance of <see cref="BitmapBinPacker"/> with a
        /// specified resolution and source file list.
        /// </summary>
        /// <param name="size">The size of the atlas.</param>
        /// <param name="sourceFiles">The source file list.</param>
        public BitmapBinPacker(Size size, IList<string> sourceFiles)
        {
            Bitmap = new Bitmap(size.Width, size.Height);
            RootNode = new BinPackerNode(0, 0, size.Width, size.Height, null, null, null);
            Size = size;

            // Bin pack the sprites found in source files now
            //
            var result = BinPackerError.None;
            var ex = new Exception("Unknown error occurred.");

            using (var g = Graphics.FromImage(Bitmap))
            {
                foreach (string filePath in sourceFiles)
                {
                    Bitmap sprite;
                    string spriteName = Path.GetFileNameWithoutExtension(filePath);
                    BinPackerNode node;

                    if (!File.Exists(filePath))
                    {
                        result = BinPackerError.MissingFile;
                        ex = new FileNotFoundException(
                            "Failed to find a source file.",
                            filePath
                            );

                        break;
                    }

                    try
                    {
                        sprite = (Bitmap)Bitmap.FromFile(filePath);
                    }
                    catch (Exception anyEx)
                    {
                        result = BinPackerError.UnreadableFile;
                        ex = anyEx;

                        break;
                    }

                    node = RootNode.Insert(
                        RootNode,
                        new Rectangle(Point.Empty, sprite.Size)
                        );

                    if (node != null)
                    {
                        g.DrawImage(sprite, node.Rect);
                        node.LeafName = spriteName;
                    }
                    else
                    {
                        result = BinPackerError.OutOfRoom;
                        ex = new InvalidOperationException(
                            "There is not enough room in the atlas to contain all of the sprites."
                            );

                        break;
                    }
                }
            }

            if (result != BinPackerError.None)
                throw ex;
        }


        /// <summary>
        /// Releases all resources used by this <see cref="BitmapBinPacker"/>.
        /// </summary>
        public void Dispose()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Saves the atlas data to disk.
        /// </summary>
        /// <param name="fullFilePath">The full file path (minus extension) to save atlas information at.</param>
        public void Save(string fullFilePath)
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// Asserts that this <see cref="BitmapBinPacker"/> has not been disposed.
        /// </summary>
        private void AssertNotDisposed()
        {

        }
    }
}
