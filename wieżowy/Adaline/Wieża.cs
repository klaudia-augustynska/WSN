using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pawelek.Adaline
{
    public class Wieża
    {
        public List<Perceptron> perceptrons = new List<Perceptron>();
        public int Id;
        public List<Group> Samples;
        public double LearningRate;

        private int dupa = 0;

        public Wieża(int id, ref List<Group> samples, ref double learningRate)
        {
            Id = id;
            Samples = samples;
            LearningRate = learningRate;
            perceptrons.Add(new Perceptron(id, ref samples, ref learningRate, Common.Globals.Pixels));
        }

        public void UczSię(int steps)
        {
            perceptrons[0].ActivatePerceptron(steps);
            while (Warunek())
            {
                int idPoprzedniego = perceptrons.Count() - 1;
                for (int i = 0; i < Samples.Count(); ++i)
                {
                    for (int j = 0; j < Samples[i].List.Count(); ++j)
                    {
                        var odpowiedź = (perceptrons[idPoprzedniego].Analize(Samples[i].List[j])) ? 1 : -1;
                        Samples[i].List[j][Common.Globals.Pixels] = odpowiedź;
                    }
                }
                perceptrons.Add(new Perceptron(Id, ref Samples, ref LearningRate,Common.Globals.Pixels));
                perceptrons[perceptrons.Count-1].ActivatePerceptron(steps);
            }
        }

        private bool Warunek()
        {
            bool klasyfikacjaJakaPowinnaByć = false;
            for (int i = 0; i < Samples.Count(); ++i)
            {
                for (int j = 0; j < Samples[i].List.Count(); ++j)
                {
                    if (Samples[i].Number == Id)
                        klasyfikacjaJakaPowinnaByć = true;
                    else
                        klasyfikacjaJakaPowinnaByć = false;
                    var skwalifikowało = perceptrons[perceptrons.Count - 1].Analize(Samples[i].List[j]);
                    if (klasyfikacjaJakaPowinnaByć != skwalifikowało)
                        return true;
                }
            }
            return false;
        }

        public bool Analize(ref double[] example)
        {
            return perceptrons[perceptrons.Count-1].Analize(example);
        }

        public void Correct(double[] array, int c)
        {
            perceptrons[perceptrons.Count - 1].Correct(array, c);
        }
    }
}
