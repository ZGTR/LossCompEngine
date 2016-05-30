using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Octree_ZGTR_WPFApp.Engine.GUI
{
    class TimeChecker
    {
        private static DateTime t1;
        private static DateTime t2;
        public static double TotalTimeCollapse;

        public static void Normalize()
        {
            TotalTimeCollapse = 0;
        }

        public static void S1()
        {
            t1 = DateTime.Now;
        }
        public static void S2()
        {
            t2 = DateTime.Now;
            TotalTimeCollapse += ((t2 - t1).TotalSeconds);
        }
    }
}
