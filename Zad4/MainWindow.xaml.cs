using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Zad4.MLP;

namespace Zad4
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ViewModel vm;
        Algorithm alg;

        public MainWindow()
        {
            InitializeComponent();
            MouseMove += Window_MouseMove;
            vm = this.DataContext as ViewModel;
            alg = new Algorithm();
        }

        private void Window_MouseMove(object sender, MouseEventArgs e)
        {
            Point p = Mouse.GetPosition(Canvas);
            if (p.X >= 0 && p.X <= Globals.Cols
                && p.Y >= 0 && p.Y <= Globals.Rows)
            {
                Debug.WriteLine("Mouse position: X:{0}, Y:{1}", p.X, p.Y);
                MoveArms(ref p);
            }
        }

        private void MoveArms(ref Point p)
        {
            Arm arm = alg.GiveAnswer(ref p);
            vm.Elbow = arm.elbow;
            vm.Hand = arm.hand;
        }
    }
}
