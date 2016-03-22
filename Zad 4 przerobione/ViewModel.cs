using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

// rĘkA rObOtA   by Klaudia Augustyńska

namespace Zad_4_przerobione
{
    class ViewModel : INotifyPropertyChanged
    {
        private Point _elbow = new Point(Globals.DługośćRamienia, Globals.PunktZaczepienia.Y);
        private Point _hand = new Point(Globals.DługośćRamienia * 2, Globals.PunktZaczepienia.Y);

        public int ParentWidth { get { return Globals.Cols; } }
        public int ParentHeight { get { return Globals.Rows; } }
        public Point PunktZaczepienia { get { return Globals.PunktZaczepienia; } }
        public Point Łokieć
        {
            get { return _elbow; }
            set { _elbow = value; NotifyPropertyChanged("Łokieć"); }
        }
        public Point Dłoń
        {
            get { return _hand; }
            set { _hand = value; NotifyPropertyChanged("Dłoń"); }
        }


        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this,
                    new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
