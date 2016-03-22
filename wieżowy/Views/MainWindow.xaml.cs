using Microsoft.Win32;
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
using pawelek.ViewModels;
using pawelek.Models;
using pawelek.Adaline;

namespace pawelek.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private int previousId;
        private bool mousePressed = false;
        ViewModel vm;
        Algorithm alg;
        private string fileName;

        public MainWindow()
        {
            InitializeComponent();
            vm = Window.DataContext as ViewModel;
        }

        private void Canvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Point p = e.GetPosition((Canvas) sender);


            int id = previousId = vm.CanvasSquareList.DetermineSquareId(p);

            vm.CanvasSquareList.ToggleFilled(id);
            mousePressed = true;

        }
        private void Canvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (mousePressed)
            {
                Point p = e.GetPosition((Canvas)sender);
                int id = vm.CanvasSquareList.DetermineSquareId(p);
                if (id != previousId)
                {
                    previousId = id;
                    vm.CanvasSquareList.ToggleFilled(id);
                }
            }
        }

        private void Canvas_MouseUp(object sender, MouseButtonEventArgs e)
        {
            mousePressed = false;
        }

        private void AddToSamples_Click(object sender, RoutedEventArgs e)
        {
            SampleManager.Instance.AddSampleToList(vm.CanvasSquareList, (int) NumberToAdd.SelectedItem);
            vm.CanvasSquareList = new SquareList();
        }

        private void ClearCanvas_Click(object sender, RoutedEventArgs e)
        {
            vm.CanvasSquareList.ClearAll();
        }

        private void SaveAs_Click(object sender, RoutedEventArgs e)
        {
            var saveFileDialog = new SaveFileDialog();
            if (saveFileDialog.ShowDialog() == true)
            {
                SampleManager.Instance.SaveToFile(saveFileDialog.FileName);
            }

        }

        private void ReadFile_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                fileName = openFileDialog.FileName;
                TeachProgram.IsEnabled = true;
            }
        }

        private void TeachProgram_Click(object sender, RoutedEventArgs e)
        {
            alg = new Algorithm(fileName);
            Recognize.IsEnabled = true;
            ItIs.IsEnabled = true;
            ItIsNot.IsEnabled = true;
        }

        private void Recognize_Click(object sender, RoutedEventArgs e)
        {
            vm.Number = alg.Recognize(vm.CanvasSquareList);
        }

        private void MoveUp_Click(object sender, RoutedEventArgs e)
        {
            vm.MoveUp();
        }

        private void MoveDown_Click(object sender, RoutedEventArgs e)
        {
            vm.MoveDown();
        }

        private void MoveLeft_Click(object sender, RoutedEventArgs e)
        {
            vm.MoveLeft();
        }

        private void MoveRight_Click(object sender, RoutedEventArgs e)
        {
            vm.MoveRight();
        }

        private void ItIs_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = (ComboBox)sender;
            int value = (int) comboBox.SelectedItem;

            // naucz (value) że ten przykład jest prawdziwy (1)
            alg.CorrectWeights(vm.CanvasSquareList, value, 1);

            // naucz (złą odpowiedź), że ten przykład to nie jest on (-1)
            //alg.CorrectWeights(vm.CanvasSquareList, vm.Number, -1);
            
            vm.Number = alg.Recognize(vm.CanvasSquareList);
        }

        private void ItIsNot_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = (ComboBox)sender;
            int value = (int)comboBox.SelectedItem;

            // naucz (value), że ten przykład jest zły (-1)
            alg.CorrectWeights(vm.CanvasSquareList, value, -1);
            vm.Number = alg.Recognize(vm.CanvasSquareList);
        }

        private void CorrectAgain_Click(object sender, RoutedEventArgs e)
        {
            if (ItIs.SelectedIndex != -1)
            {
                // naucz wartość z ItIs że to jest dla niej prawidłowy przykład
                alg.CorrectWeights(vm.CanvasSquareList, (int)ItIs.SelectedValue, 1);
            }
            if (ItIsNot.SelectedIndex != -1)
            { 
                // naucz wartość z ItItNot że to jest dla niej zły przykład
                alg.CorrectWeights(vm.CanvasSquareList, (int)ItIsNot.SelectedValue, -1);

                vm.Number = alg.Recognize(vm.CanvasSquareList);
            }
        }
    }
}
