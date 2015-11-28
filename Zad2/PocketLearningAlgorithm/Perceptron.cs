using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zad2.Common;

namespace Zad2.PocketLearningAlgorithm
{
    public class Perceptron
    {
        private int[,] samples;
        private int samplesCount = 0;
        private int pixelsCount = 0;

        private int Id { get; set; }
        private Weights Weights { get; set; }
        private Weights Pocket { get; set; }
        private double Threshold { get; set; }
        private double LearningRate { get; set; }
        
        public Perceptron(ref int[,] samples, ref int samplesCount, ref double learningRate, int id)
        {
            this.samples = samples;
            this.samplesCount = samplesCount;
            this.pixelsCount = Globals.Cols * Globals.Rows;

            Id = id;
            Weights = new Weights();
            Pocket = new Weights();
            Threshold = 0;
            LearningRate = learningRate;
        }

        public void ActivatePerceptron(int E)
        {
            var T = CheckIfExampleIsPositive(E);
            var O = Output(E);
            var ERR = T - O;

            if (ERR == 0)
            {
                IncrementWeightsLifeTime();
                if (Weights.LifeTime > Pocket.LifeTime)
                    UpdatePocket();
            }
            else
            {
                UpdateWeightsValues(ERR, E);
                UpdateThreshold(ERR);
            }
        }

        private int CheckIfExampleIsPositive(int exampleId)
        {
            return (samples[exampleId, this.Id] == 1) ? 1 : -1;
        }

        private int Output(int exampleId)
        {
            double sum = 0;
            for (int i = 0; i < pixelsCount; ++i)
            {
                sum += Weights[i] * samples[exampleId, i];
            }
            return (sum >= Threshold) ? 1 : -1;
        }

        public int Analize(int[] pixelId)
        {
            double sum = 0;
            for (int i = 0; i < pixelsCount; ++i)
            {
                sum += Weights[i] * pixelId[i];
            }
            return (sum >= Threshold) ? 1 : -1;
        }


        private void IncrementWeightsLifeTime()
        {
            this.Weights.LifeTime++;
        }

        private void UpdatePocket()
        {
            for (int i = 0; i < Weights.Count; ++i)
                Pocket[i] = Weights[i];
        }

        private void UpdateWeightsValues(double ERR, int exampleId)
        {
            for (int i = 0; i < pixelsCount; ++i)
            {
                Weights[i] += LearningRate * ERR * samples[exampleId, i];
            }
        }
        private void UpdateThreshold(double ERR)
        {
            Threshold -= LearningRate * ERR;
        }
    }
}
