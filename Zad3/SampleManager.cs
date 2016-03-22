using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Zad3.Common;
using Zad3.Models;

namespace Zad3
{
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
        private List<SampleGroup> _samples;
        #endregion

        #region properties
        public List<SampleGroup> Samples
        {
            get { return _samples ?? (_samples = new List<SampleGroup>()); }
            private set { _samples = value; }
        }
        #endregion

        #region public methods

        public void SaveToFile(string filename)
        {
            using (var sw = new StreamWriter(filename))
            {
                int max = 0;
                foreach (var sampleGroup in Samples)
                    if (sampleGroup.List.Count() > max)
                        max = sampleGroup.List.Count();

                string firstLine = Globals.Cols.ToString() + " " + Globals.Rows.ToString();
                sw.WriteLine(firstLine);


                foreach (var sampleGroup in Samples)
                {
                    sw.WriteLine("%");

                    string number = sampleGroup.Number.ToString();
                    sw.WriteLine(number);

                    for (int i = 0; i < sampleGroup.List.Count(); ++i)
                    {
                        string nextLine = sampleGroup.List[i].ToString();
                        sw.WriteLine(nextLine);
                    }
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
                    string[] firstLineData = firstLine.Split(new char[] { ' ' });
                    string cols = firstLineData[0];
                    string rows = firstLineData[1];
                    string perceptrons = firstLineData[2];

                    if (Int32.Parse(cols) != Globals.Cols || Int32.Parse(rows) != Globals.Rows)
                        throw new Exception("Zły plik");

                    this.Samples.Clear();

                    string nextLine;
                    SampleGroup group = new SampleGroup();
                    bool firstTime = true;
                    while ((nextLine = sr.ReadLine()) != null)
                    {
                        if (nextLine[0] == '%')
                        {
                            if (firstTime)
                                firstTime = false;
                            else
                                Samples.Add(group);
                            group = new SampleGroup();
                            nextLine = sr.ReadLine();
                            group.Number = int.Parse(nextLine);
                            group.List = new List<SquareList>();
                        }
                        else
                        {
                            SquareList list = new SquareList();
                            for (int i = 0; i < nextLine.Count(); ++i)
                            {
                                if (nextLine[i] == '1')
                                    list[i].IsFilled = true;
                            }
                            group.List.Add(list);
                        }
                        
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void AddSampleToList(SquareList sample, int number)
        {
            SampleGroup item = Samples.Find(s => s.Number == number);
            
            if (item == null)
            {
                var group = new SampleGroup();
                group.Number = number;
                group.List = new List<SquareList>();
                group.List.Add(sample);
                Samples.Add(group);
            }
            else
            {
                item.List.Add(sample);
            }
        }
        #endregion
    }

}
