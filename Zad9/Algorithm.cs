using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Zad9
{
    public class Algorithm
    {
        public Point[] neurons;
        private int Lambda = Globals.Lambda;
        private int Neurons = Globals.NumberOfNeurons;
        public int Iterations = Globals.NumberOfIterations;
        public List<Point> examples = null;


        public Algorithm(ref List<Point> list)
        {
            examples = list;
            Initialize();
        }

        public void ApdejtujWagi(int P, int v, int t)
        {
            neurons[v].X += Alpha(t) * Gauss(v, v) * (examples[P].X - neurons[v].X);
            neurons[v].Y += Alpha(t) * Gauss(v, v) * (examples[P].Y - neurons[v].Y);
            for (int i = 1; i < Lambda; ++i)
            {
                if (v - i >= 0)
                {
                    neurons[v-1].X += Alpha(t) * Gauss(v - i, v) * (examples[P].X - neurons[v-1].X);
                    neurons[v-1].Y += Alpha(t) * Gauss(v - i, v) * (examples[P].Y - neurons[v-1].Y);
                }
                if (v + i < Neurons)
                {
                    neurons[v+1].X += Alpha(t) * Gauss(v + i, v) * (examples[P].X - neurons[v+1].X);
                    neurons[v+1].Y += Alpha(t) * Gauss(v + i, v) * (examples[P].Y - neurons[v+1].Y);
                }
            }
        }

        public int RandomExample()
        {
            return Globals.Random.Next(0, examples.Count);
        }

        private void Initialize()
        {
            neurons = new Point[Neurons];
            for (int i = 0; i < neurons.Length; ++i)
                neurons[i] = new Point(Globals.Random.Next((int)Globals.From.X, (int)Globals.To.X), Globals.Random.Next((int)Globals.From.Y, (int)Globals.To.Y));

            for (int i = 0; i < neurons.Length; ++i)
                System.Diagnostics.Debug.WriteLine("neurons[{0}] = ({1}, {2})", i, neurons[i].X, neurons[i].Y);
        }

        public double Gauss(int w, int v)
        {
            return Math.Exp(   Math.Pow(Rho(w, v), 2) / 
                               (2.0 * Math.Pow(Lambda, 2)) *
                               -1.0
                           );
        }

        private int Rho(int w, int v)
        {
            return Math.Abs(w - v);
        }

        private double Alpha(int x)
        {
            double t = x;
            return (1.0 - (t - 1.0) / (double)Iterations);
        }

        public int FindIndexOfNearestNeuron(Point p)
        {
            var min = Double.MaxValue;
            int min_index = 0;
            for (int i = 0; i < Neurons; ++i)
            {
                var a2 = Math.Pow(neurons[i].X - p.X, 2);
                var b2 = Math.Pow(neurons[i].Y - p.Y, 2);
                var c2 = a2 + b2;
                if (c2 < min)
                {
                    min = c2;
                    min_index = i;
                }
            }
            return min_index;
        }

    }
}
