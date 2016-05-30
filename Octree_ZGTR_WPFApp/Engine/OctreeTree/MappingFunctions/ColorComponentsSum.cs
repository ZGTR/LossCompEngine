using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Octree_ZGTR_WPFApp.Engine.OctreeTree.MappingFunctions
{
    class ColorComponentsSum : IMappingFunction
    {
        public double GetDistance(Color originColor, Color color)
        {
            return (Math.Abs(originColor.G - color.G)) +
                    (Math.Abs(originColor.R - color.R)) +
                    (Math.Abs(originColor.B - color.B));
        }

        public override string ToString()
        {
            return "Color Components Sum";
        }
    }
}
