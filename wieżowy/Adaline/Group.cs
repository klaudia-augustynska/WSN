using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pawelek.Adaline
{
    public struct Group
    {
        public int Number;
        public List<double[]> List;
        public Group(int number)
        {
            Number = number;
            List = new List<double[]>();
        }
    }
}
