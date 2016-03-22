using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Zad9
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<Point> list = new List<Point>();
        Point lastPoint;
        bool pressed = false;
        Algorithm alg = null;

        public MainWindow()
        {
            InitializeComponent();
            //DrawLine(new Point(50, 50), new Point(100, 75), Globals.AlgorithmColor);
            //Point p = new Point(100, 100);
            //DrawCircle(p, Globals.AlgorithmColor);
            GetGlobals();
        }

        private void Canvas_MouseDown(object sender, MouseEventArgs e)
        {
            pressed = true;
            Point p = lastPoint = e.GetPosition(Canvas);
            list.Add(p);
            DrawCircle(p, Globals.UserColor);
        }

        private void Canvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (pressed == true)
            {
                Point p = e.GetPosition(Canvas);
                if (Globals.GapsEnabled == false || 
                    (Math.Abs(lastPoint.X - p.X) > 2
                    && Math.Abs(lastPoint.X - p.X) > 2))
                {
                    lastPoint = p;
                    list.Add(p);
                    DrawCircle(p, Brushes.Gray);
                }
            }
        }

        private void Canvas_MouseUp(object sender, MouseButtonEventArgs e)
        {
            pressed = false;
        }

        private void DrawCircle(Point p, Brush color)
        {
            var circle = new Ellipse();
            circle.Fill = color;
            circle.StrokeThickness = 0;
            circle.Width = 6;
            circle.Height = 6;
            circle.SetValue(Canvas.LeftProperty, p.X-3);
            circle.SetValue(Canvas.TopProperty, p.Y-3); 
            Canvas.Children.Add(circle);
        }

        private void DrawLine(Point point1, Point point2, Brush color)
        {
            var line = new Line();
            line.Stroke = color;
            line.StrokeThickness = 3;
            line.X1 = point1.X;
            line.Y1 = point1.Y;
            line.X2 = point2.X;
            line.Y2 = point2.Y;
            Canvas.Children.Add(line);
        }

        private void DeleteAlgorithmElements()
        {
            List<UIElement> list = new List<UIElement>();
            foreach (UIElement item in Canvas.Children)
            {
                var a = item as Line;
                if (a != null && (a.Stroke == Globals.AlgorithmColor))
                {
                    list.Add(item);
                    continue;
                }
                var b = item as Ellipse;
                if (b != null && (b.Fill == Globals.AlgorithmColor))
                {
                    list.Add(item);
                }
            }
            foreach (var aaa in list)
                Canvas.Children.Remove(aaa);
        }

        private void GetGlobals()
        {
            Lambda.Text = Globals.Lambda.ToString();
            Neurons.Text = Globals.NumberOfNeurons.ToString();
            Iterations.Text = Globals.NumberOfIterations.ToString();
        }

        private void UpdateGlobals()
        {
            Globals.Lambda = int.Parse(Lambda.Text);
            Globals.NumberOfNeurons = int.Parse(Neurons.Text);
            Globals.NumberOfIterations = int.Parse(Iterations.Text);
        }

        private void RunAlgorithm_Click(object sender, RoutedEventArgs e)
        {
            UpdateGlobals();
            Task.Run(() =>
            {
                alg = new Algorithm(ref list);

                for (int t = 0; t < alg.Iterations; ++t)
                {
                    int P = alg.RandomExample();
                    int v = alg.FindIndexOfNearestNeuron(alg.examples[P]);
                    alg.ApdejtujWagi(P, v, t);

                    if (t % 100 == 0)
                    {
                        Dispatcher.BeginInvoke(new Action(() =>
                        {
                            RefreshDisplay();
                        }));
                        System.Threading.Thread.Sleep(5);
                    }
                }
            });
        }

        private void RefreshDisplay()
        {
            DeleteAlgorithmElements();
            //foreach (Point item in alg.neurons)
            //    DrawCircle(item, Globals.AlgorithmColor);

            for (int i = 0; i < alg.neurons.Count(); ++i)
            {
                DrawCircle(alg.neurons[i], Globals.AlgorithmColor);
                if (i > 0)
                    DrawLine(alg.neurons[i - 1], alg.neurons[i], Globals.AlgorithmColor);
            }
        }

        private void ClearCanvas_Click(object sender, RoutedEventArgs e)
        {
            Canvas.Children.Clear();
            list.Clear();
        }
    }
}
