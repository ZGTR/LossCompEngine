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
    public class LZ77AlgorithmEncoder
    {
        private LookaheadBuffer _lookaheadBuffer;
        public LookaheadBuffer LookaheadBuffer
        {
            get { return _lookaheadBuffer; }
        }

        private SlidingWindow _slidingWindow;
        public SlidingWindow SlidingWindow
        {
            get { return _slidingWindow; }
        }

        public LZ77AlgorithmEncoder(int slidingWindowSize, int bufferSize)
        {
            this._slidingWindow = new SlidingWindow(slidingWindowSize);
            this._lookaheadBuffer = new LookaheadBuffer(bufferSize);
        }

        public List<PhraseToken> EncodeImage(Bitmap imageToEncode, ref CompressionDataInfo info)
        {
            TimeChecker timeChecker = new TimeChecker();
            timeChecker.S1();

            List<string> streamOfIds = ImageManipulator.ConvertTo1DimArrSBSStringImage(imageToEncode);
            List<PhraseToken> encodedStream = EncodeStream(streamOfIds);

            info.EncodingStreamCount = encodedStream.Count;
            info.TimeEncoding = timeChecker.S2();
            return encodedStream;
        }

        public List<PhraseToken> TestMe()
        {
            List<PhraseToken> list = EncodeStream(TextManipulator.ConvertToListOfStrings("ABABCBABABCAD"));
            return list;
        }

        private List<PhraseToken> EncodeStream(List<string> streamOfIds)
        {
            List<PhraseToken> encodedStream = new List<PhraseToken>();
            for (int i = 0; i < streamOfIds.Count; i++)
            {
                List<string> nextBufferBlock = _lookaheadBuffer.GetNextStringToken(streamOfIds, i);
                PhraseToken nextPhraseToken = GetBestToken(i, nextBufferBlock, streamOfIds);
                if (!nextPhraseToken.IsRawToken)
                {
                    encodedStream.Add(nextPhraseToken);
                    _slidingWindow.SlideAhead(streamOfIds, i, nextPhraseToken.MatchLength + 1);
                    i += nextPhraseToken.MatchLength;
                }
                else
                {
                    encodedStream.Add(nextPhraseToken);
                    _slidingWindow.SlideAhead(streamOfIds, i, 1);                    
                }
            }
            return encodedStream;
        }

        private PhraseToken GetBestToken(int i, List<string> nextBufferBlock, List<string> streamOfIds)
        {
            PhraseToken nextPhraseToken = null;
            if (i + nextBufferBlock.Count < streamOfIds.Count)
            {
                nextPhraseToken = _slidingWindow.GetBestToken(nextBufferBlock, streamOfIds[i + nextBufferBlock.Count]);                
            }
            else
            {
                nextPhraseToken = _slidingWindow.GetBestToken(nextBufferBlock, null);
            }
            return nextPhraseToken ?? (new PhraseToken(streamOfIds[i]));
        }

        public List<PhraseToken> EncodeTextFile(string textInEncodePath, ref CompressionDataInfo info)
        {

            TimeChecker timeChecker = new TimeChecker();
            timeChecker.S1();

            List<String> streamOfIds = TextManipulator.ConvertTo1DimArr(textInEncodePath);
            List<PhraseToken> encodedStream = EncodeStream(streamOfIds);

            info.EncodingStreamCount = encodedStream.Count;
            info.TimeEncoding = timeChecker.S2();
            return encodedStream;
        }
    }
}