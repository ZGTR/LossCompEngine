using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Octree_ZGTR_WPFApp.Engine.EncodingPackage;


namespace Octree_ZGTR_WPFApp.Engine.OctreeTree.OctreeTypes.AbstractOctree
{
    public abstract class Octree
    {
        public static double TotalTimeCollapse = 0;
        public EncodingConverter _encodingConverter;
        public EncodingConverter EncodingConverter
        {
            get { return _encodingConverter; }
        }

        protected Node _baseNode;
        public Node BaseNode
        {
            get { return _baseNode; }
        }

        protected int _leavesNum;
        public int LeavesNum
        {
            get { return _leavesNum; }
        }

        protected Color[,] _imageMatrix;
        public Color[,] ImageMatrix
        {
            get { return _imageMatrix; }
        }

        protected string _imageURI;
        public string ImageURI
        {
            get { return _imageURI; }
        }

        protected List<Node>[] _listOfLevels;
        public List<Node>[] ListOfLevels
        {
            set { _listOfLevels = value; }
            get { return _listOfLevels; }
        }

        public readonly int NumOfLeavesWanted;

        public Octree(int numOfLeavesWanted)
        {
            this._encodingConverter = new EncodingConverter(numOfLeavesWanted);
            this.NumOfLeavesWanted = numOfLeavesWanted;
            this._baseNode = new Node(this.EncodingConverter, "", -1, "");
            this._leavesNum = 0;
            this.InitializeListOfLevels();
        }

        protected void InitializeListOfLevels()
        {
            this._listOfLevels = new List<Node>[8];
            for (int i = 0; i < 8; i++)
            {
                this._listOfLevels[i] = new List<Node>();
            }
        }

        public void BuildOctreeFromImageFile(string imageURI)
        {
            // Convert Image to color matrix
            this._imageURI = imageURI;
            this._imageMatrix = ImageController.ImageController.GetBitmapVector(imageURI);
            
            // Loop throught the image matrix and build up the corresponding tree
            for (int i = 0; i < _imageMatrix.GetLength(0); i++)
            {
                for (int j = 0; j < _imageMatrix.GetLength(1); j++)
                {
                    Color currentPixelColor = _imageMatrix[i, j];
                    AddToTree(_baseNode, 0, currentPixelColor);               // Add color to tree
                }
            }
        }

        protected virtual void AddToTree(Node currentNode, int startLevel, Color currentPixelColor)
        {
            string[] stringVerticalBits = HelperModule.GetBitsVerticalString(this.EncodingConverter, currentPixelColor);
            for (int k = startLevel; k < 8; k++)
            {
                string current3Bits = stringVerticalBits[k];
                if (CanAddMoreLeaves())
                {
                    Spawn(ref currentNode, current3Bits);
                }
                else
                {
                    CollapseTree();
                    Spawn(ref currentNode, current3Bits);
                }
            }
        }

        protected virtual void Spawn(ref Node currentNode, string current3Bits)
        {
            if (!currentNode.Children.Any(node => node.BinaryID == current3Bits))
            {
                Node nodeSpawned = this.SpawnChildNode(currentNode, current3Bits);
                currentNode = nodeSpawned;
                this.AddNodeToListOfLevels(ref nodeSpawned);
                if (nodeSpawned.Level == 7)
                {
                    this._leavesNum++;
                }
            }
            else
            {
                currentNode = currentNode.Children.Where(node => node.BinaryID == current3Bits).Single();
            }
        }

        protected virtual void Spawn(ref Node currentNode, string current3Bits, ref bool shouldStop)
        {
        }

        public List<Node> GetAvailableNodesAtLevel(int i)
        {
            return this._listOfLevels[i];
        }

        public Node SpawnChildNode(Node baseCurrentNode, string current3Bits)
        {
            Node spawnedNode = new Node(this.EncodingConverter, baseCurrentNode.BinaryPath + baseCurrentNode.BinaryID,
                                    baseCurrentNode.Level + 1, current3Bits);
            baseCurrentNode.Children.Add(spawnedNode);
            return spawnedNode;
        }

        protected abstract void CollapseTree();

        protected virtual Node FindBaseNodeToCollapse()
        {
            int currentLevel = 6;
            Node nodeToReturn = null;
            int minCount = 8;
            bool isFound = false;
            while (!isFound)
            {
                //nodeToReturn = (from node in _listOfLevels[currentLevel]
                //                       where (node.Children.Count() >= 2)
                //                       orderby node.Children.Count
                //                       select node).First();
                foreach (Node node in _listOfLevels[currentLevel])
                {
                    int childCount = node.Children.Count;
                    if (childCount == 2)
                    {
                        nodeToReturn = node;
                        isFound = true;
                        break;
                    }
                    else
                    {
                        if ((childCount > 2) && (childCount < minCount))
                        {
                            minCount = node.Children.Count;
                            nodeToReturn = node;
                        }
                    }
                }
                if (nodeToReturn == null)
                {
                    currentLevel--;
                }
                else
                {
                    break;
                }
            }
            return nodeToReturn;
        }

        //public List<Node> GetAvailableNodesAtLevel(int levelWanted)
        //{
        //    return this._listOfLevels[levelWanted];
        //    //GetAvailableNodesAtLevel(this._baseNode, levelWanted);
        //}

        //protected List<Node> GetAvailableNodesAtLevel(Node currentNode, int levelWanted)
        //{
        //    List<Node> listOfNodes = new List<Node>();
        //    if (currentNode.Level == levelWanted)
        //    {
        //        listOfNodes.Add(currentNode);
        //    }
        //    else
        //    {
        //        foreach (var node in currentNode.Children)
        //        {
        //            if (node != null)
        //            {
        //                listOfNodes.AddRange(GetAvailableNodesAtLevel(node, levelWanted));
        //            }
        //        }
        //    }
        //    return listOfNodes;
        //}

        protected bool CanAddMoreLeaves()
        {
            return _leavesNum < this.NumOfLeavesWanted - 1;
        }

        public Node GetClosestNextNode(Node currentNode, string current3Bits)
        {
            Node closestNode = null;
            int intValue = EncodingConverter.ConvertBinToInt(current3Bits);
            int minDifference = (from index in (currentNode.Children.Select(node => currentNode.Children.IndexOf(node)))
                                 select Math.Abs(index - intValue)).Min();
            int closestIndex = (from index in (currentNode.Children.Select(node => currentNode.Children.IndexOf(node)))
                                where Math.Abs(index - intValue) <= minDifference
                                select index).ToList()[0];
            closestNode = currentNode.Children[closestIndex];
            return closestNode;
        }

        protected abstract Color Collapse(Node nodeToCollapse, ref int leavesNum);

        protected List<Node> GetAllChildsAtLevel(Node nodeBase, int levelId)
        {
            List<Node> listToReturn = new List<Node>();
            foreach (var nodeChild in nodeBase.Children)
            {
                if (nodeChild.Level == levelId)
                {
                    listToReturn.Add(nodeChild);
                }
                listToReturn.AddRange(GetAllChildsAtLevel(nodeChild, levelId));
            }
            return listToReturn;
        }

        protected virtual void NeutrilizeAllChildren(Node baseNode, List<List<Node>> listOfAllChilds)
        {
            foreach (List<Node> listOfChild in listOfAllChilds)
            {
                RemoveFromListOfLevels(listOfChild);
            }      
            baseNode.Children.Clear();
        }

        protected void RemoveFromListOfLevels(List<Node> children)
        {
            for (int j = 0; j < children.Count(); j++)
            {
                this._listOfLevels[children[j].Level].Remove(children[j]);
            }
        }

        //protected void RemoveFromListOfLevels(List<Node> children, int level)
        //{
        //    _listOfLevels[level].RemoveAll(node => node.IsDead);
        //}

        //protected void RemoveFromListOfLevels(List<Node> children)
        //{
        //    for (int j = 0; j < children.Count(); j++)
        //    {
        //        RemoveFromListOfLevels(children[j]);
        //    }
        //}

        //protected void RemoveFromListOfLevels(Node node)
        //{
        //    for (int i = 0; i < 8; i++)
        //    {
        //        this._listOfLevels[i].Remove(node);
        //    }
        //}

        protected void GetAllChildsDown(Node nodeBase, ref Dictionary<int, List<Node>> listToReturn)
        {
            foreach (var nodeChild in nodeBase.Children)
            {
                listToReturn[nodeChild.Level].Add(nodeChild);
                if (nodeChild.Level + 1 < 8)
                {
                    GetAllChildsDown(nodeChild, ref listToReturn);
                }
            }
        }

        protected void AddNodeToListOfLevels(ref Node nodeToAdd)
        {
            if (nodeToAdd != null)
            {
                int indexToAddIn = nodeToAdd.Level;
                this._listOfLevels[indexToAddIn].Add(nodeToAdd);
                nodeToAdd.IndexInListOfLevels = this._listOfLevels[indexToAddIn].Count - 1;
            }
        }

        public abstract List<Node> GetLeavesList();
    }
}
