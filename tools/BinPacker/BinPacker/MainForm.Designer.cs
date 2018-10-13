namespace Oddmatics.Tools.BinPacker
{
    partial class MainForm
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
            this.RenderTarget = new System.Windows.Forms.PictureBox();
            this.UiSplitView = new System.Windows.Forms.SplitContainer();
            this.NodeListBox = new System.Windows.Forms.ListBox();
            this.NodeManagementPanel = new System.Windows.Forms.Panel();
            this.RefreshButton = new System.Windows.Forms.Button();
            this.RemoveButton = new System.Windows.Forms.Button();
            this.AddButton = new System.Windows.Forms.Button();
            this.MainMenu = new System.Windows.Forms.MenuStrip();
            this.FileMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.FileNewMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.FileOpenMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.FileSaveMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.FileSaveAsMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.FileMenuSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.FileExitMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.CanvasMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.CanvasAtlasSizeMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.HelpMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.HelpAboutMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.RenderTarget)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.UiSplitView)).BeginInit();
            this.UiSplitView.Panel1.SuspendLayout();
            this.UiSplitView.Panel2.SuspendLayout();
            this.UiSplitView.SuspendLayout();
            this.NodeManagementPanel.SuspendLayout();
            this.MainMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // RenderTarget
            // 
            this.RenderTarget.Location = new System.Drawing.Point(0, 0);
            this.RenderTarget.Name = "RenderTarget";
            this.RenderTarget.Size = new System.Drawing.Size(2048, 2048);
            this.RenderTarget.TabIndex = 0;
            this.RenderTarget.TabStop = false;
            // 
            // UiSplitView
            // 
            this.UiSplitView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.UiSplitView.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.UiSplitView.IsSplitterFixed = true;
            this.UiSplitView.Location = new System.Drawing.Point(0, 24);
            this.UiSplitView.Name = "UiSplitView";
            // 
            // UiSplitView.Panel1
            // 
            this.UiSplitView.Panel1.AutoScroll = true;
            this.UiSplitView.Panel1.Controls.Add(this.RenderTarget);
            // 
            // UiSplitView.Panel2
            // 
            this.UiSplitView.Panel2.Controls.Add(this.NodeListBox);
            this.UiSplitView.Panel2.Controls.Add(this.NodeManagementPanel);
            this.UiSplitView.Size = new System.Drawing.Size(764, 437);
            this.UiSplitView.SplitterDistance = 504;
            this.UiSplitView.TabIndex = 2;
            // 
            // NodeListBox
            // 
            this.NodeListBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.NodeListBox.FormattingEnabled = true;
            this.NodeListBox.Location = new System.Drawing.Point(0, 0);
            this.NodeListBox.Name = "NodeListBox";
            this.NodeListBox.Size = new System.Drawing.Size(256, 397);
            this.NodeListBox.TabIndex = 1;
            // 
            // NodeManagementPanel
            // 
            this.NodeManagementPanel.Controls.Add(this.RefreshButton);
            this.NodeManagementPanel.Controls.Add(this.RemoveButton);
            this.NodeManagementPanel.Controls.Add(this.AddButton);
            this.NodeManagementPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.NodeManagementPanel.Location = new System.Drawing.Point(0, 397);
            this.NodeManagementPanel.Name = "NodeManagementPanel";
            this.NodeManagementPanel.Size = new System.Drawing.Size(256, 40);
            this.NodeManagementPanel.TabIndex = 0;
            // 
            // RefreshButton
            // 
            this.RefreshButton.Location = new System.Drawing.Point(7, 6);
            this.RefreshButton.Name = "RefreshButton";
            this.RefreshButton.Size = new System.Drawing.Size(75, 23);
            this.RefreshButton.TabIndex = 2;
            this.RefreshButton.Text = "Refresh";
            this.RefreshButton.UseVisualStyleBackColor = true;
            // 
            // RemoveButton
            // 
            this.RemoveButton.Location = new System.Drawing.Point(88, 6);
            this.RemoveButton.Name = "RemoveButton";
            this.RemoveButton.Size = new System.Drawing.Size(75, 23);
            this.RemoveButton.TabIndex = 1;
            this.RemoveButton.Text = "Remove";
            this.RemoveButton.UseVisualStyleBackColor = true;
            // 
            // AddButton
            // 
            this.AddButton.Location = new System.Drawing.Point(169, 6);
            this.AddButton.Name = "AddButton";
            this.AddButton.Size = new System.Drawing.Size(75, 23);
            this.AddButton.TabIndex = 0;
            this.AddButton.Text = "Add...";
            this.AddButton.UseVisualStyleBackColor = true;
            // 
            // MainMenu
            // 
            this.MainMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.FileMenu,
            this.CanvasMenu,
            this.HelpMenu});
            this.MainMenu.Location = new System.Drawing.Point(0, 0);
            this.MainMenu.Name = "MainMenu";
            this.MainMenu.Size = new System.Drawing.Size(764, 24);
            this.MainMenu.TabIndex = 4;
            this.MainMenu.Text = "menuStrip1";
            // 
            // FileMenu
            // 
            this.FileMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.FileNewMenuItem,
            this.FileOpenMenuItem,
            this.FileSaveMenuItem,
            this.FileSaveAsMenuItem,
            this.FileMenuSeparator1,
            this.FileExitMenuItem});
            this.FileMenu.Name = "FileMenu";
            this.FileMenu.Size = new System.Drawing.Size(37, 20);
            this.FileMenu.Text = "&File";
            // 
            // FileNewMenuItem
            // 
            this.FileNewMenuItem.Name = "FileNewMenuItem";
            this.FileNewMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
            this.FileNewMenuItem.Size = new System.Drawing.Size(152, 22);
            this.FileNewMenuItem.Text = "&New";
            // 
            // FileOpenMenuItem
            // 
            this.FileOpenMenuItem.Enabled = false;
            this.FileOpenMenuItem.Name = "FileOpenMenuItem";
            this.FileOpenMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.FileOpenMenuItem.Size = new System.Drawing.Size(152, 22);
            this.FileOpenMenuItem.Text = "&Open..";
            // 
            // FileSaveMenuItem
            // 
            this.FileSaveMenuItem.Name = "FileSaveMenuItem";
            this.FileSaveMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.FileSaveMenuItem.Size = new System.Drawing.Size(180, 22);
            this.FileSaveMenuItem.Text = "&Save";
            // 
            // FileSaveAsMenuItem
            // 
            this.FileSaveAsMenuItem.Name = "FileSaveAsMenuItem";
            this.FileSaveAsMenuItem.Size = new System.Drawing.Size(180, 22);
            this.FileSaveAsMenuItem.Text = "Save &As...";
            this.FileSaveAsMenuItem.Click += new System.EventHandler(this.FileSaveAsMenuItem_Click);
            // 
            // FileMenuSeparator1
            // 
            this.FileMenuSeparator1.Name = "FileMenuSeparator1";
            this.FileMenuSeparator1.Size = new System.Drawing.Size(149, 6);
            // 
            // FileExitMenuItem
            // 
            this.FileExitMenuItem.Name = "FileExitMenuItem";
            this.FileExitMenuItem.Size = new System.Drawing.Size(152, 22);
            this.FileExitMenuItem.Text = "E&xit";
            // 
            // CanvasMenu
            // 
            this.CanvasMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.CanvasAtlasSizeMenuItem});
            this.CanvasMenu.Name = "CanvasMenu";
            this.CanvasMenu.Size = new System.Drawing.Size(57, 20);
            this.CanvasMenu.Text = "&Canvas";
            // 
            // CanvasAtlasSizeMenuItem
            // 
            this.CanvasAtlasSizeMenuItem.Name = "CanvasAtlasSizeMenuItem";
            this.CanvasAtlasSizeMenuItem.Size = new System.Drawing.Size(132, 22);
            this.CanvasAtlasSizeMenuItem.Text = "Atlas Si&ze...";
            // 
            // HelpMenu
            // 
            this.HelpMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.HelpAboutMenuItem});
            this.HelpMenu.Name = "HelpMenu";
            this.HelpMenu.Size = new System.Drawing.Size(44, 20);
            this.HelpMenu.Text = "&Help";
            // 
            // HelpAboutMenuItem
            // 
            this.HelpAboutMenuItem.Name = "HelpAboutMenuItem";
            this.HelpAboutMenuItem.Size = new System.Drawing.Size(191, 22);
            this.HelpAboutMenuItem.Text = "&About Bin Packer Tool";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(764, 461);
            this.Controls.Add(this.UiSplitView);
            this.Controls.Add(this.MainMenu);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MainMenuStrip = this.MainMenu;
            this.MinimumSize = new System.Drawing.Size(780, 500);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Bin Packer Tool";
            ((System.ComponentModel.ISupportInitialize)(this.RenderTarget)).EndInit();
            this.UiSplitView.Panel1.ResumeLayout(false);
            this.UiSplitView.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.UiSplitView)).EndInit();
            this.UiSplitView.ResumeLayout(false);
            this.NodeManagementPanel.ResumeLayout(false);
            this.MainMenu.ResumeLayout(false);
            this.MainMenu.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox RenderTarget;
        private System.Windows.Forms.SplitContainer UiSplitView;
        private System.Windows.Forms.MenuStrip MainMenu;
        private System.Windows.Forms.ToolStripMenuItem FileMenu;
        private System.Windows.Forms.ToolStripMenuItem FileNewMenuItem;
        private System.Windows.Forms.ToolStripMenuItem FileOpenMenuItem;
        private System.Windows.Forms.ToolStripMenuItem FileSaveMenuItem;
        private System.Windows.Forms.ToolStripMenuItem FileSaveAsMenuItem;
        private System.Windows.Forms.ToolStripSeparator FileMenuSeparator1;
        private System.Windows.Forms.ToolStripMenuItem FileExitMenuItem;
        private System.Windows.Forms.ToolStripMenuItem HelpMenu;
        private System.Windows.Forms.ToolStripMenuItem HelpAboutMenuItem;
        private System.Windows.Forms.ListBox NodeListBox;
        private System.Windows.Forms.Panel NodeManagementPanel;
        private System.Windows.Forms.Button RemoveButton;
        private System.Windows.Forms.Button AddButton;
        private System.Windows.Forms.Button RefreshButton;
        private System.Windows.Forms.ToolStripMenuItem CanvasMenu;
        private System.Windows.Forms.ToolStripMenuItem CanvasAtlasSizeMenuItem;
    }
}

