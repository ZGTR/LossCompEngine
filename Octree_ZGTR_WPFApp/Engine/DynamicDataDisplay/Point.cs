using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Octree_ZGTR_WPFApp.Engine.DynamicDataDisplay
{
    public class Point : ICloneable
    {
        public Point(double x, double y)
        {
            X = x;
            Y = y;
        }

        public double X { get; set; }

        public double Y { get; set; }

        public object Clone()
        {
            return new Point(X, Y);
        }

        public override string ToString()
        {
            return String.Format("{0}, {1}", X, Y);
        }
    }
}
