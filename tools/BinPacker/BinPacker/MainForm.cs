using Newtonsoft.Json;
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
        const string TexturePath = @"C:\Users\Rory\Source\Repos\Junkbot\Junkbot\Content\RippedMenus";

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
                    LoadToCache(tex, Path.GetFileNameWithoutExtension(filename));
                }
            }
        }


        private void LoadToCache(Bitmap img, string filename)
        {
            Node node = RootNode.Insert(RootNode, new Rectangle(Point.Empty, img.Size));

            if (node != null)
            {
                using (var g = Graphics.FromImage(RenderTarget.BackgroundImage))
                {
                    g.DrawImage(img, node.Rect);
                }

                node.LeaveName = filename;
            }
            else
            {
                MessageBox.Show("Out of room!");
            }
        }

        private List<SpriteInfo> BuildUvMap()
        {
            var sprites = new List<SpriteInfo>();

            sprites = DiscoverSprites(RootNode, sprites);

            return sprites;
        }

        private List<SpriteInfo> DiscoverSprites(Node node, List<SpriteInfo> list)
        {
            if (node.LeftChild != null)
                list = DiscoverSprites(node.LeftChild, list);

            if (node.RightChild != null)
                list = DiscoverSprites(node.RightChild, list);

            if (node.LeaveName != null)
                list.Add(new SpriteInfo(node.LeaveName, node.Rect));

            return list;
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            var dlg = new SaveFileDialog();
            
            dlg.Filter = "PNG Image (.png)|*.png";
            dlg.Title = "Save Atlas As";

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                string noExt = Path.GetFileNameWithoutExtension(dlg.FileName);
                string path = Path.GetDirectoryName(dlg.FileName);

                RenderTarget.BackgroundImage.Save(path + "\\" + noExt + ".png");

                List<SpriteInfo> nodeSpriteInfos = BuildUvMap();

                File.WriteAllText(path + "\\" + noExt + ".json", JsonConvert.SerializeObject(nodeSpriteInfos));
            }
        }
    }


    internal class SpriteInfo
    {
        public string Name { get; private set; }
        public Rectangle Bounds { get; private set; }


        public SpriteInfo(string name, Rectangle bounds)
        {
            Name = name;
            Bounds = bounds;
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
