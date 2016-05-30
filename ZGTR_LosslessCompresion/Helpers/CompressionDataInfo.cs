using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using ZGTR_LossCompWPFApp.GraphicsEngine;

namespace ZGTR_LosslessCompresion
{
    public class CompressionDataInfo
    {
        public double TimeEncoding { get; set; }
        public double TimeDecoding { get; set; }
        public int EncodingStreamCount { get; set; }

        private int _decodingStreamCount;
        public int DecodingStreamCount
        {
            get
            {
                return _decodingStreamCount;
            }
            set 
            {
                _decodingStreamCount = value;
                CalculateCompressionRatio();
            }
        }

        public int FileSizeInKB { get; private set; }
        public string CompressionRatio { get; private set; }

        public CompressionDataInfo(string filePath)
        {
            FileSizeInKB = (int)(new FileInfo(filePath).Length / (1024));
            // Init Value 
            CompressionRatio = "1:1";
        }


        private void CalculateCompressionRatio()
        {
            double ratioDouble = DecodingStreamCount / (double)EncodingStreamCount;
            this.CompressionRatio = String.Format("{0:0.0}", ratioDouble) +":1";
        }
    }
}
