using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Octree_ZGTR_WPFApp.Engine.OctreeTree.MappingFunctions
{
    class ColorComponent : IMappingFunction
    {
        private ColorComponentType _colorComponentType;
        public ColorComponent(ColorComponentType colorComponentType)
        {
            this._colorComponentType = colorComponentType;
        }

        public override string ToString()
        {
            return "Color Component";
        }

        public double GetDistance(Color originColor, Color color)
        {
            if (_colorComponentType == ColorComponentType.Green)
            {
                return (Math.Abs(originColor.G - color.G));
            }
            else
            {
                if (_colorComponentType == ColorComponentType.Red)
                {
                    return (Math.Abs(originColor.R - color.R));
                }
                return (Math.Abs(originColor.B - color.B));
            }
        }
    }
}
