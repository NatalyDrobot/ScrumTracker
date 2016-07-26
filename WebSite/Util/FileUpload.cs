using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Web;

namespace WebSite.Util
{
    public static class FileUpload
    {
        public static char DirSeparator = System.IO.Path.DirectorySeparatorChar;

        public static string FilesPath =
            HttpContext.Current.Server.MapPath("\\Content" + DirSeparator + "images" + DirSeparator + "avatars" + DirSeparator);

        public static string UploadFile(HttpPostedFileBase file)
        {
            // Check if we have a file
            if (null == file) return "";
            // Make sure the file has content
            if (!(file.ContentLength > 0)) return "";

            string fileName = DateTime.Now.Millisecond + file.FileName;
            string fileExt = Path.GetExtension(file.FileName);

            // Make sure we were able to determine a proper extension
            if (null == fileExt) return "";
            
            if (!Directory.Exists(FilesPath))
            {
               Directory.CreateDirectory(FilesPath);
            }
            
            string path = FilesPath + DirSeparator + fileName;
            
            ResizeImage(file, 100, 102, path);

            // Save our file
            //file.SaveAs(Path.GetFullPath(path));

            return fileName;
        }

        public static void DeleteFile(string fileName)
        {
            
            if (fileName.Length == 0) return;

            string path = FilesPath + DirSeparator + fileName;

            RemoveFile(path);
        }

        private static void RemoveFile(string path)
        {
            // Check if our file exists
            if (File.Exists(Path.GetFullPath(path)))
            {
                // Delete our file
                File.Delete(Path.GetFullPath(path));
            }
        }

        public static void ResizeImage(HttpPostedFileBase file, int width, int height, string path)
        {
           //Create a stream to save the file to when we're done resizing
            FileStream stream = new FileStream(Path.GetFullPath(path), FileMode.OpenOrCreate);

            // Convert our uploaded file to an image
            Image origImage = Image.FromStream(file.InputStream);
            // Create a new bitmap with the size of our thumbnail
            Bitmap tempBitmap = new Bitmap(width, height);

            // Create a new image that contains are quality information
            Graphics newImage = Graphics.FromImage(tempBitmap);
            newImage.CompositingQuality = CompositingQuality.HighQuality;
            newImage.SmoothingMode = SmoothingMode.HighQuality;
            newImage.InterpolationMode = InterpolationMode.HighQualityBicubic;

            // Create a rectangle and draw the image
            Rectangle imageRectangle = new Rectangle(0, 0, width, height);
            newImage.DrawImage(origImage, imageRectangle);

            // Save the final file
            tempBitmap.Save(stream, origImage.RawFormat);


            // Clean up the resources
            newImage.Dispose();
            tempBitmap.Dispose();
            origImage.Dispose();
            stream.Close();
            stream.Dispose();
           
        }
    }
}