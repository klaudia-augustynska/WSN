using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using pawelek.Common;
using pawelek.Models;

namespace pawelek.Adaline
{
    public class Perceptron
    {
        public int Id { get; set; }
        private Weights Weights { get; set; }
        public Weights Pocket { get; set; }
        private double LearningRate { get; set; }
        public double Threshold { get; set; }
        private bool wasLastRandomExamplePositive = false;
        List<Group> Samples;

        public Perceptron(int i, ref List<Group> samples, ref double learningRate, int wielkość)
        {
            Id = i;
            Samples = samples;
            LearningRate = learningRate;
            Weights = new Weights(wielkość);
            Pocket = new Weights(wielkość);
            Threshold = 0;
        }

        public void ActivatePerceptron(int steps)
        {
            for (int i = 0; i < steps; ++i)
            {
                NextStep();
            }
        }

        private void NextStep()
        {
            // losujemy przykład
            var Eid = RandomExampleId();

            // bierzemy go
            var E = RandomExample(Eid);

            // pozytywny czy negatywny
            var T = CheckIfExampleIsPositive(Eid.Item1);


            if (wasLastRandomExamplePositive && T == 1)
                NextStep();
            if (!wasLastRandomExamplePositive && T == -1)
                NextStep();
            wasLastRandomExamplePositive = (T == 1) ? true : false;

            var O = DotProduct(ref E);
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
        private void UpdateThreshold(double ERR)
        {
            Threshold -= LearningRate * ERR;
        }
        private void UpdateWeightsValues(double ERR, double[] E)
        {
            for (int i = 0; i < Globals.Pixels; ++i)
            {
                Weights[i] += LearningRate * ERR *E[i];
            }
        }

        private void UpdatePocket()
        {
            for (int i = 0; i < Globals.Pixels; ++i)
            {
                Pocket[i] = Weights[i];
            }
        }

        private void IncrementWeightsLifeTime()
        {
            Weights.LifeTime++;
        }

        private Tuple<int, int> RandomExampleId()
        {
            int randomPerceptronId = Globals.Random.Next(0, this.Samples.Count);
            int randomSampleId = Globals.Random.Next(0, this.Samples[randomPerceptronId].List.Count);

            return new Tuple<int, int>(randomPerceptronId, randomSampleId);
        }
        private double[] RandomExample(Tuple<int, int> ids)
        {
            return this.Samples[ids.Item1].List[ids.Item2];
        }
        private int CheckIfExampleIsPositive(int exampleGroupId)
        {
            var nameOfGroupWhereExampleComesFrom = this.Samples[exampleGroupId].Number;
            return (nameOfGroupWhereExampleComesFrom.Equals(this.Id)) ? 1 : -1;
        }
        
        public double DotProduct(ref double[] image)
        {
            double sum = 0;
            for (int i = 0; i < Globals.Pixels; ++i)
            {
                sum += Weights[i] * image[i];
            }
            return (sum >= Threshold) ? 1 : -1;
        }

        public double DotProductPocket(ref double[] image)
        {
            double sum = 0;
            for (int i = 0; i < Globals.Pixels; ++i)
            {
                sum += Pocket[i] * image[i];
            }
            return (sum >= Threshold) ? 1 : -1;
        }

        public void Correct(double[] image, int c)
        {
            var T = c;
            var O = DotProduct(ref image);
            var ERR = T - O;

            if (ERR == 0)
            {
                IncrementWeightsLifeTime();
                if (Weights.LifeTime > Pocket.LifeTime)
                    UpdatePocket();
            }
            else
            {
                UpdateWeightsValues(ERR, image);
                UpdateThreshold(ERR);
            }
        }

        public bool Analize(double[] example)
        {
            return (DotProductPocket(ref example) == 1) ? true : false;
        }
    }
}
