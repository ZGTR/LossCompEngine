using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZGTR_LosslessCompresion.RLE.Method1
{
    public class DataUnit
    {
        public string Id { set; get; }
        public int RepeatCounter { set; get; }

        public DataUnit(string id, int repeatCounter)
        {
            this.Id = id;
            this.RepeatCounter = repeatCounter;
        }

        public DataUnit()
        {
            this.Id = String.Empty;
            this.RepeatCounter = 0;
        }
    }
}
