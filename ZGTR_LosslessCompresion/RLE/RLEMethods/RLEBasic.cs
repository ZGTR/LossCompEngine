using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using ZGTR_LossCompWPFApp.GraphicsEngine;
using ZGTR_LosslessCompresion.ImageController;
using ZGTR_LosslessCompresion.ImageEngine;
using ZGTR_LosslessCompresion.RLE.Method1;

namespace ZGTR_LosslessCompresion
{
    public static class RLEBasic
    {
        public static List<string> EncodeImage(Bitmap imageToEncode, ref CompressionDataInfo info)
        {
            TimeChecker timeChecker = new TimeChecker();
            timeChecker.S1();

            List<string> streamOfIds = ImageManipulator.ConvertTo1DimArrBulkString(imageToEncode).Select(intId => intId.ToString()).ToList();
            List<string> encodedStream =  EncodeStream(streamOfIds);
            
            info.EncodingStreamCount = encodedStream.Count;
            info.TimeEncoding = timeChecker.S2();
            return encodedStream;
        }

        public static List<string> EncodeTextFile(string fileStringPath, ref CompressionDataInfo info)
        {
            TimeChecker timeChecker = new TimeChecker();
            timeChecker.S1();

            List<string> streamOfIds = TextManipulator.ConvertTo1DimArr(fileStringPath);
            List<string> encodedStream = EncodeStream(streamOfIds);

            info.EncodingStreamCount = encodedStream.Count;
            info.TimeEncoding = timeChecker.S2();
            return encodedStream;
        }

        private static List<string> EncodeStream(List<string> streamOfIds)
        {
            List<string> compressedStream = new List<string>();
            List<string> miniCompressedStream = new List<string>();
            StringBuilder flagStr = new StringBuilder("00000000");
            int counterFlag = 0;
            for (int i = 0; i < streamOfIds.Count; i++)
            {
                counterFlag++;
                if (ShouldInsertFlag(counterFlag))
                {
                    compressedStream.AddRange(flagStr.ToString().Select(character => character.ToString()).ToList());
                    compressedStream.AddRange(miniCompressedStream.Select(character => character.ToString()).ToList());
                    miniCompressedStream = new List<string>();
                    counterFlag = 1;
                    flagStr = new StringBuilder("00000000");
                }
                //else
                //{
                    DataUnit dataUnit = CatchNextTokenEncode(streamOfIds, i);
                    if (dataUnit.RepeatCounter > 1)
                    {
                        miniCompressedStream.Add(dataUnit.RepeatCounter.ToString());
                        flagStr[counterFlag] = '1';
                        i += dataUnit.RepeatCounter - 1;
                    }
                    miniCompressedStream.Add(dataUnit.Id);
                //}
            }
            if (counterFlag != 0)
            {
                compressedStream.AddRange(flagStr.ToString().Select(character => character.ToString()).ToList());
                compressedStream.AddRange(miniCompressedStream.Select(character => character.ToString()).ToList());
            }
            return compressedStream;
        }

        private static bool ShouldInsertFlag(int counterFlag)
        {
            return counterFlag == 8;
        }

        private static bool ShouldRetrieveFlag(int counterFlag)
        {
            return counterFlag == 0;
        }

        public static DataUnit CatchNextTokenEncode(List<string> streamOfIds, int startIndexInStream)
        {
            DataUnit dataUnit = new DataUnit();
            string firstToken = streamOfIds[startIndexInStream];
            bool firstMatchOk = CheckFirstMatch(streamOfIds, startIndexInStream);
            int sameTokensCounter = 3;
            if (startIndexInStream + 3 < streamOfIds.Count)
            {
                for (int i = startIndexInStream + 3; i < streamOfIds.Count; i++)
                {
                    if (firstMatchOk)
                    {
                        if (String.Compare(streamOfIds[i], firstToken) == 0)
                        {
                            sameTokensCounter++;
                        }
                        else
                        {
                            break;
                        }
                    }
                    else
                    {
                        break;
                    }
                }
            }
            if (firstMatchOk)
            {
                //nextTokenList.Add(sameTokensCounter.ToString());
                dataUnit.RepeatCounter = sameTokensCounter;
            }
            //nextTokenList.Add(firstToken);
            dataUnit.Id = firstToken;
            return dataUnit;
        }

        private static bool CheckFirstMatch(List<string> streamOfIds, int startIndexInStream)
        {
            if (startIndexInStream + 2< streamOfIds.Count)
            {
                if ((streamOfIds[startIndexInStream] == streamOfIds[startIndexInStream + 1]) && (streamOfIds[startIndexInStream]  == streamOfIds[startIndexInStream + 2]))
                {
                    return true;
                }
            }
            return false;
        }

        public static List<string> DecodeStream(List<string> streamOfIds, ref CompressionDataInfo info)
        {
            TimeChecker timeChecker = new TimeChecker();
            timeChecker.S1();

            List<string> originalStream = DecodeStreamFromEncodedStream(streamOfIds);

            info.DecodingStreamCount = originalStream.Count;
            info.TimeDecoding = timeChecker.S2();
            return originalStream;
        }

        private static List<string> DecodeStreamFromEncodedStream(List<string> streamOfIds)
        {
            List<string> originalStream = new List<string>();
            //List<string> miniOriginalStream = new List<string>();
            string currentFlagStr = String.Empty;
            int counterFlag = 0;
            for (int i = 0; i < streamOfIds.Count; i++)
            {
                if (ShouldRetrieveFlag(counterFlag))
                {
                    currentFlagStr = GetString(streamOfIds, i, 8);
                    counterFlag = 8;
                    i += 7;
                }
                else
                {
                    List<string> nextTokenList = CatchNextTokenDecode(streamOfIds, i, currentFlagStr[8 - counterFlag].ToString());
                    originalStream.AddRange(nextTokenList);
                    if (nextTokenList.Count > 2)
                    {
                        i++;
                    }
                }
                counterFlag--;
            }
            return originalStream;
        }

        private static List<string> CatchNextTokenDecode(List<string> streamOfIds, int startIndex, string strFlag)
        {
            List<string> nextTokenList = new List<string>();
            if (strFlag == "1")
            {
                int repeatCount = Int32.Parse(streamOfIds[startIndex]);
                string strToRepeat = streamOfIds[startIndex + 1];
                nextTokenList.AddRange(GetListOfRepeats(strToRepeat, repeatCount));
            }
            else
            {
                nextTokenList.Add(streamOfIds[startIndex]);
            }
            return nextTokenList;
        }

        private static List<string> GetListOfRepeats(string strToRepeat, int repeatCount)
        {
            List<string> listToReturn = new List<string>();
            for (int i = 0; i < repeatCount; i++)
            {
                listToReturn.Add(strToRepeat);   
            }
            return listToReturn;
        }

        private static string GetString(List<string> streamOfIds, int startIndex, int count)
        {
            StringBuilder strToReturn = new StringBuilder();
            for (int i = startIndex; i < startIndex + count; i++)
            {
                if (i < streamOfIds.Count - 1)
                {
                    strToReturn.Append(streamOfIds[i]);
                }
                else
                {
                    break;
                }
            }
            return strToReturn.ToString();
        }
    }
}