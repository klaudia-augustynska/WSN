using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zad3.Common;
using Zad3.Models;

namespace Zad3.Adaline
{
    public class PerceptronAdaline
    {
        public int Id { get; set; }
        private Weights Weights { get; set; }
        private double LearningRate { get; set; }
        
        private List<Group> samplesDFT;
        private bool wasLastRandomExamplePositive = false;

        public PerceptronAdaline(int i, ref List<Group> samplesDFT, ref double learningRate)
        {
            this.Id = i;
            this.samplesDFT = samplesDFT;
            this.LearningRate = learningRate;
            Weights = new Weights();
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
            var C = CheckIfExampleIsPositive(Eid.Item1);


            if (wasLastRandomExamplePositive && C == 1)
                NextStep();
            if (!wasLastRandomExamplePositive && C == -1)
                NextStep();
            wasLastRandomExamplePositive = (C == 1) ? true : false;

            var O = DotProduct(ref E);
            for (int j = 0; j < Globals.Pixels; ++j)
            {
                Weights[j] = Weights[j] + LearningRate * (C - O) * E[j];
            }
        }

        private Tuple<int, int> RandomExampleId()
        {
            int randomPerceptronId = Globals.Random.Next(0, this.samplesDFT.Count);
            int randomSampleId = Globals.Random.Next(0, this.samplesDFT[randomPerceptronId].List.Count);

            return new Tuple<int, int>(randomPerceptronId, randomSampleId);
        }
        private double[] RandomExample(Tuple<int, int> ids)
        {
            return this.samplesDFT[ids.Item1].List[ids.Item2];
        }
        private int CheckIfExampleIsPositive(int exampleGroupId)
        {
            var nameOfGroupWhereExampleComesFrom = this.samplesDFT[exampleGroupId].Number;
            return (nameOfGroupWhereExampleComesFrom.Equals(this.Id)) ? 1 : -1;
        }
        
        public double DotProduct(ref double[] image)
        {
            double sum = 0;
            sum += Weights[0] * 1;
            for (int i = 1; i < Globals.Pixels; ++i)
            {
                sum += Weights[i] * image[i];
            }
            return sum;
        }

        public void Correct(double[] image, int c)
        {    
            var E = image;
            var C = c;

            var O = DotProduct(ref E);
            for (int j = 0; j < Globals.Pixels; ++j)
            {
                Weights[j] = Weights[j] + LearningRate * (C - O) * E[j];
            }
        }
    }
}
