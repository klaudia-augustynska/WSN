using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Zad2
{
    /// <summary>
    /// Singleton responsible for generating and maintaining a list of squares that can be used to display them in Canvas.
    /// </summary>
    public sealed class SquareManager
    {
        private static SquareManager instance;

        private SquareManager()
        {

            ParentWidth = 300;
            Cols = 5;
            Rows = 7;
            Length = ParentWidth / Cols;
        }

        public static SquareManager Instance
        {
            get { return instance ?? (instance = new SquareManager()); }
        }

        private int ParentWidth { get; set; }
        public int Cols { get; set; }
        public int Rows { get; set; }
        private int Length { get; set; }

        private List<Square> _squareList;

        public List<Square> SquareList
        {
            get { return _squareList ?? (_squareList = GenerateSquareList()); }
            set { _squareList = value; }
        }


        public List<Square> GenerateSquareList()
        {
            var list = new List<Square>();


            for (int r = 0; r < Rows; ++r)
            {
                for (int c = 0; c < Cols; ++c)
                {
                    var sq = new Square { Length = this.Length, X = c * (Length), Y = r * (Length) };
                    list.Add(sq);
                }
            }

            return list;
        }

        public int DetermineSquareId(Point p)
        {
            int c = (int) p.X/Length;
            int r = (int) p.Y/Length;
            return Cols*r + c;
        }

        public void ClearAll()
        {
            foreach (var i in SquareList)
            {
                i.IsFilled = false;
            }
        }

        public void ToggleFilled(int id)
        {
            try
            {
                if (Instance.SquareList[id].IsFilled)
                    Instance.SquareList[id].IsFilled = false;
                else
                    SquareList[id].IsFilled = true;
            }
            catch (ArgumentOutOfRangeException)
            {
                Console.WriteLine("Out of range: {0}", id);
            }
        }

        public override string ToString()
        {
            var str = "";
            foreach (var i in SquareList)
            {
                if (i.IsFilled)
                    str += "1 ";
                else
                    str += "0 ";
            }
            str += "\n";
            return str;
        }

        public string[] ToStringArray()
        {
            string[] arr = new string[SquareList.Count];
            for (int i = 0; i < SquareList.Count; ++i)
            {
                if (SquareList[i].IsFilled)
                    arr[i] = "1";
                else
                    arr[i] = "0";
            }
            return arr;
        }
        public List<string> ToStringList()
        {
            List<string> list = new List<string>();
            foreach (var item in SquareList)
                if (item.IsFilled)
                    list.Add("1");
                else
                    list.Add("0");
            return list;
        }
    }
}
