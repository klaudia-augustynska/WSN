using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zad_4_przerobione
{
    public class Jednostka
    {
        public double SumaWejściowa;
        public double[] Wagi = null;
        public double[] Wejścia = null;
        public double Wyjście;
        public double δ;
        

        public void PoliczSumęWejściową(double[] x)
        {
            // Ilość wag zależy od ilości wejść z poprzedniej warstwy,
            // więc inicjalizuję wagi przy pierwszym przebiegu sieci
            if (Wagi == null)
            {
                WygenerujMałeWagiPoczątkowe(x.Count());
            }
            // Skopiuj od razu wejścia na pamiątkę, żeby móc się do nich odwoływać później
            // we wzorach na "przebiegaj sieć w tył"
            Wejścia = new double[x.Count()];
            for (int i = 0; i < Wejścia.Count(); ++i)
                Wejścia[i] = x[i];

            SumaWejściowa = 0;
            for (int j = 0; j < x.Count(); ++j)
                SumaWejściowa += Wagi[j] * x[j];
        }

        private double Φ(double s)
        {
            return σ(s);
        }

        private double σ(double s)
        {
            return 1 /
                (1 + Math.Exp(-s));
        }

        private void WygenerujMałeWagiPoczątkowe(int ilośćWag)
        {
            Wagi = new double[ilośćWag];
            for (int i = 0; i < ilośćWag; ++i)
                Wagi[i] = Globals.Random.NextDouble()-0.5;
        }

        public void PoliczWyjście()
        {
            Wyjście = Φ(SumaWejściowa);
        }

        public void δ_DlaWarstwyWynikowej(double t)
        {
            double z = Wyjście;
            δ = (z - t) * z * (1 - z);
        }

        public void δ_DlaWarstwyUkrytej(double σ_δk_wkj)
        {
            double y = Wyjście;
            δ = σ_δk_wkj * y * (1 - y);
        }
    }
}
