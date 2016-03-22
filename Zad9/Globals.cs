using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace Zad9
{
    public class Globals
    {
        public static int Cols = 400;
        public static int Rows = 250;
        public static Point From = new Point(Cols / 3, Rows / 3);
        public static Point To = new Point(Cols-From.X, Rows-From.Y);
        public static bool GapsEnabled = true;
        public static Brush UserColor = Brushes.Gray;
        public static Brush AlgorithmColor = Brushes.Red;
        public static Random Random = new Random();
        public static int Lambda = 2;
        public static int NumberOfNeurons = 30;
        public static int NumberOfIterations = 10000;
    }
}
