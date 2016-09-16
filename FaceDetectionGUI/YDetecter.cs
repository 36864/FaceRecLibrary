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
        private int selectedIndex;
        private FaceRecLibrary.FaceRecLibrary faceRecLib;

        /// <summary>
        /// Identify if option is selected to create detection box
        /// </summary>
        private bool canIdentify = false, canDrawBox;

        Rectangle dragBox;

        private int initialX, initialY;        
        private Dictionary<ButtonEdit, Detection> detections = new Dictionary<ButtonEdit, Detection>();

        #endregion
        public YDetecter()
        {
            LoadConfig(Properties.Settings.Default.DefaultClassifierFile);
            //Directory.CreateDirectory(SAVED_DATA_PATH);
            Directory.CreateDirectory(Properties.Settings.Default.DefaultCacheFolder);
            faceRecLib = new FaceRecLibrary.FaceRecLibrary();
            faceRecLib.init(Properties.Settings.Default.DefaultClassifierFile, null);
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
            image.DetectionInfo?.Detections?.ForEach((d) => detections.Add(CreateButtonEdit(Util.ScaleRectangle(d.Area, image.DisplayScaleFactor)), d));
            if (index == selectedIndex)
                pictureBox.Invalidate();
            listSelectedImages.SelectedIndexChanged += listSelectedImages_SelectedIndexChanged;
            //SaveData(image);
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
        private void SaveData(ImageInfo[] toSave)
        {
            foreach (var item in toSave)
            {
                SaveData(item);
            }
        }
        private void SaveData(ImageInfo toSave)
        {
            toSave.DetectionInfo.Detections = detections.Values.ToList();
            faceRecLib.SaveMetadata(toSave);
        }

        /*private void ClearSavedData()
        {
            Directory.Delete(SAVED_DATA_PATH, true);
            Directory.CreateDirectory(SAVED_DATA_PATH);
        }*/

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

        private ButtonEdit CreateButtonEdit(Rectangle r)
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(YDetecter));
            TextBoxExt tb = new TextBoxExt();
            ButtonEdit btEdit = new ButtonEdit();
            ButtonEditChildButton btOK = new ButtonEditChildButton();
            ButtonEditChildButton btCancel = new ButtonEditChildButton();
            btOK.Click += btOK_Click;
            btOK.Image = ((Image)(resources.GetObject("check.Image")));
            btCancel.Click += btCancel_Click;
            btCancel.Image = ((Image)(resources.GetObject("uncheck.Image")));
            btEdit.Buttons.Add(btOK);
            btEdit.Buttons.Add(btCancel);
            btEdit.Location = new Point(r.X, (r.Y + r.Height) - tb.Height);
            btEdit.Width = r.Width;
            pictureBox.Invoke(new System.Action(  () => btEdit.Parent = pictureBox ));
            tb.KeyPress += Tb_KeyPress;
            btEdit.Invoke(new System.Action(() => btEdit.TextBox = tb));
            btEdit.Invoke(new System.Action(() => tb.Parent = btEdit));            
            return btEdit;
        }

        #region EventHandlers

        private void btOK_Click(object sender, EventArgs e)
        {
            ButtonEditChildButton btOK = (ButtonEditChildButton)sender;
            ButtonEdit btEdit = (ButtonEdit)btOK.ButtonEditParent;
            Detection d = detections[btEdit];
            if (!images[selectedIndex].DetectionInfo.Detections.Contains(d))
                images[selectedIndex].AddDetection(d);
            d.Identity.Name = btEdit.TextBox.Text;
        }

        private void btCancel_Click(object sender, EventArgs e)
        {
            ButtonEditChildButton btCancel = (ButtonEditChildButton) sender;
            ButtonEdit btEdit = (ButtonEdit)btCancel.ButtonEditParent;
            detections.Remove(btEdit);
            btEdit.Dispose();
        }

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
            foreach (var d in detections)
            {
                d.Key.Dispose();
            }
            detections.Clear();

            listSelectedImages.SelectedIndex = -1;
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
            foreach(var d in detections)
            {
                d.Key.Dispose();
            }
            detections.Clear();
            ImageInfo image = images[selectedIndex];
            faceRecLib.LoadMetadata(image);
            pictureBox.Image = Image.FromFile(image.Path);
            image.Width = pictureBox.Image.Width;
            image.Height = pictureBox.Image.Height;
            detections.Clear();
            //Resize PictureBox
            ResizePictureBox();
            image.DisplayScaleFactor = Util.FindScale(image.Width, image.Height, pictureBox.Width, pictureBox.Height);
            image.DetectionInfo?.Detections?.ForEach((d) => detections.Add(CreateButtonEdit(Util.ScaleRectangle(d.Area, image.DisplayScaleFactor)), d));
            //Run detection using loaded classifiers
            Task.Run(() => RunDetection());
        }
        private void settings_Click(object sender, EventArgs e)
        {
            SettingsForm sForm = new SettingsForm(cList);
            sForm.ShowDialog();
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
                foreach (ButtonEdit item in detections.Keys)
                {
                    item.Dispose();
                }
                detections.Clear();
                canIdentify = false;
                identifyBtn.Text = "Identify";
                Refresh();
            }
        }
        private void panelImageContainer_Resize(object sender, EventArgs e)
        {
            ResizePictureBox();
            ImageInfo image = images?[selectedIndex];
            if(image != null)
                image.DisplayScaleFactor = Util.FindScale(image.Width, image.Height, pictureBox.Width, pictureBox.Height);
            ResetEditButtonPosition();
        }


        private void ResetEditButtonPosition()
        {
            ImageInfo image = images?[selectedIndex];
            if(image != null && detections?.Count > 0)
            foreach (var d in detections)
            {

                    Rectangle newArea = Util.ScaleRectangle(d.Value.Area, image.DisplayScaleFactor);
                d.Key.Left = newArea.Left;
                d.Key.Width = newArea.Width;
                d.Key.Top = newArea.Bottom - d.Key.Height;
            }
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
                    if (Util.ScaleRectangle(d.Value.Area, images[selectedIndex].DisplayScaleFactor).Contains(e.Location))
                    {
                        //select detection to add identity info  
                        d.Key.Show();
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
                    if (Util.ScaleRectangle(d.Value.Area, images[selectedIndex].DisplayScaleFactor).Contains(e.Location))
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
                Detection newd = new Detection(Util.ScaleRectangle(dragBox, 1 / images[selectedIndex].DisplayScaleFactor));
                foreach (var entry in detections)
                {
                    if (entry.Value.Conflicts(newd))
                    {
                        newd = entry.Value;
                    }
                }
                //Save for filter the used against the errors 
                if (!detections.ContainsValue(newd))
                    detections.Add(btEdit, newd);
                dragBox = Rectangle.Empty;
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
                    if(!images[selectedIndex].DetectionInfo.Detections.Contains(d))                    
                        images[selectedIndex].AddDetection(d);
                    d.Identity.Name = tb.Text;
                }
            }
        }

        private void saveSelectedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveData(images[selectedIndex]);
        }

        private void saveAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveData(images.ToArray());
        }

        private void saveAllAsCopyxToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void saveSelectedAsCopyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveFileDialog1.FileName = Path.GetFileName(images[selectedIndex].OriginalPath);
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                File.Copy(images[selectedIndex].OriginalPath, saveFileDialog1.FileName, true);
                images[selectedIndex].OriginalPath = saveFileDialog1.FileName;
                SaveData(images[selectedIndex]);
            }
        }
        #endregion

    }
}
