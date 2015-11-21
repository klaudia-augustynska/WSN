using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zad2.Common
{
    /// <summary>
    /// Zmienne widoczne w całym projekcie, bo w C# się nie da inaczej
    /// </summary>
    static public class Globals
    {
        static public int ParentWidth = 300;
        static public int Cols = 30;
        static public int Rows = 30;
        static public int Length = ParentWidth / Cols;
        static public int ParentHeight = Rows*Length;
        static public Random Random = new Random();
    }
}
