using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ArithmeticCoder;
using System.IO;

namespace Compression
{
    public class ArCoder
    {
        private static int[] probabilties = new int[256];
        private static int[] low = new int[256];
        private static int[] high = new int[256];
        public static int size;

        public static string encodeAndDecode(string path)
        {

            initProbabilityArr();

            byte[] file = File.ReadAllBytes(path);

            size = file.Length;
            foreach (byte b in file)
                probabilties[b] += 1;
            List<Coder.symbol> cds = new List<Coder.symbol>();

            ushort tempLow = 0;
            for (int i = 0; i < 256; i++)
            {
                if (probabilties[i] > 0)
                {
                    //probabilties[i] /= (int)divideby;
                    
                    low[i] = tempLow;
                    high[i] = (tempLow + probabilties[i]);
                    tempLow = (ushort)high[i];
                    cds.Add(new Coder.symbol((char)i, (ushort)low[i], (ushort)high[i]));

                }
            }
            System.IO.MemoryStream mem_test = new System.IO.MemoryStream();

            Coder c = new Coder(cds);
            c.Scale = (ushort)size;
            char[] input = new char[size];
            for (int i = 0; i < size; i++)
                input[i] = (char)file[i];

            String s = new string(input);
            //encode
            mem_test = c.compress(new String(input));

            //decode
            string expanded = c.expand(mem_test, size);

            MemoryStream stream = new MemoryStream();
            BinaryWriter binaryWriter = new BinaryWriter(stream);

            foreach (char ch in expanded)
                binaryWriter.Write((byte)ch);

            Random r = new Random();
            String str = Convert.ToString(r.Next(1000000));
            string targetPath = path+str+path;
            File.WriteAllBytes(targetPath, stream.ToArray());

            checkIfSameFile(path, targetPath);
            return targetPath;

        }

        private static void checkIfSameFile(string sourcePath, string targetPath) 
        {
            byte[] file1 = File.ReadAllBytes(sourcePath);
            byte[] file2 = File.ReadAllBytes(targetPath);
            for (int i = 0; i < size; i++)
                if (file1[i] != file2[i])
                    throw new Exception("Not same file");
        }

        private static void initProbabilityArr()
        {
            for (int i = 0; i < 256; i++)
                probabilties[i] = 0;
        }
    }
}
