using System;
using System.Linq;


namespace Octree_ZGTR_WPFApp.Engine.OctreeTree.OctreeTypes.Balanced
{
    public class OctreeBalancedLeastVAllSidesCollapsing : OctreeBalancedLeastC7LCollapsing
    {
        public OctreeBalancedLeastVAllSidesCollapsing(int numOfLeavesWanted)
            : base(numOfLeavesWanted)
        {

        }

        public override string ToString()
        {
            return "OctreeBalancedLeastVAllSidesCollapsing";
        }

        protected override void Spawn(ref Node currentNode, string current3Bits)
        {
            currentNode.NumOfVisiting++;
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
    }
}
