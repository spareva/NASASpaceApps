using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sbirshtayn
{
    class Program
    {
        static void Main(string[] args)
        {
            Process cmd = new Process();
            // initialize some things
            Init(cmd);
            


            string execDir = AppDomain.CurrentDomain.BaseDirectory;
            string[] dirs = Directory.GetDirectories(execDir);

            foreach (var dir in dirs)
            {
                string[] fileNames = GetFileNames(dir, "*.tif");
                foreach (var fileName in fileNames)
                {
                    TranslateTifftoVrt(dir +"\\"+ fileName, cmd);
                }
            }


            // finalize
            cmd.StandardInput.Flush();
            cmd.StandardInput.Close();
            Console.WriteLine(cmd.StandardOutput.ReadToEnd());
        }

        private static void Init(Process cmd)
        {
            cmd.StartInfo.FileName = "cmd.exe";
            cmd.StartInfo.RedirectStandardInput = true;
            cmd.StartInfo.RedirectStandardOutput = true;
            cmd.StartInfo.CreateNoWindow = true;
            cmd.StartInfo.UseShellExecute = false;

            cmd.Start();

            /* execute "dir" */
            // Set fw tools environment
            cmd.StandardInput.WriteLine(@"C:\""Program Files (x86)""\FWTools2.4.7\setfw.bat");
            cmd.StandardInput.WriteLine(@"C:\""Program Files (x86)""\FWTools2.4.7\bin\setfwenv.bat");
            //cmd.StandardInput.WriteLine(@"gdalinfo c:\users\nikita\desktop\AERONET_BAHRAIN.2013104.terra.250m.tif > c:\users\nikita\desktop\test.txt");
        }


        private static string[] GetFileNames(string path, string filter)
        {
            string[] files = Directory.GetFiles(path, filter);
            for(int i = 0; i < files.Length; i++)
                files[i] = Path.GetFileName(files[i]);
            return files;
        }

        static void TranslateTifftoVrt(string fileName, Process cmd)
        {
            cmd.StandardInput.WriteLine("gdal_translate -of VRT " + fileName + " " + fileName + ".vrt");
        }

        string GenerateCombinedVrt(List<string> filePaths)
        {
            return null;
        }

        void TranslateVrtToTiff(string filePath)
        {

        }

        void CreatePyramidFile(string filePath)
        {

        }
    }
}
