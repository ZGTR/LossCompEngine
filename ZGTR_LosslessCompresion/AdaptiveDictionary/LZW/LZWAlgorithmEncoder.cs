using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using ZGTR_LossCompWPFApp.GraphicsEngine;
using ZGTR_LosslessCompresion.ImageController;
using ZGTR_LosslessCompresion.ImageEngine;

namespace ZGTR_LosslessCompresion.AdaptiveDictionary.LZW
{
    public class LZWAlgorithmEncoder
    {
        public DictionaryTable Table { get; private set; }
        public DictionaryTable TableInit { get; private set; }

        public LZWAlgorithmEncoder()
        {
            this.Table = new DictionaryTable();
        }

        public List<string> EncodeImage(Bitmap imageToEncode, ref CompressionDataInfo info)
        {
            TimeChecker timeChecker = new TimeChecker();
            timeChecker.S1();

            List<string> streamOfIds = ImageManipulator.ConvertTo1DimArrBulkString(imageToEncode).Select(intId => intId.ToString()).ToList();
            List<string> streamEncoded = EncodeStream(streamOfIds);

            info.EncodingStreamCount = streamEncoded.Count;
            info.TimeEncoding = timeChecker.S2();

            return streamEncoded;
        }

        public List<string> EncodeTextFile(string textInEncodePath, ref CompressionDataInfo info)
        {
            TimeChecker timeChecker = new TimeChecker();
            timeChecker.S1();

            List<String> streamOfIds = TextManipulator.ConvertTo1DimArr(textInEncodePath);
            List<string> encodedStream = EncodeStream(streamOfIds);

            info.EncodingStreamCount = encodedStream.Count;
            info.TimeEncoding = timeChecker.S2();

            return encodedStream;
        }

        private List<string> EncodeStream(List<string> streamOfIds)
        {
            DateTime t1 = DateTime.Now;
            List<string> encodedStream = new List<string>();
            InitializeTableColor(streamOfIds);
            for (int i = 0; i < streamOfIds.Count; i++)
            {
                List<string> window = GetNewWindow(streamOfIds, i);
                if (window.Count > 1)
                {
                    encodedStream.Add(Table.KeyOf(window.GetRange(0, window.Count - 1)).ToString());
                    if (!Table.Contains(window))
                        Table.AddToTable(window);
                    i += window.Count - 2;
                }
                else
                {
                    encodedStream.Add(Table.KeyOf(window.ToList()).ToString());
                    if (!Table.Contains(window))
                        Table.AddToTable(window);
                }
            } 
            DateTime t2 = DateTime.Now;
            Console.WriteLine((t1 - t2).TotalSeconds);
            return encodedStream;
        }

        private void InitializeTableColor(List<string> streamOfIds)
        {
            Table.InitializeFromStream(streamOfIds);
            TableInit = new DictionaryTable(Table);
        }

        private List<string> GetNewWindow(List<string> streamOfIds, int iStart)
        {
            List<string> currentToken = new List<string>() {streamOfIds[iStart]};
            if (iStart + 1 < streamOfIds.Count)
            {
                for (int i = iStart + 1; i < streamOfIds.Count; i++)
                {
                    currentToken.Add(streamOfIds[i]);
                    if (Table.Contains(currentToken))
                    {
                        continue;
                    }
                    else
                    {
                        break;
                    }
                }
            }
            return currentToken;
        }
    }
}
