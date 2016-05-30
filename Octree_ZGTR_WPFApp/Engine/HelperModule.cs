using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using Octree_ZGTR_WPFApp.Engine.EncodingPackage;

using Octree_ZGTR_WPFApp.Engine.OctreeTree;

namespace Octree_ZGTR_WPFApp.Engine
{
    public class HelperModule
    {
        public static string[] GetBitsVerticalString(EncodingConverter encoding, Color color)
        {
            string[] stringVertical = new string[8];
            string rString = encoding.ConvertIntToBin(color.R); // "00100111";
            //EncodingConverter.FixString(ref rString);
            string gString = encoding.ConvertIntToBin(color.G); // "11101000";
            //EncodingConverter.FixString(ref gString);
            string bString = encoding.ConvertIntToBin(color.B); // "01111111";
            //EncodingConverter.FixString(ref bString);
            for (int bitIndex = 0; bitIndex < 8; bitIndex++)
            {
                stringVertical[bitIndex] = "" + rString[bitIndex] + gString[bitIndex] + bString[bitIndex];
            }
            return stringVertical;
        }

        public static string[] GetColorStrings(EncodingConverter encoding, Color color)
        {
            string[] strArr = new string[3];
            string rString = encoding.ConvertIntToBin(color.R);
            //EncodingConverter.FixString(ref rString);
            string gString = encoding.ConvertIntToBin(color.G);
            //EncodingConverter.FixString(ref gString);
            string bString = encoding.ConvertIntToBin(color.B);
            //EncodingConverter.FixString(ref bString);
            strArr[0] = rString;
            strArr[1] = gString;
            strArr[2] = bString;
            return strArr;
        }

        public static Color GetColor(EncodingConverter encoding,  string binaryColor)
        {
            StringBuilder rString = new StringBuilder("00000000");
            StringBuilder gString = new StringBuilder("00000000");
            StringBuilder bString = new StringBuilder("00000000");
            for (int i = 0; i < binaryColor.Length / 3; i++)
            {
                string currentTriple = binaryColor.Substring(i * 3, 3);
                rString[i] = currentTriple[0];
                gString[i] = currentTriple[1];
                bString[i] = currentTriple[2];
            }
            Color colorToReturn = Color.FromArgb(encoding.ConvertBinToInt(rString.ToString()),
                                                 encoding.ConvertBinToInt(gString.ToString()),
                                                 encoding.ConvertBinToInt(bString.ToString()));
            return colorToReturn;
        }

        public static int[] GetColorSumComponents(List<Node> listOfAllChilds)
        {
            int[] colorCompAvg = new int[3];
            foreach (var child in listOfAllChilds)
            {
                Color childColor = child.Color;
                colorCompAvg[0] += childColor.R;
                colorCompAvg[1] += childColor.G;
                colorCompAvg[2] += childColor.B;
            }
            return colorCompAvg;
        }
    }
}
