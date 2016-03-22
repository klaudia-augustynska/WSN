using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using pawelek.Common;
using pawelek.Models;

namespace pawelek.Adaline
{
    /// <summary>
    /// 
    /// </summary>
    public class Algorithm
    {
        private List<Group> Samples;
        // teraz będzie 10 wież a każda ma ileś perceptronów
        private Wieża[] wieże;
        private Perceptron[] perceptrons;
        private int perceptronsCount, wieżaCount;
        private double learningRate = 0.01;

        public Algorithm(string filename)
        {
            ReadDataFromFile(filename);
            perceptronsCount = wieżaCount = Samples.Count();
            InitializePerceptrons();
            TeachPerceptrons(100);
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
                    double[] sample = new double[Globals.Pixels+1];
                    for (int i = 0; i < Globals.Pixels; ++i)
                    {
                        if (nextLine[i] == '0')
                            sample[i] = -1;
                        else
                            sample[i] = 1;
                    }
                    sample[Globals.Pixels] = 0;
                    group.List.Add(sample);
                }
                Samples.Add(group);
            }
        }
        
        private void InitializePerceptrons()
        {
            //if (perceptrons == null)
            //    perceptrons = new Perceptron[perceptronsCount];
            //for (int i = 0; i < perceptronsCount; ++i)
            //    perceptrons[i] = new Perceptron(i, ref Samples, ref learningRate);
            if (wieże == null)
                wieże = new Wieża[wieżaCount];
            for (int i = 0; i < wieżaCount; ++i)
                wieże[i] = new Wieża(i, ref Samples, ref learningRate);
        }

        private void TeachPerceptrons(int steps)
        {
            foreach (var w in wieże)
            {
                w.UczSię(steps);
            }
        }

        public string Recognize(SquareList a)
        {
            double[] example = a.ToDoubleArray();
            var list = GiveAnswer(ref example);
            return FormatAnswer(list);
        }

        private List<int> GiveAnswer(ref double[] example)
        {
            var list = new List<int>();
            foreach (var w in wieże)
            {
                if (w.Analize(ref example))
                    list.Add(w.Id);
            }
            return list;
        }

        private string FormatAnswer(List<int> list)
        {
            string str = "";
            for (int i = 0; i < list.Count; ++i)
            {
                str += list[i].ToString() ;
                if (i < list.Count - 1)
                    str += ", ";
            }
            return str;
        }

        public void CorrectWeights(SquareList image, int id, int C)
        {
            var array = image.ToDoubleArray();
            for (int i = 0; i < 2; ++i)
            {
                wieże[id].Correct(array, C);
            }
        }
    }
}
