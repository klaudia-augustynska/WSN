using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Zad3.Common;
using Zad3.Models;

namespace Zad3.Adaline
{
    /// <summary>
    /// 
    /// </summary>
    public class Algorithm
    {
        private List<Group> Samples;
        private List<Group> SamplesDFT;
        private PerceptronAdaline[] perceptrons;
        private int perceptronsCount;
        private double learningRate = 0.01;

        public Algorithm(string filename)
        {
            ReadDataFromFile(filename);
            perceptronsCount = Samples.Count();
            TransformSamples();
            InitializePerceptrons();
            TeachPerceptrons(1000);
        }
        
        private void ReadDataFromFile(string filename)
        {
            using (var sr = new StreamReader(filename))
            {
                Samples = new List<Group>();
                sr.ReadLine();

                string nextLine;
                Group group = new Group(0);
                bool firstTime = true;
                while ((nextLine = sr.ReadLine()) != null)
                {
                    if (nextLine[0] == '%')
                    {
                        if (firstTime)
                            firstTime = false;
                        else
                            Samples.Add(group);
                        nextLine = sr.ReadLine();
                        group = new Group(int.Parse(nextLine));
                        nextLine = sr.ReadLine();
                    }
                    double[] sample = new double[Globals.Pixels];
                    for (int i = 0; i < Globals.Pixels; ++i)
                    {
                        if (nextLine[i] == '0')
                            sample[i] = -1;
                        else
                            sample[i] = 1;
                    }
                    group.List.Add(sample);
                }
                Samples.Add(group);
            }
        }

        private void TransformSamples()
        {
            SamplesDFT = new List<Group>();
            foreach (var group in Samples)
            {
                var groupDFT = new Group(group.Number);
                foreach (var image in group.List)
                {
                    groupDFT.List.Add(DFT(image));
                }
                SamplesDFT.Add(groupDFT);
            }

        }

        private double[] DFT(double[] image)
        {            
            int N = Globals.Pixels;

            Complex[] x = new Complex[N];
            for (int i = 0; i < N; i++)
            {
                x[i] = new Complex(image[i], 0);
            }

            MathNet.Numerics.IntegralTransforms.Fourier.Forward(x);

            var amplitude = new double[N];

            for (int i = 0; i < N; i++)
            {
                amplitude[i] = Complex.Abs(x[i]);
            }

            return amplitude;
        }

        private void InitializePerceptrons()
        {
            perceptrons = new PerceptronAdaline[perceptronsCount];
            for (int i = 0; i < perceptronsCount; ++i)
                perceptrons[i] = new PerceptronAdaline(i, ref SamplesDFT, ref learningRate);
        }

        private void TeachPerceptrons(int steps)
        {
            foreach (var p in perceptrons)
            {
                p.ActivatePerceptron(steps);
            }
        }

        public int Recognize(SquareList image)
        {
            var array = image.ToDoubleArray();

            double[] dftArray = DFT(array);
            Tuple<int, double>[] outputs = new Tuple<int, double>[perceptronsCount];

            int max = 0;
            for (int i = 0; i < perceptronsCount; ++i)
            {
                outputs[i] = new Tuple<int, double>(perceptrons[i].Id, perceptrons[i].DotProduct(ref dftArray));
                if (outputs[max].Item2 < outputs[i].Item2)
                    max = i;
            }
            return outputs[max].Item1;
        }

        public void CorrectWeights(SquareList image, int id, int C)
        {
            var array = image.ToDoubleArray();
            double[] dftArray = DFT(array);
            for (int i = 0; i < 2; ++i)
            {
                perceptrons[id].Correct(dftArray, C);
            }
        }
    }
}
