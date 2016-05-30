using System.Collections.Generic;
using System.Drawing;
using System.Linq;


namespace Octree_ZGTR_WPFApp.Engine.OctreeTree.OctreeTypes.Balanced
{
    public class OctreeBalancedLeastCAllCollapsing : OctreeTree.OctreeTypes.AbstractOctree.Octree
    {
        public OctreeBalancedLeastCAllCollapsing(int numOfLeavesWanted) : base(numOfLeavesWanted)
        {
        }

        protected override void CollapseTree()
        {
            Node nodeToCollapse = FindBaseNodeToCollapse();
            Color newColorToAdd = this.Collapse(nodeToCollapse, ref this._leavesNum);
            this.AddToTree(this.BaseNode, 0, newColorToAdd);
        }

        public override string ToString()
        {
            return "OctreeBalancedLeastCAllCollapsing";
        }

        protected override Color Collapse(Node nodeToCollapse, ref int leavesNum)
        {
            Dictionary<int, List<Node>> listOfAllChilds = new Dictionary<int, List<Node>>();
            for (int i = nodeToCollapse.Level + 1; i < 8; i++)
            {
                listOfAllChilds.Add(i, new List<Node>());
            }
            GetAllChildsDown(nodeToCollapse, ref listOfAllChilds);
            List<Node> listOfAllChildsAtLevel7 = listOfAllChilds[7];

            
            int childsAtLevel7Count = listOfAllChildsAtLevel7.Count;

            
            List<Node> listOfChildFreed = new List<Node>();
            foreach (var keyValuePair in listOfAllChilds)
            {
                listOfChildFreed.AddRange(listOfAllChilds[keyValuePair.Key]);
            }
            int childsAllCount = listOfChildFreed.Count;

            int[] colorSumTriple = HelperModule.GetColorSumComponents(listOfChildFreed);
            int redAvg = colorSumTriple[0] / childsAllCount;
            int greenAvg = colorSumTriple[1] / childsAllCount;
            int blueAvg = colorSumTriple[2] / childsAllCount;
            Color newColor = Color.FromArgb(redAvg, greenAvg, blueAvg);

            leavesNum -= childsAtLevel7Count;

            // Neutralize the children downward
            this.NeutrilizeAllChildren(nodeToCollapse, listOfAllChilds.Values.ToList());

            return newColor;
        }

        public override List<Node> GetLeavesList()
        {
            return base._listOfLevels[7];
        }
    }
}
