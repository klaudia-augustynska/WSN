using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Zad4.MLP
{
    public struct Angles
    {
        public double alpha;
        public double beta;
        public Angles(double a, double b)
        {
            alpha = a;
            beta = b;
        }
    }

    public struct Arm
    {
        public static Point shoulder = Globals.MountPoint;
        public Point elbow;
        public Point hand;
    }

    public struct Weights
    {
        private double[] weights;
        public Weights(int size)
        {
            weights = new double[size];
            Clear();
        }
        public double this[int i]
        {
            get { return weights[i]; }
            set { weights[i] = value; }
        }
        public int Count { get { return weights.Length; } }

        public void Clear()
        {
            for (int i = 0; i < weights.Length; ++i)
                weights[i] = Globals.Random.NextDouble();
        }
        public bool IsNull()
        {
            if (weights == null)
                return true;
            return false;
        }
        public override string ToString()
        {
            var str = "";
            for (int i = 0; i < weights.Length; ++i)
                str += weights[i] + "  ";
            return str;
        }
    }

    public struct Example
    {
        public Point point;
        public Angles angles;
        public Example(double x, double y)
        {
            point = new Point(x, y);
            angles = new Angles(Globals.Random.NextDouble() * 180, Globals.Random.NextDouble() * 180);
        }
        public Example(double x, double y, double a, double b)
        {
            point = new Point(x, y);
            angles = new Angles(a, b);
        }
        public Example(Point p)
        {
            point = p;
            angles = new Angles(0, 0);
        }
    }
}
