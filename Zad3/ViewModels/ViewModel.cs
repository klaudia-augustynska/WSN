using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zad3.Common;
using Zad3.Models;

namespace Zad3.ViewModels
{
    /// <summary>
    /// Klasa ze wszystkimi zmiennymi do bindingowania w GUI
    /// </summary>
    public class ViewModel : ObservableObject
    {
        #region fields
        private SquareList _canvasList = new SquareList();
        private List<int> _numbers = new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
        private int _number = -1;
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

        public int ParentWidth { get { return Globals.ParentWidth; } }
        public int ParentHeight { get { return Globals.ParentHeight; } }
        public List<int> Numbers { get { return _numbers; } }
        public int Number { get { return _number; } set { _number = value; NotifyPropertyChanged("Number"); } }
        public int ButtonLength { get { return 30; } }

        #endregion
        

        public void MoveUp()
        {
            int limit = CanvasSquareList.Count() - Globals.Cols;
            for (int i = 0; i < limit; ++i)
                CanvasSquareList[i].IsFilled = CanvasSquareList[i + Globals.Cols].IsFilled;
            for (int i = limit; i < CanvasSquareList.Count(); ++i)
                CanvasSquareList[i].IsFilled = false;
        }
        public void MoveDown()
        {
            int limit = Globals.Cols;
            for (int i = CanvasSquareList.Count()-1; i >= limit; --i)
                CanvasSquareList[i].IsFilled = CanvasSquareList[i - Globals.Cols].IsFilled;
            for (int i = limit-1; i >= 0; --i)
                CanvasSquareList[i].IsFilled = false;

        }
        public void MoveLeft()
        {
            for (int r = 0; r < Globals.Rows; ++r)
            {
                for (int c = 0; c < Globals.Cols; ++c)
                {
                    var index = GetIndex(r, c);
                    if (IsLastInRow(c))
                    {
                        CanvasSquareList[index].IsFilled = false;
                    }
                    else
                    {
                        CanvasSquareList[index].IsFilled = CanvasSquareList[index+1].IsFilled;
                    }
                }
            }
        }
        public void MoveRight()
        {
            for (int r = 0; r < Globals.Rows; ++r)
            {
                for (int c = Globals.Cols - 1; c >= 0; --c)
                {
                    var index = GetIndex(r, c);
                    if (IsFirstInRow(c))
                    {
                        CanvasSquareList[index].IsFilled = false;
                    }
                    else
                    {
                        CanvasSquareList[index].IsFilled = CanvasSquareList[index - 1].IsFilled;
                    }
                }
            }
        }
        private bool IsLastInRow(int c)
        {
            if (c == Globals.Cols - 1)
                return true;
            return false;
        }

        private bool IsFirstInRow(int c)
        {
            if (c == 0)
                return true;
            return false;
        }
        private int GetIndex(int r, int c)
        {
            return r * Globals.Cols + c;
        }
    }
}
