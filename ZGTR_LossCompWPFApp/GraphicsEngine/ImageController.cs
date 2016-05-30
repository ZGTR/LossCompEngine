using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace ZGTR_LossCompWPFApp.GraphicsEngine
{
    public class ImageController
    {
        private static BitmapImage ConvertToWPFBitmapImage(System.Drawing.Image imageForm)
        {
            System.Drawing.Image image = imageForm;
            // Winforms Image we want to get the WPF Image from...
            BitmapImage bitmap = new System.Windows.Media.Imaging.BitmapImage();
            bitmap.BeginInit();
            MemoryStream memoryStream = new MemoryStream();
            // Save to a memory stream...
            image.Save(memoryStream, image.RawFormat);
            // Rewind the stream...
            memoryStream.Seek(0, System.IO.SeekOrigin.Begin);
            bitmap.StreamSource = memoryStream;
            bitmap.EndInit();
            return bitmap;
        }

        private static int counter = 0;
        public static Image ConvertToWPFImage(System.Drawing.Bitmap imageBitMap)
        {
            counter++;
            imageBitMap.Save("tempImage" + counter + ".jpg");
            System.Drawing.Image imageDecoded = System.Drawing.Image.FromFile("tempImage" + counter + ".jpg");

            // Winforms Image we want to get the WPF Image from...
            BitmapImage bitmap = new System.Windows.Media.Imaging.BitmapImage();
            bitmap.BeginInit();
            MemoryStream memoryStream = new MemoryStream();
            // Save to a memory stream...
            imageDecoded.Save(memoryStream, imageDecoded.RawFormat);
            // Rewind the stream...
            memoryStream.Seek(0, System.IO.SeekOrigin.Begin);
            bitmap.StreamSource = memoryStream;
            bitmap.EndInit();


            Image imageWPF = new Image();
            imageWPF.Source = bitmap;
            return imageWPF;
        }

        //public static Image ConvertToWPFImage(System.Drawing.Image imageForm)
        //{
        //    System.Drawing.Image image = imageForm;
        //    // Winforms Image we want to get the WPF Image from...
        //    BitmapImage bitmap = new System.Windows.Media.Imaging.BitmapImage();
        //    bitmap.BeginInit();
        //    MemoryStream memoryStream = new MemoryStream();
        //    // Save to a memory stream...
        //    image.Save(memoryStream, image.RawFormat);
        //    // Rewind the stream...
        //    memoryStream.Seek(0, System.IO.SeekOrigin.Begin);
        //    bitmap.StreamSource = memoryStream;
        //    bitmap.EndInit();
        //    Image imageWPF = new Image();
        //    imageWPF.Source = bitmap;
        //    return imageWPF;
        //}
    }
}
