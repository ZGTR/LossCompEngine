using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZGTR_LosslessCompresion.ImageEngine;

namespace ZGTR_LosslessCompresion.AdaptiveDictionary
{
    public class SlidingWindow
    {
        private int _slidingWindowSize;
        public int SlidingWindowSize
        {
            get { return _slidingWindowSize; }
        }

        private List<string> _window;
        public List<string>  Window
        {
            get { return _window; }
        }

        public string WindowString
        {
            get { return TextManipulator.ConvertToString(this._window); }
        }

        public SlidingWindow(int slidingWindowSize)
        {
            this._slidingWindowSize = slidingWindowSize;
            this._window = new List<string>();
        }

        public void SlideAhead(List<string> streamOfIds, int iStart, int slidingCount)
        {
            for (int i = iStart; i < iStart + slidingCount; i++)
            {
                if (_window.Count > _slidingWindowSize - 1)
                {
                    _window.RemoveAt(0);
                }
                _window.Add(streamOfIds[i]);
                if (!(i < streamOfIds.Count - 1))
                {
                    break;
                }
            }
        }

        public PhraseToken GetBestToken(List<string> nextBufferBlock, string postString)
        {
            string strNextBufferBlock = TextManipulator.ConvertToString(nextBufferBlock.ToArray());
            PhraseToken tokenToReturn = null;
            int maxLengthMatched = Int32.MinValue;
            for (int spBegin = 0; spBegin < nextBufferBlock.Count; spBegin++)
            {
                string currentStringToCheck = strNextBufferBlock.Substring(0, nextBufferBlock.Count - spBegin);
                if (WindowString.Contains(currentStringToCheck) && currentStringToCheck.Length > maxLengthMatched)
                {
                    maxLengthMatched = currentStringToCheck.Length;
                    if (currentStringToCheck.Length == nextBufferBlock.Count)
                    {
                        tokenToReturn = new PhraseToken(this._slidingWindowSize - (this._window.Count - WindowString.IndexOf(currentStringToCheck)),
                                                        currentStringToCheck.Length,
                                                        postString);
                    }
                    else
                    {
                        tokenToReturn = new PhraseToken(this._slidingWindowSize - (this._window.Count - WindowString.IndexOf(currentStringToCheck)),
                                                        currentStringToCheck.Length,
                                                        nextBufferBlock[currentStringToCheck.Length]);
                    }
                }
            }
            return tokenToReturn;
        }

        public List<string> GetStringInWindow(int offsetInWindow, int matchLength)
        {
            return this.WindowString.Substring(offsetInWindow - (this._slidingWindowSize - this._window.Count),
                                               matchLength).Select(character => character.ToString()).ToList();
        }
    }
}
