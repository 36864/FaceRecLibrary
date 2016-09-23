#region Copyright Syncfusion Inc. 2001 - 2016
// Copyright Syncfusion Inc. 2001 - 2016. All rights reserved.
// Use of this code is subject to the terms of our license.
// A copy of the current license can be obtained at any time by e-mailing
// licensing@syncfusion.com. Any infringement will be prosecuted under
// applicable laws. 
#endregion
using FaceRecLibrary;
using FaceRecLibrary.Types;
using FaceRecLibrary.Utilities;
using Syncfusion.Windows.Forms.Tools;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FaceDetectionGUI
{
    public partial class YDetecter : Syncfusion.Windows.Forms.MetroForm
    {
        #region StateVars
        //Info on selected images
        private List<ImageInfo> images;

        //Loaded classifier information
        ClassifierList cList;

        /// <summary>
        /// Currently selected image index.
        /// Interlock.
        /// </summary>
        private int selectedImageIndex, selectedDetectionIndex;
        private FaceRecLibrary.FaceRecLibrary faceRecLib;

        /// <summary>
        /// Identify if option is selected to create detection box
        /// </summary>
        private bool canIdentify = false, canDrawBox;

        Rectangle dragBox;

        private int initialX, initialY;
        private Dictionary<ButtonEdit, Detection> detections = new Dictionary<ButtonEdit, Detection>();
        private Dictionary<Detection, int> detectionIndexes = new Dictionary<Detection, int>();
        private Dictionary<Detection, ButtonEdit> detectionsReverse = new Dictionary<Detection, ButtonEdit>();
        private Dictionary<int, Detection> detectionIndexesReverse = new Dictionary<int, Detection>();
        #endregion

        public YDetecter()
        {
            LoadConfig(Properties.Settings.Default.DefaultClassifierFile);
            Directory.CreateDirectory(Properties.Settings.Default.DefaultCacheFolder);
            faceRecLib = new FaceRecLibrary.FaceRecLibrary(Properties.Settings.Default.DefaultClassifierFile, Properties.Settings.Default.DefaultRecognizerFile, Properties.Settings.Default.DefaultIdentitiesFile);
            InitializeComponent();
        }

        /// <summary>
        /// Loads classifier configuration from xml file
        /// </summary>
        /// <param name="configFile"></param>
        private void LoadConfig(string configFile)
        {
            cList = Util.LoadXmlConfigFile(configFile);
        }

        /// <summary>
        /// Loads all supportd files in the specified directory.
        /// </summary>
        /// <param name="directory">Directory in which to start searching.</param>
        /// <param name="includeSubFolders">If true, subdirectories will be included in the search.</param>
        private void LoadAllSupportedFiles(string directory, bool includeSubFolders)
        {
            string[] fileNames = Directory.GetFiles(directory);
            listSelectedImages.Items.AddRange(fileNames.Select((f) => Path.GetFileName(f)).ToArray());
            for (int i = 0; i < fileNames.Length; i++)
            {
                images.Add(new ImageInfo(fileNames[i]));
            }
            if (includeSubFolders)
                foreach (var dir in Directory.GetDirectories(directory))
                {
                    LoadAllSupportedFiles(dir, true);
                }
        }

        /// <summary>
        /// Runs facial detection on the selected image.
        /// CPU-intensive, should be called from a worker thread.
        /// </summary>
        void RunDetection()
        {
            int index = selectedImageIndex;
            ImageInfo image = images[index];

            if (image.Detections != null)
            {
                listSelectedImages.Invoke(new System.Action( ()=> listSelectedImages.Enabled = true));
                return;
            }
            lblProcessing.Invoke(new System.Action(() => lblProcessing.Visible = true));
            faceRecLib.DetectAndRecognize(image);

            image.Detections?.ForEach((d) => RegisterDetection(d));

            if (index == selectedImageIndex)
                pictureBox.Invalidate();

            listSelectedImages.Invoke(new System.Action(() => listSelectedImages.Enabled = true));
            lblProcessing.Invoke(new System.Action(() => lblProcessing.Visible = false));
        }

        /// <summary>
        /// Scales image size to fit the specified container size.
        /// </summary>
        /// <param name="image">Original image Size</param>
        /// <param name="container">Size of the container</param>
        /// <returns></returns>
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

        /// <summary>
        /// Resizes the picturebox to fit the form
        /// </summary>
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

        /// <summary>
        /// Saves image metadata.
        /// </summary>
        /// <param name="toSave">Image to be saved.</param>
        /// <param name="xParams">Parameters to be passed to the metadata handler.</param>
        private void SaveData(ImageInfo toSave, object xParams = null)
        {
            toSave.Detections = detections.Values.ToList();
            faceRecLib.SaveMetadata(toSave, xParams);
        }

        /// <summary>
        /// Clears image list in preparation for folder loading
        /// </summary>
        /// <returns>The folder path to be loaded</returns>
        private string folderLoadAction()
        {
            if (folderBrowserDialog.ShowDialog() != DialogResult.OK)
                return null;
            string path = folderBrowserDialog.SelectedPath;
            listSelectedImages.Items.Clear();
            images = new List<ImageInfo>();
            return path;
        }

        /// <summary>
        /// Loads all files in the given path.
        /// </summary>
        /// <param name="path">Path from which to load files.</param>
        /// <param name="includeSubs">If true, subfolders are included in the search.</param>
        private void filesLoadAction(string path, bool includeSubs)
        {
            Dictionary<string, string> originalAndNewFilesPaths = new Dictionary<string, string>();
            //List<string> originalFilesPaths = Util.DetectImages(path, includeSubs);
            foreach (var originalFilesPaths in Util.LoadAllSupportedFiles(path, includeSubs))
            {
                int index = 0;
                foreach (var item in Util.FormatImages(originalFilesPaths, Properties.Settings.Default.DefaultCacheFolder))
                {
                    originalAndNewFilesPaths.Add(originalFilesPaths[index], item.ToString());
                    LoadFiles(originalFilesPaths[index++], item.ToString());
                }
            }
        }

        private void LoadFiles(string originalPath, string newPath)
        {

            ImageInfo info = new ImageInfo(originalPath);
            //faceRecLib.LoadMetadata(info);
            info.Path = newPath;
            images.Add(info);
            listSelectedImages.Invoke(new System.Action(
                          () => listSelectedImages.Items.Add(Path.GetFileName(originalPath))
                          ));
            listSelectedImages.Invalidate();
        }

        /// <summary>
        /// Creates a ButtonEdit and places it at the bottom of a detection area.
        /// </summary>
        /// <param name="r">Rectangle that defines the detection area.</param>
        /// <returns></returns>
        private ButtonEdit CreateButtonEdit(Rectangle r)
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(YDetecter));
            ButtonEdit btEdit;
            TextBoxExt tb = new TextBoxExt();
            btEdit = new ButtonEdit();
            ButtonEditChildButton btOK = new ButtonEditChildButton();
            ButtonEditChildButton btCancel = new ButtonEditChildButton();
            btOK.Click += btOK_Click;
            btOK.Image = ((Image)(resources.GetObject("check")));
            btCancel.Click += btCancel_Click;
            btCancel.Image = ((Image)(resources.GetObject("uncheck")));
            btEdit.Buttons.Add(btOK);
            btEdit.Buttons.Add(btCancel);
            btEdit.Location = new Point(r.X, (r.Y + r.Height) - tb.Height);
            btEdit.Width = r.Width;

            btEdit.Parent = pictureBox;
            btEdit.TextBox = tb;
            btEdit.Hide();
            tb.Parent = btEdit;

            tb.KeyPress += Tb_KeyPress;
            return btEdit;
        }
    

        /// <summary>
        /// Registers a detection in the form.
        /// </summary>
        /// <param name="d">Detection to be registered</param>
        private void RegisterDetection(Detection d)
        {
            this.Invoke(new System.Action(() =>
            {
                ButtonEdit btEdit = CreateButtonEdit(Util.ScaleRectangle(d.Area, images[selectedImageIndex].DisplayScaleFactor));
                detections.Add(btEdit, d);
                detectionsReverse.Add(d, btEdit);

                string name = d.Identity?.Name ?? "Unkown";
                if (name == null)
                    name = "Unknown";

                detectionIndexes.Add(d, listDetections.Items.Count);
                detectionIndexesReverse.Add(listDetections.Items.Count, d);

                listDetections.Invoke(new System.Action(() => listDetections.Items.Add(name)));
                txtDetectionCount.Invoke(new System.Action(() => txtDetectionCount.Text = listDetections.Items.Count.ToString()));
                Refresh();
            }));
        }

        /// <summary>
        /// Clears all instance state variables to prepare for a new loading operation.
        /// Discards any unsaved data.
        /// </summary>
        private void ClearState()
        {
            Invoke(new System.Action(() =>
            {
                detectionIndexes?.Clear();
                detectionIndexesReverse?.Clear();
                txtDetectionCount.Text = "0";
                listDetections?.Items?.Clear();
                images?.Clear();
                listSelectedImages?.Items?.Clear();
                pictureBox.Image = null;
                foreach (ButtonEdit b in detections.Keys)
                {
                    b.Parent = null;
                    b?.Dispose();
                }
                detections?.Clear();
                detectionsReverse?.Clear();
                selectedDetectionIndex = -1;
                selectedImageIndex = -1;
                Refresh();
            }));
        }

        private void ResetEditButtonPosition()
        {
            Invoke(new System.Action(() =>
            {
                ImageInfo image = images?[selectedImageIndex];
                if (image != null && detections?.Count > 0)
                    foreach (var d in detections)
                    {
                        Rectangle newArea = Util.ScaleRectangle(d.Value.Area, image.DisplayScaleFactor);
                        d.Key.Left = newArea.Left;
                        d.Key.Width = newArea.Width;
                        d.Key.Top = newArea.Bottom - d.Key.Height;
                    }
            }));
        }

        private void SelectDetection(Detection d)
        {
            Invoke(new System.Action(() => {
                if (selectedDetectionIndex > -1 && selectedDetectionIndex < detectionsReverse.Count)
                    detectionsReverse[detectionIndexesReverse[selectedDetectionIndex]].Hide();

                selectedDetectionIndex = detectionIndexes[d];

                detectionsReverse[d].Show();
            }));
        }

        private void ClearDetections()
        {
            detectionIndexes.Clear();
            detectionIndexesReverse.Clear();
            listDetections.Items.Clear();
            foreach (ButtonEdit b in detections.Keys)
            {
                b.Parent = null;
                b.Dispose();
            }
            detections.Clear();
            detectionsReverse.Clear();
            txtDetectionCount.Text = "0";
            Refresh();
        }


        #region EventHandlers

        private void btOK_Click(object sender, EventArgs e)
        {
            ButtonEditChildButton btOK = (ButtonEditChildButton)sender;
            ButtonEdit btEdit = (ButtonEdit)btOK.ButtonEditParent;
            Detection d = detections[btEdit];

            d.Identity.Name = btEdit.TextBox.Text;
            IdentityInfo[] ids = faceRecLib.GetIdsFromName(d.Identity.Name);
            if (ids.Length > 0)
            {
                IdentityConfirmationForm idcform = new IdentityConfirmationForm(ids);
                if(idcform.ShowDialog() == DialogResult.OK)
                {
                    d.Identity = idcform.chosenIdentity;
                }

            }
            faceRecLib.AddOrUpdateDetection(images[selectedImageIndex], d);

            listDetections.Items[detectionIndexes[d]] = d.Identity.Name;
        }

        private void btCancel_Click(object sender, EventArgs e)
        {
            ButtonEditChildButton btCancel = (ButtonEditChildButton) sender;
            ButtonEdit btEdit = (ButtonEdit)btCancel.ButtonEditParent;
            Detection d = detections[btEdit];
            int detectionIndex = detectionIndexes[d];

            detectionsReverse.Remove(d);
            detections.Remove(btEdit);
            detectionIndexes.Remove(d);


            listDetections.Items.RemoveAt(detectionIndex);
            for (int i = detectionIndex; i < listDetections.Items.Count; ++i)
            {
                detectionIndexesReverse.Remove(i);
                detectionIndexesReverse.Add(i, detectionIndexesReverse[i + 1]);
                detectionIndexes[detectionIndexesReverse[i]] = i;
            }
            btEdit.Hide();
            btEdit.Dispose();
            txtDetectionCount.Text = listDetections.Items.Count.ToString();
            Refresh();
        }

        private void excludeSubdirectories_Click(object sender, EventArgs e)
        {
            string path = folderLoadAction();
            if (path != null)
            {
                ClearState();
                Task.Run(() => filesLoadAction(path, false));
            }
            
        }
        private void includeSubdirectories_Click(object sender, EventArgs e)
        {
            string path = folderLoadAction();
            if (path != null)
            {
                ClearState();
                Task.Run(() => filesLoadAction(path, true));
            }
        }
        private void openFile_Click(object sender, EventArgs e)
        {
            //Show file dialog
            if (openImagesDialog.ShowDialog() != DialogResult.OK)
                return;

            //Clear application state
            ClearState();
            
            images = new List<ImageInfo>();

            //Load files
            Dictionary<string, string> originalAndNewFilesPaths = new Dictionary<string, string>();
            for (int index = 0; index < openImagesDialog.FileNames.Length; ++index)
            {
                string item = Util.FormatImage(openImagesDialog.FileNames[index], Properties.Settings.Default.DefaultCacheFolder, Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
                originalAndNewFilesPaths.Add(openImagesDialog.FileNames[index], item.ToString());
                LoadFiles(openImagesDialog.FileNames[index], item.ToString());
            }

            //Select first image for display
            
            listSelectedImages.SelectedIndex = 0;
            Refresh();
        }

        private void listSelectedImages_SelectedIndexChanged(object sender, EventArgs e)
        {
            listSelectedImages.Enabled = false;

            if (pictureBox.Image != null)
            {
                pictureBox.Image.Dispose();
                pictureBox.Image = null;
                GC.Collect();
            }

            if(detections?.Count > 0)
            {
                images[selectedImageIndex].Detections = detections.Values.ToList();
            }

            selectedImageIndex = listSelectedImages.SelectedIndex;
            foreach(var d in detections)
            {
                d.Key.Dispose();
            }

            detections.Clear();
            detectionsReverse.Clear();
            detectionIndexes.Clear();
            detectionIndexesReverse.Clear();
            txtDetectionCount.Text = "0";
            listDetections.Items.Clear();
            pictureBox.Image = null;

            ImageInfo image = images[selectedImageIndex];
            faceRecLib.LoadMetadata(image, null);
            pictureBox.Image = Image.FromFile(image.Path);
            image.Width = pictureBox.Image.Width;
            image.Height = pictureBox.Image.Height;
            //Resize PictureBox
            ResizePictureBox();
            image.DisplayScaleFactor = Util.FindScale(image.Width, image.Height, pictureBox.Width, pictureBox.Height);
            image.Detections?.ForEach((d) => RegisterDetection(d));
            //Run detection using loaded classifiers
            Task.Run(() => RunDetection());
        }
        private void settings_Click(object sender, EventArgs e)
        {
            SettingsForm sForm = new SettingsForm(cList);
            if(sForm.ShowDialog() == DialogResult.OK)
                Util.SaveXmlConfigFile(cList, Properties.Settings.Default.DefaultClassifierFile);
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
                canIdentify = false;
                identifyBtn.Text = "Identify";
                Refresh();
            }
        }

        private void panelImageContainer_Resize(object sender, EventArgs e)
        {
            ResizePictureBox();
            ImageInfo image = images?[selectedImageIndex];
            if(image != null)
                image.DisplayScaleFactor = Util.FindScale(image.Width, image.Height, pictureBox.Width, pictureBox.Height);
            if(detections.Count > 0)
                ResetEditButtonPosition();
        }

        private void pictureBox_Paint(object sender, PaintEventArgs e)
        {
            base.OnPaint(e);
            
            if (listSelectedImages.SelectedIndex > -1 && images.Count > listSelectedImages.SelectedIndex)
            {
                //Check if detection has been run on image
                var image = images[listSelectedImages.SelectedIndex];
                if (!(detections == null || detections.Count < 1))
                {
                    //Find current image scale and position
                    Graphics g = e.Graphics;

                    //Draw detection rectangles on image
                    using (Pen pen = new Pen(Color.Orange, 2))
                    {
                        foreach (var item in detections.Values)
                        {
                            e.Graphics.DrawRectangle(pen, Util.ScaleRectangle(item.Area, image.DisplayScaleFactor));
                        }
                        if(canIdentify)
                            e.Graphics.DrawRectangle(pen, dragBox);
                    }
               }

            }
        }
        
        private void pictureBox_MouseClick(object sender, MouseEventArgs e)
        {
            if (detections != null)
            {
                foreach (var d in detections)
                {
                    if (Util.ScaleRectangle(d.Value.Area, images[selectedImageIndex].DisplayScaleFactor).Contains(e.Location))
                    {
                        //select detection to add identity info
                        SelectDetection(d.Value);
                    }
                    else d.Key.Hide();
                }
            }
        }

        private void pictureBox_MouseDown(object sender, MouseEventArgs e)
        {
            if (canIdentify)
            {
                canDrawBox = true;
                initialX = e.X;
                initialY = e.Y;
            }
        }
        private void pictureBox_MouseMove(object sender, MouseEventArgs e)
        {
            bool inDetection = false;

            if (detections != null)
            {
                foreach (var d in detections)
                {
                    if (Util.ScaleRectangle(d.Value.Area, images[selectedImageIndex].DisplayScaleFactor).Contains(e.Location))
                    {
                        inDetection = true;
                        break;
                    }
                }
            }
            if (!inDetection)
                Cursor = Cursors.Default;
            else
                Cursor = Cursors.Hand;

            if (!(canIdentify && canDrawBox)) return;

            //Condition to restrict the limits of the dragbox, in this way it will not get out of borders
            int eX = (e.X > 0) ? e.X : 0, eY = (e.Y > 0) ? e.Y : 0;
            if (e.X > pictureBox.Width) eX = pictureBox.Width;
            if (e.Y > pictureBox.Height) eY = pictureBox.Height;

            int x = Math.Min(initialX, eX);
            int y = Math.Min(initialY, eY);
            int width = Math.Max(initialX, eX) - Math.Min(initialX, eX);
            int height = Math.Max(initialY, eY) - Math.Min(initialY, eY);
            dragBox.Height = Math.Min(height, width);
            dragBox.Width = dragBox.Height;
            dragBox.X = x;
            dragBox.Y = y;
            if (dragBox.Right < initialX)
                dragBox.X += initialX - dragBox.Right;
            if (dragBox.Bottom < initialY)
                dragBox.Y += initialY - dragBox.Bottom;
            Refresh();
        }
        private void pictureBox_MouseEnter(object sender, EventArgs e)
        {
            Cursor = Cursors.Cross;
        }
        private void pictureBox_MouseUp(object sender, MouseEventArgs e)
        {
            if (canDrawBox)
            {
                canDrawBox = false;

                ButtonEdit btEdit = CreateButtonEdit(dragBox);
                Detection newd = new Detection(Util.ScaleRectangle(dragBox, 1 / images[selectedImageIndex].DisplayScaleFactor), 1, null, images[selectedImageIndex]);
                //Save for filter the used against the errors 
                if (!detections.ContainsValue(newd))
                {
                    detections.Add(btEdit, newd);
                }
                dragBox = Rectangle.Empty;
                Refresh();
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
        private void Tb_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                TextBox tb = (TextBox)sender;
                ButtonEdit btEdit = (ButtonEdit)tb.Parent;
                if (!string.IsNullOrEmpty(tb.Text))
                {
                    //Insert to metadata the new name
                    Detection d = detections[btEdit];
                    d.Identity.Name = tb.Text;
                    listDetections.Items[detectionIndexes[d]] = d.Identity.Name;
                }
            }
        }

        private void saveSelectedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (images[selectedImageIndex] != null)
            {
                XMPMetadataHandlerParameters xParams = new XMPMetadataHandlerParameters();
                xParams.ClearBeforeSave = true;
                xParams.SavePath = null;
                SaveData(images[selectedImageIndex], xParams);

                faceRecLib.SaveRecognizerToFile(Properties.Settings.Default.DefaultRecognizerFile);
                faceRecLib.SaveIdentityInformation(Properties.Settings.Default.DefaultIdentitiesFile);
            }
        }

        private void listDetections_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(listDetections.SelectedIndex != -1)
                SelectDetection(listDetections.SelectedIndex);
        }

        private void btnClearDetections_Click(object sender, EventArgs e)
        {
            ClearDetections();
        }


        private void btnClearImages_Click(object sender, EventArgs e)
        {
            ClearState();
        }

        private void SelectDetection(int selectedIndex)
        {
            SelectDetection(detectionIndexesReverse[selectedIndex]);
        }

        private void saveAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (images?.Count != null)
            {
                foreach (var image in images)
                {
                    XMPMetadataHandlerParameters xParams = new XMPMetadataHandlerParameters();
                    xParams.ClearBeforeSave = true;
                    xParams.SavePath = null;                
                    SaveData(image, xParams);
                }

                faceRecLib.SaveRecognizerToFile(Properties.Settings.Default.DefaultRecognizerFile);
                faceRecLib.SaveIdentityInformation(Properties.Settings.Default.DefaultIdentitiesFile);
            }

        }

        private void saveSelectedAsCopyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveFileDialog1.FileName = Path.GetFileName(images[selectedImageIndex].OriginalPath);
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                XMPMetadataHandlerParameters xParams = new XMPMetadataHandlerParameters();
                xParams.SavePath = images[selectedImageIndex].OriginalPath;
                xParams.ClearBeforeSave = true;
                SaveData(images[selectedImageIndex], xParams);

                faceRecLib.SaveRecognizerToFile(Properties.Settings.Default.DefaultRecognizerFile);
                faceRecLib.SaveIdentityInformation(Properties.Settings.Default.DefaultIdentitiesFile);
            }
        }
        #endregion
    }
}