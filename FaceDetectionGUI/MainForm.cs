using System;
using System.Windows.Forms;
using System.Drawing;
using OpenCvSharp.CPlusPlus;
using System.Linq;
using FaceRecLibrary;
using System.Threading.Tasks;
using System.IO;
using System.Collections.Generic;

namespace FaceDetectionGUI
{
    public partial class MainForm : Form
    {

        private const string DEFAULT_CLASSIFIERS_FILE = "../../Data/Classifier/Default_Classifiers.cfg";


        #region StateVars
        //Info on selected images
        private List<ImageInfo> images;

        //Full paths to loaded classifiers
        private ClassifierInfo[] classifiers;

        /// <summary>
        /// Currently selected image index.
        /// Interlock.
        /// </summary>
        private int selectedIndex;

     /*   /// <summary>
        /// Lock to prevent fast selection bugs
        /// </summary>
        private object selectionLock = new object();

        /// <summary>
        /// Lock to prevent excessive memory usage bugs
        /// </summary>
        private object detectionLock = new object();*/
        #endregion


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

        private void LoadAllSupportedFiles(string root, bool includeSubFolders)
        {
            string[] fileNames = Directory.GetFiles(root);
            listSelectedImages.Items.AddRange(fileNames.Select((f) => f.Replace(root, "")).ToArray());
            for (int i = 0; i < fileNames.Length; i++)
            {
                images.Add(new ImageInfo(fileNames[i]));
            }
            if (includeSubFolders)
                foreach (var dir in Directory.GetDirectories(root))
                {
                    LoadAllSupportedFiles(dir, true);
                }
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
                using (Mat resized = Util.ResizeImage(img, 200, 200, out image.Scale))
                    image.Detections = FaceDetect.RunDetection(resized, classifiers);
            }
            if (index == selectedIndex)
                pictureBox.Invalidate();

            listSelectedImages.SelectedIndexChanged += listSelectedImages_SelectedIndexChanged;
        }

        private System.Drawing.Size ScaleSize(System.Drawing.Size image, System.Drawing.Size container)
        {
            System.Drawing.Size newSize = new System.Drawing.Size();
            double scale;
            double scaleX;
            double scaleY;

            scaleX = ((double)container.Width / image.Width);
            scaleY = ((double)container.Height / image.Height);
            scale = Math.Min(scaleX, scaleY);

            newSize.Height = (int)(image.Height * scale);
            newSize.Width = (int)(image.Width * scale);
            return newSize;
        }

        private void ResizePictureBox()
        {
            if (pictureBox.Image == null) return;


            //Resize
            pictureBox.MaximumSize = pictureBox.Image.Size;

            //pictureBox.Width = Math.Min(pictureBox.Image.Width, panelImageContainer.Width);
            //pictureBox.Height = Math.Min(pictureBox.Image.Height, panelImageContainer.Height);
            pictureBox.Size = ScaleSize(pictureBox.Image.Size, panelImageContainer.Size);

            //Recenter
            pictureBox.Left = (panelImageContainer.Width - pictureBox.Width) / 2;
            pictureBox.Top = (panelImageContainer.Height - pictureBox.Height) / 2;

            //Adjust SizeMode to fit image
            if (pictureBox.Image.Width > pictureBox.Width || pictureBox.Image.Height > pictureBox.Height)
                pictureBox.SizeMode = PictureBoxSizeMode.Zoom;
            else
                pictureBox.SizeMode = PictureBoxSizeMode.CenterImage;

            //Redraw
            pictureBox.Invalidate();
        }

        private void excludeSubdirectoriesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            folderBrowserDialog.ShowDialog();
            string path = folderBrowserDialog.SelectedPath;
            listSelectedImages.Items.Clear();
            images = new List<ImageInfo>();
            LoadAllSupportedFiles(path, false);
        }


        private void includeSubdirectoriesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            folderBrowserDialog.ShowDialog();
            string path = folderBrowserDialog.SelectedPath;
            listSelectedImages.Items.Clear();
            images = new List<ImageInfo>();
            LoadAllSupportedFiles(path, true);
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
            images = new List<ImageInfo>();
            for (int i = 0; i < openImagesDialog.FileNames.Length; i++)
            {
                images.Add( new ImageInfo(openImagesDialog.FileNames[i]));
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

            //Load image from path

            ImageInfo image = images[selectedIndex];
            pictureBox.Image = Image.FromFile(image.Path);

            //Resize PictureBox
            ResizePictureBox();

            //Run detection using loaded classifiers
            RunDetection();
            //}
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
            if (listSelectedImages.SelectedIndex > -1 && images.Count > listSelectedImages.SelectedIndex)
            {
                //Check if detection has been run on image
                var image = images[listSelectedImages.SelectedIndex];
                if (image.Detections == null) return;

                //Find current image scale and position
                Graphics g = e.Graphics;
                
                   //Draw detection rectangles on image
                foreach (var detections in image.Detections)
                    if(detections.Length > 0)
                        g.DrawRectangles(Pens.Blue, Util.CvtRects(detections, image.Scale, pictureBox.Image.Width, pictureBox.Image.Height, pictureBox.Width, pictureBox.Height));
            }
        }

        private void panelImageContainer_Resize(object sender, EventArgs e)
        {
            ResizePictureBox();
        }


    
    }
}

