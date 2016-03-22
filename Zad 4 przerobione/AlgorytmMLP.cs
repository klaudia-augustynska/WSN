using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

// rĘkA rObOtA   by Klaudia Augustyńska

namespace Zad_4_przerobione
{
    public class AlgorytmMLP
    {
        public PrzykładUczący[] przykłady;
        public double η = 0.5;
        public List<Jednostka[]> Warstwy = new List<Jednostka[]>();

        public AlgorytmMLP()
        {
        }

        public void InicjalizujWarstwy()
        {
            for (int k = 0; k < Globals.IlośćJednostekNaWarstwę.Count(); ++k)
            {
                Warstwy.Add(new Jednostka[Globals.IlośćJednostekNaWarstwę[k]]);
                for (int j = 0; j < Warstwy[k].Count(); ++j)
                {
                    Warstwy[k][j] = new Jednostka();
                }
            }
        }

        public void UczSię()
        {
            for (int i = 0; i < Globals.IlośćKrokówUczenia; ++i)
            {
                PrzykładUczący przykład = LosujPrzykład();

                double[] x = new double[] { przykład.Punkt.X, przykład.Punkt.Y, 1 };
                PrzebiegajWprzód(x);

                double[] t = new double[] { przykład.Kąty.Alfa, przykład.Kąty.Beta };
                PrzebiegajDoTyłu(t);

                ZmieńWagi();
            }
        }

        public void ZmieńWagi()
        {
            // Dla każdej warstwy
            for (int k = 0; k < Warstwy.Count; ++k)
                // Dla każdej jednostki w warstwie
                for (int j = 0; j < Warstwy[k].Count(); ++j)
                    // Dla każdej wagi w jednostce
                    for (int w = 0; w < Warstwy[k][j].Wagi.Count(); ++w)
                        Warstwy[k][j].Wagi[w] -= η * Warstwy[k][j].δ * Warstwy[k][j].Wejścia[w];
        }

        internal RękaRobota DajOdpowiedź(Point p)
        {
            var znormalizowany = PrzykładUczący.NormalizujPunkt(p);
            //Debug.WriteLine("punkt na wejściu: {0}, {1}", p.X, p.Y);
            int Ostatnia = Warstwy.Count - 1;

            double[] x = new double[] { znormalizowany.X, znormalizowany.Y, 1 };
            PrzebiegajWprzód(x);

            Kąty kąty = new Kąty(Warstwy[Ostatnia][0].Wyjście, Warstwy[Ostatnia][1].Wyjście);
            Debug.WriteLine("wynik: {0}, {1}", kąty.Alfa.ToString(), kąty.Beta.ToString());
            var zdenormalizowane = PrzykładUczący.DenormalizujKąty(kąty);
            //Debug.WriteLine("wynik: {0}, {1}", kąty.Alfa.ToString(), kąty.Beta.ToString());
            return RękaRobota.ZamieńKątyNaRękęRobota(zdenormalizowane);
        }

        public void PrzebiegajWprzód(double[] x)
        {
            
            for (int k = 0; k < Warstwy.Count; ++k)
            {
                for (int j = 0; j < Warstwy[k].Count(); ++j)
                {
                    Warstwy[k][j].PoliczSumęWejściową(x);
                    Warstwy[k][j].PoliczWyjście();
                }

                x = new double[Warstwy[k].Count()+1];
                for (int j = 0; j < Warstwy[k].Count(); ++j)
                    x[j] = Warstwy[k][j].Wyjście;
                x[Warstwy[k].Count()] = 1;
            }
        }

        public void PrzebiegajDoTyłu(double[] t)
        {
            int NumerWarstwyWynikowej = Warstwy.Count - 1;
            int NumerOstatniejWarstwyUkrytej = Warstwy.Count - 2;

            // Inicjalizuj δ dla warstwy wynikowej
            for (int k = 0; k < t.Count(); ++k)
                Warstwy[NumerWarstwyWynikowej][k].δ_DlaWarstwyWynikowej(t[k]);

            // Przebiegaj warstwy niżej
            for (int k = NumerWarstwyWynikowej-1; k >= 0; --k)
            {
                // Dla każdej jednostki
                for (int j = 0; j < Warstwy[k].Count(); ++j)
                {
                    // Policz sumę potrzebną do wzoru na δ dla warstw ukrytych
                    double suma = Σ_δk_wkj(k,j);
                    Warstwy[k][j].δ_DlaWarstwyUkrytej(suma);
                }
            }

        }

        public double Σ_δk_wkj(int k, int j)
        {
            double Σ_δk_wkj = 0;
            for (int i = 0; i < Warstwy[k + 1].Count(); ++i)
                Σ_δk_wkj += Warstwy[k + 1][i].δ * Warstwy[k+1][i].Wagi[j];
            return Σ_δk_wkj;
        }

        private PrzykładUczący LosujPrzykład()
        {
            return przykłady[Globals.Random.Next(0, przykłady.Length)];
        }

        public void WymyślPrzykładyUczące()
        {
            przykłady = new PrzykładUczący[Globals.IlośćPrzykładówUczących];
            for (int i = 0; i < przykłady.Length; ++i)
                przykłady[i] = new PrzykładUczący();
        }
    }
}
