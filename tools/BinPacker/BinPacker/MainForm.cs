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
        

        /// <summary>
        /// (Event) Occurs when the "File > Save As" menu item is clicked.
        /// </summary>
        private void FileSaveAsMenuItem_Click(object sender, EventArgs e)
        {
            var dlg = new SaveFileDialog();

            dlg.Filter = "PNG Image (.png)|*.png";
            dlg.Title = "Save Atlas As";

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                Atlas.Save(dlg.FileName);
            }
        }
    }
}
