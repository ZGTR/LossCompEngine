using System;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using Microsoft.Win32;
using ZGTR_LossCompWPFApp.GraphicsEngine;
using Image = System.Windows.Controls.Image;

namespace ZGTR_LossCompWPFApp
{
    class ImageContainer : StackPanel
    {
        //private StackPanel _stackPanelContainer;
        private string _imageStringPath;
        private double _timeTaken;
        private bool _imageIsInApp;
        public Image ImageDisplayed { get; private set; }

        public ImageContainer(string imageStringPath, double timeTaken)
        {
            this._imageStringPath = imageStringPath;
            //this._imageIsInApp = imageIsInApp;
            //this._stackPanelContainer = stackPanelContainer;
            this._timeTaken = timeTaken;
            InitialzeImage();
            InitializeStackPanel();
        }

        public ImageContainer(Image imageWPF, double timeTaken)
        {
            //this._imageIsInApp = imageIsInApp;
            //this._stackPanelContainer = stackPanelContainer;
            this._timeTaken = timeTaken;
            //InitialzeImage();
            this.ImageDisplayed = imageWPF;
            InitializeStackPanel();
        }

        //public ImageContainer(System.Drawing.Image imageDrawing, double timeTaken)
        //{
        //    //this._imageIsInApp = imageIsInApp;
        //    //this._stackPanelContainer = stackPanelContainer;
        //    this._timeTaken = timeTaken;
        //    //InitialzeImage();
        //    this.ImageDisplayed = ImageController.ConvertToWPFImage(imageDrawing);
        //    InitializeStackPanel();
        //}

        private void InitialzeImage()
        {
            this.ImageDisplayed = new Image();
            //BitmapImage src = new BitmapImage();
            //src.BeginInit();
            //if (_imageIsInApp)
            //{
            //    //src.UriSource = new Uri(@"pack://application:,,,/"
            //    //                      + Assembly.GetExecutingAssembly().GetName().Name
            //    //                      + ";component/"
            //    //                      + _imageStringPath, UriKind.Absolute);

            //    src.UriSource = new Uri(@"pack://application:,,,/"
            //        //+ Assembly.GetExecutingAssembly().GetName().Name
            //        //+ ";component/"
            //          + "bin/Debug/"
            //          +
            //          _imageStringPath, UriKind.Absolute);
            //}
            //else
            //{
            //    src.UriSource = new Uri(_imageStringPath, UriKind.Absolute);
            //}
            //src.EndInit();
            this.ImageDisplayed.Source = new ImageSourceConverter().ConvertFromString(_imageStringPath) as ImageSource;
        }

        private void InitializeStackPanel()
        {
            StackPanel sp = new StackPanel();
            this.ImageDisplayed.Margin = new Thickness(2);
            this.ImageDisplayed.VerticalAlignment = VerticalAlignment.Center;
            this.ImageDisplayed.HorizontalAlignment = HorizontalAlignment.Center;
            this.ImageDisplayed.MouseWheel += new System.Windows.Input.MouseWheelEventHandler(ImageMouseWheel);

            TextBlock textBlock = new TextBlock();
            textBlock.Text = "Time processing: ";
            textBlock.Text += String.Format("{0:0.0}", _timeTaken) + " sec";
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

            Button buttonExit = new Button();
            buttonExit.Content = "X";
            buttonExit.Width = 20;
            buttonExit.Height = 20;
            buttonExit.Margin = new Thickness(2);
            buttonExit.VerticalAlignment = VerticalAlignment.Center;
            buttonExit.HorizontalAlignment = HorizontalAlignment.Right;
            buttonExit.Click += new RoutedEventHandler(ButtonExitClick);


            Border border = new Border();
            border.BorderThickness = new Thickness(2);
            border.Padding = new Thickness(2);
            border.BorderBrush = new SolidColorBrush(Colors.DarkGray);
            sp.Children.Add(buttonExit);
            sp.Children.Add(textBlock);
            sp.Children.Add(ImageDisplayed);
            sp.Children.Add(buttonExtract);
            border.Child = sp;

            this.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            this.VerticalAlignment = System.Windows.VerticalAlignment.Center;
            this.Children.Add(border);
        }

        void ButtonExitClick(object sender, RoutedEventArgs e)
        {
            ((StackPanel)this.Parent).Children.Remove(this);
        }

        void ImageMouseWheel(object sender, System.Windows.Input.MouseWheelEventArgs e)
        {
            try
            {
                if (this.ImageDisplayed.ActualWidth > 50)
                {
                    if (e.Delta > 0)
                        ImageDisplayed.BeginAnimation(System.Windows.Controls.Image.WidthProperty,
                                          new DoubleAnimation(this.ImageDisplayed.ActualWidth,
                                                              this.ImageDisplayed.ActualWidth + 50,
                                                              new Duration(TimeSpan.FromSeconds(0.5))));
                    else
                    {
                        ImageDisplayed.BeginAnimation(System.Windows.Controls.Image.WidthProperty,
                            new DoubleAnimation(this.ImageDisplayed.ActualWidth,
                         this.ImageDisplayed.ActualWidth - 50,
                         new Duration(TimeSpan.FromSeconds(0.5))));
                    }
  
                }
                else
                {
                    ImageDisplayed.BeginAnimation(System.Windows.Controls.Image.WidthProperty,
                     new DoubleAnimation(this.ImageDisplayed.ActualWidth,
                                         this.ImageDisplayed.ActualWidth + 50,
                                         new Duration(TimeSpan.FromSeconds(1))));
                }
            }
            catch (Exception)
            {

            }
        }

        void ButtonExtractClick(object sender, RoutedEventArgs e)
        {
            try
            {
                SaveFileDialog sd = new SaveFileDialog();
                if ((bool)sd.ShowDialog())
                {
                    using (var fileStream = new FileStream(sd.FileName, FileMode.Create))
                    {
                        BitmapEncoder encoder = new JpegBitmapEncoder();
                        encoder.Frames.Add(BitmapFrame.Create(this.ImageDisplayed.Source as BitmapImage));
                        encoder.Save(fileStream);
                    }
                }
            }
            catch (Exception)
            {
            }
        }

    }
}

