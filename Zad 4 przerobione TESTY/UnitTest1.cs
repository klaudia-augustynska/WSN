using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Zad_4_przerobione;

namespace Zad_4_przerobione_TESTY
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void PoliczSumęWejściową_CzySięLiczyPoprawnie()
        {
            Jednostka jednostka = new Jednostka();
            double[] x = new double[] { 0.8, 0.3, 1 };
            jednostka.Wagi = new double[] { 0.5, -0.2, 0.3 };

            jednostka.PoliczSumęWejściową(x);

            Assert.AreEqual(0.64, jednostka.SumaWejściowa);
        }

        [TestMethod]
        public void PoliczWyjście_CzySięLiczyPoprawnie()
        {
            Jednostka jednostka = new Jednostka();
            double[] x = new double[] { 0.8, 0.3, 1 };
            jednostka.Wagi = new double[] { 0.5, -0.2, 0.3 };

            jednostka.PoliczSumęWejściową(x);
            jednostka.PoliczWyjście();

            Assert.AreEqual(0.655, Math.Round(jednostka.Wyjście,3));
        }

        [TestMethod]
        public void PrzebiegajWprzód_CzySiećDaPoprawnyWynik()
        {
            AlgorytmMLP alg = new AlgorytmMLP();
            alg.Warstwy.Add(new Jednostka[2]);
            alg.Warstwy[0][0] = new Jednostka();
            alg.Warstwy[0][0].Wagi = new double[] { 0.5, -0.2, 0.3 };
            alg.Warstwy[0][1] = new Jednostka();
            alg.Warstwy[0][1].Wagi = new double[] { 0.1, -0.2, -0.5 };
            alg.Warstwy.Add(new Jednostka[1]);
            alg.Warstwy[1][0] = new Jednostka();
            alg.Warstwy[1][0].Wagi = new double[] { -0.8, 0.1, -0.4 };
            double[] przykład = new double[] { 0.8, 0.3, 1 };

            alg.PrzebiegajWprzód(przykład);

            Assert.AreEqual(0.655, Math.Round(alg.Warstwy[0][0].Wyjście, 3));
            Assert.AreEqual(0.382, Math.Round(alg.Warstwy[0][1].Wyjście, 3));
            Assert.AreEqual(0.292, Math.Round(alg.Warstwy[1][0].Wyjście, 3));
        }

        [TestMethod]
        public void δ_DlaWarstwyWynikowej_CzyLiczyPoprawnie()
        {
            Jednostka jednostka = new Jednostka();
            jednostka.Wyjście = 0.292;
            double t = 0.8;

            jednostka.δ_DlaWarstwyWynikowej(t);

            Assert.AreEqual(-0.105, Math.Round(jednostka.δ, 3));
        }

        [TestMethod]
        public void Σ_δk_wkj_CzyLiczyPoprawnie()
        {
            AlgorytmMLP alg = new AlgorytmMLP();
            alg.Warstwy.Add(new Jednostka[2]);
            alg.Warstwy[0][0] = new Jednostka();
            alg.Warstwy[0][0].Wagi = new double[] { 0.5, -0.2, 0.3 };
            alg.Warstwy[0][0].Wyjście = 0.655;
            alg.Warstwy[0][1] = new Jednostka();
            alg.Warstwy[0][1].Wagi = new double[] { 0.1, -0.2, -0.5 };
            alg.Warstwy[0][1].Wyjście = 0.382;
            alg.Warstwy.Add(new Jednostka[1]);
            alg.Warstwy[1][0] = new Jednostka();
            alg.Warstwy[1][0].Wagi = new double[] { -0.8, 0.1, -0.4 };
            alg.Warstwy[1][0].Wyjście = 0.292;
            alg.Warstwy[1][0].δ = -0.105;

            var suma1 = alg.Σ_δk_wkj(0, 0);
            var suma2 = alg.Σ_δk_wkj(0, 1);

            Assert.AreEqual(-0.105*-0.8, suma1);
            Assert.AreEqual(-0.105*0.1, suma2);
        }

        [TestMethod]
        public void δ_DlaWarstwyUkrytej_CzyLiczyPoprawnie()
        {
            AlgorytmMLP alg = new AlgorytmMLP();
            alg.Warstwy.Add(new Jednostka[2]);
            alg.Warstwy[0][0] = new Jednostka();
            alg.Warstwy[0][0].Wagi = new double[] { 0.5, -0.2, 0.3 };
            alg.Warstwy[0][0].Wyjście = 0.655;
            alg.Warstwy[0][1] = new Jednostka();
            alg.Warstwy[0][1].Wagi = new double[] { 0.1, -0.2, -0.5 };
            alg.Warstwy[0][1].Wyjście = 0.382;
            alg.Warstwy.Add(new Jednostka[1]);
            alg.Warstwy[1][0] = new Jednostka();
            alg.Warstwy[1][0].Wagi = new double[] { -0.8, 0.1, -0.4 };
            alg.Warstwy[1][0].Wyjście = 0.292;
            alg.Warstwy[1][0].δ = -0.105;
            double suma1 = -0.105*-0.8;
            double suma2 = -0.105*0.1;

            alg.Warstwy[0][0].δ_DlaWarstwyUkrytej(suma1);
            alg.Warstwy[0][1].δ_DlaWarstwyUkrytej(suma2);

            Assert.AreEqual(0.019, Math.Round(alg.Warstwy[0][0].δ, 3));
            Assert.AreEqual(-0.002, Math.Round(alg.Warstwy[0][1].δ, 3));
        }

        [TestMethod]
        public void PrzebiegajDoTyłu_SiećLiczyPoprawnieDelty()
        {
            AlgorytmMLP alg = new AlgorytmMLP();
            alg.Warstwy.Add(new Jednostka[2]);
            alg.Warstwy[0][0] = new Jednostka();
            alg.Warstwy[0][0].Wagi = new double[] { 0.5, -0.2, 0.3 };
            alg.Warstwy[0][0].Wyjście = 0.655;
            alg.Warstwy[0][1] = new Jednostka();
            alg.Warstwy[0][1].Wagi = new double[] { 0.1, -0.2, -0.5 };
            alg.Warstwy[0][1].Wyjście = 0.382;
            alg.Warstwy.Add(new Jednostka[1]);
            alg.Warstwy[1][0] = new Jednostka();
            alg.Warstwy[1][0].Wagi = new double[] { -0.8, 0.1, -0.4 };
            alg.Warstwy[1][0].Wyjście = 0.292;
            double[] t = new double[] { 0.8 };

            alg.PrzebiegajDoTyłu(t);

            Assert.AreEqual(-0.105, Math.Round(alg.Warstwy[1][0].δ,3));
            Assert.AreEqual(0.019, Math.Round(alg.Warstwy[0][0].δ,3));
            Assert.AreEqual(-0.002, Math.Round(alg.Warstwy[0][1].δ,3));
        }

        [TestMethod]
        public void ZmieńWagi_RobiDobreWagi()
        {
            AlgorytmMLP alg = new AlgorytmMLP();
            alg.η = 0.1;
            alg.Warstwy.Add(new Jednostka[2]);
            alg.Warstwy[0][0] = new Jednostka();
            alg.Warstwy[0][0].Wagi = new double[] { 0.5, -0.2, 0.3 };
            alg.Warstwy[0][0].Wejścia = new double[] { 0.8, 0.3, 1 };
            alg.Warstwy[0][0].Wyjście = 0.655;
            alg.Warstwy[0][0].δ = 0.019;
            alg.Warstwy[0][1] = new Jednostka();
            alg.Warstwy[0][1].Wagi = new double[] { 0.1, -0.2, -0.5 };
            alg.Warstwy[0][1].Wejścia = new double[] { 0.8, 0.3, 1 };
            alg.Warstwy[0][1].Wyjście = 0.382;
            alg.Warstwy[0][1].δ = -0.002;
            alg.Warstwy.Add(new Jednostka[1]);
            alg.Warstwy[1][0] = new Jednostka();
            alg.Warstwy[1][0].Wagi = new double[] { -0.8, 0.1, -0.4 };
            alg.Warstwy[1][0].Wejścia = new double[] { 0.655, 0.382, 1 };
            alg.Warstwy[1][0].Wyjście = 0.292;
            alg.Warstwy[1][0].δ = -0.105;

            alg.ZmieńWagi();

            Assert.AreEqual(0.4985, Math.Round(alg.Warstwy[0][0].Wagi[0], 4));
            Assert.AreEqual(0.1002, Math.Round(alg.Warstwy[0][1].Wagi[0], 4));
            Assert.AreEqual(-0.2006, Math.Round(alg.Warstwy[0][0].Wagi[1], 4));
            Assert.AreEqual(-0.1999, Math.Round(alg.Warstwy[0][1].Wagi[1], 4));
            Assert.AreEqual(0.2981, Math.Round(alg.Warstwy[0][0].Wagi[2], 4));
            Assert.AreEqual(-0.4998, Math.Round(alg.Warstwy[0][1].Wagi[2], 4));
            Assert.AreEqual(-0.7931, Math.Round(alg.Warstwy[1][0].Wagi[0], 4));
            Assert.AreEqual(0.1040, Math.Round(alg.Warstwy[1][0].Wagi[1], 4));
            Assert.AreEqual(-0.3895, Math.Round(alg.Warstwy[1][0].Wagi[2], 4));
        }

        [TestMethod]
        public void WymyślPrzykładyUczące_WymyślaRóżneKąty()
        {
            Kąty GraniceKątów = new Kąty(0, 180);
            GraniceKątów = PrzykładUczący.NormalizujKąty(GraniceKątów);
            AlgorytmMLP alg = new AlgorytmMLP();
            int[] przedziały = new int[4] { 0,0,0,0 };
            double interwał = (GraniceKątów.Beta - GraniceKątów.Alfa) / 4;

            alg.WymyślPrzykładyUczące();

            foreach (var przykład in alg.przykłady)
            {
                if (przykład.Kąty.Alfa < interwał || przykład.Kąty.Beta < interwał)
                    przedziały[0]++;
                if (przykład.Kąty.Alfa < 2*interwał || przykład.Kąty.Beta < 2*interwał)
                    przedziały[1]++;
                if (przykład.Kąty.Alfa < 3 * interwał || przykład.Kąty.Beta < 3 * interwał)
                    przedziały[2]++;
                if (przykład.Kąty.Alfa < 4 * interwał || przykład.Kąty.Beta < 4 * interwał)
                    przedziały[3]++;
            }
        }

        [TestMethod]
        public void NormalizujKąty_NormalizujeDoPrzedziału()
        {
            Kąty kąty = new Kąty(0, 180);
            Kąty takiJakTrzeba = new Kąty(0.1, 0.9);

            kąty = PrzykładUczący.NormalizujKąty(kąty);

            Assert.AreEqual(takiJakTrzeba, kąty);
        }
    }
}
