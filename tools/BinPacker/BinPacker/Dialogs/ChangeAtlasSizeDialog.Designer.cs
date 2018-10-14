namespace Oddmatics.Tools.BinPacker.Dialogs
{
    partial class ChangeAtlasSizeDialog
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.DimensionsGroupBox = new System.Windows.Forms.GroupBox();
            this.CancelButton = new System.Windows.Forms.Button();
            this.OKButton = new System.Windows.Forms.Button();
            this.WidthLabel = new System.Windows.Forms.Label();
            this.HeightLabel = new System.Windows.Forms.Label();
            this.HeightComboBox = new System.Windows.Forms.ComboBox();
            this.WidthComboBox = new System.Windows.Forms.ComboBox();
            this.DimensionsGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // DimensionsGroupBox
            // 
            this.DimensionsGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.DimensionsGroupBox.Controls.Add(this.WidthComboBox);
            this.DimensionsGroupBox.Controls.Add(this.HeightComboBox);
            this.DimensionsGroupBox.Controls.Add(this.HeightLabel);
            this.DimensionsGroupBox.Controls.Add(this.WidthLabel);
            this.DimensionsGroupBox.Location = new System.Drawing.Point(12, 12);
            this.DimensionsGroupBox.Name = "DimensionsGroupBox";
            this.DimensionsGroupBox.Size = new System.Drawing.Size(272, 100);
            this.DimensionsGroupBox.TabIndex = 0;
            this.DimensionsGroupBox.TabStop = false;
            this.DimensionsGroupBox.Text = "Dimensions";
            // 
            // CancelButton
            // 
            this.CancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.CancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CancelButton.Location = new System.Drawing.Point(209, 126);
            this.CancelButton.Name = "CancelButton";
            this.CancelButton.Size = new System.Drawing.Size(75, 23);
            this.CancelButton.TabIndex = 1;
            this.CancelButton.Text = "Cancel";
            this.CancelButton.UseVisualStyleBackColor = true;
            this.CancelButton.Click += new System.EventHandler(this.CancelButton_Click);
            // 
            // OKButton
            // 
            this.OKButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.OKButton.Location = new System.Drawing.Point(128, 126);
            this.OKButton.Name = "OKButton";
            this.OKButton.Size = new System.Drawing.Size(75, 23);
            this.OKButton.TabIndex = 2;
            this.OKButton.Text = "OK";
            this.OKButton.UseVisualStyleBackColor = true;
            this.OKButton.Click += new System.EventHandler(this.OKButton_Click);
            // 
            // WidthLabel
            // 
            this.WidthLabel.AutoSize = true;
            this.WidthLabel.Location = new System.Drawing.Point(6, 45);
            this.WidthLabel.Name = "WidthLabel";
            this.WidthLabel.Size = new System.Drawing.Size(38, 13);
            this.WidthLabel.TabIndex = 1;
            this.WidthLabel.Text = "Width:";
            // 
            // HeightLabel
            // 
            this.HeightLabel.AutoSize = true;
            this.HeightLabel.Location = new System.Drawing.Point(142, 45);
            this.HeightLabel.Name = "HeightLabel";
            this.HeightLabel.Size = new System.Drawing.Size(41, 13);
            this.HeightLabel.TabIndex = 2;
            this.HeightLabel.Text = "Height:";
            // 
            // HeightComboBox
            // 
            this.HeightComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.HeightComboBox.FormattingEnabled = true;
            this.HeightComboBox.Location = new System.Drawing.Point(189, 42);
            this.HeightComboBox.Name = "HeightComboBox";
            this.HeightComboBox.Size = new System.Drawing.Size(70, 21);
            this.HeightComboBox.TabIndex = 3;
            // 
            // WidthComboBox
            // 
            this.WidthComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.WidthComboBox.FormattingEnabled = true;
            this.WidthComboBox.Location = new System.Drawing.Point(50, 42);
            this.WidthComboBox.Name = "WidthComboBox";
            this.WidthComboBox.Size = new System.Drawing.Size(70, 21);
            this.WidthComboBox.TabIndex = 4;
            // 
            // ChangeAtlasSizeDialog
            // 
            this.AcceptButton = this.OKButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.CancelButton;
            this.ClientSize = new System.Drawing.Size(296, 161);
            this.Controls.Add(this.OKButton);
            this.Controls.Add(this.CancelButton);
            this.Controls.Add(this.DimensionsGroupBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ChangeAtlasSizeDialog";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Atlas Size";
            this.Load += new System.EventHandler(this.ChangeAtlasSizeDialog_Load);
            this.DimensionsGroupBox.ResumeLayout(false);
            this.DimensionsGroupBox.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox DimensionsGroupBox;
        private System.Windows.Forms.Label HeightLabel;
        private System.Windows.Forms.Label WidthLabel;
        private System.Windows.Forms.Button CancelButton;
        private System.Windows.Forms.Button OKButton;
        private System.Windows.Forms.ComboBox WidthComboBox;
        private System.Windows.Forms.ComboBox HeightComboBox;
    }
}