using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using pawelek.Common;

namespace pawelek.Models
{
    /// <summary>
    /// Lista kwadracików czyli coś do jest ważne dla ViewModelu do wyświetlania danych w GUI
    /// </summary>
    public class SquareList : IEnumerable<Square>
    {
        #region Indexer
        private List<Square> _list;
        private List<Square> List
        {
            get { return _list ?? (_list = GenerateSquareList()); }
            set { _list = value; }
        }
        public Square this[int i]
        {
            get
            {
                return List[i];
            }
            set
            {
                List.Insert(i, value);
            }
        }
        #endregion

        public SquareList()
        {
            List = GenerateSquareList();
        }

        #region methods

        public List<Square> GenerateSquareList()
        {
            var list = new List<Square>();


            for (int r = 0; r < Globals.Rows; ++r)
            {
                for (int c = 0; c < Globals.Cols; ++c)
                {
                    var sq = new Square { Length = Globals.Length, X = c * (Globals.Length), Y = r * (Globals.Length) };
                    list.Add(sq);
                }
            }

            return list;
        }


        public int DetermineSquareId(Point p)
        {
            int c = (int)p.X / Globals.Length;
            int r = (int)p.Y / Globals.Length;
            return Globals.Cols * r + c;
        }

        public void ClearAll()
        {
            foreach (var i in List)
            {
                i.IsFilled = false;
            }
        }

        public void ToggleFilled(int id)
        {
            try
            {
                if (List[id].IsFilled)
                    List[id].IsFilled = false;
                else
                    List[id].IsFilled = true;
            }
            catch (ArgumentOutOfRangeException)
            {
                Console.WriteLine("Out of range: {0}", id);
            }
        }

        public override string ToString()
        {
            var str = "";
            foreach (var i in List)
            {
                if (i.IsFilled)
                    str += "1";
                else
                    str += "0";
            }
            return str;
        }

        public int[] ToIntArray()
        {
            int[] arr = new int[List.Count()];
            for (int i = 0; i < List.Count(); ++i)
            {
                if (List[i].IsFilled)
                    arr[i] = 1;
                else
                    arr[i] = -1;
            }
            return arr;
        }

        static public SquareList Parse(int[] array, int n)
        {
            var list = new SquareList();
            for (int i = 0; i < n; ++i)
            {
                if (array[i] == 1)
                    list[i].IsFilled = true;
            }
            return list;
        }

        public IEnumerator<Square> GetEnumerator()
        {
            return List.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public double[] ToDoubleArray()
        {
            double[] arr = new double[List.Count()];
            for (int i = 0; i < List.Count(); ++i)
            {
                if (List[i].IsFilled)
                    arr[i] = 1;
                else
                    arr[i] = -1;
            }
            return arr;
        }
        

        #endregion
    }
}
