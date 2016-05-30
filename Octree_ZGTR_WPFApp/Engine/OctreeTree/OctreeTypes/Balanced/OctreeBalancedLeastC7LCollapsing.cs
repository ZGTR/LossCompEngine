using System.Collections.Generic;
using System.Drawing;
using System.Linq;


namespace Octree_ZGTR_WPFApp.Engine.OctreeTree.OctreeTypes.Balanced
{
    public class OctreeBalancedLeastC7LCollapsing : OctreeTree.OctreeTypes.AbstractOctree.Octree
    {
        public OctreeBalancedLeastC7LCollapsing(int numOfLeavesWanted) 
            : base(numOfLeavesWanted)
        {
        }

        public override string ToString()
        {
            return "OctreeBalancedLeastC7LCollapsing";
        }

        protected override void CollapseTree()
        {
            Node nodeToCollapse = FindBaseNodeToCollapse();
            Color newColorToAdd = this.Collapse(nodeToCollapse, ref this._leavesNum);
            this.AddToTree(this.BaseNode, 0, newColorToAdd);
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
            
            int childsAllCount = listOfAllChilds.Values.Count;
            int childsAtLevel7Count = listOfAllChildsAtLevel7.Count;
            
            int[] colorSumTriple = HelperModule.GetColorSumComponents(listOfAllChildsAtLevel7);
            int redAvg = colorSumTriple[0] / childsAtLevel7Count;
            int greenAvg = colorSumTriple[1] / childsAtLevel7Count;
            int blueAvg = colorSumTriple[2] / childsAtLevel7Count;
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
