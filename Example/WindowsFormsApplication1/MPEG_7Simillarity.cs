// This code is based on the Caliph and Emir project: http://www.SemanticMetadata.net. */
//(c) Mathias Lux under GLU using Java
// Edit by Savvas Chatzichristofis - (c) Democritus university of Thrace - Using C#


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
using System.Text;

namespace WindowsFormsApplication1
{
    class MPEG_7Simillarity
    {
            private static double[,] QuantTable =
            {{0.010867, 0.057915, 0.099526, 0.144849, 0.195573, 0.260504, 0.358031, 0.530128},
                    {0.012266, 0.069934, 0.125879, 0.182307, 0.243396, 0.314563, 0.411728, 0.564319},
                    {0.004193, 0.025852, 0.046860, 0.068519, 0.093286, 0.123490, 0.161505, 0.228960},
                    {0.004174, 0.025924, 0.046232, 0.067163, 0.089655, 0.115391, 0.151904, 0.217745},
                    {0.006778, 0.051667, 0.108650, 0.166257, 0.224226, 0.285691, 0.356375, 0.450972}};
        protected static int[,] weightMatrix = new int[3, 64];

        public double calculateSCDDistance(double[] HistogramA, double[] HistogramB)
        {

            double result = 0;

            for (int l = 0; l < 64; l++)
            {
                result += Math.Abs(HistogramA[l] - HistogramB[l]);

            }
            return result;
        }

        public double calculateEHDDistance(double[] edgeHistogramA, double[] edgeHistogramB)
        {

            double result = 0;

            for (int i = 0; i < edgeHistogramA.Length; i++)
            {

                result += Math.Abs((float)QuantTable[i % 5, (int)edgeHistogramA[i]] - (float)QuantTable[i % 5, (int)edgeHistogramB[i]]);
            }
            for (int i = 0; i <= 4; i++)
            {
                result += 5f * Math.Abs((float)edgeHistogramA[i] - (float)edgeHistogramB[i]);
            }
            for (int i = 5; i < 80; i++)
            {
                result += Math.Abs((float)edgeHistogramA[i] - (float)edgeHistogramB[i]);
            }
            return result;
        }

        private static void setWeightingValues()
        {
            weightMatrix[0, 0] = 2;
            weightMatrix[0, 1] = weightMatrix[0, 2] = 2;
            weightMatrix[1, 0] = 2;
            weightMatrix[1, 1] = weightMatrix[1, 2] = 1;
            weightMatrix[2, 0] = 4;
            weightMatrix[2, 1] = weightMatrix[2, 2] = 2;
            for (int i = 0; i < 3; i++)
            {
                for (int j = 3; j < 64; j++)
                    weightMatrix[i, j] = 1;
            }
        }

        public  double calculateCLDDistance(int[] YCoeff1, int[] CbCoeff1, int[] CrCoeff1, int[] YCoeff2, int[] CbCoeff2, int[] CrCoeff2)
        {
            int numYCoeff1, numYCoeff2, CCoeff1, CCoeff2, YCoeff, CCoeff;
            //Numbers of the Coefficients of two descriptor values.
            numYCoeff1 = YCoeff1.Length;
            numYCoeff2 = YCoeff2.Length;
            CCoeff1 = CbCoeff1.Length;
            CCoeff2 = CbCoeff2.Length;
            //take the Minimal Coeff-number
            YCoeff = Math.Min(numYCoeff1, numYCoeff2);
            CCoeff = Math.Min(CCoeff1, CCoeff2);
            setWeightingValues();
            int j;
            int[] sum = new int[3];
            int diff;
            sum[0] = 0;
            for (j = 0; j < YCoeff; j++)
            {
                diff = (YCoeff1[j] - YCoeff2[j]);
                sum[0] += (weightMatrix[0, j] * diff * diff);
            }
            sum[1] = 0;
            for (j = 0; j < CCoeff; j++)
            {
                diff = (CbCoeff1[j] - CbCoeff2[j]);
                sum[1] += (weightMatrix[1, j] * diff * diff);
            }
            sum[2] = 0;
            for (j = 0; j < CCoeff; j++)
            {
                diff = (CrCoeff1[j] - CrCoeff2[j]);
                sum[2] += (weightMatrix[2, j] * diff * diff);
            }
            //returns the distance between the two desciptor values
            return Math.Sqrt(sum[0] * 1.0) + Math.Sqrt(sum[1] * 1.0) + Math.Sqrt(sum[2] * 1.0);
        }



 

    }
}
