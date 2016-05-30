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
    public class LZWAlgorithmDecoder
    {
        public DictionaryTable Table { get; private set; }
        public List<string> EncodedStream { get; private set; }
        //public Bitmap ImageToDecode { get; private set; }

        public LZWAlgorithmDecoder(DictionaryTable initialTable, List<string> encodedStream)
        {
            this.Table = initialTable;
            this.EncodedStream = encodedStream;
            //this.ImageToDecode = imageToDecode;
        }

        public List<string> DecodeStream(ref CompressionDataInfo info)
        {
            TimeChecker timeChecker = new TimeChecker();
            timeChecker.S1();

            List<string> originalStream = DecodeStream(EncodedStream);

            info.DecodingStreamCount = originalStream.Count;
            info.TimeDecoding = timeChecker.S2();
            return originalStream;
        }

        public void TestMe()
        {
            this.Table = new DictionaryTable();
            Table.Table.Add(0,new List<string>(){"A"});
            Table.Table.Add(1, new List<string>() { "B" });
            DecodeStream(TextManipulator.ConvertToListOfStrings("0221520"));
        }

        private List<string> DecodeStream(List<string> encodedStream)
        {
            DateTime t1 = DateTime.Now;
            List<string> dencodedStream = new List<string>();
            List<string> strSoFar = new List<string>();
            for (int i = 0; i < encodedStream.Count; i++)
            {
                int codeKey = Int32.Parse(encodedStream[i]);
                List<string> matchedCodeInTable = Table.GetMatchedSymbol(codeKey);
                if (matchedCodeInTable == null)
                {
                    matchedCodeInTable = new List<string>();
                    matchedCodeInTable.AddRange(strSoFar);
                    matchedCodeInTable.Add(strSoFar[0]);
                }
                dencodedStream.AddRange(matchedCodeInTable);
                if (strSoFar.Count != 0)
                {
                    List<string> listToAdd = new List<string>();
                    listToAdd.AddRange(strSoFar);
                    listToAdd.Add(matchedCodeInTable[0]);
                    this.Table.AddToTable(listToAdd);
                }
                strSoFar.Clear();
                strSoFar.AddRange(matchedCodeInTable);
            }
            DateTime t2 = DateTime.Now;
            Console.WriteLine((t1 - t2).TotalSeconds);
            return dencodedStream;
        }
    }
}
