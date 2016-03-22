using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zad3.Common;

namespace Zad3.Models
{
    /// <summary>
    /// Pojedynczy kwadracik do wyświetlania w GUI, to co umieszcza się w klasie SquareList
    /// </summary>
    public class Square : ObservableObject
    {
        private bool isFilled = false;

        public double X { get; set; }
        public double Y { get; set; }
        public double Length { get; set; }

        public bool IsFilled
        {
            get
            {
                return isFilled;
            }
            set
            {
                isFilled = value;
                NotifyPropertyChanged("IsFilled");
            }
        }

        public override string ToString()
        {
            return String.Format("X: {0}, Y: {1}, bok: {2}", X, Y, Length);
        }

    }
}
