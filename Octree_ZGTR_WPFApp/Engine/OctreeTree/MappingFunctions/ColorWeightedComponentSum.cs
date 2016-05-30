using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Octree_ZGTR_WPFApp.Engine.OctreeTree.MappingFunctions
{
    class ColorWeightedComponentSum : IMappingFunction
    {
        public double GetDistance(Color originColor, Color color)
        {
            return (Math.Abs(originColor.G - color.G) * 4) +
                    (Math.Abs(originColor.R - color.R) * 4) +
                    (Math.Abs(originColor.B - color.B) * 2);
        }

        public override string ToString()
        {
            return "Color Weighted Component Sum";
        }
    }
}
