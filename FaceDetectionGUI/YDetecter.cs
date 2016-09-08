#region Copyright Syncfusion Inc. 2001 - 2016
// Copyright Syncfusion Inc. 2001 - 2016. All rights reserved.
// Use of this code is subject to the terms of our license.
// A copy of the current license can be obtained at any time by e-mailing
// licensing@syncfusion.com. Any infringement will be prosecuted under
// applicable laws. 
#endregion
using FaceRecLibrary;
using FaceRecLibrary.Utilities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;

namespace FaceDetectionGUI
{
    public partial class YDetecter : Syncfusion.Windows.Forms.MetroForm
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

        /// <summary>
        /// Identify if option is selected to create detection box
        /// </summary>
        private bool canIdentify = false, canDrawBox;
        Rectangle dragBox;
        private int initialX, initialY;
        private TextBox tb;
        private Dictionary<Rectangle, TextBox> detections = new Dictionary<Rectangle, TextBox>();

        #endregion
        public YDetecter()
        {
            LoadConfig(DEFAULT_CLASSIFIERS_FILE);
            Directory.CreateDirectory(SAVED_DATA_PATH);
            Directory.CreateDirectory(CACHED_IMAGES);
            faceRecLib = new FaceRecLibrary.FaceRecLibrary();
            faceRecLib.init(DEFAULT_CLASSIFIERS_FILE, null);
            InitializeComponent();
            DoubleBuffered = true;
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
        private Size ScaleSize(Size image, Size container)
        {
            Size newSize = new Size();
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
            faceRecLib.LoadMetadata(loadedInfo);
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
            faceRecLib.LoadMetadata(info);
            info.Path = newPath;
            images.Add(info);
            listSelectedImages.Invalidate();
        }

        #region EventHandlers
        private void excludeSubdirectories_Click(object sender, EventArgs e)
        {
            //            folderLoadAction(false);
            string path = folderLoadAction();
            if (path != null)
                Task.Run(() => filesLoadAction(path, false));
            //LoadAllSupportedFiles(path, false);
        }
        private void includeSubdirectories_Click(object sender, EventArgs e)
        {
            //folderLoadAction(true);
            string path = folderLoadAction();
            if (path != null)
                Task.Run(() => filesLoadAction(path, true));

            //LoadAllSupportedFiles(path, true);
        }
        private void openFile_Click(object sender, EventArgs e)
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
            faceRecLib.LoadMetadata(image);
            pictureBox.Image = Image.FromFile(image.Path);

            //Resize PictureBox
            ResizePictureBox();

            //Run detection using loaded classifiers
            Task.Run(() => RunDetection());

        }
        private void pictureBox_Paint(object sender, PaintEventArgs e)
        {
            base.OnPaint(e);
            if (listSelectedImages.SelectedIndex > -1 && images.Count > listSelectedImages.SelectedIndex)
            {
                //Check if detection has been run on image
                var image = images[listSelectedImages.SelectedIndex];
                if (!(image.DetectionInfo == null || image.DetectionInfo.Detections.Count < 1))
                {
                    //Find current image scale and position
                    Graphics g = e.Graphics;

                    //image.DisplayScaleFactor = Util.FindScale(image.Width, image.Height, pictureBox.Width, pictureBox.Height);

                    //Draw detection rectangles on image
                    g.DrawRectangles(Pens.Blue, image.DetectionInfo.Detections.Select((d) => Util.ScaleRectangle(d.Area, image.DisplayScaleFactor)).ToArray());
                }
                if (canIdentify)
                {
                    using (Pen pen = new Pen(Color.Orange, 2))
                    {
                        foreach (var item in detections.Keys)
                        {
                            e.Graphics.DrawRectangle(pen, item);
                        }
                        e.Graphics.DrawRectangle(pen, dragBox);
                    }
                }
            }
        }
        private void panelImageContainer_Resize(object sender, EventArgs e)
        {
            ResizePictureBox();
        }
        private void settings_Click(object sender, EventArgs e)
        {
            SettingsForm sForm = new SettingsForm(cList);
            sForm.ShowDialog();
            ClearSavedData();
            Util.SaveXmlConfigFile(cList, DEFAULT_CLASSIFIERS_FILE);
        }
        private void identify_Click(object sender, EventArgs e)
        {
            if (identifyBtn.Text == "Identify")
            {
                canIdentify = true;
                identifyBtn.Text = "Stop Identifying";
            }
            else if (identifyBtn.Text == "Stop Identifying")
            {
                foreach (TextBox item in detections.Values)
                {
                    item.Dispose();
                }
                detections.Clear();
                canIdentify = false;
                identifyBtn.Text = "Identify";
                Refresh();
            }
        }
        private void pictureBox_MouseDown(object sender, MouseEventArgs e)
        {
            if (tb != null && string.IsNullOrEmpty(tb.Text))
            {
                detections.Remove(dragBox);
                tb.Dispose();
            }

            if (canIdentify)
            {
                canDrawBox = true;
                initialX = e.X;
                initialY = e.Y;
            }
        }
        private void pictureBox_MouseMove(object sender, MouseEventArgs e)
        {
            if (!(canIdentify && canDrawBox)) return;

            //Condition to restrict the limits of the dragbox, in this way it will not get out of borders
            int eX = (e.X > 0) ? e.X : 0, eY = (e.Y > 0) ? e.Y : 0;
            if (e.X > pictureBox.Width) eX = pictureBox.Width;
            if (e.Y > pictureBox.Height) eY = pictureBox.Height;

            int x = Math.Min(initialX, eX);
            int y = Math.Min(initialY, eY);
            int width = Math.Max(initialX, eX) - Math.Min(initialX, eX);
            int height = Math.Max(initialY, eY) - Math.Min(initialY, eY);
            dragBox = new Rectangle(x, y, width, height);
            Refresh();
        }
        private void pictureBox_MouseEnter(object sender, EventArgs e)
        {
            Cursor = Cursors.Arrow;
        }
        private void pictureBox_MouseUp(object sender, MouseEventArgs e)
        {
            canDrawBox = false;
            tb = new TextBox();
            tb.Location = new Point(dragBox.X, (dragBox.Y + dragBox.Height) - tb.Height);
            tb.Width = dragBox.Width;
            //tb.(dragBox.X, (dragBox.Y + dragBox.Height) - tb.Height, dragBox.Width, 20);
            tb.Parent = pictureBox;
            tb.KeyPress += Tb_KeyPress;
            //Save for filter the used against the errors 
            if (!detections.ContainsKey(dragBox))
                detections.Add(dragBox, tb);

        }
        private void Tb_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                if (!string.IsNullOrEmpty(tb.Text))
                {
                    //Insert to metadata the new name
                    Detection d = new Detection();
                    d.Identity.Name = tb.Text;
                    d.Area = dragBox;
                    images[selectedIndex].AddDetection(d);
                }
            }
        }
        private void pictureBox_MouseLeave(object sender, EventArgs e)
        {
            Cursor = Cursors.Default;
        }
        private void pictureBox_MouseHover(object sender, EventArgs e)
        {
            Cursor = Cursors.Arrow;
        }
        #endregion

    }
}