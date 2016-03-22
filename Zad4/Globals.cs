using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Zad4
{
    public static class Globals
    {
        public static int Cols = 300;
        public static int Rows = 400;

        public static Point MountPoint = new Point(0, Rows / 2);
        public static int ArmLength = 100;

        public static Random Random = new Random();

     //   public static int[] Dimensions = { 50, 40, 30, 20, 10, 2 };
        public static int[] Dimensions = { 4, 4, 2 };
        public static int LayerCount = Dimensions.Length;
        public static int LearningExamplesCount = 10000;
        public static int LearningSteps = 10000; // 1000000000
        public static int Interval = 500; // 15000
    }
}
