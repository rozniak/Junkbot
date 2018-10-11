using Newtonsoft.Json;
using Oddmatics.Tools.BinPacker.Algorithm;
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

namespace Oddmatics.Tools.BinPacker
{
    /// <summary>
    /// Represents the main Bin Packer Tool window.
    /// </summary>
    public partial class MainForm : Form
    {
        /// <summary>
        /// The bin packed atlas.
        /// </summary>
        private BitmapBinPacker Atlas;


        /// <summary>
        /// Initializes a new instance of the <see cref="MainForm"/> class.
        /// </summary>
        public MainForm()
        {
            InitializeComponent();
        }


        /// <summary>
        /// Creates a new atlas.
        /// </summary>
        public void CreateNew()
        {
            throw new NotImplementedException();
        }


        private void MainForm_Load(object sender, EventArgs e)
        {
            
        }
        

        //private List<SpriteInfo> BuildUvMap()
        //{
        //    var sprites = new List<SpriteInfo>();

        //    sprites = DiscoverSprites(RootNode, sprites);

        //    return sprites;
        //}

        //private List<SpriteInfo> DiscoverSprites(Node node, List<SpriteInfo> list)
        //{
        //    if (node.LeftChild != null)
        //        list = DiscoverSprites(node.LeftChild, list);

        //    if (node.RightChild != null)
        //        list = DiscoverSprites(node.RightChild, list);

        //    if (node.LeaveName != null)
        //        list.Add(new SpriteInfo(node.LeaveName, node.Rect));

        //    return list;
        //}

        //private void SaveButton_Click(object sender, EventArgs e)
        //{
            
        //}

        //private void FileSaveMenuItem_Click(object sender, EventArgs e)
        //{
        //    var dlg = new SaveFileDialog();

        //    dlg.Filter = "PNG Image (.png)|*.png";
        //    dlg.Title = "Save Atlas As";

        //    if (dlg.ShowDialog() == DialogResult.OK)
        //    {
        //        string noExt = Path.GetFileNameWithoutExtension(dlg.FileName);
        //        string path = Path.GetDirectoryName(dlg.FileName);

        //        RenderTarget.BackgroundImage.Save(path + "\\" + noExt + ".png");

        //        List<SpriteInfo> nodeSpriteInfos = BuildUvMap();

        //        File.WriteAllText(path + "\\" + noExt + ".json", JsonConvert.SerializeObject(nodeSpriteInfos));
        //    }
        //}
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
}
