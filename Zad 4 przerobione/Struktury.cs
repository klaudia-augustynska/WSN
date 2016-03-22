using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

// rĘkA rObOtA   by Klaudia Augustyńska

namespace Zad_4_przerobione
{
    public struct RękaRobota
    {
        public static Point Bark = Globals.PunktZaczepienia;
        public Point Łokieć;
        public Point Dłoń;

        public static RękaRobota ZamieńKątyNaRękęRobota(Kąty kąty)
        {
            RękaRobota arm = new RękaRobota();

            kąty.Alfa = kąty.Alfa * Math.PI / 180;
            kąty.Beta = kąty.Beta * Math.PI / 180;
            kąty.Alfa -= Math.PI / 2;
            kąty.Beta = kąty.Alfa + kąty.Beta - Math.PI;
            
            arm.Łokieć = new Point((int)Math.Round(Bark.X + Math.Cos(kąty.Alfa) * Globals.DługośćRamienia), (int)Math.Round(Bark.Y + Math.Sin(kąty.Alfa) * Globals.DługośćRamienia));
            arm.Dłoń = new Point((int)Math.Round(arm.Łokieć.X + Math.Cos(kąty.Beta) * Globals.DługośćRamienia), (int)Math.Round(arm.Łokieć.Y + Math.Sin(kąty.Beta) * Globals.DługośćRamienia));

            return arm;
        }
    }

    public struct Kąty
    {
        public double Alfa;
        public double Beta;
        public Kąty(double a, double b)
        {
            Alfa = a;
            Beta = b;
        }
    }

    public class PrzykładUczący
    {
        public Point Punkt;
        public Kąty Kąty;
        static private double WspółczynnikSkalowaniaX = (0.9 - 0.1) / Globals.Cols;
        static private double WspółczynnikSkalowaniaY = (0.9 - 0.1) / Globals.Rows;
        static private double WspółczynnikSkalowaniaKąta = (0.9 - 0.1) / 180;
        public PrzykładUczący()
        {
            Kąty = new Kąty(Globals.Random.NextDouble() * 180, Globals.Random.NextDouble() * 180);
            RękaRobota ręka = RękaRobota.ZamieńKątyNaRękęRobota(Kąty);
            Punkt = ręka.Dłoń;
            Punkt = NormalizujPunkt(Punkt);
            Kąty = NormalizujKąty(Kąty);
        }

        public static Point NormalizujPunkt(Point p)
        {
            // Rzutowanie z przedziału  0   .. Cols
            // na przedział             0.1 .. 0.9
            p.X = p.X * WspółczynnikSkalowaniaX + 0.1;
            p.Y = p.Y * WspółczynnikSkalowaniaY + 0.1;
            return p;
        }

        public static Kąty NormalizujKąty(Kąty a)
        {
            // Rzutowanie z przedziału  0   .. 180
            // na przedział             0.1 .. 0.9
            a.Alfa = a.Alfa * WspółczynnikSkalowaniaKąta + 0.1;
            a.Beta = a.Beta * WspółczynnikSkalowaniaKąta + 0.1;
            return a;
        }
        public static Point DenormalizujPunkt(Point p)
        {
            // Rzutowanie z przedziału  0.1 .. 0.9
            // na przedział             0   .. Cols
            p.X = (p.X - 0.1) / WspółczynnikSkalowaniaX;
            p.Y = (p.Y - 0.1) / WspółczynnikSkalowaniaY;
            return p;
        }

        public static Kąty DenormalizujKąty(Kąty a)
        {
            // Rzutowanie z przedziału  0.1 .. 0.9
            // na przedział             0   .. 180
            a.Alfa = (a.Alfa - 0.1) / WspółczynnikSkalowaniaKąta;
            a.Beta = (a.Beta - 0.1) / WspółczynnikSkalowaniaKąta;
            return a;
        }
    }
}
