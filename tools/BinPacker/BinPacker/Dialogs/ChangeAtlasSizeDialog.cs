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
    /// Prompts the user to pick a new size for an atlas.
    /// </summary>
    public partial class ChangeAtlasSizeDialog : Form
    {
        /// <summary>
        /// The highest power of two available to select.
        /// </summary>
        private const uint HighestPowerOfTwo = 4096;

        /// <summary>
        /// The lowest power of two available to select.
        /// </summary>
        private const uint LowestPowerOfTwo = 128;


        /// <summary>
        /// Gets or sets the <see cref="Size"/> chosen in this dialog.
        /// </summary>
        public Size ChosenSize { get; set; }


        /// <summary>
        /// Initializes a new instance of the <see cref="ChangeAtlasSizeDialog"/>
        /// class.
        /// </summary>
        public ChangeAtlasSizeDialog()
        {
            InitializeComponent();
        }


        /// <summary>
        /// Helper method for retrieving the power of two for a given dimension.
        /// </summary>
        /// <param name="value">The value of the dimension.</param>
        /// <returns>The power of two used to get the value specified.</returns>
        private uint GetPowerOfTwo(int value)
        {
            // Check the value is within range
            //
            if (value < LowestPowerOfTwo || value > HighestPowerOfTwo)
            {
                throw new ArgumentOutOfRangeException(
                    String.Format(
                        "Specified dimension was out of the range of acceptable values {0}-{1}.",
                        LowestPowerOfTwo,
                        HighestPowerOfTwo
                        )
                    );
            }

            // Obtain the exponent and check it is an integer
            //
            double pow = Math.Log(value, 2);

            if (pow % 1 != 0)
            {
                throw new ArgumentException("Encountered dimension that isn't a power of two.");
            }

            return (uint)pow;
        }


        #region WinForms Events

        /// <summary>
        /// (Event) Occurs when the "Cancel" button is clicked.
        /// </summary>
        private void CancelButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        /// <summary>
        /// (Event) Occurs when this form is loaded.
        /// </summary>
        private void ChangeAtlasSizeDialog_Load(object sender, EventArgs e)
        {
            // Populate drop downs with powers of two
            //
            double num = LowestPowerOfTwo;

            for (uint pow = GetPowerOfTwo((int)num); num <= HighestPowerOfTwo; num = Math.Pow(2, ++pow))
            {
                HeightComboBox.Items.Add(num);
                WidthComboBox.Items.Add(num);
            }

            // Check if a Size has been specified, if not, use the first option
            //
            if (ChosenSize != Size.Empty)
            {
                // Retrieve dimension powers
                //
                uint heightPow = GetPowerOfTwo(ChosenSize.Height);
                uint widthPow = GetPowerOfTwo(ChosenSize.Width);

                // Retrieve the power of the lowest number available in the dropdowns,
                // this number will be used as an offset so we can select the right
                // item easily
                //
                uint lowestPow = GetPowerOfTwo((int)LowestPowerOfTwo);

                // Select the power
                //
                uint heightIndex = heightPow - lowestPow;
                uint widthIndex = widthPow - lowestPow;

                HeightComboBox.SelectedIndex = (int)heightIndex;
                WidthComboBox.SelectedIndex = (int)widthIndex;
            }
            else
            {
                HeightComboBox.SelectedIndex = 0;
                WidthComboBox.SelectedIndex = 0;
            }
        }

        /// <summary>
        /// (Event) Occurs when the "OK" button is clicked.
        /// </summary>
        private void OKButton_Click(object sender, EventArgs e)
        {
            ChosenSize = new Size(
                Convert.ToInt32(WidthComboBox.SelectedItem.ToString()),
                Convert.ToInt32(HeightComboBox.SelectedItem.ToString())
                );

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        #endregion
    }
}
