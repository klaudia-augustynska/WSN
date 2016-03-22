using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zad2
{
    /// <summary>
    /// Stores data to be displayed in program's GUI.
    /// </summary>
    public class ViewModel : ObservableObject
    {
        private SquareManager _squareManager = SquareManager.Instance;

        public List<Square> SquareList
        {
            get { return _squareManager.SquareList; }
            set { _squareManager.SquareList = value; }
        }

        private string result = "Na razie nic";

        public string Result
        {
            get { return result; }
            set
            {
                if (value == "")
                    result = "Na razie nic";
                else
                    result = value;
                NotifyPropertyChanged("Result");
            }
        }
        
    }
}
