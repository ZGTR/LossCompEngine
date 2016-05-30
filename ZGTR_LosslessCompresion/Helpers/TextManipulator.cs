using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ZGTR_LosslessCompresion.ImageEngine
{
    public class TextManipulator
    {
        //public static List<int> ConverTo1DimArr(string text)
        //{
        //    List<int> streamIds = new List<int>();
        //    for (int i = 0; i < bitmapImage.Height; i++)
        //    {
        //        for (int j = 0; j < bitmapImage.Width; j++)
        //        {
        //            streamIds.Add(bitmapImage.GetPixel(i, j).ToArgb());
        //        }
        //    }
        //    return streamIds;
        //}

        public static string ConvertToString(string[] strArr)
        {
            StringBuilder stringBuilder = new StringBuilder(String.Empty);
            for (int i = 0; i < strArr.Length; i++)
            {
                stringBuilder.Append(strArr[i]);
            }
            return stringBuilder.ToString();
        }

        public static string ConvertToString(List<string> strArr)
        {
            StringBuilder stringBuilder = new StringBuilder(String.Empty);
            for (int i = 0; i < strArr.Count; i++)
            {
                stringBuilder.Append(strArr[i]);
            }
            return stringBuilder.ToString();
        }

        public static List<string> ConvertToListOfStrings(string str)
        {
            return str.ToString().Select(character => character.ToString()).ToList();
        }

        public static List<string> ConvertTo1DimArr(string fileStringPath)
        {
            StreamReader reader = new StreamReader(fileStringPath);
            string str = reader.ReadToEnd();
            reader.Close();
            return str.Select(str1 => str1.ToString()).ToList();
        }

        public static void RetrieveToTextFile(List<string> decoder, string targettextfileTxt)
        {
            StreamWriter writer = new StreamWriter(targettextfileTxt);
            for (int i = 0; i < decoder.Count; i++)
            {
                writer.Write(decoder[i]);
            }
            writer.Close();
        }
    }
}
