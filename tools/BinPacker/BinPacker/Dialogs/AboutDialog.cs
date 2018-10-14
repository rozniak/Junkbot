using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Oddmatics.Tools.BinPacker.Dialogs
{
    /// <summary>
    /// Displays information about the Bin Packer Tool program.
    /// </summary>
    public partial class AboutDialog : Form
    {
        /// <summary>
        /// The placeholder-oriented format for the application info.
        /// </summary>
        private const string AppInfoFormat = "{0} Version {1}";

        /// <summary>
        /// The placeholder-oriented format for the window title.
        /// </summary>
        private const string WindowTitleFormat = "About {0}";


        /// <summary>
        /// Initializes a new instance of the <see cref="AboutDialog"/> class.
        /// </summary>
        public AboutDialog()
        {
            InitializeComponent();

            AboutLabel.Text = AboutLabel.Text.Replace(
                "{APPINFO}",
                String.Format(
                    AppInfoFormat,
                    Application.ProductName,
                    Application.ProductVersion
                    )
                );
            this.Text = String.Format(
                WindowTitleFormat,
                Application.ProductName
                );
        }


        #region WinForms Events

        private void CloseButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        #endregion
    }
}
