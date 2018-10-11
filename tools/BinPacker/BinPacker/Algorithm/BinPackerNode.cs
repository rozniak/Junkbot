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
        public BinPackerNode LeftChild;
        public BinPackerNode RightChild;
        public Rectangle Rect;
        public String LeafName;


        public BinPackerNode(int x, int y, int width, int height, BinPackerNode leftChild, BinPackerNode rightChild, string leafName)
        {
            Rect = new Rectangle(x, y, width, height);
            LeftChild = leftChild;
            RightChild = rightChild;
            LeafName = leafName;
        }

        public BinPackerNode()
        {
            Rect = new Rectangle();
        }


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

                if (node.Rect.Width == rect.Width && node.Rect.Height == rect.Height) return node;

                if (node.Rect.Width < rect.Width || node.Rect.Height < rect.Height) return null;

                node.LeftChild = new BinPackerNode();
                node.RightChild = new BinPackerNode();

                int deltaWidth = node.Rect.Width - rect.Width;
                int deltaHeight = node.Rect.Height - rect.Height;

                if (deltaWidth > deltaHeight)
                {
                    node.LeftChild.Rect = new Rectangle(node.Rect.X, node.Rect.Y, rect.Width, node.Rect.Height);
                    node.RightChild.Rect = new Rectangle(node.Rect.X + rect.Width, node.Rect.Y, node.Rect.Width - rect.Width, node.Rect.Height);
                }
                else
                {
                    node.LeftChild.Rect = new Rectangle(node.Rect.X, node.Rect.Y, node.Rect.Width, rect.Height);
                    node.RightChild.Rect = new Rectangle(node.Rect.X, node.Rect.Y + rect.Height, node.Rect.Width, node.Rect.Height - rect.Height);
                }

                return Insert(node.LeftChild, rect);
            }
        }
    }
}
