using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZGTR_LosslessCompresion.AdaptiveDictionary
{
    public class PhraseToken
    {
        private int _offset;
        public int Offset
        {
            get { return _offset; }
        }

        private int _matchLength;
        public int MatchLength
        {
            get { return _matchLength; }
        }

        private string _firstPostSymbol;
        public string FirstPostSymbol
        {
            get { return _firstPostSymbol; }
        }

        private bool _isRawToken;
        public bool IsRawToken
        {
            get { return _isRawToken; }
        }

        private string _strInDictionary;
        public string StrInDictionary
        {
            get { return _strInDictionary; }
        }

        public PhraseToken(int offset, int numOfMatches, string firstPostSymbol)
        {
            this._strInDictionary = String.Empty;
            this._offset = offset;
            this._matchLength = numOfMatches;
            this._firstPostSymbol = firstPostSymbol;
            this._isRawToken = false;
        }

        public PhraseToken(string strInDictionary)
        {
            this._strInDictionary = strInDictionary;
            this._offset = -1;
            this._matchLength = -1;
            this._firstPostSymbol = String.Empty;
            this._isRawToken = true;
        }
    }
}
