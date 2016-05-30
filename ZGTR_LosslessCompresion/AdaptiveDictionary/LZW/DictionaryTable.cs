using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace ZGTR_LosslessCompresion.AdaptiveDictionary.LZW
{
    public class DictionaryTable
    {
        public Dictionary<int, List<string>> Table { get; private set; }
        public int TableKeyCounter { get; private set; }

        public DictionaryTable()
        {
            Table = new Dictionary<int, List<string>>();
            TableKeyCounter = 0;
        }

        public DictionaryTable(DictionaryTable tableIn)
        {
            this.Table = new Dictionary<int, List<string>>(tableIn.Table);
            this.TableKeyCounter = GetNextKeyAvailable();
        }

        private int GetNextKeyAvailable()
        {
            return this.Table.Keys.Max(key => key) + 1;
        }

        public void AddToTable(string str)
        {
            Table.Add(TableKeyCounter, new List<string>() { str });
            TableKeyCounter++;
        }

        public void AddToTable(List<string> listOfColors)
        {
            Table.Add(TableKeyCounter, new List<string>());
            Table[TableKeyCounter].AddRange(listOfColors);
            TableKeyCounter++;
        }

        public bool Contains(string value)
        {
            foreach (KeyValuePair<int, List<string>> keyValuePair in Table)
            {
                if (keyValuePair.Value.Count == 1)
                {
                    if (keyValuePair.Value[0] == value)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public bool Contains(List<string> currentToken)
        {
            bool checker = false;
            bool finalCheck = false;
            int counterToken = 0;
            foreach (KeyValuePair<int, List<string>> keyValuePair in Table)
            {
                if (keyValuePair.Value.Count == currentToken.Count)
                {
                    checker = false;
                    counterToken = 0;
                    finalCheck = false;
                    foreach (var str in keyValuePair.Value)
                    {
                        if (str == currentToken[counterToken])
                        {
                            checker = true;
                            counterToken++;
                        }
                        else
                        {
                            checker = false;
                            break;
                        }
                    }
                    if (checker && counterToken == currentToken.Count)
                    {
                        finalCheck = true;
                        break;
                    }
                }
            }
            return finalCheck;
        }

        public void InitializeFromStream(List<string> streamOfIds)
        {
            for (int i = 0; i < streamOfIds.Count; i++)
            {
                if (!this.Contains(streamOfIds[i]))
                {
                    this.AddToTable(streamOfIds[i]);
                }
            }
        }

        public int KeyOf(List<string> listOfIds)
        {
            bool checker = false;
            int counterToken = 0;
            int key = -1;
            foreach (KeyValuePair<int, List<string>> keyValuePair in Table)
            {
                if (keyValuePair.Value.Count == listOfIds.Count)
                {
                    checker = false;
                    counterToken = 0;
                    foreach (var str in keyValuePair.Value)
                    {
                        if (str == listOfIds[counterToken])
                        {
                            checker = true;
                            counterToken++;
                        }
                        else
                        {
                            checker = false;
                            break;
                        }
                    }
                    if (checker && counterToken == listOfIds.Count)
                    {
                        key = keyValuePair.Key;
                        break;
                    }
                }
            }
            return key;
        }

        public List<string> GetMatchedSymbol(int key)
        {
            try
            {
                return this.Table[key];
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
