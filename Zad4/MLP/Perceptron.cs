using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Zad4;

namespace Zad4.MLP
{
    public class Perceptron
    {
        //private int[,] samples;
        //private int samplesCount = 0;
        //private int pixelsCount = 0;

        //private int Id { get; set; }
        public Weights weights;
        //private Weights Pocket { get; set; }
        //private double Threshold { get; set; }
        private double learningRate;
        public double bias;
        private double x;
        public double sigmaX;
        public double delta;
        public double[] input;
        
        public Perceptron(ref double learningRate)
        {
            this.learningRate = learningRate;
        }

        public void CalculateSigmaX(double[] aaa)
        {
            if (weights.IsNull())
            {
                weights = new Weights(aaa.Length);
                bias = Globals.Random.NextDouble();
            }
            this.input = aaa;
            x = 0;
            for (int i = 0; i < aaa.Length; ++i)
                x += weights[i] * aaa[i];
            x += bias * 1;
            sigmaX = CalculateSigmoida(x);
        }

        public void CalculateDelta(double t)
        {
            delta = (sigmaX - t) * sigmaX * (1 - sigmaX);
        }
        public void CalculateDelta2(double t)
        {
            delta = t * sigmaX * (1 - sigmaX);
        }

        private double CalculateSigmoida(double x)
        {
            return (1 / (1 + Math.Pow(Math.E, -x)));
        }

        public void UpdateWeights()
        {
            for (int i = 0; i < weights.Count; i++)
            {
                weights[i] = weights[i] - learningRate * delta * input[i];
            }
            bias = bias - learningRate * delta * 1;
        }
    }
}
