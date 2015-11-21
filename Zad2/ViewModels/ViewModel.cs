using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zad2.Common;
using Zad2.Models;

namespace Zad2.ViewModels
{
    /// <summary>
    /// Klasa ze wszystkimi zmiennymi do bindingowania w GUI
    /// </summary>
    public class ViewModel : ObservableObject
    {
        #region fields
        private SquareList _canvasList = new SquareList();
        private List<SquareList> _imageList = SampleManager.Instance.Samples;
        private bool _isTaught = false;
        private bool _canReduceNoise = false;
        private bool _areChangesUnsaved = false;
        private string _filename = "[bez nazwy]";
        private int _sampleId = 0;
        private int _sampleCount = 0;
        #endregion

        #region properties

        public SquareList CanvasSquareList
        {
            get { return _canvasList; }
            set {
                _canvasList = value;
                NotifyPropertyChanged("CanvasSquareList");
            }
        }

        public SquareList ImageSquareList
        {
            get {
                if (_sampleCount > 0 && _sampleId - 1 >= 0 && _sampleId - 1 < _sampleCount)
                    return _imageList[_sampleId-1];
                return null;
            }
            set {
                _imageList[_sampleId-1] = value;
            }
        }

        public bool IsTaught { get { return _isTaught; } set { _isTaught = value; NotifyPropertyChanged("IsTaught"); } }
        public bool CanReduceNoise { get { return _canReduceNoise; } set { _canReduceNoise = value; NotifyPropertyChanged("CanReduceNoise"); } }
        public bool AreChangesUnsaved { get { return _areChangesUnsaved; } set { _areChangesUnsaved = value; NotifyPropertyChanged("AreChangesUnsaved"); } }
        public string FileName { get { return _filename; } set { _filename = value; NotifyPropertyChanged("FileName"); } }
        public int SampleId { get { return _sampleId; } set { _sampleId = value;
                NotifyPropertyChanged("SampleId");
                NotifyPropertyChanged("ImageSquareList");
            } }
        public int SampleCount { get { return _sampleCount; } set { _sampleCount = value; NotifyPropertyChanged("SampleCount"); } }
        public int ParentWidth { get { return Globals.ParentWidth; } }
        public int ParentHeight { get { return Globals.ParentHeight; } }

        #endregion

        public void LoadNewImageSquareList()
        {
            NotifyPropertyChanged("ImageSquareList");
            SampleCount = _imageList.Count();
            SampleId = 1;
            AreChangesUnsaved = false;
        }

        public void RemoveActualItemFromCollection()
        {
            if (SampleCount > 0)
            {
                _imageList.Remove(ImageSquareList);

                /*
                ta konstrukcja jest po to żeby jak się pokazuje np. "1/2" (że zdjęcie 1 z 2)
                to żeby po usunięciu jakiegoś się odpowiednie cyferki pokazywały
                nie jestem dumna z tej konstrukcji, ale zdobywam cenne doświadczenie
                i nastęnym razem zaprojektuję program jakoś tak by był ładniej niż to...
                */

                if (SampleId == 1 && SampleCount == 1)
                    SampleId = SampleCount = 0;
                else
                {
                    SampleCount--;
                    if (SampleId > 1)
                        SampleId--;
                    else
                        NotifyPropertyChanged("ImageSquareList");
                }
            }
        }

        public void LoadImageToCanvas()
        {
            for (int i = 0; i < ImageSquareList.Count(); ++i)
            {
                if (ImageSquareList[i].IsFilled)
                    CanvasSquareList[i].IsFilled = true;
                else
                    CanvasSquareList[i].IsFilled = false;
            }
        }
    }
}
