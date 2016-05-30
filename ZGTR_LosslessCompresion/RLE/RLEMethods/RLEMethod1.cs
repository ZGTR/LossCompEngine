using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using ZGTR_LossCompWPFApp.GraphicsEngine;
using ZGTR_LosslessCompresion.ImageController;
using ZGTR_LosslessCompresion.ImageEngine;
using ZGTR_LosslessCompresion.RLE.Method1;

namespace ZGTR_LosslessCompresion.RLE
{
    public class RLEMethod1
    {
        public static List<List<DataUnit>> EncodeImage(Bitmap imageToEncode, ref CompressionDataInfo info)
        {
            TimeChecker timeChecker = new TimeChecker();
            timeChecker.S1();

            List<List<DataUnit>> compressedStream = new List<List<DataUnit>>();
            List<List<Color>> colorArr = ImageManipulator.GetBitmapList(imageToEncode);
            for (int i = 0; i < colorArr.Count; i++)
            {
                List<string> currentRow = colorArr[i].Select(color => color.ToArgb().ToString()).ToList();
                List<DataUnit> currentRowTokens = TokenizeRow(currentRow);
                compressedStream.Add(currentRowTokens);
            }

            info.EncodingStreamCount = compressedStream.Count;
            info.TimeEncoding = timeChecker.S2();
            return compressedStream;
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

        public static List<string> EncodeTextStream(string text)
        {
            throw new NotImplementedException();
        }

        public static List<string> DecodeStream(List<List<DataUnit>> streamOfIds, ref CompressionDataInfo info)
        {
            TimeChecker timeChecker = new TimeChecker();
            timeChecker.S1();

            List<string> listToReturn = DecodeStreamFromEncodedStream(streamOfIds);

            info.DecodingStreamCount = listToReturn.Count;
            info.TimeDecoding = timeChecker.S2();
            return listToReturn;
        }

        private static List<string> DecodeStreamFromEncodedStream(List<List<DataUnit>> streamOfIds)
        {
            List<string> listToReturn = new List<string>();
            for (int i = 0; i < streamOfIds.Count; i++)
            {
                List<string> currentRow = GetRowFromDataUnits(streamOfIds[i]);
                listToReturn.AddRange(currentRow);
            }
            return listToReturn;
        }

        private static List<string> GetRowFromDataUnits(List<DataUnit> dataUnitRowList)
        {
            List<string> listToReturn = new List<string>();
            for (int i = 0; i < dataUnitRowList.Count; i++)
            {
                DataUnit currentDataUnit = dataUnitRowList[i];
                if (currentDataUnit.RepeatCounter > 1)
                {
                    for (int j = 0; j < currentDataUnit.RepeatCounter; j++)
                    {
                        listToReturn.Add(currentDataUnit.Id);
                    }
                }
                else
                {
                    listToReturn.Add(currentDataUnit.Id);
                }
            }
            return listToReturn;
        }
    }
}
