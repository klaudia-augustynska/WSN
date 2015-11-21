using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Zad2
{
    /// <summary>
    /// Represents a set of weights used in the algorithm.
    /// </summary>
    public class Weights
    {
        private double[] weights;
        private Random random = new Random();
        public Weights(int n)
        {
            this.Count = n;
            weights = new double[this.Count];
            Clear();
        }

        public Weights(List<string> l)
        {
            this.Count = l.Count;
            weights = new double[this.Count];
            for (int i = 0; i < this.Count; ++i)
                weights[i] = Double.Parse(l[i]);
            this.LifeTime = 0;
        }
        public double this[int i]
        {
            get { return weights[i]; }
            set { weights[i] = value; }
        }
        public int Count { get; private set; }
        public int LifeTime { get; set; }

        public void Clear()
        {
            for (int i = 0; i < this.Count; ++i)
                weights[i] = (double) random.Next(-50, 50) / 200;
            this.LifeTime = 0;
        }

        public override string ToString()
        {
            var str = "";
            for (int i = 0; i < this.Count; ++i)
                str += weights[i] + "  ";
            return str;
        }
    }

    public class PerceptronJSON
    {
        public PerceptronJSON()
        {
            
        }
        public PerceptronJSON(Perceptron p)
        {
            this.Name = p.Name;
            this.Weights = new List<string>();
            this.Threshold = p.Threshold.ToString();
            for (int i = 0; i < p.Pocket.Count; ++i)
            {
                this.Weights.Add(p.Pocket[i].ToString());
            }
        }
        public string Name { get; set; }
        public List<string> Weights { get; set; }
        public string Threshold { get; set; }
    }

    public class Perceptron
    {
        public Perceptron()
        {
            
        }
        public Perceptron(PerceptronJSON p)
        {
            this.Name = p.Name;
            this.Weights = new Weights(p.Weights);
            this.Threshold = Double.Parse(p.Threshold);
        }

        private Random random = new Random();
        private int n { get { return Weights.Count; } }
        /// <summary>
        /// Collection of samples that the perceptron can use to learn.
        /// </summary>
        public List<SampleGroup> Samples { get; set; }
        /// <summary>
        /// String identifying the perceptron and its positive samples in collection of samples.
        /// </summary>
        public string Name { get; set; }
        public Weights Weights { get; set; }
        public Weights Pocket { get; set; }
        public double Threshold { get; set; }
        public double LearningRate { get; set; }
        private bool wasLastRandomExamplePositive = false;

        public void ActivatePerceptron(int steps)
        {
            for (int i = 0; i < steps; ++i)
                NextStep();
        }

        private void NextStep()
        {
            var Eid = RandomExampleId(); 
            var E = RandomExample(Eid);
            var T = CheckIfExampleIsPositive(Eid.Item1);

            if (wasLastRandomExamplePositive && T == 1)
                return;
            if (!wasLastRandomExamplePositive && T == -1)
                return;
            wasLastRandomExamplePositive = (T == 1) ? true : false;

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

        /// <summary>
        /// Gets identifiers of random example from sample set.
        /// </summary>
        /// <returns>First value - id of group of samples. Second value - id of sample in a given group.</returns>
        private Tuple<int, int> RandomExampleId()
        {
            int randomPerceptronId = this.random.Next(0, this.Samples.Count-1);
            int randomSampleId = this.random.Next(0, this.Samples[randomPerceptronId].Samples.Count-1);

            return new Tuple<int, int>(randomPerceptronId, randomSampleId);
        }

        /// <summary>
        /// Gets a reference to a single sample.
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        private List<string> RandomExample(Tuple<int, int> ids)
        {
            return this.Samples[ids.Item1].Samples[ids.Item2];
        }

        private int Output(List<string> example)
        {
            double sum = 0;
            for (int i = 0; i < this.n; ++i)
            {
                sum += Weights[i] * Double.Parse(example[i]);
            }
            return (sum >= Threshold) ? 1 : -1;
        }


        /// <summary>
        /// Checks if randomly selected sample is a positive or negative example for perceptron.
        /// </summary>
        /// <param name="exampleGroupId">Id of group of samples.</param>
        /// <returns>+1 if positive, -1 if negative.</returns>
        private int CheckIfExampleIsPositive(int exampleGroupId)
        {
            var nameOfGroupWhereExampleComesFrom = this.Samples[exampleGroupId].Name;
            return (nameOfGroupWhereExampleComesFrom.Equals(this.Name)) ? 1 : -1;
        }

        private void IncrementWeightsLifeTime()
        {
            this.Weights.LifeTime++;
        }

        private void UpdatePocket()
        {
            // swap references so Pocket uses a better set of weights
            var temp = this.Pocket;
            this.Pocket = this.Weights;
            this.Weights = temp;

            // clear weights
            this.Weights.Clear();
        }

        private void UpdateWeightsValues(double ERR, List<string> E)
        {
            for (int i = 0; i < this.n; ++i)
            {
                Weights[i] += LearningRate * ERR * Double.Parse(E[i]);
            }
        }
        private void UpdateThreshold(double ERR)
        {
            Threshold -= LearningRate * ERR;
        }

        public bool Analize(List<string> example)
        {
            return (Output(example) == 1) ? true : false;
        }
    }

    /// <summary>
    /// An algorithm used to teach perceptrons. 
    /// </summary>
    public class PocketLearningAlgorithm
    {
        private List<SampleGroup> samples;
        private List<Perceptron> perceptrons;
        private int n;
        private readonly int learningRate = 1;
        private string filename = "weights.txt";
        private List<Perceptron> savedPerceptrons;

        private static PocketLearningAlgorithm instance;
        public static PocketLearningAlgorithm Instance
        {
            get { return instance ?? (instance = new PocketLearningAlgorithm()); }
        }

        private PocketLearningAlgorithm()
        {
        }

        /// <summary>
        /// Run algorithm for each perceptron
        /// </summary>
        public void TeachPerceptrons(int steps, List<SampleGroup> samples)
        {
            this.n = samples.Count;
            this.samples = samples;

            Console.WriteLine();

            InitializePerceptrons();

            foreach (var p in perceptrons)
            {
                p.ActivatePerceptron(steps);
            }
            SaveResults();
        }

        /// <summary>
        /// Create list of perceptrons and fill them with initial values
        /// </summary>
        private void InitializePerceptrons()
        {
            if (this.perceptrons == null)
                this.perceptrons = new List<Perceptron>();
            this.perceptrons.Clear();

            foreach (var item in samples)
            {
                this.perceptrons.Add(new Perceptron
                {
                    Name = item.Name,
                    Samples = samples,
                    Weights = new Weights(this.n),
                    Pocket = new Weights(this.n),
                    Threshold = 0,
                    LearningRate = this.learningRate
                });
            }
        }

        private void SaveResults()
        {
            using (var sw = new StreamWriter(filename, false))
            {
                var list = new List<PerceptronJSON>();
                foreach (var p in perceptrons)
                {
                    list.Add(new PerceptronJSON(p));
                }
                string json = JsonConvert.SerializeObject(list);
                sw.Write(json);
            }
            LoadWeightsFromFile();
        }

        /// <summary>
        /// Determines which perceptrons give positive answer
        /// </summary>
        /// <param name="example"></param>
        public string Recognize(List<string> example)
        {
            if (File.Exists(filename))
            {
                if (savedPerceptrons == null || savedPerceptrons.Count == 0)
                    LoadWeightsFromFile();
                var list = GiveAnswer(example);
                return FormatAnswer(list);
            }
            return "";
        }

        private string FormatAnswer(List<Tuple<string, bool>> list)
        {
            string str = "";
            foreach (var item in list)
            {
                if (item.Item2)
                {
                    str += item.Item1 + ", ";
                }
            }
            return str;
        }

        private List<Tuple<string,bool>> GiveAnswer(List<string> example)
        {
            var list = new List<Tuple<string, bool>>();
            foreach (var p in savedPerceptrons)
            {
                list.Add(new Tuple<string,bool>(p.Name, p.Analize(example)));
            }
            return list;
        }

        private void LoadWeightsFromFile()
        {
            if (File.Exists(filename))
            {
                List<PerceptronJSON> list; 
                using (var sr = new StreamReader(filename))
                {
                    string str = sr.ReadToEnd();
                    list = JsonConvert.DeserializeObject<List<PerceptronJSON>>(str);
                }
                if (savedPerceptrons == null)
                    savedPerceptrons = new List<Perceptron>();
                savedPerceptrons.Clear();
                foreach (var item in list)
                {
                    savedPerceptrons.Add(new Perceptron(item));
                }
            }
        }

    }
}
