using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using Octree_ZGTR_WPFApp.Engine.OctreeTree;

namespace Octree_ZGTR_WPFApp.Engine.EncodingPackage
{
    public class EncodingConverter
    {
        private Dictionary<string, int> _stringToIntDic;
        private string[] _intToStringDic;
        private int _numOfLeaves;

        public EncodingConverter(int numOfLeaves)
        {
            this._numOfLeaves = numOfLeaves;
            IntializeStructure();
        }

        private void IntializeStructure()
        {
            _stringToIntDic = new Dictionary<string, int>();
            _intToStringDic = new string[256];
            for (int i = 0; i < 256; i++)
            {
                string str = ConvertIntToBinByDict(i);
                FixString(ref str);
                _intToStringDic[i] = str;
                _stringToIntDic.Add(_intToStringDic[i], i);
            }
        }

        private string ConvertIntToBinByDict(long integerNumber)
        {
            long intNum = integerNumber;
            string binaryString = "";
            do
            {
                long tempValue = intNum % 2;
                binaryString = Convert.ToString(tempValue) + binaryString;
                intNum = intNum / 2;
            } while (intNum != 0);
            return binaryString;

        }

        private int ConvertBinToIntByDict(string binaryString)
        {
            int intValue = 0;
            for (int x = 1; x <= binaryString.Length; x++)
            {
                intValue = intValue +
                    Convert.ToInt32(binaryString.Substring((binaryString.Length - x), 1)) * (int)(Math.Pow(2, (x - 1)));
            }
            return intValue;
        }

        //public static void FixString(ref string str)
        //{
        //    StringBuilder s = new StringBuilder(str);
        //    if (str.Length < 8)
        //    {
        //        int zerosToAdd = 8 - str.Length;
        //        s = new StringBuilder(str);
        //        for (int i = 0; i < zerosToAdd; i++)
        //        {
        //            s.Insert(0, "0");
        //        }
        //    }
        //    str = s.ToString();
        //}

        private void FixStringInReverse(ref string str)
        {
            StringBuilder sOutput = new StringBuilder("");
            int counter = 0;
            while (counter < 8)
            {
                if (str[counter].ToString() == "0")
                {
                    counter++;
                    continue;
                }
                else
                {
                    sOutput.Insert(0, str[counter].ToString(), 8 - counter);
                    break;
                }
                //counter++;
            }
            if (String.IsNullOrEmpty(sOutput.ToString()))
            {
                sOutput.Insert(0, "0");
            }
            str = sOutput.ToString();
        }

        public int ConvertBinToInt(string stringIn)
        {
            //if (_stringToIntDic.ContainsKey(stringIn))
            //{
            //    return _stringToIntDic[stringIn];
            //}
            //else
            {
                
                //FixStringInReverse(ref stringIn);
                int i =  _stringToIntDic[stringIn];
                return i;
            }
        }

        public string ConvertIntToBin(int intIn)
        {
            string str = _intToStringDic[intIn];
            //FixString(ref str);
            return str;
        }


        public static string ConvertIntToBinStatic(long integerNumber)
        {
            long intNum = integerNumber;
            string binaryString = "";
            do
            {
                long tempValue = intNum % 2;
                binaryString = Convert.ToString(tempValue) + binaryString;
                intNum = intNum / 2;
            } while (intNum != 0);
            return binaryString;

        }

        public static int ConvertBinToIntStatic(string binaryString)
        {
            int intValue = 0;
            for (int x = 1; x <= binaryString.Length; x++)
            {
                intValue = intValue +
                    Convert.ToInt32(binaryString.Substring((binaryString.Length - x), 1)) * (int)(Math.Pow(2, (x - 1)));
            }
            return intValue;
        }

        public string[] GetBitsVerticalString(Color color)
        {
            string[] stringVertical = new string[8];
            string rString = this.ConvertIntToBin(color.R); // "00100111";
            EncodingConverter.FixString(ref rString);
            string gString = this.ConvertIntToBin(color.G); // "11101000";
            EncodingConverter.FixString(ref gString);
            string bString = this.ConvertIntToBin(color.B); // "01111111";
            EncodingConverter.FixString(ref bString);
            for (int bitIndex = 0; bitIndex < 8; bitIndex++)
            {
                stringVertical[bitIndex] = "" + rString[bitIndex] + gString[bitIndex] + bString[bitIndex];
            }
            return stringVertical;
        }

        public string[] GetColorStrings(Color color)
        {
            string[] strArr = new string[3];
            string rString = this.ConvertIntToBin(color.R);
            EncodingConverter.FixString(ref rString);
            string gString = this.ConvertIntToBin(color.G);
            EncodingConverter.FixString(ref gString);
            string bString = this.ConvertIntToBin(color.B);
            EncodingConverter.FixString(ref bString);
            strArr[0] = rString;
            strArr[1] = gString;
            strArr[2] = bString;
            return strArr;
        }

        public static void FixString(ref string str)
        {
            StringBuilder s = new StringBuilder(str);
            if (str.Length < 8)
            {
                int zerosToAdd = 8 - str.Length;
                s = new StringBuilder(str);
                for (int i = 0; i < zerosToAdd; i++)
                {
                    s.Insert(0, "0");
                }
            }
            str = s.ToString();
        }

        public Color GetColor(string binaryColor)
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
            Color colorToReturn = Color.FromArgb(this.ConvertBinToInt(rString.ToString()),
                                                 this.ConvertBinToInt(gString.ToString()),
                                                 this.ConvertBinToInt(bString.ToString()));
            return colorToReturn;
        }

        public int[] GetColorSumComponents(List<Node> listOfAllChilds)
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
