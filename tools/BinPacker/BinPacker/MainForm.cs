using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BinPacker
{
    public partial class MainForm : Form
    {
        const string TexturePath = @"C:\Users\Rory\Source\Repos\Junkbot\Junkbot\Content";

        private Node RootNode;


        public MainForm()
        {
            InitializeComponent();

            RenderTarget.BackgroundImage = new Bitmap(RenderTarget.Width, RenderTarget.Height);

            RootNode = new Node(0, 0, RenderTarget.Width, RenderTarget.Height, null, null, null);
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            string[] textureFiles = Directory.GetFiles(TexturePath);
            

            foreach (string filename in textureFiles)
            {
                using (var tex = (Bitmap)Image.FromFile(filename))
                {
                    LoadToCache(tex);
                }
            }
        }


        private void LoadToCache(Bitmap img)
        {
            Node node = RootNode.Insert(RootNode, new Rectangle(Point.Empty, img.Size));

            if (node != null)
            {
                using (var g = Graphics.FromImage(RenderTarget.BackgroundImage))
                {
                    g.DrawImage(img, node.Rect);
                }

                node.LeaveName = "117";
            }
            else
            {
                MessageBox.Show("Out of room!");
            }
        }
    }


    internal class Node
    {
        public Node LeftChild;
        public Node RightChild;
        public Rectangle Rect;
        public String LeaveName;


        public Node(int x, int y, int width, int height, Node leftChild, Node rightChild, string leaveName)
        {
            Rect = new Rectangle(x, y, width, height);
            LeftChild = leftChild;
            RightChild = rightChild;
            LeaveName = leaveName;
        }

        public Node()
        {
            Rect = new Rectangle();
        }


        public Node Insert(Node node, Rectangle rect)
        {
            if (node.LeaveName == null && node.LeftChild != null && node.RightChild != null)
            {
                Node newNode = null;

                newNode = Insert(node.LeftChild, rect);

                if (newNode == null) newNode = Insert(node.RightChild, rect);

                return newNode;
            }
            else
            {
                if (node.LeaveName != null) return null;

                if (node.Rect.Width == rect.Width && node.Rect.Height == rect.Height) return node;

                if (node.Rect.Width < rect.Width || node.Rect.Height < rect.Height) return null;

                node.LeftChild = new Node();
                node.RightChild = new Node();

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
