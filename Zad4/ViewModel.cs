using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Zad4
{
    class ViewModel : INotifyPropertyChanged
    {
        private Point _elbow = new Point(Globals.ArmLength, Globals.MountPoint.Y);
        private Point _hand = new Point(Globals.ArmLength*2, Globals.MountPoint.Y);

        public int ParentWidth { get { return Globals.Cols; } }
        public int ParentHeight { get { return Globals.Rows; } }
        public Point MountPoint { get { return Globals.MountPoint; } }
        public Point Elbow
        {
            get { return _elbow; }
            set { _elbow = value; NotifyPropertyChanged("Elbow"); }
        }
        public Point Hand
        {
            get { return _hand; }
            set { _hand = value; NotifyPropertyChanged("Hand"); }
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
