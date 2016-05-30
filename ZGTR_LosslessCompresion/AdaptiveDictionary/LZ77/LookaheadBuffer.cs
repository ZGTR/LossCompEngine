using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZGTR_LosslessCompresion.AdaptiveDictionary
{
    public class LookaheadBuffer
    {
        private int _bufferSize;
        public int BufferSize
        {
            get { return _bufferSize; }
        }

        private string[] _buffer;
        public string[] Buffer
        {
            get { return _buffer; }
        }

        public LookaheadBuffer(int bufferSize)
        {
            this._bufferSize = bufferSize;
            this._buffer = new string[bufferSize];
        }

        public List<string> GetNextStringToken(List<string> streamOfIds, int iStart)
        {
            List<string> listToReturn = new List<string>();
            for (int i = iStart; i < iStart + _bufferSize; i++)
            {
                listToReturn.Add(streamOfIds[i]);
                if (!(i < streamOfIds.Count - 1))
                {
                    break;
                }
            }
            return listToReturn;
        }
    }
}
