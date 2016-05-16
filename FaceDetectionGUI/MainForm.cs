using System;
using System.Windows.Forms;
using System.Drawing;
using OpenCvSharp.CPlusPlus;
using System.Collections.Generic;
using FaceRecLibrary;

namespace FaceDetectionGUI
{
    public partial class MainForm : Form
    {
        

        //Info on selected images
        private ImageInfo[] images;

        //Full paths to loaded classifiers
        private ClassifierInfo[] classifiers;

        public MainForm()
        {
            InitializeComponent();
        }

        private void excludeSubdirectoriesToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void includeSubdirectoriesToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void openFilesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Show file dialog
            if (openImagesDialog.ShowDialog() != DialogResult.OK)
                return;
            
            //Clear list
            listSelectedImages.Items.Clear();

            //Add chosen files to list
            listSelectedImages.Items.AddRange(openImagesDialog.SafeFileNames);
            images = new ImageInfo[openImagesDialog.FileNames.Length];
            for (int i = 0; i < openImagesDialog.FileNames.Length; i++)
            {
                images[i] = new ImageInfo(openImagesDialog.FileNames[i]);
            }
                        
            //Select first image for display
            listSelectedImages.SelectedIndex = 0;
        }

        private void listSelectedImages_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Free memory
            if (pictureBox.Image != null)
                pictureBox.Image.Dispose();

            ImageInfo image = images[listSelectedImages.SelectedIndex];
            //Load image from path
            pictureBox.Image = Image.FromFile(image.Path);

            //Adjust SizeMode to fit image
            if (pictureBox.Image.Width > pictureBox.Width || pictureBox.Image.Height > pictureBox.Height)
                pictureBox.SizeMode = PictureBoxSizeMode.Zoom;
            else
                pictureBox.SizeMode = PictureBoxSizeMode.CenterImage;

            //Run detection using loaded classifiers
            pictureBox.Hide();
            using (Mat img = new Mat(image.Path))
            {
                using (Mat resized = Util.ResizeImage(new Mat(image.Path), 200, 200, out image.Scale))
                    image.Detections = FaceDetect.RunDetection(resized, classifiers);
            }
                
            pictureBox.Show();
        }

        private void pictureBox_Resize(object sender, EventArgs e)
        {
            if (pictureBox.Image == null) return;

            //Adjust SizeMode to fit image
            if (pictureBox.Image.Width > pictureBox.Width || pictureBox.Image.Height > pictureBox.Height)
                pictureBox.SizeMode = PictureBoxSizeMode.Zoom;
            else
                pictureBox.SizeMode = PictureBoxSizeMode.CenterImage;
        }

        private void loadClassifiersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Show file dialog
            if (openClassifiersDialog.ShowDialog() != DialogResult.OK)
                return;

            classifiers = new ClassifierInfo[openClassifiersDialog.FileNames.Length];
            for (int i = 0; i < classifiers.Length; i++)
            {
                classifiers[i] = new ClassifierInfo(openClassifiersDialog.FileNames[i]);
                
            }
        }

        private void pictureBox_Paint(object sender, PaintEventArgs e)
        {
            base.OnPaint(e);
            if (listSelectedImages.SelectedIndex > -1 && images.Length > listSelectedImages.SelectedIndex)
            {
                Graphics g = e.Graphics;
                var image = images[listSelectedImages.SelectedIndex];
                int imageX = 0;
                int imageY = 0;
                float scale = 0.0f;
                if (pictureBox.SizeMode == PictureBoxSizeMode.CenterImage)
                {
                    imageX = pictureBox.Width / 2 - pictureBox.Image.Width / 2;
                    imageY = pictureBox.Height / 2 - pictureBox.Image.Height / 2;
                    scale = image.Scale;
                }
                else
                {
                    scale = image.Scale *( pictureBox.Image.Width / pictureBox.Width);
                }
                foreach (var detections in image.Detections)
                    if(detections.Length > 0)
                        g.DrawRectangles(Pens.Blue, Util.CvtRects(detections, scale, imageX, imageY));
            }
        }
    }
}
