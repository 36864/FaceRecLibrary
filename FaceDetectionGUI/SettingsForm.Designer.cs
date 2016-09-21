namespace FaceDetectionGUI
{
    partial class SettingsForm
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
            this.label1 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.textBoxConfidence = new System.Windows.Forms.TextBox();
            this.lblConfidence = new System.Windows.Forms.Label();
            this.lblHeight = new System.Windows.Forms.Label();
            this.lblWidth = new System.Windows.Forms.Label();
            this.textBoxSizeHeight = new System.Windows.Forms.TextBox();
            this.textBoxMinNeighbors = new System.Windows.Forms.TextBox();
            this.textBoxSizeWidth = new System.Windows.Forms.TextBox();
            this.textBoxScale = new System.Windows.Forms.TextBox();
            this.lblDimensions = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.btnSave = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.listClassifiers = new System.Windows.Forms.ListBox();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(72, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "Classifiers";
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this.textBoxConfidence);
            this.panel1.Controls.Add(this.lblConfidence);
            this.panel1.Controls.Add(this.lblHeight);
            this.panel1.Controls.Add(this.lblWidth);
            this.panel1.Controls.Add(this.textBoxSizeHeight);
            this.panel1.Controls.Add(this.textBoxMinNeighbors);
            this.panel1.Controls.Add(this.textBoxSizeWidth);
            this.panel1.Controls.Add(this.textBoxScale);
            this.panel1.Controls.Add(this.lblDimensions);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Location = new System.Drawing.Point(178, 34);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(410, 164);
            this.panel1.TabIndex = 2;
            // 
            // textBoxConfidence
            // 
            this.textBoxConfidence.Location = new System.Drawing.Point(327, 64);
            this.textBoxConfidence.Name = "textBoxConfidence";
            this.textBoxConfidence.Size = new System.Drawing.Size(67, 22);
            this.textBoxConfidence.TabIndex = 10;
            // 
            // lblConfidence
            // 
            this.lblConfidence.AutoSize = true;
            this.lblConfidence.Location = new System.Drawing.Point(7, 67);
            this.lblConfidence.Name = "lblConfidence";
            this.lblConfidence.Size = new System.Drawing.Size(79, 17);
            this.lblConfidence.TabIndex = 9;
            this.lblConfidence.Text = "Confidence";
            // 
            // lblHeight
            // 
            this.lblHeight.AutoSize = true;
            this.lblHeight.Location = new System.Drawing.Point(190, 137);
            this.lblHeight.Name = "lblHeight";
            this.lblHeight.Size = new System.Drawing.Size(49, 17);
            this.lblHeight.TabIndex = 8;
            this.lblHeight.Text = "Height";
            // 
            // lblWidth
            // 
            this.lblWidth.AutoSize = true;
            this.lblWidth.Location = new System.Drawing.Point(7, 137);
            this.lblWidth.Name = "lblWidth";
            this.lblWidth.Size = new System.Drawing.Size(44, 17);
            this.lblWidth.TabIndex = 7;
            this.lblWidth.Text = "Width";
            // 
            // textBoxSizeHeight
            // 
            this.textBoxSizeHeight.Location = new System.Drawing.Point(245, 134);
            this.textBoxSizeHeight.Name = "textBoxSizeHeight";
            this.textBoxSizeHeight.Size = new System.Drawing.Size(100, 22);
            this.textBoxSizeHeight.TabIndex = 6;
            // 
            // textBoxMinNeighbors
            // 
            this.textBoxMinNeighbors.Location = new System.Drawing.Point(327, 36);
            this.textBoxMinNeighbors.Name = "textBoxMinNeighbors";
            this.textBoxMinNeighbors.Size = new System.Drawing.Size(67, 22);
            this.textBoxMinNeighbors.TabIndex = 5;
            // 
            // textBoxSizeWidth
            // 
            this.textBoxSizeWidth.Location = new System.Drawing.Point(57, 134);
            this.textBoxSizeWidth.Name = "textBoxSizeWidth";
            this.textBoxSizeWidth.Size = new System.Drawing.Size(100, 22);
            this.textBoxSizeWidth.TabIndex = 4;
            // 
            // textBoxScale
            // 
            this.textBoxScale.Location = new System.Drawing.Point(327, 8);
            this.textBoxScale.Name = "textBoxScale";
            this.textBoxScale.Size = new System.Drawing.Size(67, 22);
            this.textBoxScale.TabIndex = 3;
            this.textBoxScale.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // lblDimensions
            // 
            this.lblDimensions.AutoSize = true;
            this.lblDimensions.Location = new System.Drawing.Point(7, 107);
            this.lblDimensions.Name = "lblDimensions";
            this.lblDimensions.Size = new System.Drawing.Size(81, 17);
            this.lblDimensions.TabIndex = 2;
            this.lblDimensions.Text = "Dimensions";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(7, 39);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(132, 17);
            this.label4.TabIndex = 1;
            this.label4.Text = "Minimum Neighbors";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(7, 11);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(43, 17);
            this.label3.TabIndex = 0;
            this.label3.Text = "Scale";
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.Location = new System.Drawing.Point(350, 238);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 9;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.button1_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(178, 13);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(57, 17);
            this.label2.TabIndex = 3;
            this.label2.Text = "Options";
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.Location = new System.Drawing.Point(431, 239);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 4;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(512, 238);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // listClassifiers
            // 
            this.listClassifiers.FormattingEnabled = true;
            this.listClassifiers.ItemHeight = 16;
            this.listClassifiers.Location = new System.Drawing.Point(16, 34);
            this.listClassifiers.Name = "listClassifiers";
            this.listClassifiers.Size = new System.Drawing.Size(156, 228);
            this.listClassifiers.TabIndex = 6;
            this.listClassifiers.SelectedIndexChanged += new System.EventHandler(this.listClassifiers_SelectedIndexChanged);
            // 
            // SettingsForm
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(600, 279);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.listClassifiers);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.label1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SettingsForm";
            this.Text = "Settings";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TextBox textBoxSizeHeight;
        private System.Windows.Forms.TextBox textBoxMinNeighbors;
        private System.Windows.Forms.TextBox textBoxSizeWidth;
        private System.Windows.Forms.TextBox textBoxScale;
        private System.Windows.Forms.Label lblDimensions;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblWidth;
        private System.Windows.Forms.Label lblHeight;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.ListBox listClassifiers;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.TextBox textBoxConfidence;
        private System.Windows.Forms.Label lblConfidence;
    }
}