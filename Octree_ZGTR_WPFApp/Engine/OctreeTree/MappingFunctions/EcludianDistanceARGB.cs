using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Octree_ZGTR_WPFApp.Engine.OctreeTree.MappingFunctions
{
    class EcludianDistanceARGB : IMappingFunction
    {
        public double GetDistance(Color originColor, Color color)
        {
            return (Math.Sqrt(Math.Pow(Math.Abs(color.ToArgb() - originColor.ToArgb()), 2)));
        }

        public override string ToString()
        {
            return "Ecludian Distance ARGB";
        }
    }
}
