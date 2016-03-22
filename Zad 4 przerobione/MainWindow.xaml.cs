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

// rĘkA rObOtA   by Klaudia Augustyńska

namespace Zad_4_przerobione
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ViewModel vm;
        AlgorytmMLP alg;
        Point poprzedniPunkt;

        public MainWindow()
        {
            InitializeComponent();
            MouseMove += Window_MouseMove;
            vm = this.DataContext as ViewModel;
            alg = new AlgorytmMLP();
            alg.InicjalizujWarstwy();
            alg.WymyślPrzykładyUczące();
            alg.UczSię();
        }

        private void Window_MouseMove(object sender, MouseEventArgs e)
        {
            Point p = Mouse.GetPosition(Canvas);
            if (poprzedniPunkt == null)
                poprzedniPunkt = p;
            if (p.X >= 0 && p.X <= Globals.Cols
                && p.Y >= 0 && p.Y <= Globals.Rows)
            {
                if (Math.Abs(poprzedniPunkt.X - p.X) > 4
                    && Math.Abs(poprzedniPunkt.Y - p.Y) > 4)
                {
                    //Debug.WriteLine("Mouse position: X:{0}, Y:{1}", p.X, p.Y);
                    MoveArms(p);
                }
            }
        }

        private void MoveArms(Point p)
        {
            RękaRobota ręka = alg.DajOdpowiedź(p);
            //Debug.WriteLine("ręka: {0}, {1}, {2}, {3}", ręka.Łokieć.X, ręka.Łokieć.Y, ręka.Dłoń.X, ręka.Dłoń.Y);
            vm.Łokieć = ręka.Łokieć;
            vm.Dłoń = ręka.Dłoń;
        }
    }
}
