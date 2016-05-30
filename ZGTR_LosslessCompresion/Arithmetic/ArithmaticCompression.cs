using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Compression
{
    class ArithmaticCompression
    {
        //private string _path;

        //public string Path {
        //                    get {return _path;}
        //                    set { _path = value; }
        //                    }

        private long encodingNum;
        private long size;
        static int segma;

        private int[] probabilties = new int[256];
        private int[] low = new int[256];
        private int[] high = new int[256];

        public double Encode(string path)
        {

            initProbabilityArr();

            byte[] file = File.ReadAllBytes(path);
            size = file.Length;
            foreach (byte b in file)
                probabilties[b] += 1;
            var length = file.Length;

            int tempLow = 0;
            long divideby;
            divideby = (long)Math.Ceiling((double)length / (1 << (sizeof(long)*8 -2)));


            for (int i = 0; i < 256; i++)
            {
                if (probabilties[i] > 0)
                {
                    probabilties[i] /= (int)divideby;
                    if (probabilties[i] == 0)
                        probabilties[i] = 1;
                    low[i] = tempLow;
                    high[i] = (tempLow + probabilties[i]-1);
                    tempLow += probabilties[i];

                }
            }


            
            //segma = tempHigh;
            int temp = 0;
            foreach (int i in high)
                temp += i;
            //segma = temp;
            segma = tempLow;
            int tempHigh = segma;
            tempLow = 0;
            int range;

            foreach (byte b in file)
            {
                if (probabilties[b] > 0)
                {
                    range = tempHigh - tempLow +1;
                    tempHigh = tempLow + ((range * high[b]) / segma) -1;
                    tempLow = tempLow + (range * low[b])/ segma;
                    tempHigh = (tempHigh < tempLow ? tempLow : tempHigh);
                }
            }
            

            //tempLow = 0;
            //double tempHigh = 1, range;
            //foreach (byte b in file)
            //{
            //    if (probabilties[b] > 0)
            //    {
            //        range = tempHigh - tempLow;
            //        tempHigh = tempLow + range * high[b];
            //        tempLow = tempLow + range * low[b];
            //    }
            //}


            //Dictionary<byte, double> d = new Dictionary<byte, double>();            
            encodingNum = (long)tempLow;
            return encodingNum;
        }


        public double Decode(string targetPath)
        {
            int x = (int)encodingNum;
            int range;
            byte symbol;
            //FileStream fs = new FileStream();
            MemoryStream stream = new MemoryStream();
            BinaryWriter binaryWriter = new BinaryWriter(stream);

            ushort lowBound = 0;
            ushort highBound = (ushort)segma;
            for (long i = 0; i < size; i++)
            {
                //range = (highBound - lowBound) + 1;
                //int count = (ushort)((((long)(x - lowBound) + 1) * segma - 1) / range);

                //symbol = findSymbotInRange(x);
                //binaryWriter.Write(symbol);
                //range = (ushort)(high[symbol] - low[symbol] + 1);
                //x = (ushort)(x - low[symbol]) + 1;
                //x = (ushort)(x * segma) / range;


                range = (highBound - lowBound + 1);
                x = (x - lowBound) + 1;
                x = (ushort)((x * segma) / range);

                symbol = findSymbotInRange(x);
                binaryWriter.Write(symbol);

                Console.WriteLine((char)symbol);
           
                range = highBound - lowBound + 1;
                highBound = (ushort)((lowBound + (range * high[symbol]) / segma) - 1);
                lowBound = (ushort)(lowBound + (range * low[symbol]) / segma);
                highBound= (highBound< lowBound? lowBound: highBound);


        
            }
            //FileStream writeStream = new FileStream("2.txt", FileMode.Create, FileAccess.Write);
            File.WriteAllBytes(targetPath, stream.ToArray());
            return 0;
        }

        private byte findSymbotInRange(double x)
        {
            for (int i = 0; i < 256; i++)
                //if (i > 95)
                    if (high[i] > x)
                        if (low[i] <= x)
                            return (byte)(i);


            return 255;
        }

        private void initProbabilityArr()
        {
            for (int i = 0; i < 256; i++)
                probabilties[i] = 0;
        }
    }
}
