using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Zad2
{
    /// <summary>
    /// Represents a JSON group that assigns a collection of samples to a given name (like "1", "2" etc.)
    /// </summary>
    public class SampleGroup
    {
        public string Name { get; set; }
        public List<List<string>> Samples { get; set; }
    }

    /// <summary>
    /// Singleton that manages actions of loading and saving JSON samples stored in a text file.
    /// </summary>
    public class SamplesRepository
    {
        private static SamplesRepository instance;

        private List<SampleGroup> samples;
        private string filename = "samples.txt";

        public List<SampleGroup> Samples
        {
            get { return samples ?? (samples = new List<SampleGroup>()); }
            private set { samples = value; }
        }

        private SamplesRepository()
        {
            LoadSamplesFromFile();
        }

        public static SamplesRepository Instance
        {
            get { return instance ?? (instance = new SamplesRepository()); }
        }


        private void LoadSamplesFromFile()
        {
            if (File.Exists(filename))
                using (var sr = new StreamReader(filename))
                {
                    string str = sr.ReadToEnd();
                    Samples = JsonConvert.DeserializeObject<List<SampleGroup>>(str);
                }
        }

        async public Task SaveSamples(string charToLearn)
        {
            await Task.Run(() =>
            {
                var LockingVar = new object();

                Predicate<SampleGroup> pre = delegate(SampleGroup a) { return a.Name.Equals(charToLearn, StringComparison.Ordinal); };
                var id = Samples.FindIndex(pre);

                lock (LockingVar)
                {

                    if (id >= 0)
                    {
                        Samples[id].Samples.Add(SquareManager.Instance.ToStringList());
                    }
                    else
                    {
                        var list = new List<List<string>>();
                        list.Add(SquareManager.Instance.ToStringList());

                        Samples.Add(new SampleGroup
                        {
                            Name = charToLearn,
                            Samples = list
                        });
                    }

                    using (var sw = new StreamWriter(filename))
                    {
                        string json;
                        json = JsonConvert.SerializeObject(Samples);
                        sw.Write(json);
                    }
                }
            });
        }
    }
}
