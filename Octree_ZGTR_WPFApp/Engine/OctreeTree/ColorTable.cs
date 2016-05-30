using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Media.Imaging;
using Octree_ZGTR_WPFApp.Engine.OctreeTree.MappingFunctions;
using Octree_ZGTR_WPFApp.Engine.OctreeTree.OctreeTypes.AbstractOctree;
using Image = System.Windows.Controls.Image;

namespace Octree_ZGTR_WPFApp.Engine.OctreeTree
{
    public class ColorTable
    {
        private Color[] _table;
        public Color[] Table
        {
            get { return _table; }
        }

        public OctreeTree.OctreeTypes.AbstractOctree.Octree Octree;

        public ColorTable(OctreeTree.OctreeTypes.AbstractOctree.Octree octree)
        {
            this.Octree = octree;
            _table = new Color[Octree.NumOfLeavesWanted];
        }

        public void AddToTable(int key, Color value)
        {
            _table[key] =  value;
        }

        public void BuildColorTable()
        {
            List<Node> leavesNodes = Octree.GetLeavesList();
            for (int i = 0; i < leavesNodes.Count; i++)
            {
                this._table[i] = leavesNodes[i].Color;
                leavesNodes[i].IndexInTable = i;
            }
        }

        private int counterOutput = 0;
        /// <summary>
        /// Return the new "octreed!" image that corresponds to the original one
        /// </summary>
        /// <param name="targetURIDestination"> String of the destination image directory</param>
        /// <returns> Instance of the retrieved image </returns>
        public Image RetriveImageFromTableToImageFile(string targetURIDestination, IMappingFunction function)
        {
            System.Drawing.Bitmap dummyBitmap = new Bitmap(Octree.ImageURI);
            System.Drawing.Bitmap bitmapForm = new Bitmap(dummyBitmap.Width, dummyBitmap.Height);
            for (int i = 0; i < bitmapForm.Height; i++)
            {
                for (int j = 0; j < bitmapForm.Width; j++)
                {
                    Color closestColor = this.GetColorForPixel(i, j, function);
                    bitmapForm.SetPixel(j, i, closestColor);
                }
            }
            bitmapForm.Save(targetURIDestination);
            System.Drawing.Image im = System.Drawing.Image.FromFile(targetURIDestination);
            BitmapImage bitmapImage = ImageController.ImageController.ConvertToWPFBitmapImage(im);
            Image image = new Image();
            image.Source = bitmapImage;
            counterOutput++;
            return image;
            //return  new Image();
        }

        public BitmapImage RetriveImageFromTableToImageFile(ref System.Drawing.Bitmap bitmapForm,IMappingFunction function)
        {
            System.Drawing.Bitmap dummyBitmap = new Bitmap(Octree.ImageURI);
            bitmapForm = new Bitmap(dummyBitmap.Width, dummyBitmap.Height);
            for (int i = 0; i < bitmapForm.Height; i++)
            {
                for (int j = 0; j < bitmapForm.Width; j++)
                {
                    Color closestColor = this.GetColorForPixel(i, j, function);
                    bitmapForm.SetPixel(j, i, closestColor);
                }
            }
            bitmapForm.Save("tempImage.jpg");
            System.Drawing.Image im = System.Drawing.Image.FromFile("tempImage.jpg");
            BitmapImage bitmapImage = ImageController.ImageController.ConvertToWPFBitmapImage(im);
            return bitmapImage;
            //return  new Image();
        }

        public byte[] RetriveImageFromTableToIndexedArr(IMappingFunction function)
        {

            System.Drawing.Bitmap dummyBitmap = new Bitmap(Octree.ImageURI);
            System.Drawing.Bitmap bitmapForm = new Bitmap(dummyBitmap.Width, dummyBitmap.Height);
            byte[] indexedArr = new byte[bitmapForm.Height * bitmapForm.Width];
            for (int i = 0; i < bitmapForm.Height; i++)
            {
                for (int j = 0; j < bitmapForm.Width; j++)
                {
                    indexedArr[i * bitmapForm.Width + j] = (byte)this.GetIndexColorForPixel(i, j, function);
                    //bitmapForm.SetPixel(j, i, closestColor);
                }
            }
            return indexedArr;
        }


        private int LoopThroughTree(Color currentPixelColor)
        {
            int indexInTable = 0;
            Node currentNode = this.Octree.BaseNode;
            string[] stringVerticalBits = HelperModule.GetBitsVerticalString(Octree.EncodingConverter, currentPixelColor);
            for (int k = 0; k < 8; k++)
            {
                string current3Bits = stringVerticalBits[k];
                try
                {
                    Node nextPathNode = (from node in currentNode.Children
                                         where String.Equals(node.BinaryID, current3Bits)
                                         select node).Single();
                    currentNode = nextPathNode;
                }
                catch (Exception)
                {
                    currentNode = this.Octree.GetClosestNextNode(currentNode, current3Bits);
                }
            }
            return currentNode.IndexInTable;
        }

        public Color GetColorForPixel(int i, int j, IMappingFunction function)
        {
            Color originColor = this.Octree.ImageMatrix[i, j];
            double minError =
                this.Table.Min(color => function.GetDistance(originColor, color));
            Color closestColor = (from color in this._table
                                  where
                                      function.GetDistance(originColor, color) ==
                                      minError
                                  select color).First();
            closestColor = Color.FromArgb(originColor.A, closestColor.R, closestColor.G, closestColor.B);
            //this.LoopThroughTree(originColor);)
            return closestColor;
        }

        private int GetIndexColorForPixel(int i, int j, IMappingFunction function)
        {
            Color originColor = this.Octree.ImageMatrix[i, j];
            double minError =
                this.Table.Min(color => function.GetDistance(originColor, color));
            Color closestColor = (this._table.Where(color => function.GetDistance(originColor, color) ==
                                                             minError)).First();
            int indexToReturn = 0;
            for (var counter = 0; counter < _table.Length; counter++)
            {
                if (function.GetDistance(_table[counter], originColor) != minError) continue;
                indexToReturn = counter;
                break;
            }
            return indexToReturn;
        }
    }
}
