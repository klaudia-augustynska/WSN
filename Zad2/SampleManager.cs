using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Zad2.Common;
using Zad2.Models;

namespace Zad2
{
    /*
    Opis formatu pliku

m n
011001010... m*n razy
   
    */

    /// <summary>
    /// Klasa pośrednicząca między formatem danych dla GUI i do zapisu danych
    /// </summary>
    public class SampleManager
    {
        #region singleton
        private static SampleManager instance;
        private SampleManager()
        {
        }

        public static SampleManager Instance
        {
            get { return instance ?? (instance = new SampleManager()); }
        }
        #endregion

        #region fields
        private List<SquareList> _samples;
        #endregion

        #region properties
        public List<SquareList> Samples
        {
            get { return _samples ?? (_samples = new List<SquareList>()); }
            private set { _samples = value; }
        }
        #endregion

        #region public methods

        public void SaveToFile(string filename, bool isTaught)
        {
            using (var sw = new StreamWriter(filename))
            {
                string firstLine = Globals.Cols.ToString() + " " + Globals.Rows.ToString() + Samples.Count().ToString();
                sw.WriteLine(firstLine);

                foreach (var item in Samples)
                {
                    string nextLine = item.ToString();
                    sw.WriteLine(nextLine);
                }
            }
        }

        public void ReadFromFile(string filename)
        {
            try
            {
                using (var sr = new StreamReader(filename))
                {

                    string firstLine = sr.ReadLine();
                    string[] colsAndRows = firstLine.Split(new char[] { ' ' });
                    string cols = colsAndRows[0];
                    string rows = colsAndRows[1];

                    if (Int32.Parse(cols) != Globals.Cols || Int32.Parse(rows) != Globals.Rows)
                        throw new Exception("Zły plik");

                    this.Samples.Clear();

                    string nextLine;
                    while ((nextLine = sr.ReadLine()) != null)
                    {
                        SquareList list = new SquareList();
                        for (int i = 0; i < nextLine.Count(); ++i)
                        {
                            if (nextLine[i] == '1')
                                list[i].IsFilled = true;
                        }
                        Samples.Add(list);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void AddSampleToList(SquareList sample)
        {
            Samples.Add(sample);
        }
        #endregion
    }

}
