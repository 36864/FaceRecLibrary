using FaceRecLibrary;
using System;
using System.Drawing;
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
        }

        private void listClassifiers_SelectedIndexChanged(object sender, EventArgs e)
        {
            ClassifierInfo cInfo = classifiers[listClassifiers.SelectedIndex];
            textBoxScale.Text = cInfo.Scale.ToString();
            textBoxMinNeighbors.Text = cInfo.MinNeighbors.ToString();
            if (cInfo is FaceClassifier) {
                textBoxSizeHeight.Show();
                textBoxSizeWidth.Show();
                lblDimensions.Show();
                lblHeight.Show();
                lblWidth.Show();
                textBoxConfidence.Show();
                lblConfidence.Show();
                textBoxConfidence.Text = cInfo.Confidence.ToString();
                textBoxSizeWidth.Text = ((FaceClassifier)cInfo).MaxDimensions.Width.ToString();
                textBoxSizeHeight.Text = ((FaceClassifier)cInfo).MaxDimensions.Height.ToString();
            }
            else
            {
                lblWidth.Hide();
                lblHeight.Hide();
                lblDimensions.Hide();
                textBoxSizeWidth.Hide();
                textBoxSizeHeight.Hide();
                textBoxConfidence.Hide();
                lblConfidence.Hide();
                textBoxSizeWidth.Text = null;
                textBoxSizeHeight.Text = null;
                textBoxConfidence.Text = null;
            }
        }

        private void Save()
        {
            ClassifierInfo cInfo = classifiers[listClassifiers.SelectedIndex];
            double scale;
            if (double.TryParse(textBoxScale.Text, out scale))
                cInfo.Scale = scale;
            int minNeighbors;
            if (int.TryParse(textBoxMinNeighbors.Text, out minNeighbors))
                cInfo.MinNeighbors = minNeighbors;

            if (cInfo is FaceClassifier)
            {
                double confidence;
                if (double.TryParse(textBoxConfidence.Text, out confidence))
                    cInfo.Confidence = confidence;
                FaceClassifier fCInfo = (FaceClassifier)cInfo;
                int width;
                int height;
                if (int.TryParse(textBoxSizeWidth.Text, out width) && int.TryParse(textBoxSizeHeight.Text, out height))
                    fCInfo.MaxDimensions = new Size(width, height);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Save();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            Save();
            this.Close();
        }
    }
}
