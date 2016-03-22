using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

// rĘkA rObOtA   by Klaudia Augustyńska

namespace Zad_4_przerobione
{
    public static class Globals
    {
        public static int Cols = 300;
        public static int Rows = 400;

        public static Point PunktZaczepienia = new Point(0, Rows / 2);
        public static int DługośćRamienia = 100;

        public static Random Random = new Random();
        
        public static int[] IlośćJednostekNaWarstwę = { 4, 4, 2 };
        public static int IlośćPrzykładówUczących = 10000;
        public static int IlośćKrokówUczenia = 1000000;
    }
}
