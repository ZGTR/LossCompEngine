using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Microsoft.Research.DynamicDataDisplay;
using Microsoft.Research.DynamicDataDisplay.DataSources;
using Microsoft.Research.DynamicDataDisplay.PointMarkers;
using Octree_ZGTR_WPFApp.Engine.EncodingPackage;
using Octree_ZGTR_WPFApp.Engine.GUI;
using Octree_ZGTR_WPFApp.Engine.OctreeTree;
using Color = System.Windows.Media.Color;
using FontFamily = System.Windows.Media.FontFamily;
using Image = System.Windows.Controls.Image;
using Point = Octree_ZGTR_WPFApp.Engine.DynamicDataDisplay.Point;

namespace Octree_ZGTR_WPFApp
{
    /// <summary>
    /// Interaction logic for ColorTableWindow.xaml
    /// </summary>
    public partial class ColorTableWindow : Window
    {
        public ObservableCollection<ColorTableTriple> ColorsInTable = new ObservableCollection<ColorTableTriple>();
        private ColorTable _colorTable;
        private Image _image;
        private System.Drawing.Bitmap _bitmap;

        public ColorTableWindow(ColorTable colorTable, System.Drawing.Bitmap bitmap)
        {
            _colorTable = colorTable;
            this._bitmap = bitmap;
            //BuildObesrvable();
            //DataContext = ColorsInTable;
            InitializeComponent();
            BuildVisualData();
            BuildPlotter();
            this.Show();
        }

        private void BuildPlotter()
        {
            List<Point> listOfPoints = BuildPointsList();
            SolidColorBrush brush = new SolidColorBrush(Colors.Red);
            PlotPoints(listOfPoints, brush);
        }

        private void PlotPoints(IEnumerable<Point> points, System.Windows.Media.Brush brush)
        {
            var marker = new CirclePointMarker();
            marker.Fill = brush;
            var ds = new EnumerableDataSource<Point>(points);
            ds.SetXMapping(p => p.X);
            ds.SetYMapping(p => p.Y);
            var markerGraph = new MarkerPointsGraph
            {
                DataSource = ds,
                Marker = marker
            };
            this.chartPlotterHistogram.Children.Add(markerGraph);
        }

        private List<Point> BuildPointsList()
        {
            int[] arrOfOccurences = new int[_colorTable.Octree.NumOfLeavesWanted];
            


            List<System.Drawing.Color> listOfColor = this._colorTable.Table.ToList();

            for (int i = 0; i < _bitmap.Height; i++)
            {
                for (int j = 0; j < _bitmap.Width; j++)
                {
                    System.Drawing.Color currentColor = _bitmap.GetPixel(j, i);
                    System.Drawing.Color colorMatched = (from color in listOfColor
                             where color.R == currentColor.R && color.G == currentColor.G && color.B == currentColor.B
                             select color).First();
                    int index = listOfColor.IndexOf(colorMatched);
                    arrOfOccurences[index]++;
                }
            }

            List<Point> listOfPoints = new List<Point>();
            for (int i = 0; i < arrOfOccurences.Count(); i++)
            {
                listOfPoints.Add(new Point(i, arrOfOccurences[i]));
            }

            return listOfPoints;
        }

        private void BuildVisualData()
        {
            for (int i = 0; i < this._colorTable.Table.Length; i++)
            {
                StackPanel spSmall = new StackPanel();
                spSmall.Orientation = Orientation.Horizontal;
                spSmall.VerticalAlignment = VerticalAlignment.Center;
                spSmall.HorizontalAlignment = HorizontalAlignment.Center;
                spSmall.Margin = new Thickness(1);
                System.Drawing.Color c = this._colorTable.Table[i];
                TextBlock t = new TextBlock() {Text = i.ToString()};
                t.VerticalAlignment = VerticalAlignment.Center;
                t.FontFamily = new FontFamily("Century Gothic");
                t.Width = 30;
                t.Height = 30;
                TextBlock tColor = new TextBlock() { Background = new SolidColorBrush(Color.FromRgb(c.R, c.G, c.B)) };
                tColor.Width = 60;
                tColor.Height = 30;
                spSmall.Children.Add(t);
                spSmall.Children.Add(tColor);
                this.stackPanelColorTable.Children.Add(spSmall);
            }
        }

        private void BuildObesrvable()
        {
            ColorsInTable = new ObservableCollection<ColorTableTriple>();
            for (int i = 0; i < this._colorTable.Table.Length; i++)
            {
                ColorsInTable.Add(new ColorTableTriple(i, this._colorTable.Table[i]));
            }
        }
    }
}
