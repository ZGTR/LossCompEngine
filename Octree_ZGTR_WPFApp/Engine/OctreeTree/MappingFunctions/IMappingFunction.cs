using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Octree_ZGTR_WPFApp.Engine.OctreeTree.MappingFunctions
{
    public interface IMappingFunction
    {
        new double GetDistance(Color originColor, Color color);
    }
}