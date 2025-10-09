using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD.Global_Classes
{
    public class clsUtil
    {

        public static string GenerateNewGUID()
        {
            return Guid.NewGuid().ToString();
        }

        public static bool CreateFolderIfDoesNotExist(string folderPath)
        {
            if (!Directory.Exists(folderPath))
            {

                try
                {
                    Directory.CreateDirectory(folderPath);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error creating folder: " + ex.Message);
                    return false;
                }
            }

            return true;    
        }

        public static string ReplaceFileNameWithGUID(string sourceFile)
        {
            FileInfo fileInfo = new FileInfo(sourceFile);

            string extension = fileInfo.Extension;

            string newFileName = GenerateNewGUID() + extension;

            return newFileName;
        }

        public static bool CopyImageToProjectImagesFolder(ref string sourceFile)
        {
            string destinationFolder = @"C:\DVLD-Pepole-Images\";

            if (!CreateFolderIfDoesNotExist(destinationFolder))
            {
                return false;
            }

            string newFilePath = destinationFolder + ReplaceFileNameWithGUID(sourceFile);

            try
            {

                File.Copy(sourceFile, newFilePath, true);  

            }
            catch (IOException iox)
            {
                MessageBox.Show(iox.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            sourceFile = newFilePath;
            return true;
        }
    }
}
