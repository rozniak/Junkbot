using Microsoft.WindowsAPICodePack.Dialogs;
using Newtonsoft.Json;
using Oddmatics.Tools.BinPacker.Algorithm;
using Oddmatics.Tools.BinPacker.Data;
using Oddmatics.Tools.BinPacker.Dialogs;
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
        /// The placeholder-oriented format for the window title.
        /// </summary>
        private const string WindowTitleFormat = "{0} - {1} {2}";


        /// <summary>
        /// The working file.
        /// </summary>
        private WorkingFile File;


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
            TaskDialogResult shouldSaveFirst = CheckDiscard();

            switch (shouldSaveFirst)
            {
                case TaskDialogResult.Yes:
                    bool saveOccurred = Save(File.LastFileName);

                    if (!saveOccurred)
                        return;

                    break;

                case TaskDialogResult.Cancel:
                    return;
            }

            // User should select a new atlas size
            //
            var dialog = new ChangeAtlasSizeDialog();
            Size newAtlasSize = Size.Empty;

            if (dialog.ShowDialog() == DialogResult.OK)
                newAtlasSize = dialog.ChosenSize;
            else if (File == null)
                newAtlasSize = new Size(2048, 2048); // FIXME: Replace with constants
            else
                return;

            // Store a reference to the old file
            //
            WorkingFile oldFile = File;

            // Create a new file in its place
            //
            File = new WorkingFile();

            File.SetAtlasSize(newAtlasSize);

            File.Invalidated += File_Invalidated;
            File.Saved += File_Saved;

            // Update the form
            //
            NodeListBox.Items.Clear();
            RefreshRenderTarget();
            UpdateTitle();

            // Dispose the old file if needed
            //
            if (oldFile != null)
                DisposeWorkingFile(ref oldFile);
        }

        /// <summary>
        /// Saves the working file.
        /// </summary>
        /// <param name="fullFilePath">
        /// The full file path to save to, if not specified, the user will be prompted
        /// for a save location.
        /// </param>
        public bool Save(string fullFilePath = "")
        {
            string targetPath;

            if (String.IsNullOrWhiteSpace(fullFilePath))
            {
                var saveDialog = new SaveFileDialog();

                saveDialog.Filter = "Atlas and UV Files (*.json;*.png)|*.json;*.png";
                saveDialog.Title = "Save Atlas As";

                if (saveDialog.ShowDialog() == DialogResult.OK)
                    targetPath = saveDialog.FileName;
                else
                    return false;
            }
            else
                targetPath = fullFilePath;

            try
            {
                File.Save(targetPath);
            }
            catch (Exception ex)
            {
                var errorDialog = new TaskDialog();

                errorDialog.Caption = Application.ProductName;
                errorDialog.FooterText = String.Format(
                    "Exception Message: {0}\n\nStack Trace:\n{1}",
                    ex.Message,
                    ex.StackTrace
                    );
                errorDialog.Icon = TaskDialogStandardIcon.Error;
                errorDialog.InstructionText = "An error occurred whilst saving the atlas.";
                errorDialog.OwnerWindowHandle = this.Handle;
                errorDialog.StandardButtons = TaskDialogStandardButtons.Ok;

                errorDialog.Show();

                return false;
            }

            return true;
        }


        /// <summary>
        /// Prompt the user if they would like to discard unsaved changes, if the file
        /// contains any.
        /// </summary>
        /// <returns>
        /// Yes if the user would like to save, No if the discard operation should
        /// continue, Cancel if the user would like to go back to the program.
        /// </returns>
        private TaskDialogResult CheckDiscard()
        {
            // No need to prompt if the file has been saved or no file exists
            //
            if (File == null || !File.IsUnsaved)
                return TaskDialogResult.No;

            var dialog = new TaskDialog();

            dialog.Caption = Application.ProductName;
            dialog.InstructionText = "Do you want to save changes to the atlas?";
            dialog.OwnerWindowHandle = this.Handle;
            dialog.StandardButtons =
                TaskDialogStandardButtons.Yes | 
                TaskDialogStandardButtons.No |
                TaskDialogStandardButtons.Cancel;

            return dialog.Show();
        }

        /// <summary>
        /// Disposes a <see cref="WorkingFile"/> completely.
        /// </summary>
        /// <param name="file">
        /// A reference to the <see cref="WorkingFile"/> to dispose.
        /// </param>
        private void DisposeWorkingFile(ref WorkingFile file)
        {
            file.Invalidated -= File_Invalidated;
            file.Saved -= File_Saved;

            file.Dispose();
        }

        /// <summary>
        /// Refreshes the <see cref="PictureBox"/> render target displayed in this
        /// form.
        /// </summary>
        private void RefreshRenderTarget()
        {
            try
            {
                RenderTarget.Image = File.GrabAtlasBitmap();
            }
            catch (Exception ex)
            {
                var errorDialog = new TaskDialog();

                errorDialog.Caption = Application.ProductName;
                errorDialog.Icon = TaskDialogStandardIcon.Error;
                errorDialog.InstructionText = "Failed to generate the atlas.";
                errorDialog.OwnerWindowHandle = this.Handle;
                errorDialog.Text = ex.Message;
                errorDialog.StandardButtons = TaskDialogStandardButtons.Ok;

                errorDialog.Show();
            }
        }

        /// <summary>
        /// Updates the window title.
        /// </summary>
        private void UpdateTitle()
        {
            this.Text = String.Format(
                WindowTitleFormat,
                Application.ProductName,
                (String.IsNullOrEmpty(File.LastFileName) ? "Untitled" : File.LastFileName),
                (File.IsUnsaved ? "*" : String.Empty)
                );
        }


        #region WorkingFile Events

        /// <summary>
        /// (Event) Occurs when the current <see cref="WorkingFile"/> has its
        /// previously saved copy invalidated.
        /// </summary>
        private void File_Invalidated(object sender, EventArgs e)
        {
            UpdateTitle();
        }

        /// <summary>
        /// (Event) Occurs when the current <see cref="WorkingFile"/> instance is
        /// saved.
        /// </summary>
        private void File_Saved(object sender, EventArgs e)
        {
            UpdateTitle();
        }

        #endregion


        #region WinForms Events

        /// <summary>
        /// (Event) Occurs when the "Add..." button is clicked.
        /// </summary>
        private void AddButton_Click(object sender, EventArgs e)
        {
            var openDialog = new OpenFileDialog();

            openDialog.Filter = "Image Files (*.bmp;*.jpg;*.jpeg;*.gif;*.png)|*.bmp;*.jpg;*.jpeg;*.gif;*.png";
            openDialog.Title = "Add Image";

            if (openDialog.ShowDialog() == DialogResult.OK)
            {
                File.AddFile(openDialog.FileName);
                NodeListBox.Items.Add(
                    Path.GetFileNameWithoutExtension(openDialog.FileName)
                    );
            }
        }

        /// <summary>
        /// (Event) Occurs when the "Canvas > Atlas Size..." menu item is clicked.
        /// </summary>
        private void CanvasAtlasSizeMenuItem_Click(object sender, EventArgs e)
        {
            var dialog = new ChangeAtlasSizeDialog();

            dialog.ChosenSize = RenderTarget.Image.Size;

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                File.SetAtlasSize(dialog.ChosenSize);
                RefreshRenderTarget();
            }
        }

        /// <summary>
        /// (Event) Occurs when the "Help > About Bin Packer Tool..." menu item is clicked.
        /// </summary>
        private void HelpAboutMenuItem_Click(object sender, EventArgs e)
        {
            var aboutDialog = new AboutDialog();

            aboutDialog.ShowDialog();
        }

        /// <summary>
        /// (Event) Occurs when the "File > Exit" menu item is clicked.
        /// </summary>
        private void FileExitMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// (Event) Occurs when the "File > New..." menu item is clicked.
        /// </summary>
        private void FileNewMenuItem_Click(object sender, EventArgs e)
        {
            CreateNew();
        }

        /// <summary>
        /// (Event) Occurs when the "File > Save" menu item is clicked.
        /// </summary>
        private void FileSaveMenuItem_Click(object sender, EventArgs e)
        {
            Save(File.LastFileName);
        }

        /// <summary>
        /// (Event) Occurs when the "File > Save As" menu item is clicked.
        /// </summary>
        private void FileSaveAsMenuItem_Click(object sender, EventArgs e)
        {
            Save();
        }

        /// <summary>
        /// (Event) Occurs when this form is closing.
        /// </summary>
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            TaskDialogResult shouldSaveFirst = CheckDiscard();

            switch (shouldSaveFirst)
            {
                case TaskDialogResult.Yes:
                    bool saveOccurred = Save(File.LastFileName);

                    if (!saveOccurred)
                        e.Cancel = true;

                    break;

                case TaskDialogResult.Cancel:
                    e.Cancel = true;
                    break;
            }
        }

        /// <summary>
        /// (Event) Occurs when this form has been displayed.
        /// </summary>
        private void MainForm_Shown(object sender, EventArgs e)
        {
            CreateNew();
            UpdateTitle();
        }

        /// <summary>
        /// (Event) Occurs when the selected index in the node <see cref="ListBox"/> is
        /// changed.
        /// </summary>
        private void NodeListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            RemoveButton.Enabled = NodeListBox.SelectedItem != null;
        }

        /// <summary>
        /// (Event) Occurs when the "Refresh" button is clicked.
        /// </summary>
        private void RefreshButton_Click(object sender, EventArgs e)
        {
            RefreshRenderTarget();
        }

        /// <summary>
        /// (Event) Occurs when the "Remove" button is clicked.
        /// </summary>
        private void RemoveButton_Click(object sender, EventArgs e)
        {
            File.RemoveFileByIndex(NodeListBox.SelectedIndex);
            NodeListBox.Items.RemoveAt(NodeListBox.SelectedIndex);
        }

        #endregion
    }
}
