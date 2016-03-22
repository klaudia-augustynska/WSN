using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using pawelek.Common;

namespace pawelek.Adaline
{
    /// <summary>
    /// Represents a set of weights used in the algorithm.
    /// </summary>
    public class Weights
    {
        private double[] weights;
        public Weights(int wielkość)
        {
            this.Count = wielkość;
            weights = new double[this.Count];
            Clear();
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
                weights[i] = (double)Globals.Random.Next(1, 100) / 100;
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
}
