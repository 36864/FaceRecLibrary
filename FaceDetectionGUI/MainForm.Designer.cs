namespace FaceDetectionGUI
{
    partial class MetroForm1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MetroForm1));
            this.openImagesDialog = new System.Windows.Forms.OpenFileDialog();
            this.listSelectedImages = new System.Windows.Forms.ListBox();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileBtn = new System.Windows.Forms.ToolStripMenuItem();
            this.openFile = new System.Windows.Forms.ToolStripMenuItem();
            this.openDirectory = new System.Windows.Forms.ToolStripMenuItem();
            this.includeSubdirectories = new System.Windows.Forms.ToolStripMenuItem();
            this.excludeSubdirectories = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveSelectedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAllAsCopyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveSelectedAsCopyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.settings = new System.Windows.Forms.ToolStripMenuItem();
            this.identify = new System.Windows.Forms.ToolStripMenuItem();
            this.pictureBox = new System.Windows.Forms.PictureBox();
            this.folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.panelImageContainer = new System.Windows.Forms.Panel();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
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
            this.listSelectedImages.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.listSelectedImages.Dock = System.Windows.Forms.DockStyle.Left;
            this.listSelectedImages.FormattingEnabled = true;
            this.listSelectedImages.ItemHeight = 16;
            this.listSelectedImages.Location = new System.Drawing.Point(0, 28);
            this.listSelectedImages.Name = "listSelectedImages";
            this.listSelectedImages.Size = new System.Drawing.Size(174, 490);
            this.listSelectedImages.TabIndex = 1;
            this.listSelectedImages.SelectedIndexChanged += new System.EventHandler(this.listSelectedImages_SelectedIndexChanged);
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileBtn,
            this.settings,
            this.identify});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(907, 28);
            this.menuStrip1.TabIndex = 2;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileBtn
            // 
            this.fileBtn.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openFile,
            this.openDirectory,
            this.saveToolStripMenuItem});
            this.fileBtn.Name = "fileBtn";
            this.fileBtn.Size = new System.Drawing.Size(44, 24);
            this.fileBtn.Text = "File";
            // 
            // openFile
            // 
            this.openFile.Image = ((System.Drawing.Image)(resources.GetObject("openFile.Image")));
            this.openFile.Name = "openFile";
            this.openFile.Size = new System.Drawing.Size(166, 26);
            this.openFile.Text = "Open File(s)";
            this.openFile.Click += new System.EventHandler(this.openFile_Click);
            // 
            // openDirectory
            // 
            this.openDirectory.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.includeSubdirectories,
            this.excludeSubdirectories});
            this.openDirectory.Name = "openDirectory";
            this.openDirectory.Size = new System.Drawing.Size(166, 26);
            this.openDirectory.Text = "Open Folder";
            // 
            // includeSubdirectories
            // 
            this.includeSubdirectories.Name = "includeSubdirectories";
            this.includeSubdirectories.Size = new System.Drawing.Size(210, 26);
            this.includeSubdirectories.Text = "Include Subfolders";
            this.includeSubdirectories.Click += new System.EventHandler(this.includeSubdirectories_Click);
            // 
            // excludeSubdirectories
            // 
            this.excludeSubdirectories.Name = "excludeSubdirectories";
            this.excludeSubdirectories.Size = new System.Drawing.Size(210, 26);
            this.excludeSubdirectories.Text = "Exclude Subfolders";
            this.excludeSubdirectories.Click += new System.EventHandler(this.excludeSubdirectories_Click);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.saveAllToolStripMenuItem,
            this.saveSelectedToolStripMenuItem,
            this.saveAllAsCopyToolStripMenuItem,
            this.saveSelectedAsCopyToolStripMenuItem});
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(166, 26);
            this.saveToolStripMenuItem.Text = "Save";
            // 
            // saveAllToolStripMenuItem
            // 
            this.saveAllToolStripMenuItem.Name = "saveAllToolStripMenuItem";
            this.saveAllToolStripMenuItem.Size = new System.Drawing.Size(234, 26);
            this.saveAllToolStripMenuItem.Text = "Save All";
            this.saveAllToolStripMenuItem.Click += new System.EventHandler(this.saveAllToolStripMenuItem_Click);
            // 
            // saveSelectedToolStripMenuItem
            // 
            this.saveSelectedToolStripMenuItem.Name = "saveSelectedToolStripMenuItem";
            this.saveSelectedToolStripMenuItem.Size = new System.Drawing.Size(234, 26);
            this.saveSelectedToolStripMenuItem.Text = "Save Selected";
            this.saveSelectedToolStripMenuItem.Click += new System.EventHandler(this.saveSelectedToolStripMenuItem_Click);
            // 
            // saveAllAsCopyToolStripMenuItem
            // 
            this.saveAllAsCopyToolStripMenuItem.Name = "saveAllAsCopyToolStripMenuItem";
            this.saveAllAsCopyToolStripMenuItem.Size = new System.Drawing.Size(234, 26);
            this.saveAllAsCopyToolStripMenuItem.Text = "Save All As Copy";
            this.saveAllAsCopyToolStripMenuItem.Click += new System.EventHandler(this.saveAllAsCopyToolStripMenuItem_Click);
            // 
            // saveSelectedAsCopyToolStripMenuItem
            // 
            this.saveSelectedAsCopyToolStripMenuItem.Name = "saveSelectedAsCopyToolStripMenuItem";
            this.saveSelectedAsCopyToolStripMenuItem.Size = new System.Drawing.Size(234, 26);
            this.saveSelectedAsCopyToolStripMenuItem.Text = "Save Selected As Copy";
            this.saveSelectedAsCopyToolStripMenuItem.Click += new System.EventHandler(this.saveSelectedAsCopyToolStripMenuItem_Click);
            // 
            // settings
            // 
            this.settings.Name = "settings";
            this.settings.Size = new System.Drawing.Size(74, 24);
            this.settings.Text = "Settings";
            this.settings.Click += new System.EventHandler(this.settings_Click);
            // 
            // identify
            // 
            this.identify.CheckOnClick = true;
            this.identify.Name = "identify";
            this.identify.Size = new System.Drawing.Size(71, 24);
            this.identify.Text = "Identify";
            this.identify.Click += new System.EventHandler(this.identify_Click);
            // 
            // pictureBox
            // 
            this.pictureBox.Location = new System.Drawing.Point(6, 3);
            this.pictureBox.Name = "pictureBox";
            this.pictureBox.Size = new System.Drawing.Size(244, 181);
            this.pictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox.TabIndex = 3;
            this.pictureBox.TabStop = false;
            this.pictureBox.Paint += new System.Windows.Forms.PaintEventHandler(this.pictureBox_Paint);
            this.pictureBox.MouseClick += new System.Windows.Forms.MouseEventHandler(this.pictureBox_MouseClick);
            this.pictureBox.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictureBox_MouseDown);
            this.pictureBox.MouseEnter += new System.EventHandler(this.pictureBox_MouseEnter);
            this.pictureBox.MouseLeave += new System.EventHandler(this.pictureBox_MouseLeave);
            this.pictureBox.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pictureBox_MouseMove);
            this.pictureBox.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pictureBox_MouseUp);
            // 
            // panelImageContainer
            // 
            this.panelImageContainer.Controls.Add(this.pictureBox);
            this.panelImageContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelImageContainer.Location = new System.Drawing.Point(174, 28);
            this.panelImageContainer.Name = "panelImageContainer";
            this.panelImageContainer.Size = new System.Drawing.Size(733, 490);
            this.panelImageContainer.TabIndex = 4;
            this.panelImageContainer.Resize += new System.EventHandler(this.panelImageContainer_Resize);
            // 
            // MetroForm1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BorderColor = System.Drawing.Color.Orange;
            this.BorderThickness = 10;
            this.CaptionBarColor = System.Drawing.Color.Orange;
            this.CaptionBarHeight = 40;
            this.CaptionFont = new System.Drawing.Font("Microsoft YaHei", 10F);
            this.CaptionForeColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(907, 518);
            this.Controls.Add(this.panelImageContainer);
            this.Controls.Add(this.listSelectedImages);
            this.Controls.Add(this.menuStrip1);
            this.DropShadow = true;
            this.ForeColor = System.Drawing.Color.Black;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.MetroColor = System.Drawing.Color.Orange;
            this.Name = "MetroForm1";
            this.Text = "YDetecter";
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
        private System.Windows.Forms.ToolStripMenuItem excludeSubdirectories;
        private System.Windows.Forms.ToolStripMenuItem includeSubdirectories;
        private System.Windows.Forms.ToolStripMenuItem openDirectory;
        private System.Windows.Forms.ToolStripMenuItem openFile;
        private System.Windows.Forms.ToolStripMenuItem fileBtn;
        private System.Windows.Forms.PictureBox pictureBox;
        private System.Windows.Forms.ToolStripMenuItem settings;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog;
        private System.Windows.Forms.Panel panelImageContainer;
        private System.Windows.Forms.ToolStripMenuItem identifyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem identify;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveAllToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveSelectedToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveAllAsCopyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveSelectedAsCopyToolStripMenuItem;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
    }
}

