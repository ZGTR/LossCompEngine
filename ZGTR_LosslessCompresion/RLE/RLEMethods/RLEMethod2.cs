using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using ZGTR_LossCompWPFApp.GraphicsEngine;
using ZGTR_LosslessCompresion.ImageController;
using ZGTR_LosslessCompresion.RLE.Method1;

namespace ZGTR_LosslessCompresion.RLE
{
    public class RLEMethod2
    {
        public static List<DataUnit> EncodeImage(Bitmap imageToEncode, ref CompressionDataInfo info)
        {
            TimeChecker timeChecker = new TimeChecker();
            timeChecker.S1();

            List<DataUnit> compressedStream = new List<DataUnit>();
            List<Color> colorArr = RLEMethod2.WrapDataUnitsAroundCorners(ImageManipulator.GetBitmapList(imageToEncode));
            //for (int i = 0; i < colorArr.Count; i++)
            //{
                List<string> currentRow = colorArr.Select(color => color.ToArgb().ToString()).ToList();
                List<DataUnit> currentRowTokens = TokenizeRow(currentRow);
                compressedStream.AddRange(currentRowTokens);
            //}

            info.EncodingStreamCount = compressedStream.Count;
            info.TimeEncoding = timeChecker.S2();
            return compressedStream;
        }

        private static List<Color> WrapDataUnitsAroundCorners(List<List<Color>> listOfDataUnits)
        {
            List<Color> listOfWrappedUnits = new List<Color>();
            bool isLineEven = true;
            for (int i = 0; i < listOfDataUnits.Count; i++)
            {
                if (isLineEven)
                {
                    listOfWrappedUnits.AddRange(listOfDataUnits[i]);
                    isLineEven = false;
                }
                else
                {
                    for (int j = listOfDataUnits[i].Count - 1; j >= 0; j--)
                    {
                        listOfWrappedUnits.Add(listOfDataUnits[i][j]);
                    }
                    isLineEven = true;
                }
            }
            return listOfWrappedUnits;
        }

        private static List<DataUnit> TokenizeRow(List<string> currentRow)
        {
            List<DataUnit> listToReturn = new List<DataUnit>();
            for (int i = 0; i < currentRow.Count; i++)
            {
                DataUnit dataUnit = RLEBasic.CatchNextTokenEncode(currentRow, i);
                listToReturn.Add(dataUnit);
                if (dataUnit.RepeatCounter > 1)
                {
                    i += dataUnit.RepeatCounter - 1;
                }
            }
            return listToReturn;
        }

        public static List<string> DecodeStream(List<DataUnit> streamOfIds, int imageWidth, ref CompressionDataInfo info)
        {
            TimeChecker timeChecker = new TimeChecker();
            timeChecker.S1();

            List<string> originalStream =  GetStreamFromDataUnits(streamOfIds, imageWidth);

            info.DecodingStreamCount = originalStream.Count;
            info.TimeDecoding = timeChecker.S2();
            return originalStream;
        }

        private static List<string> GetStreamFromDataUnits(List<DataUnit> dataUnitRowList, int imageWidth)
        {
            List<string> listToReturn = new List<string>();
            bool isEvenLine = false;
            int newLineCounter = 0;
            List<string> listTemp = new List<string>();
            for (int i = 0; i < dataUnitRowList.Count; i++)
            {
                DataUnit currentDataUnit = dataUnitRowList[i];
                if (currentDataUnit.RepeatCounter > 1)
                {
                    for (int j = 0; j < currentDataUnit.RepeatCounter; j++)
                    {
                        AddToList(currentDataUnit, ref newLineCounter, ref isEvenLine, imageWidth, ref listTemp, ref listToReturn);
                    }
                }
                else
                {
                    AddToList(currentDataUnit, ref newLineCounter, ref isEvenLine, imageWidth, ref listTemp, ref listToReturn);
                }
            }
            return listToReturn;
        }

        private static void AddToList(DataUnit currentDataUnit, ref int newLineCounter,ref bool isEvenLine, int imageWidth,ref List<string> listTemp, ref List<string> listToReturn)
        {
            listTemp.Add(currentDataUnit.Id);
            newLineCounter++;
            if (newLineCounter == imageWidth)
            {
                if (isEvenLine)
                {
                    listTemp.Reverse();
                    listToReturn.AddRange(listTemp);
                    listTemp = new List<string>();
                    isEvenLine = false;
                    newLineCounter = 0;
                }
                else
                {
                    listToReturn.AddRange(listTemp);
                    listTemp = new List<string>();
                    isEvenLine = true;
                    newLineCounter = 0;
                }
            }
        }
    }
}
