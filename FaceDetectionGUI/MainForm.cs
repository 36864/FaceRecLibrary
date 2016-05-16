using System;
using System.Windows.Forms;
using System.Drawing;
using OpenCvSharp.CPlusPlus;
using System.Collections.Generic;
using FaceRecLibrary;
using System.Threading.Tasks;
using System.Threading;
using System.IO;

namespace FaceDetectionGUI
{
    public partial class MainForm : Form
    {

        private const string DEFAULT_CLASSIFIERS_FILE = "../../Data/Classifier/Default_Classifiers.cfg";
        
        //Info on selected images
        private ImageInfo[] images;

        //Full paths to loaded classifiers
        private ClassifierInfo[] classifiers;

        /// <summary>
        /// Currently selected image index.
        /// Interlock.
        /// </summary>
        private int selectedIndex;

        /// <summary>
        /// Lock to prevent fast selection bugs
        /// </summary>
        private object selectionLock = new object();

        /// <summary>
        /// Lock to prevent excessive memory usage bugs
        /// </summary>
        private object detectionLock = new object();

        public MainForm()
        {
            InitializeComponent();
            LoadDefaultClassifiers();
        }

        private void LoadDefaultClassifiers()
        {
            string[] defaults = Util.Read_List(DEFAULT_CLASSIFIERS_FILE);
            classifiers = new ClassifierInfo[defaults.Length];
            for (int i = 0; i < defaults.Length; i++)
            {
                string[] classifier_info = defaults[i].Split(':');
                classifiers[i] = new ClassifierInfo( classifier_info[0]);
                if (!File.Exists(classifiers[i].Path))
                {
                    string cfgDir = DEFAULT_CLASSIFIERS_FILE.Substring(0, DEFAULT_CLASSIFIERS_FILE.LastIndexOfAny(new char[] { '/', '\\' })+1);
                    if (File.Exists(cfgDir + classifiers[i].Path))
                        classifiers[i].Path = cfgDir + classifiers[i].Path;
                    else {
                        classifiers[i].Path = null;
                        continue;
                    }
                }
                    
                if (classifier_info.Length > 1)
                    classifiers[i].Scale = double.Parse(classifier_info[1]);
                if (classifier_info.Length > 2)
                    classifiers[i].MinNeighbors = int.Parse(classifier_info[2]);
            }
        }

        private void excludeSubdirectoriesToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void includeSubdirectoriesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            folderBrowserDialogIncludeSubs.ShowDialog();
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
            listSelectedImages.SelectedIndexChanged -= listSelectedImages_SelectedIndexChanged;

           // lock (selectionLock)
            //{
                //Free memory
                if (pictureBox.Image != null)
                {
                    pictureBox.Image.Dispose();
                    pictureBox.Image = null;
                    GC.Collect();
                }

                selectedIndex = listSelectedImages.SelectedIndex;
                selectedIndex = listSelectedImages.SelectedIndex;

                //Load image from path
                
                ImageInfo image = images[selectedIndex];
                pictureBox.Image = Image.FromFile(image.Path);
                
                //Adjust SizeMode to fit image
                if (pictureBox.Image.Width > pictureBox.Width || pictureBox.Image.Height > pictureBox.Height)
                    pictureBox.SizeMode = PictureBoxSizeMode.Zoom;
                else
                    pictureBox.SizeMode = PictureBoxSizeMode.CenterImage;

                //Run detection using loaded classifiers
                pictureBox.Hide();

                RunDetection();

                pictureBox.Show();
            //}
        }

        async Task RunDetection()
        {    
            int index = selectedIndex;
            ImageInfo image = images[index];
            if (image.Detections != null)
            {

                listSelectedImages.SelectedIndexChanged += listSelectedImages_SelectedIndexChanged; return;
            }
            using (Mat img = new Mat(image.Path))
            {
                using (Mat resized = Util.ResizeImage(new Mat(image.Path), 200, 200, out image.Scale))
                    image.Detections = FaceDetect.RunDetection(resized, classifiers);
            }
            if (index == selectedIndex)
                pictureBox.Invalidate();

            listSelectedImages.SelectedIndexChanged += listSelectedImages_SelectedIndexChanged;
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
                //Check if detection has been run on image
                var image = images[listSelectedImages.SelectedIndex];
                if (image.Detections == null) return;

                //Find current image scale and position
                Graphics g = e.Graphics;                
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
                
                //Draw detection rectangles on image
                foreach (var detections in image.Detections)
                    if(detections.Length > 0)
                        g.DrawRectangles(Pens.Blue, Util.CvtRects(detections, scale, imageX, imageY));
            }
        }
    }
}
