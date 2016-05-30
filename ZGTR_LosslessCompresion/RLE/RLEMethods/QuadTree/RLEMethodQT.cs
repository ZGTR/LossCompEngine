using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using ZGTR_LossCompWPFApp.GraphicsEngine;
using ZGTR_LosslessCompresion.ImageController;
using ZGTR_LosslessCompresion.RLE.RLEMethods.QuadTree;

namespace ZGTR_LosslessCompresion.RLE.Method1
{
    public class RLEMethodQT
    {

        public static RLEQuadTree EncodeImage(Bitmap imageToEncode, ref CompressionDataInfo info)
        {
            TimeChecker timeChecker = new TimeChecker();
            timeChecker.S1();

            Color[,] colorArr = ImageManipulator.GetBitmapArray(imageToEncode);
            RLEQuadTree tree = new RLEQuadTree(colorArr);
            tree.BuildTree();

            info.EncodingStreamCount = tree.NodesCount;
            info.TimeEncoding = timeChecker.S2();
            return tree;
        }

        public static Color[,] DecodeImage(RLEQuadTree tree, ref CompressionDataInfo info)
        {
            TimeChecker timeChecker = new TimeChecker();
            timeChecker.S1();

            Color[,] colorArr = tree.RetriveColorArrFromQuadTree();

            info.DecodingStreamCount = tree.ColorArr.GetLength(0) * tree.ColorArr.GetLength(1);
            info.TimeDecoding = timeChecker.S2();
            return colorArr;
        }
    }
}
