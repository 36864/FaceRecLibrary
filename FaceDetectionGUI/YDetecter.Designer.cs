#region Copyright Syncfusion Inc. 2001 - 2016
// Copyright Syncfusion Inc. 2001 - 2016. All rights reserved.
// Use of this code is subject to the terms of our license.
// A copy of the current license can be obtained at any time by e-mailing
// licensing@syncfusion.com. Any infringement will be prosecuted under
// applicable laws. 
#endregion
namespace FaceDetectionGUI
{
    partial class YDetecter
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(YDetecter));
            this.optionsMenu = new Syncfusion.Windows.Forms.Tools.ToolStripEx();
            this.fileBtn = new System.Windows.Forms.ToolStripDropDownButton();
            this.openFileBtn = new System.Windows.Forms.ToolStripMenuItem();
            this.openFoldersBtn = new System.Windows.Forms.ToolStripMenuItem();
            this.includeSubFoldersBtn = new System.Windows.Forms.ToolStripMenuItem();
            this.excludeSubFoldersBtn = new System.Windows.Forms.ToolStripMenuItem();
            this.saveBtn = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveSelectedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveSelectedAsCopyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.settingsBtn = new System.Windows.Forms.ToolStripButton();
            this.identifyBtn = new System.Windows.Forms.ToolStripButton();
            this.listSelectedImages = new System.Windows.Forms.ListBox();
            this.panelImageContainer = new System.Windows.Forms.Panel();
            this.pictureBox = new System.Windows.Forms.PictureBox();
            this.openImagesDialog = new System.Windows.Forms.OpenFileDialog();
            this.folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnClearDetections = new System.Windows.Forms.Button();
            this.txtDetectionCount = new System.Windows.Forms.TextBox();
            this.listDetections = new System.Windows.Forms.ListBox();
            this.lblIdentityList = new System.Windows.Forms.Label();
            this.lblDetectionCount = new System.Windows.Forms.Label();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.btnClearImages = new System.Windows.Forms.Button();
            this.lblProcessing = new System.Windows.Forms.Label();
            this.optionsMenu.SuspendLayout();
            this.panelImageContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
            this.panel1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // optionsMenu
            // 
            this.optionsMenu.BackColor = System.Drawing.Color.Transparent;
            this.optionsMenu.ForeColor = System.Drawing.Color.MidnightBlue;
            this.optionsMenu.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.optionsMenu.Image = null;
            this.optionsMenu.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.optionsMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileBtn,
            this.settingsBtn,
            this.identifyBtn});
            this.optionsMenu.Location = new System.Drawing.Point(0, 0);
            this.optionsMenu.Name = "optionsMenu";
            this.optionsMenu.Office12Mode = false;
            this.optionsMenu.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.optionsMenu.Size = new System.Drawing.Size(907, 27);
            this.optionsMenu.TabIndex = 0;
            this.optionsMenu.Text = "toolStripEx1";
            // 
            // fileBtn
            // 
            this.fileBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.fileBtn.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openFileBtn,
            this.openFoldersBtn,
            this.saveBtn});
            this.fileBtn.ForeColor = System.Drawing.Color.Black;
            this.fileBtn.Image = ((System.Drawing.Image)(resources.GetObject("fileBtn.Image")));
            this.fileBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.fileBtn.Name = "fileBtn";
            this.fileBtn.Size = new System.Drawing.Size(46, 24);
            this.fileBtn.Text = "File";
            // 
            // openFileBtn
            // 
            this.openFileBtn.BackColor = System.Drawing.SystemColors.Control;
            this.openFileBtn.Image = ((System.Drawing.Image)(resources.GetObject("openFileBtn.Image")));
            this.openFileBtn.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.openFileBtn.ImageTransparentColor = System.Drawing.Color.Transparent;
            this.openFileBtn.Name = "openFileBtn";
            this.openFileBtn.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.openFileBtn.ShowShortcutKeys = false;
            this.openFileBtn.Size = new System.Drawing.Size(166, 26);
            this.openFileBtn.Text = "Open";
            this.openFileBtn.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            this.openFileBtn.ToolTipText = "Open folder/folders to select images.";
            this.openFileBtn.Click += new System.EventHandler(this.openFile_Click);
            // 
            // openFoldersBtn
            // 
            this.openFoldersBtn.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.includeSubFoldersBtn,
            this.excludeSubFoldersBtn});
            this.openFoldersBtn.Image = ((System.Drawing.Image)(resources.GetObject("openFoldersBtn.Image")));
            this.openFoldersBtn.ImageTransparentColor = System.Drawing.Color.Transparent;
            this.openFoldersBtn.Name = "openFoldersBtn";
            this.openFoldersBtn.Size = new System.Drawing.Size(166, 26);
            this.openFoldersBtn.Text = "Open Folder";
            // 
            // includeSubFoldersBtn
            // 
            this.includeSubFoldersBtn.Image = ((System.Drawing.Image)(resources.GetObject("includeSubFoldersBtn.Image")));
            this.includeSubFoldersBtn.Name = "includeSubFoldersBtn";
            this.includeSubFoldersBtn.Size = new System.Drawing.Size(210, 26);
            this.includeSubFoldersBtn.Text = "Include Subfolders";
            this.includeSubFoldersBtn.Click += new System.EventHandler(this.includeSubdirectories_Click);
            // 
            // excludeSubFoldersBtn
            // 
            this.excludeSubFoldersBtn.Image = ((System.Drawing.Image)(resources.GetObject("excludeSubFoldersBtn.Image")));
            this.excludeSubFoldersBtn.Name = "excludeSubFoldersBtn";
            this.excludeSubFoldersBtn.Size = new System.Drawing.Size(210, 26);
            this.excludeSubFoldersBtn.Text = "Exclude Subfolders";
            this.excludeSubFoldersBtn.Click += new System.EventHandler(this.excludeSubdirectories_Click);
            // 
            // saveBtn
            // 
            this.saveBtn.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.saveAllToolStripMenuItem,
            this.saveSelectedToolStripMenuItem,
            this.saveSelectedAsCopyToolStripMenuItem});
            this.saveBtn.Image = ((System.Drawing.Image)(resources.GetObject("saveBtn.Image")));
            this.saveBtn.ImageTransparentColor = System.Drawing.Color.Transparent;
            this.saveBtn.Name = "saveBtn";
            this.saveBtn.Size = new System.Drawing.Size(166, 26);
            this.saveBtn.Text = "Save";
            // 
            // saveAllToolStripMenuItem
            // 
            this.saveAllToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("saveAllToolStripMenuItem.Image")));
            this.saveAllToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Transparent;
            this.saveAllToolStripMenuItem.Name = "saveAllToolStripMenuItem";
            this.saveAllToolStripMenuItem.Size = new System.Drawing.Size(232, 26);
            this.saveAllToolStripMenuItem.Text = "Save All";
            this.saveAllToolStripMenuItem.Click += new System.EventHandler(this.saveAllToolStripMenuItem_Click);
            // 
            // saveSelectedToolStripMenuItem
            // 
            this.saveSelectedToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("saveSelectedToolStripMenuItem.Image")));
            this.saveSelectedToolStripMenuItem.Name = "saveSelectedToolStripMenuItem";
            this.saveSelectedToolStripMenuItem.Size = new System.Drawing.Size(232, 26);
            this.saveSelectedToolStripMenuItem.Text = "Save Selected";
            this.saveSelectedToolStripMenuItem.Click += new System.EventHandler(this.saveSelectedToolStripMenuItem_Click);
            // 
            // saveSelectedAsCopyToolStripMenuItem
            // 
            this.saveSelectedAsCopyToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("saveSelectedAsCopyToolStripMenuItem.Image")));
            this.saveSelectedAsCopyToolStripMenuItem.Name = "saveSelectedAsCopyToolStripMenuItem";
            this.saveSelectedAsCopyToolStripMenuItem.Size = new System.Drawing.Size(232, 26);
            this.saveSelectedAsCopyToolStripMenuItem.Text = "Save Selected as Copy";
            this.saveSelectedAsCopyToolStripMenuItem.Click += new System.EventHandler(this.saveSelectedAsCopyToolStripMenuItem_Click);
            // 
            // settingsBtn
            // 
            this.settingsBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.settingsBtn.ForeColor = System.Drawing.Color.Black;
            this.settingsBtn.Image = ((System.Drawing.Image)(resources.GetObject("settingsBtn.Image")));
            this.settingsBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.settingsBtn.Name = "settingsBtn";
            this.settingsBtn.Size = new System.Drawing.Size(66, 24);
            this.settingsBtn.Text = "Settings";
            this.settingsBtn.Click += new System.EventHandler(this.settings_Click);
            // 
            // identifyBtn
            // 
            this.identifyBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.identifyBtn.ForeColor = System.Drawing.Color.Black;
            this.identifyBtn.Image = ((System.Drawing.Image)(resources.GetObject("identifyBtn.Image")));
            this.identifyBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.identifyBtn.Name = "identifyBtn";
            this.identifyBtn.Size = new System.Drawing.Size(63, 24);
            this.identifyBtn.Text = "Identify";
            this.identifyBtn.Click += new System.EventHandler(this.identify_Click);
            // 
            // listSelectedImages
            // 
            this.listSelectedImages.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listSelectedImages.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.listSelectedImages.Font = new System.Drawing.Font("Microsoft YaHei", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.listSelectedImages.FormattingEnabled = true;
            this.listSelectedImages.ItemHeight = 19;
            this.listSelectedImages.Location = new System.Drawing.Point(0, 61);
            this.listSelectedImages.Name = "listSelectedImages";
            this.listSelectedImages.Size = new System.Drawing.Size(218, 363);
            this.listSelectedImages.TabIndex = 1;
            this.listSelectedImages.SelectedIndexChanged += new System.EventHandler(this.listSelectedImages_SelectedIndexChanged);
            // 
            // panelImageContainer
            // 
            this.panelImageContainer.Controls.Add(this.lblProcessing);
            this.panelImageContainer.Controls.Add(this.pictureBox);
            this.panelImageContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelImageContainer.Location = new System.Drawing.Point(229, 3);
            this.panelImageContainer.Name = "panelImageContainer";
            this.panelImageContainer.Size = new System.Drawing.Size(447, 490);
            this.panelImageContainer.TabIndex = 2;
            this.panelImageContainer.Resize += new System.EventHandler(this.panelImageContainer_Resize);
            // 
            // pictureBox
            // 
            this.pictureBox.Location = new System.Drawing.Point(104, 147);
            this.pictureBox.Name = "pictureBox";
            this.pictureBox.Size = new System.Drawing.Size(246, 158);
            this.pictureBox.TabIndex = 0;
            this.pictureBox.TabStop = false;
            this.pictureBox.Paint += new System.Windows.Forms.PaintEventHandler(this.pictureBox_Paint);
            this.pictureBox.MouseClick += new System.Windows.Forms.MouseEventHandler(this.pictureBox_MouseClick);
            this.pictureBox.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictureBox_MouseDown);
            this.pictureBox.MouseEnter += new System.EventHandler(this.pictureBox_MouseEnter);
            this.pictureBox.MouseLeave += new System.EventHandler(this.pictureBox_MouseLeave);
            this.pictureBox.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pictureBox_MouseMove);
            this.pictureBox.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pictureBox_MouseUp);
            // 
            // openImagesDialog
            // 
            this.openImagesDialog.FileName = "openFileDialog1";
            this.openImagesDialog.Filter = "Supported Image Types|*.jpg;*.jpeg;*.png;*.bmp;*.tiff;*.tif;*.sr;*.ras;*.jp2;*.jp" +
    "e;*.dib";
            this.openImagesDialog.Multiselect = true;
            // 
            // saveFileDialog1
            // 
            this.saveFileDialog1.DefaultExt = "jpg";
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.btnClearDetections);
            this.panel1.Controls.Add(this.txtDetectionCount);
            this.panel1.Controls.Add(this.listDetections);
            this.panel1.Controls.Add(this.lblIdentityList);
            this.panel1.Controls.Add(this.lblDetectionCount);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(682, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(222, 490);
            this.panel1.TabIndex = 1;
            // 
            // btnClearDetections
            // 
            this.btnClearDetections.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClearDetections.Location = new System.Drawing.Point(51, 437);
            this.btnClearDetections.Name = "btnClearDetections";
            this.btnClearDetections.Size = new System.Drawing.Size(128, 31);
            this.btnClearDetections.TabIndex = 5;
            this.btnClearDetections.Text = "Clear Detections";
            this.btnClearDetections.UseVisualStyleBackColor = true;
            this.btnClearDetections.Click += new System.EventHandler(this.btnClearDetections_Click);
            // 
            // txtDetectionCount
            // 
            this.txtDetectionCount.Location = new System.Drawing.Point(112, 30);
            this.txtDetectionCount.Name = "txtDetectionCount";
            this.txtDetectionCount.Size = new System.Drawing.Size(100, 22);
            this.txtDetectionCount.TabIndex = 3;
            // 
            // listDetections
            // 
            this.listDetections.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listDetections.FormattingEnabled = true;
            this.listDetections.ItemHeight = 16;
            this.listDetections.Location = new System.Drawing.Point(3, 75);
            this.listDetections.Name = "listDetections";
            this.listDetections.Size = new System.Drawing.Size(217, 356);
            this.listDetections.TabIndex = 4;
            this.listDetections.SelectedIndexChanged += new System.EventHandler(this.listDetections_SelectedIndexChanged);
            // 
            // lblIdentityList
            // 
            this.lblIdentityList.AutoSize = true;
            this.lblIdentityList.Location = new System.Drawing.Point(137, 55);
            this.lblIdentityList.Name = "lblIdentityList";
            this.lblIdentityList.Size = new System.Drawing.Size(75, 17);
            this.lblIdentityList.TabIndex = 1;
            this.lblIdentityList.Text = "Detections";
            // 
            // lblDetectionCount
            // 
            this.lblDetectionCount.AutoSize = true;
            this.lblDetectionCount.Location = new System.Drawing.Point(103, 10);
            this.lblDetectionCount.Name = "lblDetectionCount";
            this.lblDetectionCount.Size = new System.Drawing.Size(109, 17);
            this.lblDetectionCount.TabIndex = 0;
            this.lblDetectionCount.Text = "Detection Count";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.Controls.Add(this.panel2, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.panel1, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.panelImageContainer, 1, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.GrowStyle = System.Windows.Forms.TableLayoutPanelGrowStyle.FixedSize;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 27);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(907, 496);
            this.tableLayoutPanel1.TabIndex = 3;
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.label1);
            this.panel2.Controls.Add(this.btnClearImages);
            this.panel2.Controls.Add(this.listSelectedImages);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(3, 3);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(220, 490);
            this.panel2.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 17);
            this.label1.TabIndex = 5;
            this.label1.Text = "Images";
            // 
            // btnClearImages
            // 
            this.btnClearImages.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClearImages.Location = new System.Drawing.Point(40, 446);
            this.btnClearImages.Name = "btnClearImages";
            this.btnClearImages.Size = new System.Drawing.Size(126, 32);
            this.btnClearImages.TabIndex = 1;
            this.btnClearImages.Text = "Clear Image List";
            this.btnClearImages.UseVisualStyleBackColor = true;
            this.btnClearImages.Click += new System.EventHandler(this.btnClearImages_Click);
            // 
            // lblProcessing
            // 
            this.lblProcessing.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lblProcessing.AutoSize = true;
            this.lblProcessing.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblProcessing.Location = new System.Drawing.Point(120, 202);
            this.lblProcessing.Name = "lblProcessing";
            this.lblProcessing.Size = new System.Drawing.Size(219, 46);
            this.lblProcessing.TabIndex = 1;
            this.lblProcessing.Text = "Processing";
            this.lblProcessing.Visible = false;
            // 
            // YDetecter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BorderColor = System.Drawing.Color.Orange;
            this.BorderThickness = 10;
            this.CaptionBarColor = System.Drawing.Color.Orange;
            this.CaptionFont = new System.Drawing.Font("Microsoft YaHei", 10F);
            this.CaptionForeColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(907, 523);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.optionsMenu);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MetroColor = System.Drawing.Color.Orange;
            this.Name = "YDetecter";
            this.Text = "YDetecter";
            this.optionsMenu.ResumeLayout(false);
            this.optionsMenu.PerformLayout();
            this.panelImageContainer.ResumeLayout(false);
            this.panelImageContainer.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Syncfusion.Windows.Forms.Tools.ToolStripEx optionsMenu;
        private System.Windows.Forms.ToolStripDropDownButton fileBtn;
        private System.Windows.Forms.ToolStripButton settingsBtn;
        private System.Windows.Forms.ToolStripButton identifyBtn;
        private System.Windows.Forms.ToolStripMenuItem openFileBtn;
        private System.Windows.Forms.ToolStripMenuItem openFoldersBtn;
        private System.Windows.Forms.ToolStripMenuItem includeSubFoldersBtn;
        private System.Windows.Forms.ToolStripMenuItem excludeSubFoldersBtn;
        private System.Windows.Forms.ListBox listSelectedImages;
        private System.Windows.Forms.Panel panelImageContainer;
        private System.Windows.Forms.PictureBox pictureBox;
        private System.Windows.Forms.OpenFileDialog openImagesDialog;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog;
        private System.Windows.Forms.ToolStripMenuItem saveBtn;
        private System.Windows.Forms.ToolStripMenuItem saveAllToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveSelectedToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveSelectedAsCopyToolStripMenuItem;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TextBox txtDetectionCount;
        private System.Windows.Forms.Label lblIdentityList;
        private System.Windows.Forms.Label lblDetectionCount;
        private System.Windows.Forms.ListBox listDetections;
        private System.Windows.Forms.Button btnClearDetections;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnClearImages;
        private System.Windows.Forms.Label lblProcessing;
    }
}