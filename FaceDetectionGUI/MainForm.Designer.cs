namespace FaceDetectionGUI
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
            this.openImagesDialog = new System.Windows.Forms.OpenFileDialog();
            this.listSelectedImages = new System.Windows.Forms.ListBox();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openFilesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openDirectoryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.includeSubdirectoriesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.excludeSubdirectoriesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.identifyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pictureBox = new System.Windows.Forms.PictureBox();
            this.loadConfigDialog = new System.Windows.Forms.OpenFileDialog();
            this.folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.panelImageContainer = new System.Windows.Forms.Panel();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
            this.panelImageContainer.SuspendLayout();
            this.SuspendLayout();
            // 
            // openImagesDialog
            // 
            this.openImagesDialog.FileName = "openFileDialog1";
            this.openImagesDialog.Filter = "Supported Image Types|*.jpg;*.jpeg;*.png;*.bmp;*.tiff;*.tif;*.sr;*.ras;*.jp2;*.jp" +
    "e;*.dib";
            this.openImagesDialog.Multiselect = true;
            this.openImagesDialog.RestoreDirectory = true;
            // 
            // listSelectedImages
            // 
            this.listSelectedImages.Dock = System.Windows.Forms.DockStyle.Left;
            this.listSelectedImages.FormattingEnabled = true;
            this.listSelectedImages.ItemHeight = 16;
            this.listSelectedImages.Location = new System.Drawing.Point(0, 28);
            this.listSelectedImages.Name = "listSelectedImages";
            this.listSelectedImages.Size = new System.Drawing.Size(174, 495);
            this.listSelectedImages.TabIndex = 1;
            this.listSelectedImages.SelectedIndexChanged += new System.EventHandler(this.listSelectedImages_SelectedIndexChanged);
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.settingsToolStripMenuItem,
            this.identifyToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(907, 28);
            this.menuStrip1.TabIndex = 2;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openFilesToolStripMenuItem,
            this.openDirectoryToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(44, 24);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // openFilesToolStripMenuItem
            // 
            this.openFilesToolStripMenuItem.Name = "openFilesToolStripMenuItem";
            this.openFilesToolStripMenuItem.Size = new System.Drawing.Size(166, 26);
            this.openFilesToolStripMenuItem.Text = "Open File(s)";
            this.openFilesToolStripMenuItem.Click += new System.EventHandler(this.openFilesToolStripMenuItem_Click);
            // 
            // openDirectoryToolStripMenuItem
            // 
            this.openDirectoryToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.includeSubdirectoriesToolStripMenuItem,
            this.excludeSubdirectoriesToolStripMenuItem});
            this.openDirectoryToolStripMenuItem.Name = "openDirectoryToolStripMenuItem";
            this.openDirectoryToolStripMenuItem.Size = new System.Drawing.Size(166, 26);
            this.openDirectoryToolStripMenuItem.Text = "Open Folder";
            // 
            // includeSubdirectoriesToolStripMenuItem
            // 
            this.includeSubdirectoriesToolStripMenuItem.Name = "includeSubdirectoriesToolStripMenuItem";
            this.includeSubdirectoriesToolStripMenuItem.Size = new System.Drawing.Size(210, 26);
            this.includeSubdirectoriesToolStripMenuItem.Text = "Include Subfolders";
            this.includeSubdirectoriesToolStripMenuItem.Click += new System.EventHandler(this.includeSubdirectoriesToolStripMenuItem_Click);
            // 
            // excludeSubdirectoriesToolStripMenuItem
            // 
            this.excludeSubdirectoriesToolStripMenuItem.Name = "excludeSubdirectoriesToolStripMenuItem";
            this.excludeSubdirectoriesToolStripMenuItem.Size = new System.Drawing.Size(210, 26);
            this.excludeSubdirectoriesToolStripMenuItem.Text = "Exclude Subfolders";
            this.excludeSubdirectoriesToolStripMenuItem.Click += new System.EventHandler(this.excludeSubdirectoriesToolStripMenuItem_Click);
            // 
            // settingsToolStripMenuItem
            // 
            this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            this.settingsToolStripMenuItem.Size = new System.Drawing.Size(74, 24);
            this.settingsToolStripMenuItem.Text = "Settings";
            this.settingsToolStripMenuItem.Click += new System.EventHandler(this.settingsToolStripMenuItem_Click);
            // 
            // identifyToolStripMenuItem
            // 
            this.identifyToolStripMenuItem.CheckOnClick = true;
            this.identifyToolStripMenuItem.Name = "identifyToolStripMenuItem";
            this.identifyToolStripMenuItem.Size = new System.Drawing.Size(71, 24);
            this.identifyToolStripMenuItem.Text = "Identify";
            this.identifyToolStripMenuItem.Click += new System.EventHandler(this.identifyToolStripMenuItem_Click);
            // 
            // pictureBox
            // 
            this.pictureBox.Cursor = System.Windows.Forms.Cursors.Default;
            this.pictureBox.Location = new System.Drawing.Point(6, 3);
            this.pictureBox.Name = "pictureBox";
            this.pictureBox.Size = new System.Drawing.Size(244, 181);
            this.pictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox.TabIndex = 3;
            this.pictureBox.TabStop = false;
            this.pictureBox.Paint += new System.Windows.Forms.PaintEventHandler(this.pictureBox_Paint);
            this.pictureBox.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictureBox_MouseDown);
            this.pictureBox.MouseEnter += new System.EventHandler(this.pictureBox_MouseEnter);
            this.pictureBox.MouseLeave += new System.EventHandler(this.pictureBox_MouseLeave);
            this.pictureBox.MouseHover += new System.EventHandler(this.pictureBox_MouseHover);
            this.pictureBox.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pictureBox_MouseMove);
            this.pictureBox.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pictureBox_MouseUp);
            // 
            // loadConfigDialog
            // 
            this.loadConfigDialog.Filter = "XML Files|*.xml";
            this.loadConfigDialog.Multiselect = true;
            this.loadConfigDialog.RestoreDirectory = true;
            // 
            // panelImageContainer
            // 
            this.panelImageContainer.Controls.Add(this.pictureBox);
            this.panelImageContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelImageContainer.Location = new System.Drawing.Point(174, 28);
            this.panelImageContainer.Name = "panelImageContainer";
            this.panelImageContainer.Size = new System.Drawing.Size(733, 495);
            this.panelImageContainer.TabIndex = 4;
            this.panelImageContainer.Resize += new System.EventHandler(this.panelImageContainer_Resize);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(907, 523);
            this.Controls.Add(this.panelImageContainer);
            this.Controls.Add(this.listSelectedImages);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainForm";
            this.Text = "Form1";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
            this.panelImageContainer.ResumeLayout(false);
            this.panelImageContainer.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.OpenFileDialog openImagesDialog;
        private System.Windows.Forms.ListBox listSelectedImages;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem excludeSubdirectoriesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem includeSubdirectoriesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openDirectoryToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openFilesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.PictureBox pictureBox;
        private System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem;
        private System.Windows.Forms.OpenFileDialog loadConfigDialog;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog;
        private System.Windows.Forms.Panel panelImageContainer;
        private System.Windows.Forms.ToolStripMenuItem identifyToolStripMenuItem;
    }
}

