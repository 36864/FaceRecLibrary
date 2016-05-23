using System;
using System.Windows.Forms;
using System.Drawing;
using OpenCvSharp.CPlusPlus;
using System.Linq;
using FaceRecLibrary;
using System.Threading.Tasks;
using System.IO;
using System.Collections.Generic;
using System.Globalization;

namespace FaceDetectionGUI
{
    public partial class MainForm : Form
    {
        private const string DEFAULT_CLASSIFIERS_FILE = "../../Data/Classifier/Default_Classifiers.cfg";
        private const string SAVED_DATA_PATH = "../../Data/Saved/";

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
            LoadConfig(DEFAULT_CLASSIFIERS_FILE);
        }

        private void LoadConfig(string configFile)
        {
            string[] defaults = Util.Read_List(configFile);
            classifiers = new ClassifierInfo[defaults.Length];
            for (int i = 0; i < defaults.Length; i++)
            {
                try
                {
                    classifiers[i] = LoadClassifier(defaults[i], configFile);
                }
                catch (FileNotFoundException e)
                {
                    MessageBox.Show("File Not Found: " + e.Message);
                }
            }
        }

        private ClassifierInfo LoadClassifier(string data, string configFile)
        {
            string cfgDir = Path.GetDirectoryName(configFile) + '\\'; //configFile.Substring(0, configFile.LastIndexOfAny(new char[] { '/', '\\' }) + 1);
            int lastIndex = data.LastIndexOf("\\") + 1;
            string classifier = data;
            string path = null;
            string[] classifierValues = null;
            if (lastIndex > -1)
            {
                classifier = data.Substring(lastIndex);
                path = data.Substring(0, lastIndex);
            }
            classifierValues = classifier.Split(':');
            ClassifierInfo c = new ClassifierInfo(classifierValues[0]);
            if (path != null) c.Path = path;

            if (!File.Exists(c.FullName))
            {
                if (File.Exists(cfgDir + c.Name))
                    c.Path = cfgDir;
                else
                    throw new FileNotFoundException(c.FullName);
            }

            if (classifierValues.Length > 1)
            {
                c.Scale = double.Parse(classifierValues[1], CultureInfo.InvariantCulture);
                if (classifierValues.Length > 2)
                    c.MinNeighbors = int.Parse(classifierValues[2]);
            }
            return c;
        }

        private void LoadAllSupportedFiles(string root, bool includeSubFolders)
        {
            string[] fileNames = Directory.GetFiles(root);            
            listSelectedImages.Items.AddRange(fileNames.Select((f) => Path.GetFileName(f)).ToArray());
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

            //Set a mode that allows pictureBox resizing
            pictureBox.SizeMode = PictureBoxSizeMode.Zoom;
          
            //Resize
            pictureBox.MaximumSize = pictureBox.Image.Size;

            //pictureBox.Width = Math.Min(pictureBox.Image.Width, panelImageContainer.Width);
            //pictureBox.Height = Math.Min(pictureBox.Image.Height, panelImageContainer.Height);
            pictureBox.Size = ScaleSize(pictureBox.Image.Size, panelImageContainer.Size);
            
            //Recenter
            pictureBox.Left = (panelImageContainer.Width - pictureBox.Width) / 2;
            pictureBox.Top = Math.Max(0, (panelImageContainer.Height - pictureBox.Height) / 2);

            //Adjust SizeMode to fit image
            if (pictureBox.Image.Width > pictureBox.Width || pictureBox.Image.Height > pictureBox.Height)
                pictureBox.SizeMode = PictureBoxSizeMode.Zoom;
            else
                pictureBox.SizeMode = PictureBoxSizeMode.CenterImage;

            //Redraw
            pictureBox.Invalidate();
        }

        private ImageInfo LoadSavedData(int hash)
        {
            StreamReader sr = new StreamReader(File.OpenRead(SAVED_DATA_PATH + hash + ".dat"));
            string[] data = sr.ReadToEnd().Split('\n');
            ImageInfo retVal = new ImageInfo(data[0]);
            retVal.Scale = int.Parse(data[1]);
            int classifierCount = int.Parse(data[2]);
            retVal.Detections = new Rect[classifierCount][];
            int idx = 3;
            for (int i = 0; i < classifierCount; ++i)
            {
                int detectionCount = int.Parse(data[i+3]);
                retVal.Detections[i] = new Rect[detectionCount];
                ++idx;
            }

            return retVal;
        }

        private bool HasSavedData(int hash)
        {
            if (File.Exists(SAVED_DATA_PATH + hash + ".dat"))
                return true;
            return false;            
        }


        private void SaveData(ImageInfo[] toSave)
        {
            foreach (var item in toSave)
            {
                SaveData(toSave);
            }
        }

        private void SaveData(ImageInfo toSave)
        {
            int hash = toSave.Path.GetHashCode();
            StreamWriter fs = new StreamWriter(File.Create(SAVED_DATA_PATH + hash + ".dat"));
            fs.WriteLine(toSave.Path);
            fs.WriteLine(toSave.Scale);
            fs.WriteLine(toSave.Detections.Length);
            foreach (var detections in toSave.Detections)
            {
                foreach (var detection in detections)
                {
                    fs.WriteLine(detection.Top + "|" + detection.Left + "|" + detection.Width + "|" + detection.Height + "|");
                }
            }
        }

        #region EventHandlers
        private void excludeSubdirectoriesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog.ShowDialog() != DialogResult.OK)
                return;
            string path = folderBrowserDialog.SelectedPath;
            listSelectedImages.Items.Clear();
            images = new List<ImageInfo>();
            LoadAllSupportedFiles(path, false);
        }


        private void includeSubdirectoriesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(folderBrowserDialog.ShowDialog() != DialogResult.OK)
                return;
            
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
                images.Add(new ImageInfo(openImagesDialog.FileNames[i]));
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

            //Check saved data
            if (HasSavedData(images[selectedIndex].Path.GetHashCode()))
            {
                images[selectedIndex] = LoadSavedData(images[selectedIndex].Path.GetHashCode());
            }

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
                    if (detections.Length > 0)
                        g.DrawRectangles(Pens.Blue, Util.CvtRects(detections, image.Scale, pictureBox.Image.Width, pictureBox.Image.Height, pictureBox.Width, pictureBox.Height));
            }
        }

        private void panelImageContainer_Resize(object sender, EventArgs e)
        {
            ResizePictureBox();
        }
        #endregion
    }
}

