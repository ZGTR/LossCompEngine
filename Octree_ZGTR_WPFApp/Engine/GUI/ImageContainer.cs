using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using Microsoft.Win32;
using Octree_ZGTR_WPFApp.Engine.OctreeTree;
using Octree_ZGTR_WPFApp.Engine.OctreeTree.MappingFunctions;
using Octree_ZGTR_WPFApp.Engine.OctreeTree.OctreeTypes;
using Octree_ZGTR_WPFApp.Engine.OctreeTree.OctreeTypes.AbstractOctree;
using Image = System.Windows.Controls.Image;

namespace Octree_ZGTR_WPFApp.Engine.GUI
{
    class ImageContainer : StackPanel
    {
        private Image _image;
        private Octree _octree;
        private ColorTable _colorTable;
        private IMappingFunction _function;
        private MainWindow _mainWindow;
        public Image Image { get { return _image; } }
        private string _currentImageString;
        public System.Drawing.Bitmap _bitmapImage;

        public ImageContainer(MainWindow mainW, Octree octree, ColorTable colorTable,
            Image image, System.Drawing.Bitmap bitmap, string currentImageString, double timeTaken, IMappingFunction function, string imageString)
        {
            _bitmapImage = bitmap;
            _currentImageString = currentImageString;
            this._function = function;
            this._mainWindow = mainW;
            this._octree = octree;
            this._colorTable = colorTable;
            InitializeStackPanel(image, timeTaken);
        }

        private void InitializeStackPanel(Image image, double timeTaken)
        {

            StackPanel sp = new StackPanel();
            this.Width = image.Width;
            this.Height = image.Height;
            this._image = image;
            this._image.Margin = new Thickness(2);
            this._image.VerticalAlignment = VerticalAlignment.Center;
            this._image.HorizontalAlignment = HorizontalAlignment.Center;
            this._image.MouseWheel += new System.Windows.Input.MouseWheelEventHandler(ImageMouseWheel);

            TextBlock textBlock = new TextBlock();
            textBlock.Text = "Time processing: ";
            textBlock.Text += String.Format("{0:0.0}", timeTaken) + " sec";
            //if (function != null)
            //textBlock.Text += Environment.NewLine + "Mapping function" + function.ToString();
            textBlock.Margin = new Thickness(2);
            textBlock.VerticalAlignment = VerticalAlignment.Center;
            textBlock.HorizontalAlignment = HorizontalAlignment.Center;

            Button buttonExtract = new Button();
            buttonExtract.Content = "Extract to file";
            buttonExtract.Width = 100;
            buttonExtract.Height = 30;
            buttonExtract.Margin = new Thickness(2);
            buttonExtract.VerticalAlignment = VerticalAlignment.Center;
            buttonExtract.HorizontalAlignment = HorizontalAlignment.Center;
            buttonExtract.Click += new RoutedEventHandler(ButtonExtractClick);

            
            Button buttonExtractGIF = new Button();
            buttonExtractGIF.Content = "Extract to GIF file";
            buttonExtractGIF.Width = 100;
            buttonExtractGIF.Height = 30;
            buttonExtractGIF.Margin = new Thickness(2);
            buttonExtractGIF.VerticalAlignment = VerticalAlignment.Center;
            buttonExtractGIF.HorizontalAlignment = HorizontalAlignment.Center;
            buttonExtractGIF.Click += new RoutedEventHandler(buttonExtractGIF_Click);


            Button buttonColorTable = new Button();
            buttonColorTable.Content = "Show Color Table";
            buttonColorTable.Width = 100;
            buttonColorTable.Height = 20;
            buttonColorTable.Margin = new Thickness(2);
            buttonColorTable.VerticalAlignment = VerticalAlignment.Center;
            buttonColorTable.HorizontalAlignment = HorizontalAlignment.Left;
            buttonColorTable.Click += new RoutedEventHandler(buttonColorTable_Click);

            Button buttonExit = new Button();
            buttonExit.Content = "X";
            buttonExit.Width = 20;
            buttonExit.Height = 20;
            buttonExit.Margin = new Thickness(2);
            buttonExit.VerticalAlignment = VerticalAlignment.Center;
            buttonExit.HorizontalAlignment = HorizontalAlignment.Right;
            buttonExit.Click += new RoutedEventHandler(ButtonExitClick);

            StackPanel spSmall1 = new StackPanel();
            spSmall1.Orientation = Orientation.Horizontal;
            spSmall1.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            spSmall1.VerticalAlignment = System.Windows.VerticalAlignment.Center;
            spSmall1.Children.Add(buttonColorTable);
            spSmall1.Children.Add(buttonExit);


            Border border = new Border();
            border.BorderThickness = new Thickness(2);
            border.Padding = new Thickness(2);
            border.BorderBrush = new SolidColorBrush(Colors.DarkGray);
            sp.Children.Add(spSmall1);
            sp.Children.Add(textBlock);
            sp.Children.Add(_image);
            sp.Children.Add(buttonExtract);
            sp.Children.Add(buttonExtractGIF);
            border.Child = sp;

            this.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            this.VerticalAlignment = System.Windows.VerticalAlignment.Center;
            this.Children.Add(border);
        }
        
        void buttonColorTable_Click(object sender, RoutedEventArgs e)
        {
            new ColorTableWindow(this._colorTable, this._bitmapImage);
        }

        void ButtonExitClick(object sender, RoutedEventArgs e)
        {
            this._mainWindow.stackPanelHorizontal.Children.Remove(this);
        }

        void ImageMouseWheel(object sender, System.Windows.Input.MouseWheelEventArgs e)
        {
            try
            {
                if (this._image.ActualWidth > 50)
                {
                    if (e.Delta > 0)
                    _image.BeginAnimation(System.Windows.Controls.Image.WidthProperty,
                                          new DoubleAnimation(this._image.ActualWidth,
                                                              this._image.ActualWidth + 50,
                                                              new Duration(TimeSpan.FromSeconds(1))));
                    else
                    {
                        _image.BeginAnimation(System.Windows.Controls.Image.WidthProperty,
                            new DoubleAnimation(this._image.ActualWidth,
                         this._image.ActualWidth - 50,
                         new Duration(TimeSpan.FromSeconds(1))));
                    }
  
                }
                else
                {
                    _image.BeginAnimation(System.Windows.Controls.Image.WidthProperty,
                     new DoubleAnimation(this._image.ActualWidth,
                                         this._image.ActualWidth + 50,
                                         new Duration(TimeSpan.FromSeconds(1))));

                }
            }
            catch (Exception)
            {
                

            }
        }

        void ButtonExtractClick(object sender, RoutedEventArgs e)
        {
            string stringToOutputTheGIFImage = "";
            SaveFileDialog sd = new SaveFileDialog();
            if ((bool)sd.ShowDialog())
            {


                RenderTargetBitmap renderTargetBitmap = new RenderTargetBitmap((int)_image.Source.Width,
                                                                (int)_image.Source.Height,
                                                                100, 100, PixelFormats.Default);
                renderTargetBitmap.Render(this._image);
                JpegBitmapEncoder jpegBitmapEncoder = new JpegBitmapEncoder();
                jpegBitmapEncoder.Frames.Add(BitmapFrame.Create(renderTargetBitmap));
                using (FileStream fileStream = new FileStream(sd.FileName, FileMode.Create))
                {
                    jpegBitmapEncoder.Save(fileStream);
                    fileStream.Flush();
                    fileStream.Close();
                }
        
            }
        }

        void  buttonExtractGIF_Click(object sender, RoutedEventArgs e)
        {
            string stringToOutputTheGIFImage = "";
            SaveFileDialog sd = new SaveFileDialog();
            if ((bool) sd.ShowDialog())
            {
                stringToOutputTheGIFImage = sd.FileName;
            }
            GIFHelper.SaveGIFWithCustomColorTable(stringToOutputTheGIFImage,
                _colorTable.RetriveImageFromTableToIndexedArr(this._function), _colorTable, (int)this._image.ActualWidth, (int)_image.ActualHeight);
        }
    }
}

