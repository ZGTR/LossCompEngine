using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Compression;
using ZGTR_LossCompWPFApp.GraphicsEngine;
using ZGTR_LosslessCompresion;
using ZGTR_LosslessCompresion.AdaptiveDictionary;
using ZGTR_LosslessCompresion.AdaptiveDictionary.LZW;
using ZGTR_LosslessCompresion.ImageController;
using ZGTR_LosslessCompresion.ImageEngine;
using ZGTR_LosslessCompresion.RLE;
using ZGTR_LosslessCompresion.RLE.Method1;
using ZGTR_LosslessCompresion.RLE.RLEMethods.QuadTree;
using Color = System.Drawing.Color;
using FontFamily = System.Windows.Media.FontFamily;
using FontStyle = System.Drawing.FontStyle;
using Image = System.Windows.Controls.Image;

namespace ZGTR_LossCompWPFApp
{
    public class GUIController
    {
        private static MainWindow _mainWindow;
        public static CompressionDataInfo CurrentInfo;
        public const string ImageTargetDecodedPath = "targetImageOut.jpg";
        public const string TextInEncodePath = "sourceTextFile.txt";
        public const string TextOutDecodedPath = "targetTextFile.txt";

        public static void Initialize(MainWindow mainWindow)
        {
            _mainWindow = mainWindow;
        }

        public static Algorithm GetAlgorithmChosenText()
        {
            Algorithm algo = Algorithm.RLEBasic;
            if ((bool) _mainWindow.rbRLEBasicT.IsChecked)
            {
                algo = Algorithm.RLEBasic;
            }
            else
            {

                if ((bool)_mainWindow.rbLZWT.IsChecked)
                {
                    algo = Algorithm.LZW;
                }
                else
                {
                    if ((bool)_mainWindow.rbLZ77T.IsChecked)
                    {
                        algo = Algorithm.LZ77;
                    }
                    else
                    {
                        if ((bool)_mainWindow.rbArithmeticT.IsChecked)
                        {
                            algo = Algorithm.Arithmetic;
                        }
                    }
                }
            }
            return algo;
        }

        public static Algorithm GetAlgorithmChosenImage()
        {
            Algorithm algo = Algorithm.RLEBasic;
            if ((bool)_mainWindow.rbRLEBasicI.IsChecked)
            {
                algo = Algorithm.RLEBasic;
            }
            else
            {

                if ((bool)_mainWindow.rbLZWI.IsChecked)
                {
                    algo = Algorithm.LZW;
                }
                else
                {
                    if ((bool)_mainWindow.rbLZ77I.IsChecked)
                    {
                        algo = Algorithm.LZ77;
                    }
                    else
                    {

                        if ((bool)_mainWindow.rbRLEMethod1I.IsChecked)
                        {
                            algo = Algorithm.RLEMethod1;
                        }
                        else
                        {
                            if ((bool)_mainWindow.rbRLEMethod2I.IsChecked)
                            {
                                algo = Algorithm.RLEMethod2;
                            }
                            else
                            {
                                if ((bool)_mainWindow.rbRLEQTI.IsChecked)
                                {
                                    algo = Algorithm.RLEQuadTree;
                                }
                            }
                        }
                    }
                }
            }
            return algo;
        }

        public static void EncodeDecodeText(Algorithm algoChosenText)
        {
            File.WriteAllText(TextInEncodePath, _mainWindow.textBoxTextIn.Text);
            CompressionDataInfo info = new CompressionDataInfo(TextInEncodePath);
            switch (algoChosenText)
            {
                case Algorithm.RLEBasic:
                    List<string> encoder = RLEBasic.EncodeTextFile(TextInEncodePath, ref info);
                    List<string> decoder = RLEBasic.DecodeStream(encoder, ref info);
                    TextManipulator.RetrieveToTextFile(decoder, TextOutDecodedPath);
                    _mainWindow.textBoxTextOut.Text = File.ReadAllText(TextOutDecodedPath);
                    break;
                case Algorithm.LZ77:
                    int windowSize = Int32.Parse(_mainWindow.textBoxWindowT.Text);
                    int bufferSize = Int32.Parse(_mainWindow.textBoxBufferT.Text);
                    LZ77AlgorithmEncoder lZ77Encoder = new LZ77AlgorithmEncoder(windowSize, bufferSize);
                    List<PhraseToken> encodedlZ77 = lZ77Encoder.EncodeTextFile(TextInEncodePath, ref info);
                    LZ77AlgorithmDecoder lZ77Decoder = new LZ77AlgorithmDecoder(windowSize, encodedlZ77);
                    List<string> decoderlZ77 = lZ77Decoder.DecodeTextFile(ref info);
                    TextManipulator.RetrieveToTextFile(decoderlZ77, TextOutDecodedPath);
                    _mainWindow.textBoxTextOut.Text = File.ReadAllText(TextOutDecodedPath);
                    break;
                case Algorithm.LZW:
                    LZWAlgorithmEncoder lZWEncoder = new LZWAlgorithmEncoder();
                    List<string> enocderlZW = lZWEncoder.EncodeTextFile(TextInEncodePath, ref info);
                    LZWAlgorithmDecoder lZWDecoder = new LZWAlgorithmDecoder(lZWEncoder.TableInit, enocderlZW);
                    List<string> decoderlZW = lZWDecoder.DecodeStream(ref info);
                    TextManipulator.RetrieveToTextFile(decoderlZW, TextOutDecodedPath);
                    _mainWindow.textBoxTextOut.Text = File.ReadAllText(TextOutDecodedPath);
                    break;
                case Algorithm.Arithmetic:
                    string pathOut = ArCoder.encodeAndDecode(GUIController.TextInEncodePath);
                    _mainWindow.textBoxTextOut.Text = File.ReadAllText(pathOut);
                    break;
            }
            ShowCompressionInfoText(info);
        }

        public static Image EncodeDecodeImage(Algorithm algoChosenText, string imagePath)
        {
            System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap(imagePath);
            System.Drawing.Bitmap bitmapDecoded = null;
            CompressionDataInfo info = new CompressionDataInfo(imagePath);
            switch (algoChosenText)
            {
                case Algorithm.RLEBasic:
                    List<string> encoder = RLEBasic.EncodeImage(bitmap, ref info);
                    List<string> decoder = RLEBasic.DecodeStream(encoder, ref info);
                    bitmapDecoded = ImageManipulator.RetrieveImage(decoder, bitmap.Width, bitmap.Height);
                    break;
                case Algorithm.RLEMethod1:
                    List<List<DataUnit>> encoderRLEMethod1 = RLEMethod1.EncodeImage(bitmap, ref info);
                    List<string> decoderRLEMethod1 = RLEMethod1.DecodeStream(encoderRLEMethod1, ref info);
                    bitmapDecoded = ImageManipulator.RetrieveImage(decoderRLEMethod1, bitmap.Width, bitmap.Height);
                    break;
                case Algorithm.RLEMethod2:
                    List<DataUnit> encoderRLEMethod2 = RLEMethod2.EncodeImage(bitmap, ref info);
                    List<string> decoderRLEMethod2 = RLEMethod2.DecodeStream(encoderRLEMethod2, bitmap.Width, ref info);
                    bitmapDecoded = ImageManipulator.RetrieveImage(decoderRLEMethod2, bitmap.Width, bitmap.Height);
                    break;
                case  Algorithm.RLEQuadTree:
                    RLEQuadTree tree = RLEMethodQT.EncodeImage(bitmap, ref info);
                    Color[,] colorArr = RLEMethodQT.DecodeImage(tree, ref info);
                    bitmapDecoded = ImageManipulator.RetrieveImage(colorArr);
                    break;
                case  Algorithm.LZ77:
                    int windowSize = Int32.Parse(_mainWindow.textBoxWindowI.Text);
                    int bufferSize = Int32.Parse(_mainWindow.textBoxBufferI.Text);
                    LZ77AlgorithmEncoder lZ77E = new LZ77AlgorithmEncoder(windowSize, bufferSize);
                    List<PhraseToken> encodedStream = lZ77E.EncodeImage(bitmap, ref info);
                    LZ77AlgorithmDecoder lZ77D = new LZ77AlgorithmDecoder(windowSize, encodedStream);
                    List<string> decoderlZ77 = lZ77D.DecodeImage(ref info);
                    bitmapDecoded = ImageManipulator.RetrieveImage(decoderlZ77, bitmap.Width, bitmap.Height);
                    break;
                case  Algorithm.LZW:
                    LZWAlgorithmEncoder lZWEncoder = new LZWAlgorithmEncoder();
                    List<string> enocderlZW = lZWEncoder.EncodeImage(bitmap, ref info);
                    LZWAlgorithmDecoder lZWDecoder = new LZWAlgorithmDecoder(lZWEncoder.TableInit, enocderlZW);
                    List<string> decoderlZW = lZWDecoder.DecodeStream(ref info);
                    bitmapDecoded = ImageManipulator.RetrieveImage(decoderlZW, bitmap.Width, bitmap.Height);
                    break;
            }
            ShowCompressionInfoImage(info);
            return ImageController.ConvertToWPFImage(bitmapDecoded);
        }

        public static void ShowImage(string currentImagePath, double timeTaken)
        {
            _mainWindow.stackPanelImageHorizontal.Children.Add(new ImageContainer(currentImagePath, timeTaken));
        }

        public static void ShowImage(Image imageDecoded, double timeTaken)
        {
           
            _mainWindow.stackPanelImageHorizontal.Children.Add(new ImageContainer(imageDecoded, timeTaken));
        }

        public static void ShowCompressionInfoImage(CompressionDataInfo info)
        {
            NeutrilizePrevInfo(ref _mainWindow.spInfoImage);
            _mainWindow.spInfoImage.Children.Add(BuildStackPanelInfo(info));
        }

        private static void NeutrilizePrevInfo(ref StackPanel spInfo)
        {
            try
            {
                spInfo.Children.RemoveAt(1);
            }
            catch (Exception)
            {
               
            }
        }

        public static void ShowCompressionInfoText(CompressionDataInfo info)
        {
            NeutrilizePrevInfo(ref _mainWindow.spInfoText);
            _mainWindow.spInfoText.Children.Add(BuildStackPanelInfo(info));
        }

        private static UIElement BuildStackPanelInfo(CompressionDataInfo info)
        {
            StackPanel sp = new StackPanel();
            sp.Margin = new Thickness(10);
            //sp.HorizontalAlignment = HorizontalAlignment.Center;
            sp.VerticalAlignment = VerticalAlignment.Center;
            TextBlock t1 = new TextBlock() { Text = "File Size: ", FontFamily = new FontFamily("Century Gothic"), FontStyle = FontStyles.Italic, FontWeight = FontWeights.Bold };
            TextBlock t11 = new TextBlock() { Text = info.FileSizeInKB + "KB", HorizontalAlignment = HorizontalAlignment.Center, FontFamily = new FontFamily("Century Gothic") };
            TextBlock t2 = new TextBlock() { Text = "Original Stream: ", FontFamily = new FontFamily("Century Gothic"), FontStyle = FontStyles.Italic, FontWeight = FontWeights.Bold };
            TextBlock t22 = new TextBlock() { Text = info.DecodingStreamCount.ToString(), HorizontalAlignment = HorizontalAlignment.Center, FontFamily = new FontFamily("Century Gothic") };
            TextBlock t3 = new TextBlock() { Text = "Compressed Stream: ", FontFamily = new FontFamily("Century Gothic"), FontStyle = FontStyles.Italic, FontWeight = FontWeights.Bold };
            TextBlock t33 = new TextBlock() { Text = info.EncodingStreamCount.ToString(), HorizontalAlignment = HorizontalAlignment.Center, FontFamily = new FontFamily("Century Gothic") };
            TextBlock t4 = new TextBlock() { Text = "Encoding Time: ", FontFamily = new FontFamily("Century Gothic"), FontStyle = FontStyles.Italic, FontWeight = FontWeights.Bold };
            TextBlock t44 = new TextBlock() { Text = Format1AfterPoint(info.TimeEncoding) + " sec", HorizontalAlignment = HorizontalAlignment.Center, FontFamily = new FontFamily("Century Gothic") };
            TextBlock t5 = new TextBlock() { Text = "Encoding Decoding: ", FontFamily = new FontFamily("Century Gothic"), FontStyle = FontStyles.Italic, FontWeight = FontWeights.Bold };
            TextBlock t55 = new TextBlock() { Text = Format1AfterPoint(info.TimeDecoding) + " sec", HorizontalAlignment = HorizontalAlignment.Center, FontFamily = new FontFamily("Century Gothic") };
            TextBlock t6 = new TextBlock() { Text = "Compression Ratio: ", FontFamily = new FontFamily("Century Gothic"), FontStyle = FontStyles.Italic, FontWeight = FontWeights.Bold };
            TextBlock t66 = new TextBlock() { Text = info.CompressionRatio, HorizontalAlignment = HorizontalAlignment.Center, FontFamily = new FontFamily("Century Gothic")};
            sp.Children.Add(t1);
            sp.Children.Add(t11);
            sp.Children.Add(t2);
            sp.Children.Add(t22);
            sp.Children.Add(t3);
            sp.Children.Add(t33);
            sp.Children.Add(t4);
            sp.Children.Add(t44);
            sp.Children.Add(t5);
            sp.Children.Add(t55);
            sp.Children.Add(t6);
            sp.Children.Add(t66);
            return sp;
        }

        private static string Format1AfterPoint(string strIn)
        {
            return String.Format("{0:0.0}", strIn);
        }

        private static string Format1AfterPoint(double doubleNum)
        {
            return String.Format("{0:0.0}", doubleNum);
        }
    }
}
