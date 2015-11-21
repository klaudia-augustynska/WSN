using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zad2.Common;
using Zad2.Models;

namespace Zad2.PocketLearningAlgorithm
{
    /// <summary>
    /// 
    /// </summary>
    public class Algorithm
    {
        private int[,] samples;
        private int samplesCount = 0;
        private int pixelsCount = 0;
        private double learningRate = 1;

        private Perceptron[] perceptrons;

        public Algorithm(string filename)
        {
            ReadDataFromFile(filename);
            TeachPerceptrons(100);
        }

        private void InitializePerceptrons()
        {
            perceptrons = new Perceptron[pixelsCount];
            for (int i = 0; i < pixelsCount; ++i)
            {
                perceptrons[i] = new Perceptron(ref samples, ref samplesCount, ref learningRate, i);
            }
        }

        private void ReadDataFromFile(string filename)
        {
            using (var sr = new StreamReader(filename))
            {

                string firstLine = sr.ReadLine();
                string[] samplesDescription = firstLine.Split(new char[] { ' ' });
                string cols = samplesDescription[0];
                string rows = samplesDescription[1];
                string count = samplesDescription[2];

                if (Int32.Parse(cols) != Globals.Cols || Int32.Parse(rows) != Globals.Rows || Int32.Parse(count) <= 0)
                    throw new Exception("Zły plik");

                pixelsCount = Globals.Cols * Globals.Rows;
                samplesCount = Int32.Parse(count);

                samples = new int[samplesCount,pixelsCount];

                string nextLine;
                for (int j = 0; (nextLine = sr.ReadLine()) != null && j < samplesCount; ++j)
                {
                    for (int i = 0; i < pixelsCount; ++i)
                    {
                        if (nextLine[i] == '1')
                            samples[j, i] = 1;
                        else
                            samples[j, i] = -1;
                    }
                }
            }
        }

        private void TeachPerceptrons(int steps)
        {
            InitializePerceptrons();

            for (int i = 0; i < steps; ++i)
            {
                int eid = RandomExampleId();
                for (int j = 0; j < pixelsCount; ++j)
                    perceptrons[j].ActivatePerceptron(eid);
                /*
                chodzi o to że mamy 900 perceptronów
                losujemy sobie 1000x jeden przykład
                i uczymy te 900 perceptronów tego przykładu

                */
            }
        }

        
        private int RandomExampleId()
        {
            return Globals.Random.Next(0, samplesCount);
        }

        public SquareList RemoveNoise(SquareList image)
        {
            int[] array = image.ToIntArray();
            for (int i = 0; i < pixelsCount; ++i)
            {
                array[i] = perceptrons[i].Analize(array);
            }
            return SquareList.Parse(array, pixelsCount);
        }
        
    }
}
