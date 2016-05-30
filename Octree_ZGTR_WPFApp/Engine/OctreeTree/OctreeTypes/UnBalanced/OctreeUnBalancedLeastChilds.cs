using System.Collections.Generic;
using System.Drawing;
using System.Linq;


namespace Octree_ZGTR_WPFApp.Engine.OctreeTree.OctreeTypes.UnBalanced
{
    public class OctreeUnBalancedLeastChilds: OctreeTree.OctreeTypes.AbstractOctree.Octree
    {
        public OctreeUnBalancedLeastChilds(int numOfLeavesWanted)
            : base(numOfLeavesWanted)
        {

        }

        protected override void AddToTree(Node currentNode, int startLevel, Color currentPixelColor)
        {
            string[] stringVerticalBits = HelperModule.GetBitsVerticalString(this.EncodingConverter, currentPixelColor);
            bool shouldStop = false;
            for (int k = startLevel; k < 8; k++)
            {
                if (shouldStop)
                {
                    break;
                }
                string current3Bits = stringVerticalBits[k];
                if (CanAddMoreLeaves())
                {
                    Spawn(ref currentNode, current3Bits, ref shouldStop);
                }
                else
                {
                    CollapseTree();
                    Spawn(ref currentNode, current3Bits, ref shouldStop);
                }
            }
        }

        protected override void Spawn(ref Node currentNode, string current3Bits, ref bool shouldStop)
        {
            if (!currentNode.IsLeaf)
            {
                if (!currentNode.Children.Any(node => node.BinaryID == current3Bits))
                {
                    Node nodeSpawned = this.SpawnChildNode(currentNode, current3Bits);
                    currentNode = nodeSpawned;
                    this.AddNodeToListOfLevels(ref nodeSpawned);
                    if (nodeSpawned.Level == 7)
                    {
                        this._leavesNum++;
                        nodeSpawned.IsLeaf = true;
                    }
                }
                else
                {
                    currentNode = currentNode.Children.Where(node => node.BinaryID == current3Bits).Single();
                }
            }
            else
            {
                shouldStop = true;
            }
        }

        protected override void CollapseTree()
        {
            Node nodeToCollapse = this.FindBaseNodeToCollapse();
            this.Collapse(nodeToCollapse, ref this._leavesNum);
            //base.AddToTree(this.BaseNode, 0, newColorToAdd);
        }

        protected override Node FindBaseNodeToCollapse()
        {
            int currentLevel = 6;
            Node nodeToReturn = null;
            int minCount = 8;
            bool isFound = false;
            while (!isFound)
            {
                foreach (Node node in _listOfLevels[currentLevel])
                {
                    if (!node.IsLeaf)
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

        protected override Color Collapse(Node nodeToCollapse, ref int leavesNum)
        {
            Dictionary<int, List<Node>> listOfAllChilds = new Dictionary<int, List<Node>>();
            for (int i = nodeToCollapse.Level + 1; i < 8; i++)
            {
                listOfAllChilds.Add(i, new List<Node>());
            }
            GetAllChildsDown(nodeToCollapse, ref listOfAllChilds);
            List<Node> listOfLeaves = GetAllChildIfLeaf(listOfAllChilds);

            leavesNum -= listOfLeaves.Count;
            
            // Neutralize the children downward
            this.NeutrilizeAllChildren(nodeToCollapse, listOfAllChilds.Values.ToList());
            nodeToCollapse.IsLeaf = true;

            // Coz the current nodeToCollapse is a new leaf
            leavesNum += 1;

            // Should be nodeToCollapse.Color, but it's for performance
            return new Color();
        }

        protected List<Node> GetAllChildIfLeaf(Dictionary<int, List<Node>> listOfAllChilds)
        {
            return (from keyValuePair in listOfAllChilds
                    from node in listOfAllChilds[keyValuePair.Key]
                    where node.IsLeaf
                    select node).ToList();
        }

        protected override void NeutrilizeAllChildren(Node baseNode, List<List<Node>> listOfAllChilds)
        {
            foreach (List<Node> listOfChild in listOfAllChilds)
            {
                RemoveFromListOfLevels(listOfChild);
            }
            baseNode.Children.Clear();
        }

        public override List<Node> GetLeavesList()
        {
            return (from listOfNode in base._listOfLevels 
                    from node in listOfNode 
                    where node.IsLeaf select node).ToList();
        }
    }
}
