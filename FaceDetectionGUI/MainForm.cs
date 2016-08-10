using System;
using System.Windows.Forms;
using System.Drawing;
using System.Linq;
using FaceRecLibrary;
using System.Threading.Tasks;
using System.IO;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using FaceRecLibrary.Utilities;
using FaceRecLibrary;

namespace FaceDetectionGUI
{
    public partial class MainForm : Form
    {

        private const string DEFAULT_RECOGNIZER_FILE = "../../../Data/Recognizer/Default_Recognizer.xml";
        private const string DEFAULT_CLASSIFIERS_FILE = "../../../Data/Classifier/Default_Classifiers.xml";
        private const string SAVED_DATA_PATH = "../../../Data/Saved/";
        private const string CACHED_IMAGES = "../../../Data/Cached/";

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

        private FaceRecLibrary.FaceRecLibrary faceRecLib;
        
        #endregion

        public MainForm()
        {
            LoadConfig(DEFAULT_CLASSIFIERS_FILE);
            Directory.CreateDirectory(SAVED_DATA_PATH);
            Directory.CreateDirectory(CACHED_IMAGES);
            faceRecLib = new FaceRecLibrary.FaceRecLibrary();
            faceRecLib.init(DEFAULT_CLASSIFIERS_FILE, null);
            InitializeComponent();
        }

        private void LoadConfig(string configFile)
        {
            cList = Util.LoadXmlConfigFile(configFile);
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

        void RunDetection()
        {
            int index = selectedIndex;
            ImageInfo image = images[index];
            if (image.DetectionInfo != null)
            {
                listSelectedImages.SelectedIndexChanged += listSelectedImages_SelectedIndexChanged;
                return;
            }
            faceRecLib.DetectAndRecognize(image);
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
            xReader.Close();
            xReader.Dispose();
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
            
            faceRecLib.SaveMetadata(toSave);
        }

   

        private void ClearSavedData()
        {
            Directory.Delete(SAVED_DATA_PATH, true);
            Directory.CreateDirectory(SAVED_DATA_PATH);
        }

        private string folderLoadAction()
        {
            if (folderBrowserDialog.ShowDialog() != DialogResult.OK)
                return null;
            string path = folderBrowserDialog.SelectedPath;
            listSelectedImages.Items.Clear();
            images = new List<ImageInfo>();
            return path;
        }

        private void filesLoadAction(string path, bool includeSubs)
        {
            Dictionary<string, string> originalAndNewFilesPaths = new Dictionary<string, string>();
            //List<string> originalFilesPaths = Util.DetectImages(path, includeSubs);
            foreach (var originalFilesPaths in Util.LoadAllSupportedFiles(path, includeSubs))
            {
                int index = 0;
                foreach (var item in Util.FormatImages(originalFilesPaths, CACHED_IMAGES))
                {
                    originalAndNewFilesPaths.Add(originalFilesPaths[index], item.ToString());
                    LoadFiles(originalFilesPaths[index++], item.ToString());
                }
            }
        }

        private void LoadFiles(string originalPath, string newPath)
        {
            listSelectedImages.Invoke(new Action(
                                      () => listSelectedImages.Items.Add(Path.GetFileName(originalPath))
                                      ));
            ImageInfo info = new ImageInfo(originalPath);
            info.Path = newPath;
            images.Add(info);
            listSelectedImages.Invalidate();
        }

        #region EventHandlers
        private void excludeSubdirectoriesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //            folderLoadAction(false);
            string path = folderLoadAction();
            if (path != null)
                Task.Run(() => filesLoadAction(path, false));
            //LoadAllSupportedFiles(path, false);
        }


        private void includeSubdirectoriesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //folderLoadAction(true);
            string path = folderLoadAction();
            if (path != null)
                Task.Run(() => filesLoadAction(path, true));

            //LoadAllSupportedFiles(path, true);
        }



        private void openFilesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Show file dialog
            if (openImagesDialog.ShowDialog() != DialogResult.OK)
                return;

            //Clear list
            listSelectedImages.Items.Clear();
            images = new List<ImageInfo>();

            //Load files
            Dictionary<string, string> originalAndNewFilesPaths = new Dictionary<string, string>();
            for (int index = 0; index < openImagesDialog.FileNames.Length; ++index)
            {
                string item = Util.FormatImage(openImagesDialog.FileNames[index], CACHED_IMAGES, Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
                originalAndNewFilesPaths.Add(openImagesDialog.FileNames[index], item.ToString());
                LoadFiles(openImagesDialog.FileNames[index], item.ToString());
            }

            //Select first image for display
            listSelectedImages.SelectedIndex = 0;
        }

        private void listSelectedImages_SelectedIndexChanged(object sender, EventArgs e)
        {
            listSelectedImages.SelectedIndexChanged -= listSelectedImages_SelectedIndexChanged;

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
            Task.Run(() => RunDetection());

        }



        private void loadClassifiersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Show file dialog
            if (loadConfigDialog.ShowDialog() != DialogResult.OK)
                return;
            if (Path.GetExtension(loadConfigDialog.FileName).Equals(".xml"))
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

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SettingsForm sForm = new SettingsForm(cList);
            sForm.ShowDialog();
            ClearSavedData();
            Util.SaveXmlConfigFile(cList, DEFAULT_CLASSIFIERS_FILE);
        }
        #endregion
    }
}
