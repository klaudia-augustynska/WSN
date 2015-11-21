using System;
using System.Collections.Generic;
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

namespace Zad2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly SquareManager _squareManager = SquareManager.Instance;

        private int previousId;
        private bool mousePressed = false;
        private PocketLearningAlgorithm _pocketLearningAlgorithm;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Canvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Point p = e.GetPosition((Canvas) sender);

            int id = previousId = _squareManager.DetermineSquareId(p);

            _squareManager.ToggleFilled(id);
            mousePressed = true;
            var vm = Window.DataContext as ViewModel;
            vm.Result = PocketLearningAlgorithm.Instance.Recognize(_squareManager.ToStringList());

        }
        private void Canvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (mousePressed)
            {
                Point p = e.GetPosition((Canvas)sender);
                int id = _squareManager.DetermineSquareId(p);
                if (id != previousId)
                {
                    previousId = id;
                    _squareManager.ToggleFilled(id);

                    var vm = Window.DataContext as ViewModel;
                    vm.Result = PocketLearningAlgorithm.Instance.Recognize(_squareManager.ToStringList());
                }
            }
        }


        private void Canvas_MouseUp(object sender, MouseButtonEventArgs e)
        {
            mousePressed = false;
        }

        private void ClearCanvasButton_Click(object sender, RoutedEventArgs e)
        {
            _squareManager.ClearAll();
            var vm = Window.DataContext as ViewModel;
            vm.Result = "";
        }

        async private void SaveExampleButton_Click(object sender, RoutedEventArgs e)
        {
            await SamplesRepository.Instance.SaveSamples(CharToLearn.Text);
        }

        private void TeachButton_Click(object sender, RoutedEventArgs e)
        {
            PocketLearningAlgorithm.Instance.TeachPerceptrons(100000, SamplesRepository.Instance.Samples);
        }
    }
}
