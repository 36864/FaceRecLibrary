using FaceRecLibrary;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FaceDetectionGUI
{
    public partial class SettingsForm : Form
    {
        private ClassifierInfo[] classifiers;


        public SettingsForm(ClassifierList cList)
        {
            InitializeComponent();
            classifiers = new ClassifierInfo[cList.FaceClassifiers.Count + cList.EyeClassifiers.Count];
            int i = 0;
            foreach (ClassifierInfo cInfo in cList.FaceClassifiers)
            {
                classifiers[i++] = cInfo;
                listClassifiers.Items.Add(cInfo.Name);
            }
            foreach (ClassifierInfo cInfo in cList.EyeClassifiers)
            {
                classifiers[i++] = cInfo;
                listClassifiers.Items.Add(cInfo.Name);
            }
            listClassifiers.SelectedIndex = 0;

            lblWidth.Hide();
            lblHeight.Hide();
            lblDimensions.Hide();
            textBoxSizeWidth.Hide();
            textBoxSizeHeight.Hide();
            textBoxSizeWidth.Text = null;
            textBoxSizeHeight.Text = null;
        }

        private void listClassifiers_SelectedIndexChanged(object sender, EventArgs e)
        {
            ClassifierInfo cInfo = classifiers[listClassifiers.SelectedIndex];
            textBoxScale.Text = cInfo.Scale.ToString();
            textBoxMinNeighbors.Text = cInfo.MinNeighbors.ToString();

            //if (cInfo is FaceClassifier) {
            //    textBoxSizeHeight.Show();
            //    textBoxSizeWidth.Show();
            //    lblDimensions.Show();
            //    lblHeight.Show();
            //    lblWidth.Show();
            //    textBoxSizeWidth.Text = ((FaceClassifier)cInfo).MaxDimensions.Width.ToString();
            //    textBoxSizeHeight.Text = ((FaceClassifier)cInfo).MaxDimensions.Height.ToString();
            //}
            //else
            //{
            //lblWidth.Hide();
            //lblHeight.Hide();
            //lblDimensions.Hide();
            //textBoxSizeWidth.Hide();
            //textBoxSizeHeight.Hide();
            //textBoxSizeWidth.Text = null;
            //textBoxSizeHeight.Text = null;
            //}
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ClassifierInfo cInfo = classifiers[listClassifiers.SelectedIndex];
            double scale;
            if (double.TryParse(textBoxScale.Text, out scale))
                cInfo.Scale = scale;
            int minNeighbors;
            if (int.TryParse(textBoxMinNeighbors.Text, out minNeighbors))
                cInfo.MinNeighbors = minNeighbors;
            //if (cInfo is FaceClassifier)
            //{
            //    FaceClassifier fCInfo = (FaceClassifier) cInfo;
            //    int width;
            //    if (int.TryParse(textBoxSizeWidth.Text, out width))
            //        fCInfo.MaxDimensions.Width = width;
            //}
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
