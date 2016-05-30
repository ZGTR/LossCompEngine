using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace ZGTR_LosslessCompresion.ImageController
{
    public class ImageManipulator
    {
        public static List<string> ConvertTo1DimArrBulkString(Bitmap bitmapToEncode)
        {
            List<int> streamIds = new List<int>();
            for (int i = 0; i < bitmapToEncode.Height; i++)
            {
                for (int j = 0; j < bitmapToEncode.Width; j++)
                {
                    streamIds.Add(bitmapToEncode.GetPixel(j, i).ToArgb());
                }
            }
            return streamIds.Select(num => num.ToString()).ToList();
        }

        public static Bitmap RetrieveImage(List<string> streamOfIds, int width, int height)
        {
            List<int> listOfColors = streamOfIds.Select(strId => Int32.Parse(strId)).ToList();
            Bitmap bitmap = new Bitmap(width, height);
            for (int i = 0; i < bitmap.Height; i++)
            {
                for (int j = 0; j < bitmap.Width; j++)
                {
                    try
                    {
                        Color currentColor = Color.FromArgb(listOfColors[i * width + j]);
                        bitmap.SetPixel(j, i, currentColor);
                    }
                    catch (Exception)
                    {
                       
                    }

                }
            }
            return bitmap;
        }

        public static Bitmap RetrieveImage(Color[,] colorArr)
        {
            Bitmap bitmap = new Bitmap(colorArr.GetLength(0), colorArr.GetLength(1));
            for (int i = 0; i < bitmap.Height; i++)
            {
                for (int j = 0; j < bitmap.Width; j++)
                {
                    bitmap.SetPixel(j, i, colorArr[i,j]);
                }
            }
            return bitmap;
        }

        public static Color[,] GetBitmapArray(Bitmap bitmapForm)
        {
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

        public static Color[,] GetSquaredBitmapArray(Bitmap bitmapForm)
        {
            int dim = Math.Min(bitmapForm.Width, bitmapForm.Height);
            if (dim % 2 != 0)
            {
                dim--;
            }
            Color[,] colorArr = new Color[dim, dim];
            for (int i = 0; i < dim; i++)
            {
                for (int j = 0; j < dim; j++)
                {
                    colorArr[i, j] = bitmapForm.GetPixel(j, i);
                }
            }
            return colorArr;
        }

        public static List<List<Color>> GetBitmapList(Bitmap bitmapForm)
        {
            List<List<Color>> colorArr = new List<List<Color>>();
            for (int i = 0; i < bitmapForm.Height; i++)
            {
                List<Color> rowList = new List<Color>();
                for (int j = 0; j < bitmapForm.Width; j++)
                {
                   rowList.Add(bitmapForm.GetPixel(j, i));
                }
                colorArr.Add(rowList);
            }
            return colorArr;
        }

        public static List<string> ConvertTo1DimArrSBSStringImage(Bitmap bitmapToEncode)
        {
            string imageColorDelimiter = "+";
            List<int> streamIds = new List<int>();
            List<string> streamIdsStrings = new List<string>();
            for (int i = 0; i < bitmapToEncode.Height; i++)
            {
                for (int j = 0; j < bitmapToEncode.Width; j++)
                {
                    streamIds.Add(bitmapToEncode.GetPixel(j, i).ToArgb());
                }
            }
            for (int i = 0; i < streamIds.Count; i++)
            {
                List<string> str = (streamIds[i] + imageColorDelimiter).ToString().ToArray().Select(character => character.ToString()).ToList();
                streamIdsStrings.AddRange(str);
            }
            return streamIdsStrings;
        }

        public static List<string> ConvertToBulkStringImage(List<string> decodeStream)
        {
            StringBuilder strAll = new StringBuilder(String.Empty);
            foreach (var str in decodeStream)
            {
                strAll.Append(str);
            }
            List<string> listOfStrings = strAll.ToString().Split('+').ToList();
            listOfStrings.RemoveAt(listOfStrings.Count - 1);
            return listOfStrings;
        }
    }
}
