using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Media.Imaging;
using Color = System.Drawing.Color;
using Image = System.Windows.Controls.Image;

namespace Octree_ZGTR_WPFApp.Engine.ImageController
{
    public class ImageController
    {

        public static Image ConvertToImage(BitmapImage bitmapImage)
        {
            Image image = new Image();
            image.Source = bitmapImage;
            return image;
        }
        //// Create the writeable bitmap will be used to write and update.
        //private static WriteableBitmap _wb = new WriteableBitmap(50, 50, 96, 96, PixelFormats.Bgra32, null);
        //// Define the rectangle of the writeable image we will modify. 
        //// The size is that of the writeable bitmap.
        //private static Int32Rect _rect = new Int32Rect(0, 0, _wb.PixelWidth, _wb.PixelHeight);
        //// Calculate the number of bytes per pixel. 
        //private static int _bytesPerPixel = (_wb.Format.BitsPerPixel + 7) / 8;
        //// Stride is bytes per pixel times the number of pixels.
        //// Stride is the byte width of a single rectangle row.
        //private static int _stride = _wb.PixelWidth * _bytesPerPixel;

        //// Create a byte array for a the entire size of bitmap.
        //private static int _arraySize = _stride * _wb.PixelHeight;
        //private static byte[] _pixelArray = new byte[_arraySize];

        ////public static void SetImageWithPixelData(Image imageToSet, String stringURI)
        ////{
        ////    ////Update the color array with new random colors
        ////    //Random value = new Random();
        ////    //value.NextBytes(_colorArray);

        ////    _pixelArray = GetImageBytes(stringURI);

        ////    //Update writeable bitmap with the colorArray to the image.
        ////    _wb.WritePixels(_rect, _pixelArray, _stride, 0);

        ////    //Set the Image source.
        ////    imageToSet.Source = _wb;
        ////}

        public static Image GetImage(String stringURI)
        {
            Image imageToSet = new Image();
            Uri imageUri = new Uri(stringURI, UriKind.RelativeOrAbsolute);
            BitmapImage bitmap = new BitmapImage(imageUri);
            imageToSet.Source = bitmap;
            return imageToSet;
        }

        public static Color[,] GetBitmapVector(String stringURI)
        {
            try
            {
                System.Drawing.Bitmap bitmapForm = new Bitmap(stringURI);
                Color[,] colorArr = new Color[bitmapForm.Height, bitmapForm.Width];
                for (int i = 0; i < bitmapForm.Height; i++)
                {
                    for (int j = 0; j < bitmapForm.Width; j++)
                    {
                        colorArr[i, j] = bitmapForm.GetPixel(j, i);
                    }
                }
                return colorArr;
            }
            catch (Exception)
            {
            }
            return null;
        }


        public static int GetNumColorsOfImage(String stringURI)
        {
            System.Drawing.Bitmap bitmapForm = new Bitmap(stringURI);
            List<Color> colorArr = new List<Color>();
            for (int i = 0; i < bitmapForm.Height; i++)
            {
                for (int j = 0; j < bitmapForm.Width; j++)
                {
                    if (!colorArr.Contains(bitmapForm.GetPixel(j,i)))
                    {
                        colorArr.Add(bitmapForm.GetPixel(j, i));
                    }
                }
            }
            return colorArr.Count;
        }

        //private static int counterOutput = 0;
        //public static void SetImageFromTable(Image image)
        //{
        //    System.Drawing.Bitmap bitmapForm = new Bitmap(50, 50);
        //    int horizontalCounter = 0;
        //    bitmapForm = new Bitmap(50, 50);
        //    for (int i = 0; i < 50; i++)
        //    {
        //        for (int j = 0; j < 50; j++)
        //        {
        //            horizontalCounter = j + i * 50;
        //            if (inputVecotr[horizontalCounter] == -1)
        //            {
        //                bitmapForm.SetPixel(i, j, Color.Black);
        //            }
        //            else
        //            {
        //                bitmapForm.SetPixel(i, j, Color.White);
        //            }
        //        }
        //    }
        //    String str = "OutputPic" + counterOutput.ToString() + ".jpg";
        //    bitmapForm.Save(str);
        //    System.Drawing.Image im = System.Drawing.Image.FromFile(str);
        //    BitmapImage bitmapImage = ConvertToWPFBitmapImage(im);
        //    image.Source = bitmapImage;
        //    counterOutput++;
        //}

        //public static byte[] GetImageBytes(String stringURI)
        //{
        //    Uri imageUri = new Uri(stringURI, UriKind.RelativeOrAbsolute);
        //    byte[] pixelArray = new byte[_arraySize];
        //    //arraySize and stride previously defined
        //    BitmapImage bitmap = new BitmapImage(imageUri);
        //    // myBitmapImage.UriSource = new Uri(picStudentURI, UriKind.RelativeOrAbsolute);

        //    bitmap.CopyPixels(pixelArray, _stride, 0);
        //    return pixelArray;
        //}

        public static BitmapImage ConvertToWPFBitmapImage(System.Drawing.Image imageForm)
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

        //public static BitmapImage ConvertToWPFBitmapImage(System.Drawing.Bitmap bitmapFormIn)
        //{
        //    System.Drawing.Image bitmapForm = bitmapFormIn;
        //    // Winforms Image we want to get the WPF Image from...
        //    BitmapImage bitmapImage = new System.Windows.Media.Imaging.BitmapImage();
        //    bitmapImage.BeginInit();
        //        MemoryStream memoryStream = new MemoryStream();
        //        // Save to a memory stream...
        //        bitmapForm.Save(memoryStream, bitmapForm.RawFormat);
        //        // Rewind the stream...
        //        memoryStream.Seek(0, System.IO.SeekOrigin.Begin);
        //        bitmapImage.StreamSource = memoryStream;
        //    bitmapImage.EndInit();
        //    return bitmapImage;
        //}
    }
}
