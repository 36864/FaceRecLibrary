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
using System.Xml;
using System.Xml.Serialization;

namespace FaceDetectionGUI
{
    public partial class MainForm : Form
    {
        private const string DEFAULT_CLASSIFIERS_FILE = "../../Data/Classifier/Default_Classifiers.xml";
        private const string SAVED_DATA_PATH = "../../Data/Saved/";

        #region StateVars
        //Info on selected images
        private List<ImageInfo> images;

        //Loaded classifier information
        ClassifierList cList;

        /// <summary>
        /// Currently selected image index.
        /// Interlock.
        /// </summary>
        private int selectedIndex;


        #endregion

        public MainForm()
        {
            InitializeComponent();
            LoadConfig(DEFAULT_CLASSIFIERS_FILE);
            Directory.CreateDirectory(SAVED_DATA_PATH);
        }

        private void LoadConfig(string configFile)
        {
            XmlReader xReader = XmlReader.Create(configFile);
            XmlSerializer xSerializer = new XmlSerializer(typeof(ClassifierList));
            if(xSerializer.CanDeserialize(xReader))
               cList = (ClassifierList) xSerializer.Deserialize(xReader);
            else
                MessageBox.Show("Error loading configuration file");
            /*
            string[] defaults = Util.Read_List(configFile);
            faceClassifiers = new FaceClassifier[defaults.Length];
            for (int i = 0; i < defaults.Length; i++)
            {
                try
                {
                    faceClassifiers[i] = LoadClassifier(defaults[i], configFile);
                }
                catch (FileNotFoundException e)
                {
                    MessageBox.Show("File Not Found: " + e.Message);
                }
            }*/
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
            if (image.DetectionInfo != null)
            {

                listSelectedImages.SelectedIndexChanged += listSelectedImages_SelectedIndexChanged; return;
            }
           
           image.DetectionInfo = FaceDetect.RunDetection(image, cList);
           
            if (index == selectedIndex)
                pictureBox.Invalidate();

            listSelectedImages.SelectedIndexChanged += listSelectedImages_SelectedIndexChanged;
            SaveData(image);
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
            XmlReader xReader = XmlReader.Create(File.OpenRead(SAVED_DATA_PATH + hash + ".dat"));
            ImageInfo loadedInfo = null;
            XmlSerializer xSerializer = new XmlSerializer(typeof(ImageInfo));
            if (xSerializer.CanDeserialize(xReader))
            {
                loadedInfo = (ImageInfo)xSerializer.Deserialize(xReader);
                loadedInfo.IsSaved = true;
        }
            return loadedInfo;
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
            XmlSerializer xSerializer = new XmlSerializer(typeof(ImageInfo));
            

            if (toSave.IsSaved) return;
            int hash = toSave.Path.GetHashCode();
            FileInfo fi = new FileInfo(toSave.Path);
            string newFilePath = SAVED_DATA_PATH + hash + ".dat";
            XmlWriterSettings xSettings = new XmlWriterSettings();
            xSettings.Indent = true;
            XmlWriter xWriter = XmlWriter.Create(newFilePath, xSettings);
            xSerializer.Serialize(xWriter, toSave);
            toSave.IsSaved = true;
            xWriter.Flush();
            xWriter.Close();
            xWriter.Dispose();
        }

        #region EventHandlers
        private void excludeSubdirectoriesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //            folderLoadAction(false);
            string path = folderLoadAction();
            Task.Run(() => filesLoadAction(path, false));
            //LoadAllSupportedFiles(path, false);
        }


        private void includeSubdirectoriesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //folderLoadAction(true);
            string path = folderLoadAction();
            Task.Run(() => filesLoadAction(path, true));

            //LoadAllSupportedFiles(path, true);
        }

        private string folderLoadAction()
        {
            folderBrowserDialog.ShowDialog();
            string path = folderBrowserDialog.SelectedPath;
            listSelectedImages.Items.Clear();
            images = new List<ImageInfo>();
            return path ;
        }

        private void filesLoadAction(string path, bool includeSubs)
        {
            Dictionary<string, string> originalAndNewFilesPaths = new Dictionary<string, string>();
            //List<string> originalFilesPaths = Util.DetectImages(path, includeSubs);
            foreach (var originalFilesPaths in Util.LoadAllSupportedFiles(path, includeSubs))
            {
                int index = 0;
                foreach (var item in Util.FormatImages(originalFilesPaths, SAVED_DATA_PATH))
                {
                    originalAndNewFilesPaths.Add(originalFilesPaths[index++], item.ToString());
                    LoadFiles(item.ToString());
                }
            }
        }

        private void LoadFiles(string file)
        {
            listSelectedImages.Invoke(new Action(
                                      ()=> listSelectedImages.Items.Add(Path.GetFileName(file)) 
                                      ));
            images.Add(new ImageInfo(file));
            listSelectedImages.Invalidate();
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
            if (loadConfigDialog.ShowDialog() != DialogResult.OK)
                return;
            if(Path.GetExtension(loadConfigDialog.FileName).Equals(".xml"))
                LoadConfig(loadConfigDialog.FileName);
        }

        private void pictureBox_Paint(object sender, PaintEventArgs e)
        {
            base.OnPaint(e);
            if (listSelectedImages.SelectedIndex > -1 && images.Count > listSelectedImages.SelectedIndex)
            {
                //Check if detection has been run on image
                var image = images[listSelectedImages.SelectedIndex];
                if (image.DetectionInfo == null || image.DetectionInfo.Detections.Count < 1) return;
                
                //Find current image scale and position
                Graphics g = e.Graphics;

                image.DisplayScaleFactor = Util.FindScale(image.Width, image.Height, pictureBox.Width, pictureBox.Height);

                //Draw detection rectangles on image
                g.DrawRectangles(Pens.Blue, image.DetectionInfo.Detections.Select((d) => Util.ScaleRectangle(d.Area, image.DisplayScaleFactor)).ToArray());
            }
        }

        private void panelImageContainer_Resize(object sender, EventArgs e)
        {
            ResizePictureBox();
        }
        #endregion
    }
}

