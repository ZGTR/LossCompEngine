using System.Collections.Generic;
using System.Drawing;
using Octree_ZGTR_WPFApp.Engine.EncodingPackage;

namespace Octree_ZGTR_WPFApp.Engine.OctreeTree
{
    public class Node
    {
        private EncodingConverter _encoding;
        private string _binaryID;
        public string BinaryID
        {
            get { return _binaryID; }
        }

        public Color Color
        {
            get 
            {
                return HelperModule.GetColor(_encoding, this._binaryPath + this._binaryID); 
            }
        }

        public int NumOfVisiting { set; get; }

        private int _level;
        public int Level
        {
            get { return _level; }
        }

        public bool _isLeaf;
        public bool IsLeaf
        {
            set { _isLeaf = true; }
            get { return _isLeaf; }
        }

        public List<Node> Children { set; get; }

        private string _binaryPath;
        public string BinaryPath
        {
            get { return _binaryPath; }
        }

        private int _indexInTable;
        public int IndexInListOfLevels;

        public int IndexInTable
        {
            set { _indexInTable = value; }
            get { return _indexInTable; }
        }

        //public List<Node> ChildrenAvailable
        //{
        //    get { return GetAvailableChildren(Children); }
        //}

        //private static List<Node> GetAvailableChildren(List<Node> children)
        //{
        //    return children.Where(child => child != null).ToArray();
        //}

        //private Octree _octree;
        //public Octree Octree
        //{
        //    get { return _octree; }
        //}

        //private Node _baseNode;
        //public Node BaseNode
        //{
        //    get { return _baseNode; }
        //}

        public Node(EncodingConverter encoding, string binaryPath, int level, string binaryID)
        {
            //this._octree = octree;
            this._isLeaf = false;
            this._encoding = encoding;
            this._binaryPath = binaryPath;
            this._level = level;
            this._binaryID = binaryID;
            this.Children = new List<Node>();
        }

        //public void BuildChildrenArr()
        //{
        //    this.Children = new Node[8];
        //    Children[0] = new Node(this._level + 1, "000");
        //    Children[1] = new Node(this._level + 1, "001");
        //    Children[2] = new Node(this._level + 1, "010");
        //    Children[3] = new Node(this._level + 1, "011");
        //    Children[4] = new Node(this._level + 1, "100");
        //    Children[5] = new Node(this._level + 1, "101");
        //    Children[6] = new Node(this._level + 1, "110");
        //    Children[7] = new Node(this._level + 1, "111");
        //}
        
        //private void AddNewCollapsedNodes(Node currentNode, int startLevel, Color currentPixelColor)
        //{
        //    for (int k = startLevel; k < 8; k++)
        //    {
        //        string current3Bits = HelperModule.Get3BitsString(currentPixelColor, k);

        //            bool isSpawned = currentNode.SpawnChildNode(current3Bits);
        //            if (k == 7 && isSpawned)
        //            {
        //                _leavesNum++;
        //            }
        //            currentNode = currentNode.GetNodeWithBinaryID(current3Bits);
        //    }
        //}

        //private Color FindNewChildColor()
        //{
        //    // int rInt = EncodingConverter.ConvertBinToInt(EncodingConverter.ConvertIntToBin(colorAvg));
        //    int rInt = 0;
        //    int gInt = 0;
        //    int bInt = 0;
        //    foreach (var node in this.ChildrenAvailable)
        //    {
        //        string _3BitsColorString = node._binaryID;
        //        //Color color = HelperModule.GetColor(colorString);
        //        rInt += EncodingConverter.ConvertBinToInt(_3BitsColorString[0].ToString());
        //        gInt += EncodingConverter.ConvertBinToInt(_3BitsColorString[1].ToString());
        //        bInt += EncodingConverter.ConvertBinToInt(_3BitsColorString[2].ToString());
        //    }
        //    Color colorToReturn = Color.FromArgb(rInt/ChildrenAvailable.Count(),
        //                                         gInt/ChildrenAvailable.Count(),
        //                                         bInt/ChildrenAvailable.Count());
        //    return colorToReturn;
        //}

    }
}

