using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Octree_ZGTR_WPFApp.Engine.OctreeTree.OctreeTypes.UnBalanced
{
    public class OctreeUnBalancedLeastV1SideCollapsing : OctreeUnBalancedLeastChilds
    {
        public OctreeUnBalancedLeastV1SideCollapsing(int numOfLeavesWanted)
            : base(numOfLeavesWanted)
        {
        }


        protected override void Spawn(ref Node currentNode, string current3Bits, ref bool shouldStop)
        {
            currentNode.NumOfVisiting++;
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

        protected override Node FindBaseNodeToCollapse()
        {
            int currentLevel = 6;
            Node nodeToReturn = null;
            int minCount = 8;
            bool isFound = false;
            while (nodeToReturn == null)
            {
                try
                {
                    nodeToReturn = (from node in _listOfLevels[currentLevel]
                                    where (node.Children.Count() >= 2)
                                    orderby node.Children.Count, node.NumOfVisiting
                                    select node).First();
                }
                catch(Exception)
                {
                    currentLevel--;
                }
            }
            return nodeToReturn;
        }

        //protected override Color Collapse(Node nodeToCollapse, ref int leavesNum)
        //{
        //    Dictionary<int, List<Node>> listOfAllChilds = new Dictionary<int, List<Node>>();
        //    for (int i = nodeToCollapse.Level + 1; i < 8; i++)
        //    {
        //        listOfAllChilds.Add(i, new List<Node>());
        //    }
        //    GetAllChildsDown(nodeToCollapse, ref listOfAllChilds);
        //    List<Node> listOfChildFreed = GetListOfChildFreed(listOfAllChilds);
        //    List<Node> listOfLeaves = GetAllChildIfLeaf(listOfAllChilds);

        //    Node nodeIn7WithLeastVisiting = (from node in listOfLeaves
        //                                     orderby node.NumOfVisiting
        //                                     select node).First();

        //    Node parentOfLeastVNode = (from node in listOfChildFreed
        //                               where node.Children.Contains(nodeIn7WithLeastVisiting)
        //                               select node).Single();

        //    parentOfLeastVNode.Children.Clear();
        //    parentOfLeastVNode.IsLeaf = true;
        //    _listOfLevels[parentOfLeastVNode.Level].Remove(parentOfLeastVNode);

        //    // One leaf is gone
        //    leavesNum -= 1;
        //    // Coz the parentOfLeastVNode is a new leaf
        //    leavesNum += 1;

        //    // Should be nodeToCollapse.Color, but it's for performance
        //    return new Color();
        //}

        private List<Node> GetListOfChildFreed(Dictionary<int, List<Node>> listOfAllChilds)
        {
            var  listOfChildFreed = new List<Node>();
            foreach (var keyValuePair in listOfAllChilds)
            {
                listOfChildFreed.AddRange(listOfAllChilds[keyValuePair.Key]);
            }
            return listOfChildFreed;
        }

        protected override Color Collapse(Node nodeToCollapse, ref int leavesNum)
        {
            //Dictionary<int, List<Node>> listOfAllChilds = new Dictionary<int, List<Node>>();
            //for (int i = nodeToCollapse.Level + 1; i < 8; i++)
            //{
            //    listOfAllChilds.Add(i, new List<Node>());
            //}
            //GetAllChildsDown(nodeToCollapse, ref listOfAllChilds);
            //List<Node> listOfLeaves = GetAllChildIfLeaf(listOfAllChilds);
            Node nodeToCutFromOutward = GetSideToBeCut(nodeToCollapse);
            Dictionary<int, List<Node>> listOfAllChilds = new Dictionary<int, List<Node>>();
            for (int i = nodeToCutFromOutward.Level + 1; i < 8; i++)
            {
                listOfAllChilds.Add(i, new List<Node>());
            }
            GetAllChildsDown(nodeToCutFromOutward, ref listOfAllChilds);
            List<Node> listOfLeaves = GetAllChildIfLeaf(listOfAllChilds);

            leavesNum -= listOfLeaves.Count;

            // Neutralize the children downward
            this.NeutrilizeAllChildren(nodeToCutFromOutward, listOfAllChilds.Values.ToList());
            _listOfLevels[nodeToCutFromOutward.Level].Remove(nodeToCutFromOutward);
            nodeToCollapse.Children.Remove(nodeToCutFromOutward);
            //nodeToCollapse.IsLeaf = true;

            // Coz the current nodeToCollapse is a new leaf
            //leavesNum += 1;

            // Should be nodeToCollapse.Color, but it's for performance
            return new Color();
        }

        private Node GetSideToBeCut(Node baseNode)
        {
            Node currentNodeToCut = null;
            int numOfVisiting = 999999999;
            foreach (Node node in baseNode.Children)
            {
                int currentNum = node.NumOfVisiting;
                GetVisitingNum(node, ref currentNum);
                if (currentNum < numOfVisiting)
                {
                    currentNodeToCut = node;
                    numOfVisiting = currentNum;
                }
            }
            return currentNodeToCut;
        }

        private void GetVisitingNum(Node node, ref int currentNum)
        {
            foreach (Node child in node.Children)
            {
                currentNum += child.NumOfVisiting;
                GetVisitingNum(child, ref currentNum);
            }
        }
    }
}
