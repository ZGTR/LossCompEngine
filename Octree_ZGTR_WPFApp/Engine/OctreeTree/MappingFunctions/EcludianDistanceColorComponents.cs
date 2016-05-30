using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Octree_ZGTR_WPFApp.Engine.OctreeTree.MappingFunctions
{
    class EcludianDistanceColorComponents : IMappingFunction
    {
        public double GetDistance(Color originColor, Color color)
        {
            int rComponentDiff = originColor.R - color.R;
            int gComponentDiff = originColor.G - color.G;
            int bComponentDiff = originColor.B - color.B;
            double minDistance = Math.Sqrt(Math.Pow(rComponentDiff, 2) +
                                           Math.Pow(gComponentDiff, 2) +
                                           Math.Pow(bComponentDiff, 2));
            return minDistance;
        }

        public override string ToString()
        {
            return "Ecludian Distance Color Components";
        }
    }
}
