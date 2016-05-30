using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;
using ZGTR_LossCompWPFApp.GraphicsEngine;
using ZGTR_LosslessCompresion;

namespace ZGTR_LossCompWPFApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static string CurrentImagePath;
        public MainWindow()
        {
            InitializeComponent();
            GUIController.Initialize(this);
        }

        private void buttonUploadText_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                OpenFileDialog opFileDialog = new OpenFileDialog();
                opFileDialog.Filter = "Text Files (.txt)|*.txt";
                if ((bool) opFileDialog.ShowDialog())
                {
                    this.textBoxTextIn.Text = File.ReadAllText(opFileDialog.FileName);
                }
            }
            catch(Exception)
            {
                MessageBox.Show("Can't open file.");
            }
        }

        private void buttonChooseImage_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                OpenFileDialog opFileDialog = new OpenFileDialog();
                //opFileDialog.Filter = "Images Files (.jpg)|*.jpg";
                if ((bool)opFileDialog.ShowDialog())
                {
                    TimeChecker time = new TimeChecker();
                    time.S1();
                    CurrentImagePath = opFileDialog.FileName;
                    GUIController.ShowImage(CurrentImagePath, time.S2());
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Can't open file.");
            }
        }

        private void buttonCompressText_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Algorithm algoChosenText = GUIController.GetAlgorithmChosenText();
                GUIController.EncodeDecodeText(algoChosenText);
            }
            catch (Exception)
            {
                MessageBox.Show("Error occured.");
            }
        }

        private void buttonCompressImage_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                TimeChecker time = new TimeChecker();
                time.S1();
                Algorithm algoChosenText = GUIController.GetAlgorithmChosenImage();
                Image imageDecode = GUIController.EncodeDecodeImage(algoChosenText, CurrentImagePath);
                GUIController.ShowImage(imageDecode, time.S2());
            }
            catch (Exception)
            {
                MessageBox.Show("Error occured.");
            }
        }

        private void buttonTextInClear_Click(object sender, RoutedEventArgs e)
        {
            this.textBoxTextIn.Clear();
        }

        private void buttonTextOutClear_Click(object sender, RoutedEventArgs e)
        {
            this.textBoxTextOut.Clear();
        }
    }
}
