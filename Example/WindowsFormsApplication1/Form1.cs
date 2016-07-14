/* (C) 2008 Savvas Chatzichristofis -- Part of img(Rummager) Project
 * 1. S. Α. Chatzichristofis and Y. S. Boutalis, “CEDD: COLOR AND EDGE DIRECTIVITY DESCRIPTOR - A COMPACT DESCRIPTOR FOR IMAGE INDEXING AND RETRIEVAL.” « 6th International Conference in advanced research on Computer Vision Systems ICVS 2008», May 12 to May 15, 2008, Santorini, Greece
 * 2. S. A. Chatzichristofis and Y. S. Boutalis, “FCTH: FUZZY COLOR AND TEXTURE HISTOGRAM- A LOW LEVEL FEATURE FOR ACCURATE IMAGE RETRIEVAL” «9th International Workshop on Image Analysis for Multimedia Interactive Services”, Proceedings: IEEE Computer Society , May 7 to May 9, 2008, Klagenfurt, Austria
 * 3. S. A. Chatzichristofis and Y. Boutalis, “A HYBRID SCHEME FOR FAST AND ACCURATE IMAGE RETRIEVAL BASED ON COLOR DESCRIPTORS”, «IASTED International Conference on Artificial Intelligence and Soft Computing (ASC 2007)», August 29 to August 31, 2007, Palma De Mallorca, Spain.
 * 4. Mathias Lux, S. A. Chatzichristofis, "LIRe: Lucene Image Retrieval - An Extensible Java CBIR Library", ACM International Conference on Multimedia 2008, Vancouver, BC, Canada October 27 – 31, 2008, Open Source Application Competition. 
 *
 * savvash@gmail.com
 * 
 * */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;


namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        Bitmap SourceImage;
        string[] ImagesToSearch;
        public DataTable DataResults = new DataTable("Items"); // Here you store the results

        private void button1_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                this.Cursor = Cursors.WaitCursor;

                try
                {
                    pictureBox1.Image = new Bitmap(openFileDialog1.FileName);
                    SourceImage = new Bitmap(pictureBox1.Image);
                }
                catch
                {
                }
                this.Cursor = Cursors.Default ;
                
            }
        }


        private void Setlabels(string Phase, string Description)
        {
            label55.Text = Phase;
            label54.Text = Description;
            label55.Refresh();
            label54.Refresh();


        }


        #region This Part Of the code controls the image Folders
        private void button12_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {

                int Alreadyexist = 0;
                for (int i = 1; i <= listBox2.Items.Count; i++)
                {
                    listBox2.SetSelected(i - 1, true);

                    if (folderBrowserDialog1.SelectedPath == listBox2.Text.ToString())
                    {
                        Alreadyexist = 1;
                        MessageBox.Show("Folder " + folderBrowserDialog1.SelectedPath.ToString() + " Already Exist In The List", "Demo", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    }
                }

                if (Alreadyexist == 0)
                {
                    DirectoryInfo dir = new DirectoryInfo((string)folderBrowserDialog1.SelectedPath.ToString());
                    FileInfo[] bmpfiles = dir.GetFiles("*.jpg");


                    if (bmpfiles.Length >= 1)
                    {
                        listBox2.Items.Add(folderBrowserDialog1.SelectedPath);
                        checkforimagesinfolders();

                    }
                    else
                        MessageBox.Show("Folder " + folderBrowserDialog1.SelectedPath.ToString() + " Does Not Contain Any Images", "Demo", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);


                }
                if (listBox2.Items.Count >= 1)
                {
                    button13.Enabled = true;
                    button2.Enabled = true;
                }
                else
                {
                    button2.Enabled = false;
                    button13.Enabled = false;
                }
            }
        }

        private void button13_Click(object sender, EventArgs e)
        {
            listBox2.Items.Remove(listBox2.SelectedItem);
            checkforimagesinfolders();
            if (listBox2.Items.Count >= 1)
            {
                button2.Enabled = true;
                button13.Enabled = true;
            }
            else
            {
                button2.Enabled = false;
                button13.Enabled = false;
            }
        }


        private void checkforimagesinfolders()
        {
            int totalimages = 0;
            googleresults.Items.Clear();


           

            for (int i = 1; i <= listBox2.Items.Count; i++)
            {
                listBox2.SetSelected(i - 1, true);

                DirectoryInfo dir = new DirectoryInfo((string)listBox2.Text.ToString());
                FileInfo[] bmpfiles = dir.GetFiles("*.jpg");
                totalimages = totalimages + bmpfiles.Length;

                for (int j = 0; j <= bmpfiles.Length - 1; j++)
                {
                    googleresults.Items.Add(listBox2.Text + "\\" + bmpfiles[j]);

                }
            }

            ImagesToSearch = new string[totalimages];

            for (int i = 0; i < totalimages; i++)
            {
                ImagesToSearch[i] = googleresults.Items[i].ToString();

            }

           
            textBox18.Text = totalimages.ToString();

        }

        #endregion

        private void button2_Click(object sender, EventArgs e)
        {
            NewMethod();




            // Show The Results and Sort them

            dataGrid4.DataSource = DataResults;
            dataGrid4.ReadOnly = false;
            dataGrid4.Columns[1].ReadOnly = true;
            dataGrid4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            dataGrid4.Columns[1].Width = 20;
            dataGrid4.Columns[0].Width = 160;

            int thirdColumn = 1;
            DataGridViewColumn column = dataGrid4.Columns[thirdColumn];
            dataGrid4.Sort(column, (ListSortDirection)0);
            dataGrid4.ReadOnly = true;
            if (dataGrid4.Rows.Count > 1)
            {
                dataGrid4[0, 0].Selected = true;
                PreviewResults();
            }


            panel1.Visible = false;
            panel1.Refresh();


        }

        private void NewMethod()
        {
            // Create 2 tables for each descriptor. In the first Table you 
            // keep information about "Source Image" (initial image) and in the second table
            // you keep information about "To Compare" image
            // SOS in this version we do not use multiple threads 
            panel1.Visible = true;
            panel1.Refresh();
            Setlabels("Reading Image", "Please Wait");


            // Setup Parameters For CEDD and FCTH



            CEDD_Descriptor.CEDD CEDDDescriptor = new CEDD_Descriptor.CEDD();  // using CEDD.DLL
            FCTH_Descriptor.FCTH FCTHDescriptor = new FCTH_Descriptor.FCTH();  // using FCTH.DLL

            double[] CEDDTable_Source = new double[144];
            double[] FCTHTable_Source = new double[192];

            double[] CEDDTable_ToCompare = new double[144];
            double[] FCTHTable_ToCompare = new double[192];


            //Setup Parameters For MPEG-7 Descriptors


            // Setup Parameters For MPEG-7 SCD

            SCD_Descriptor Mpeg7SCD = new SCD_Descriptor(); // using SCD.DLL
            double[] SourceSCDTable = new double[64];
            double[] ToCompareSCDTable = new double[64];


            // Setup Parameters For MPEG-7 CLD

            CLD_Descriptor Mpeg7CLD = new CLD_Descriptor(); // using CDL.DLL
            int[] SourceYCDL = new int[6];
            int[] SourceCbCDL = new int[3];
            int[] SourceCrCDL = new int[3];
            int[] ToCompareYCDL = new int[6];
            int[] ToCompareCbCDL = new int[3];
            int[] ToCompareCrCDL = new int[3];

            // Setup Parameters For MPEG-7 EHD

            EHD_Descriptor Mpeg7EHD = new EHD_Descriptor(11); // using EHD.DLL
            double[] SourceEHDTable = new double[80];
            double[] ToCompareEHDTable = new double[80];


            //Check the descriptor Selected By User
            int Descriptor = 0;
            /*
             * 0 for CEDD
             * 1 for FCTH
             * 2 for SCD
             * 3 For CLD
             * 4 For EHD
             * */

            switch (comboBox1.Text.ToString())
            {
                case ("CEDD"): Descriptor = 0; break;
                case ("FCTH"): Descriptor = 1; break;
                case ("SCD"): Descriptor = 2; break;
                case ("CLD"): Descriptor = 3; break;
                case ("EHD"): Descriptor = 4; break;
                default: Descriptor = 0; break;


            }

            // Get the descriptor for the source image
            if (Descriptor == 0) CEDDTable_Source = CEDDDescriptor.Apply(SourceImage); // GET The CEDD Descriptor for the Initial Image

            if (Descriptor == 1) FCTHTable_Source = FCTHDescriptor.Apply(SourceImage, 2);  // GET The FCTH Descriptor for the Initial Image

            if (Descriptor == 2)
            {
                Mpeg7SCD.Apply(SourceImage, 64, 0);
                SourceSCDTable = Mpeg7SCD.Norm4BitHistogram;

            }
            if (Descriptor == 3)
            {
                Mpeg7CLD.Apply(SourceImage);


                for (int h = 0; h < 6; h++)
                {
                    SourceYCDL[h] = Mpeg7CLD.YCoeff[h];
                    if (h < 3)
                    {
                        SourceCbCDL[h] = Mpeg7CLD.CbCoeff[h];
                        SourceCrCDL[h] = Mpeg7CLD.CrCoeff[h];
                    }
                }

            }
            if (Descriptor == 4)
            {
                SourceEHDTable = Mpeg7EHD.Apply(SourceImage);
                SourceEHDTable = Mpeg7EHD.Quant(SourceEHDTable); // Dont Forget to Quant The Descriptor

            }



            // Ok, now you have the descriptor for the images.

            // Remember that you can save the descripors for all the images in one XLM type file
            // You can use the Img(Rummager) "manage data" to create these files and then search using these files in order to increase the searching speed

            int NumberOfImages = Convert.ToInt32(textBox18.Text.ToString());
            double TotalDeviation = 0; // Here you will store the deviation of the images (pairwise)
            Bitmap ToCompare;
            MPEG_7Simillarity MPEGComparer = new MPEG_7Simillarity(); // Use this class in order to campare images using MPEG-7 Descriptors
            DataResults.Clear(); // Clear the results

            for (int i = 0; i < NumberOfImages; i++)
            {

                ToCompare = new Bitmap(ImagesToSearch[i].ToString());
                //Force 32bpp
                ToCompare = new Bitmap(ToCompare);

                if (Descriptor == 0) //CEDD
                {

                    Setlabels("Checking CEDD (3-bit) Similarity", "So Far Results: " + i.ToString());

                    CEDDTable_ToCompare = CEDDDescriptor.Apply(ToCompare);
                    TotalDeviation = TanimotoClassifier(CEDDTable_ToCompare, CEDDTable_Source); // We are Using Tanimoto in order to Compare Images

                }



                if (Descriptor == 1) //FCTH
                {

                    Setlabels("Checking FCTH (3-bit) Similarity", "So Far Results: " + i.ToString());

                    FCTHTable_ToCompare = FCTHDescriptor.Apply(ToCompare, 2);
                    TotalDeviation = TanimotoClassifier(FCTHTable_ToCompare, FCTHTable_Source); // We are Using Tanimoto in order to Compare Images

                }


                // MPEG-7


                if (Descriptor == 2) //SCD
                {

                    Setlabels("Checking MPEG7:SCD Similarity", "So Far Results: " + i.ToString());

                    Mpeg7SCD.Apply(ToCompare, 64, 0);
                    ToCompareSCDTable = Mpeg7SCD.haarTransformedHistogram;
                    TotalDeviation = MPEGComparer.calculateSCDDistance(ToCompareSCDTable, SourceSCDTable);


                }


                if (Descriptor == 3) //CLD
                {

                    Setlabels("Checking MPEG7:CLD Similarity", "So Far Results: " + i.ToString());

                    Mpeg7CLD.Apply(ToCompare);

                    for (int h = 0; h < 6; h++)
                    {
                        ToCompareYCDL[h] = Mpeg7CLD.YCoeff[h];
                        if (h < 3)
                        {
                            ToCompareCbCDL[h] = Mpeg7CLD.CbCoeff[h];
                            ToCompareCrCDL[h] = Mpeg7CLD.CrCoeff[h];
                        }
                    }

                    TotalDeviation = MPEGComparer.calculateCLDDistance(ToCompareYCDL, ToCompareCbCDL, ToCompareCrCDL, SourceYCDL, SourceCbCDL, SourceCrCDL);



                }



                if (Descriptor == 4) //EHD
                {
                    Setlabels("Checking MPEG7:EHD Similarity", "So Far Results: " + i.ToString());

                    ToCompareEHDTable = Mpeg7EHD.Apply(ToCompare);
                    ToCompareEHDTable = Mpeg7EHD.Quant(ToCompareEHDTable);

                    TotalDeviation = MPEGComparer.calculateEHDDistance(ToCompareEHDTable, SourceEHDTable);

                }


                DataResults.Rows.Add(ImagesToSearch[i].ToString(), TotalDeviation);


            }
        }

        private double TanimotoClassifier(double[] Table1, double[] Table2)
        {
            double Result = 0;
            double Temp1 = 0;
            double Temp2 = 0;

            double TempCount1 = 0, TempCount2 = 0, TempCount3 = 0;

            for (int i = 0; i < Table1.Length; i++)
            {
                Temp1 += Table1[i];
                Temp2 += Table2[i];
            }

            if (Temp1 == 0 || Temp2 == 0) Result = 100;
            if (Temp1 == 0 && Temp2 == 0) Result = 0;

            if (Temp1 > 0 && Temp2 > 0)
            {
                for (int i = 0; i < Table1.Length; i++)
                {
                    TempCount1 += (Table1[i] / Temp1) * (Table2[i] / Temp2);
                    TempCount2 += (Table2[i] / Temp2) * (Table2[i] / Temp2);
                    TempCount3 += (Table1[i] / Temp1) * (Table1[i] / Temp1);

                }

                Result = (100 - 100 * (TempCount1 / (TempCount2 + TempCount3 - TempCount1))); //Tanimoto
            }

            return (Result);

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            DataColumn dcItemValue = new DataColumn("Name");
            DataColumn dcItemN1 = new DataColumn("Deviation");
            dcItemN1.DataType = System.Type.GetType("System.Double");
            DataResults.Columns.Add(dcItemValue);   
            DataResults.Columns.Add(dcItemN1);

        } //....


        private void PreviewResults()
        {
            Bitmap ResultImage = null;
            pictureBox2.Image = ResultImage;
            if (dataGrid4.RowCount > 0)
            {
                try
                {

                    string ImageFile = dataGrid4[0, dataGrid4.CurrentRow.Index].Value.ToString();
                    ResultImage = new Bitmap(ImageFile);
                    pictureBox2.Image = ResultImage;

                }
                catch
                {
                    pictureBox2.Image =null;


                }

            }
        }

        private void dataGrid4_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            PreviewResults();
        }

        private void dataGrid4_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
          
        }

        private void dataGrid4_SelectionChanged(object sender, EventArgs e)
        {
            PreviewResults();
        }

        private void dataGrid4_KeyPress(object sender, KeyPressEventArgs e)
        {
            PreviewResults();
        }

    }
}
