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
            this.settingsBtn = new System.Windows.Forms.ToolStripButton();
            this.identifyBtn = new System.Windows.Forms.ToolStripButton();
            this.openFoldersToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.includeSubfoldersToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.excludeSubfoldersToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.listSelectedImages = new System.Windows.Forms.ListBox();
            this.panelImageContainer = new System.Windows.Forms.Panel();
            this.pictureBox = new System.Windows.Forms.PictureBox();
            this.openImagesDialog = new System.Windows.Forms.OpenFileDialog();
            this.folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.optionsMenu.SuspendLayout();
            this.panelImageContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
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
            this.optionsMenu.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.toolStripEx1_ItemClicked);
            // 
            // fileBtn
            // 
            this.fileBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.fileBtn.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openFileBtn,
            this.openFoldersToolStripMenuItem});
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
            this.openFileBtn.Name = "openFileBtn";
            this.openFileBtn.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.openFileBtn.ShowShortcutKeys = false;
            this.openFileBtn.Size = new System.Drawing.Size(181, 26);
            this.openFileBtn.Text = "Open";
            this.openFileBtn.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            this.openFileBtn.ToolTipText = "Open folder/folders to select images.";
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
            // 
            // openFoldersToolStripMenuItem
            // 
            this.openFoldersToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.includeSubfoldersToolStripMenuItem,
            this.excludeSubfoldersToolStripMenuItem});
            this.openFoldersToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("openFoldersToolStripMenuItem.Image")));
            this.openFoldersToolStripMenuItem.Name = "openFoldersToolStripMenuItem";
            this.openFoldersToolStripMenuItem.Size = new System.Drawing.Size(181, 26);
            this.openFoldersToolStripMenuItem.Text = "Open Folder";
            // 
            // includeSubfoldersToolStripMenuItem
            // 
            this.includeSubfoldersToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("includeSubfoldersToolStripMenuItem.Image")));
            this.includeSubfoldersToolStripMenuItem.Name = "includeSubfoldersToolStripMenuItem";
            this.includeSubfoldersToolStripMenuItem.Size = new System.Drawing.Size(210, 26);
            this.includeSubfoldersToolStripMenuItem.Text = "Include Subfolders";
            // 
            // excludeSubfoldersToolStripMenuItem
            // 
            this.excludeSubfoldersToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("excludeSubfoldersToolStripMenuItem.Image")));
            this.excludeSubfoldersToolStripMenuItem.Name = "excludeSubfoldersToolStripMenuItem";
            this.excludeSubfoldersToolStripMenuItem.Size = new System.Drawing.Size(210, 26);
            this.excludeSubfoldersToolStripMenuItem.Text = "Exclude Subfolders";
            // 
            // listSelectedImages
            // 
            this.listSelectedImages.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.listSelectedImages.Dock = System.Windows.Forms.DockStyle.Left;
            this.listSelectedImages.Font = new System.Drawing.Font("Microsoft YaHei", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.listSelectedImages.FormattingEnabled = true;
            this.listSelectedImages.ItemHeight = 19;
            this.listSelectedImages.Location = new System.Drawing.Point(0, 27);
            this.listSelectedImages.Name = "listSelectedImages";
            this.listSelectedImages.Size = new System.Drawing.Size(174, 496);
            this.listSelectedImages.TabIndex = 1;
            // 
            // panelImageContainer
            // 
            this.panelImageContainer.Controls.Add(this.pictureBox);
            this.panelImageContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelImageContainer.Location = new System.Drawing.Point(174, 27);
            this.panelImageContainer.Name = "panelImageContainer";
            this.panelImageContainer.Size = new System.Drawing.Size(733, 496);
            this.panelImageContainer.TabIndex = 2;
            // 
            // pictureBox
            // 
            this.pictureBox.Location = new System.Drawing.Point(0, 0);
            this.pictureBox.Name = "pictureBox";
            this.pictureBox.Size = new System.Drawing.Size(236, 158);
            this.pictureBox.TabIndex = 0;
            this.pictureBox.TabStop = false;
            // 
            // openImagesDialog
            // 
            this.openImagesDialog.FileName = "openFileDialog1";
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
            this.Controls.Add(this.panelImageContainer);
            this.Controls.Add(this.listSelectedImages);
            this.Controls.Add(this.optionsMenu);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MetroColor = System.Drawing.Color.Orange;
            this.Name = "YDetecter";
            this.Text = "YDetecter";
            this.optionsMenu.ResumeLayout(false);
            this.optionsMenu.PerformLayout();
            this.panelImageContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Syncfusion.Windows.Forms.Tools.ToolStripEx optionsMenu;
        private System.Windows.Forms.ToolStripDropDownButton fileBtn;
        private System.Windows.Forms.ToolStripButton settingsBtn;
        private System.Windows.Forms.ToolStripButton identifyBtn;
        private System.Windows.Forms.ToolStripMenuItem openFileBtn;
        private System.Windows.Forms.ToolStripMenuItem openFoldersToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem includeSubfoldersToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem excludeSubfoldersToolStripMenuItem;
        private System.Windows.Forms.ListBox listSelectedImages;
        private System.Windows.Forms.Panel panelImageContainer;
        private System.Windows.Forms.PictureBox pictureBox;
        private System.Windows.Forms.OpenFileDialog openImagesDialog;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog;
    }
}