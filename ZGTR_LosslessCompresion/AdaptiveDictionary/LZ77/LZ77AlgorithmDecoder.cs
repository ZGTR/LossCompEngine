using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using ZGTR_LossCompWPFApp.GraphicsEngine;
using ZGTR_LosslessCompresion.ImageController;
using ZGTR_LosslessCompresion.ImageEngine;

namespace ZGTR_LosslessCompresion.AdaptiveDictionary
{
    public class LZ77AlgorithmDecoder
    {
        public SlidingWindow SlidingWindow{ get; private set; }
        public List<PhraseToken> EnocededStream { get; private set; }

        public LZ77AlgorithmDecoder(int slidingWindowSize, List<PhraseToken> encodedStream)
        {
            this.SlidingWindow = new SlidingWindow(slidingWindowSize);
            this.EnocededStream = encodedStream;
        }

        public List<string> DecodeImage(ref CompressionDataInfo info)
        {
            TimeChecker timeChecker = new TimeChecker();
            timeChecker.S1();

            List<string> originalStream = ImageManipulator.ConvertToBulkStringImage(DecodeStream(EnocededStream));

            info.DecodingStreamCount = originalStream.Count;
            info.TimeDecoding = timeChecker.S2();
            return originalStream;
        }

        private List<string> DecodeStream(List<PhraseToken> encodedStream)
        {
            List<string> decodedStream = new List<string>();
            for (int i = 0; i < encodedStream.Count; i++)
            {
                PhraseToken currentToken = encodedStream[i];
                if (currentToken.IsRawToken)
                {
                    decodedStream.Add(currentToken.StrInDictionary);
                    this.SlidingWindow.SlideAhead(decodedStream, decodedStream.Count - 1, 1);
                }
                else
                {
                    int offset = currentToken.Offset;
                    int matchLength = currentToken.MatchLength;
                    string postSymbol = currentToken.FirstPostSymbol;
                    List<string> strDecoded = this.SlidingWindow.GetStringInWindow(offset, matchLength);
                    decodedStream.AddRange(strDecoded);
                    this.SlidingWindow.SlideAhead(decodedStream, decodedStream.Count - strDecoded.Count, strDecoded.Count);
                    if (postSymbol != null)
                        decodedStream.Add(postSymbol);
                    this.SlidingWindow.SlideAhead(decodedStream, decodedStream.Count - 1, 1);
                }
            }
            return decodedStream;
        }

        public List<string> DecodeTextFile(ref CompressionDataInfo info)
        {
            TimeChecker timeChecker = new TimeChecker();
            timeChecker.S1();

            List<string> originalStream = this.DecodeStream(this.EnocededStream);

            info.DecodingStreamCount = originalStream.Count;
            info.TimeDecoding = timeChecker.S2();
            return originalStream;
        }
    }
}