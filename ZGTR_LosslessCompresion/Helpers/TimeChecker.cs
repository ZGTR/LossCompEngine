using System;

namespace ZGTR_LossCompWPFApp.GraphicsEngine
{
    class TimeChecker
    {
        private DateTime t1;
        private DateTime t2;
        public double TotalTimeCollapse { get; private set; }

        public TimeChecker()
        {
            TotalTimeCollapse = 0;
        }

        public void S1()
        {
            t1 = DateTime.Now;
        }

        public double S2()
        {
            t2 = DateTime.Now;
            TotalTimeCollapse += ((t2 - t1).TotalSeconds);
            return TotalTimeCollapse;
        }
    }
}
