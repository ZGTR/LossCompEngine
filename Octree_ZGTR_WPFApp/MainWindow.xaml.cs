using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Research.DynamicDataDisplay.DataSources;

using Microsoft.Research.DynamicDataDisplay;
using Microsoft.Win32;
using Octree_ZGTR_WPFApp.Engine.GUI;
using Octree_ZGTR_WPFApp.Engine.ImageController;
using Octree_ZGTR_WPFApp.Engine.OctreeTree;
using Octree_ZGTR_WPFApp.Engine.OctreeTree.MappingFunctions;
using Octree_ZGTR_WPFApp.Engine.OctreeTree.OctreeTypes;
using Octree_ZGTR_WPFApp.Engine.OctreeTree.OctreeTypes.AbstractOctree;
using Octree_ZGTR_WPFApp.Engine.OctreeTree.OctreeTypes.Balanced;

namespace Octree_ZGTR_WPFApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Octree _currentOctree;
        private ColorTable _colorTable;
        private IMappingFunction _function;
        private string _currentImageString;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void buttonChooseInputImage_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog op = new OpenFileDialog();
            if ((bool)op.ShowDialog())
            {
                _currentImageString = op.FileName;
                this.stackPanelHorizontal.Children.Add(new ImageContainer(this, _currentOctree, _colorTable,
                                                                          ImageController.GetImage(_currentImageString),
                                                                          null,
                                                                          op.FileName, 0, null, null));
            }
        }

        //private void buttonExportToGIFFile_Click(object sender, RoutedEventArgs e)
        //{
        //    string stringToOutputTheGIFImage = "";
        //    SaveFileDialog sd = new SaveFileDialog();
        //    if ((bool) sd.ShowDialog())
        //    {
        //        stringToOutputTheGIFImage = sd.FileName;
        //    }
        //    // HASAN AREA
        //    //ctree = new OctreeUnBalanced(256);
        //    //octree.BuildOctreeFromImageFile("Weighted.jpg");
        //    //ColorTable colorTable = new ColorTable(octree);
        //    //colorTable.BuildColorTable();
        //    //Image image = colorTable.RetriveImageFromTableToImageFile("s");
        //    //byte[] byteArr = ;
        //    GIFHelper.SaveGIFWithCustomColorTable(stringToOutputTheGIFImage,
        //        _colorTable.RetriveImageFromTableToIndexedArr(_function), _colorTable, (int)_im.Width, (int)image.Height);

        //}

        private void buttonProcessImage_Click(object sender, RoutedEventArgs e)
        {
            int colorTableDim = Int32.Parse(textBoxColorTableDim.Text);
            _currentOctree = GUIController.GetOctreeType(this, colorTableDim);
            _colorTable = new ColorTable(_currentOctree);
            _function = GUIController.GetMappingFuncitonType(this);
            
            // Process
            TimeChecker.Normalize();
            TimeChecker.S1();
            _currentOctree.BuildOctreeFromImageFile(_currentImageString);
            _colorTable.BuildColorTable();
            TimeChecker.S2();
            System.Drawing.Bitmap bitmapForm = null;
            BitmapImage bitmapImage = _colorTable.RetriveImageFromTableToImageFile(ref bitmapForm, _function);
            this.stackPanelHorizontal.Children.Add(new ImageContainer(this, _currentOctree, _colorTable, ImageController.ConvertToImage(bitmapImage), bitmapForm,
                _currentImageString, 
                TimeChecker.TotalTimeCollapse,
                _function, null));
        }
    }
}
