using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oddmatics.Tools.BinPacker.Algorithm
{
    /// <summary>
    /// Represents a single node as part of the overall bin packing algorithm.
    /// </summary>
    internal sealed class BinPackerNode
    {
        /// <summary>
        /// Gets or sets the bounding box rectangle of this node.
        /// </summary>
        public Rectangle Bounds;

        /// <summary>
        /// Gets or sets the name of this node.
        /// </summary>
        public String LeafName;

        /// <summary>
        /// Gets or sets the "left-hand" child node of this node.
        /// </summary>
        public BinPackerNode LeftChild;

        /// <summary>
        /// Gets or sets the "right-hand" child node of this node.
        /// </summary>
        public BinPackerNode RightChild;


        /// <summary>
        /// Initializes a new instance of the BinPackerNode class.
        /// </summary>
        internal BinPackerNode()
        {
            Bounds = new Rectangle();
        }

        /// <summary>
        /// Initializes a new instance of the BinPackerNode class with specified
        /// parameters.
        /// </summary>
        /// <param name="bounds">The bounding box of the node.</param>
        /// <param name="leftChild">The node's "left-hand" child node.</param>
        /// <param name="rightChild">The node's "right-hand" child node.</param>
        /// <param name="leafName">The name of the node.</param>
        internal BinPackerNode(
            Rectangle bounds,
            BinPackerNode leftChild,
            BinPackerNode rightChild,
            string leafName
            )
        {
            Bounds = bounds;
            LeftChild = leftChild;
            RightChild = rightChild;
            LeafName = leafName;
        }


        /// <summary>
        /// Attempts to insert a new node as a child of this node.
        /// </summary>
        /// <param name="node">The node to insert.</param>
        /// <param name="rect">The bounding box rectangle of the node.</param>
        /// <returns>The inserted node.</returns>
        public BinPackerNode Insert(BinPackerNode node, Rectangle rect)
        {
            if (node.LeafName == null && node.LeftChild != null && node.RightChild != null)
            {
                BinPackerNode newNode = null;

                newNode = Insert(node.LeftChild, rect);

                if (newNode == null) newNode = Insert(node.RightChild, rect);

                return newNode;
            }
            else
            {
                if (node.LeafName != null) return null;

                if (node.Bounds.Width == rect.Width && node.Bounds.Height == rect.Height) return node;

                if (node.Bounds.Width < rect.Width || node.Bounds.Height < rect.Height) return null;

                node.LeftChild = new BinPackerNode();
                node.RightChild = new BinPackerNode();

                int deltaWidth = node.Bounds.Width - rect.Width;
                int deltaHeight = node.Bounds.Height - rect.Height;

                if (deltaWidth > deltaHeight)
                {
                    node.LeftChild.Bounds = new Rectangle(node.Bounds.X, node.Bounds.Y, rect.Width, node.Bounds.Height);
                    node.RightChild.Bounds = new Rectangle(node.Bounds.X + rect.Width, node.Bounds.Y, node.Bounds.Width - rect.Width, node.Bounds.Height);
                }
                else
                {
                    node.LeftChild.Bounds = new Rectangle(node.Bounds.X, node.Bounds.Y, node.Bounds.Width, rect.Height);
                    node.RightChild.Bounds = new Rectangle(node.Bounds.X, node.Bounds.Y + rect.Height, node.Bounds.Width, node.Bounds.Height - rect.Height);
                }

                return Insert(node.LeftChild, rect);
            }
        }
    }
}
