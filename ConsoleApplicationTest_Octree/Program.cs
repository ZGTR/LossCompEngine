using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Text;
using Octree_ZGTR_WPFApp;

using Octree_ZGTR_WPFApp.Engine.OctreeTree;
using Octree_ZGTR_WPFApp.Engine.OctreeTree.OctreeTypes.AbstractOctree;
using Octree_ZGTR_WPFApp.Engine.OctreeTree.OctreeTypes.UnBalanced;
using ZGTR_LosslessCompresion;
using ZGTR_LosslessCompresion.AdaptiveDictionary;
using ZGTR_LosslessCompresion.AdaptiveDictionary.LZW;
using ZGTR_LosslessCompresion.ImageController;
using ZGTR_LosslessCompresion.ImageEngine;
using ZGTR_LosslessCompresion.RLE;
using ZGTR_LosslessCompresion.RLE.Method1;

namespace ConsoleApplicationTest_Octree
{
    public class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            // ------- 1 ------- 
            //Bitmap bitmap = new Bitmap("1.jpg");
            //List<string> encoder = RLEBasic.EncodeImage(bitmap);
            //List<string> decoder = RLEBasic.DecodeStream(encoder);
            //Bitmap image = ImageManipulator.RetrieveImage(decoder, bitmap.Width, bitmap.Height);
            //image.Save("target.jpg");

            //List<string> encoder = RLEBasic.EncodeTextFile("testTextFile.txt");
            //List<string> decoder = RLEBasic.DecodeStream(encoder);
            //TextManipulator.RetrieveToTextFile(decoder, "targetTextFile.txt");


            // ------- 2 -------
            //Bitmap bitmap = new Bitmap("1.jpg");
            //List<List<DataUnit>> listOfDURLEMethod1 = RLEMethod1.EncodeImage(bitmap);
            //List<List<DataUnit>> encoderRLEMethod1 = RLEMethod1.EncodeImage(bitmap);
            //List<string> decoderRLEMethod1 = RLEMethod1.DecodeStream(encoderRLEMethod1);
            //Bitmap imageRLEMethod1 = ImageManipulator.RetrieveImage(decoderRLEMethod1, bitmap.Width, bitmap.Height);
            //imageRLEMethod1.Save("target.jpg");

            // ------- 3 -------
            //Bitmap bitmap = new Bitmap("1.jpg");
            //List<DataUnit> listOfDU = RLEMethod2.EncodeImage(bitmap);
            //List<DataUnit> encoder = RLEMethod2.EncodeImage(bitmap);
            //List<string> decoder = RLEMethod2.DecodeStream(encoder, bitmap.Width);
            //Bitmap image = ImageManipulator.RetrieveImage(decoder, bitmap.Width, bitmap.Height);
            //image.Save("target.jpg");

            // ------- 4 -------
            //Bitmap bitmap = new Bitmap("1.jpg");
            //RLEMethodQT rleMethodQt = new RLEMethodQT(bitmap);
            //rleMethodQt.EncodeImage();
            //Color[,] colorArr = rleMethodQt.DecodeImage();
            //Bitmap image = ImageManipulator.RetrieveImage(colorArr);
            //image.Save("target.jpg");

            // ------- 5 -------
            //Bitmap bitmap = new Bitmap("cube.jpg");
            //LZ77AlgorithmEncoder lZ77E = new LZ77AlgorithmEncoder(15, 15);
            //List<PhraseToken> encodedStream = lZ77E.EncodeImage(bitmap);
            //LZ77AlgorithmDecoder lZ77D = new LZ77AlgorithmDecoder(15, encodedStream);
            //List<string> decoder = lZ77D.DecodeImage();
            //Bitmap image = ImageManipulator.RetrieveImage(decoder, bitmap.Width, bitmap.Height);
            //image.Save("target.jpg");

            //LZ77AlgorithmEncoder lZ77 = new LZ77AlgorithmEncoder(15, 15);
            //List<PhraseToken> encodedStream = lZ77.EncodeTextFile("testTextFile.txt");
            //LZ77AlgorithmDecoder lZD = new LZ77AlgorithmDecoder(15, encodedStream);
            //List<string> decoder = lZD.DecodeTextFile();
            //TextManipulator.RetrieveToTextFile(decoder, "targetTextFile.txt");

            // ------- 6 -------
            //Bitmap bitmap = new Bitmap("1.jpg");
            //LZWAlgorithmEncoder lZWEncoder = new LZWAlgorithmEncoder();
            //List<string> enocder = lZWEncoder.EncodeImage(bitmap);
            //LZWAlgorithmDecoder lZWDecoder = new LZWAlgorithmDecoder(lZWEncoder.TableInit, enocder);
            //List<string> decoder = lZWDecoder.DecodeImage();
            //Bitmap image = ImageManipulator.RetrieveImage(decoder, bitmap.Width, bitmap.Height);
            //image.Save("target.jpg");

            //LZWAlgorithmEncoder lZWEncoder = new LZWAlgorithmEncoder();
            //List<string> enocder = lZWEncoder.EncodeTextFile("testTextFile.txt");
            //LZWAlgorithmDecoder lZWDecoder = new LZWAlgorithmDecoder(lZWEncoder.TableInit, enocder);
            //List<string> decoder = lZWDecoder.DecodeImage();
            //TextManipulator.RetrieveToTextFile(decoder, "targetTextFile.txt");
        }
    }
}
 
