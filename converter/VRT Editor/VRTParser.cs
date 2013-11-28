using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace VRT_Editor
{
    class VRTParser
    {
        static void Main(string[] args)
        {
            List<string> newFile = new List<string>();
            List<string> outputFile = new List<string>();
            List<string> rasterBand = new List<string>();
            List<StringBuilder> newRasterBand = new List<StringBuilder>();

            ReadFile(newFile, @"1.vrt");

            bool isFound = false;

            foreach (var line in newFile)
            {
                if (line.Equals("  <VRTRasterBand dataType=\"Byte\" band=\"1\">"))
                {
                    isFound = true;
                }
                if (isFound)
                {
                    rasterBand.Add(line);
                }
                else
                {
                    outputFile.Add(line);
                }
                if (line.Equals("  </VRTRasterBand>"))
                {
                    isFound = false;
                    break;
                }
            }

            string vrtRasterMatch = "band=\"";
            string tifMatch = "0.tif</SourceFilename>";

            for (int band = 1; band <= 7; band++)
            {
                if (band == 6)
                {
                    continue;
                }
                for (int line = 0; line < rasterBand.Count; line++)
                {
                    StringBuilder initialize = new StringBuilder();
                    newRasterBand.Add(initialize);
                    if (line == 0)
                    {
                        int bandIndex = rasterBand[line].IndexOf(vrtRasterMatch);
                        string vrtRasterBand = rasterBand[line].Substring(0, bandIndex + vrtRasterMatch.Length);
                        newRasterBand[line].Append(vrtRasterBand);
                        newRasterBand[line].Append(band.ToString());
                        newRasterBand[line].Append(rasterBand[line].Substring(bandIndex + vrtRasterMatch.Length + 1));
                    }
                    else if (line == 4)
                    {
                        int tifIndex = rasterBand[line].IndexOf(tifMatch);
                        string tifString = rasterBand[line].Substring(tifIndex);
                        newRasterBand[line].Append(rasterBand[line].Substring(0, tifIndex - 1));
                        newRasterBand[line].Append(band.ToString());
                        newRasterBand[line].Append(tifString);          
                    }
                    else
                    {
                        newRasterBand[line].Append(rasterBand[line]);
                    }
                    outputFile.Add(newRasterBand[line].ToString());
                }
                newRasterBand.Clear();
            }

            outputFile.Add("</VRTDataset>");

            WriteFile(outputFile, @"output.txt");
        }

        private static void WriteFile(List<string> newFile, string fileName)
        {
            StreamWriter writer = new StreamWriter(fileName);
            using (writer)
            {
                for (int i = 0; i < newFile.Count; i++)
                {
                    writer.WriteLine(newFile[i]);
                }
            }
        }

        private static void ReadFile(List<string> newFile, string fileName)
        {
            StreamReader reader = new StreamReader(fileName);
            using (reader)
            {
                string line = reader.ReadLine();
                while (line != null)
                {
                    newFile.Add(line);
                    line = reader.ReadLine();
                }
            }
        }
    }
}
