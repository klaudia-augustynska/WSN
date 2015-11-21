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
using Zad2.ViewModels;
using Zad2.Models;
using Zad2.PocketLearningAlgorithm;

namespace Zad2.Views
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
            SampleManager.Instance.AddSampleToList(vm.CanvasSquareList);
            vm.SampleCount = SampleManager.Instance.Samples.Count;
            if (vm.SampleId == 0)
                vm.SampleId = 1;
            vm.CanvasSquareList = new SquareList();

            DetermineButtonsState();

            vm.AreChangesUnsaved = true;
        }

        private void NextImage_Click(object sender, RoutedEventArgs e)
        {
            if (vm.SampleId < vm.SampleCount)
                vm.SampleId++;
            vm.LoadImageToCanvas();
            DetermineButtonsState();
        }

        private void PreviousImage_Click(object sender, RoutedEventArgs e)
        {
            if (vm.SampleId > 1)
                vm.SampleId--;
            vm.LoadImageToCanvas();
            DetermineButtonsState();
        }

        private void DetermineButtonsState()
        {
            if (vm.SampleId <= 1)
                PreviousImage.IsEnabled = false;
            else
                PreviousImage.IsEnabled = true;
            if (vm.SampleId == vm.SampleCount)
                NextImage.IsEnabled = false;
            else
                NextImage.IsEnabled = true;
            if (vm.SampleCount >= 1)
                SaveAs.IsEnabled = RemoveFromCollection.IsEnabled = true;
            else
                SaveAs.IsEnabled = RemoveFromCollection.IsEnabled = false;
            if (vm.SampleCount >= 2)
                TeachProgram.IsEnabled = true;
            else
                TeachProgram.IsEnabled = false;

        }


        private void ClearCanvas_Click(object sender, RoutedEventArgs e)
        {
            vm.CanvasSquareList.ClearAll();
        }

        private void RemoveFromCollection_Click(object sender, RoutedEventArgs e)
        {
            vm.RemoveActualItemFromCollection();
            vm.AreChangesUnsaved = true;
            DetermineButtonsState();
        }

        private void SaveAs_Click(object sender, RoutedEventArgs e)
        {
            var saveFileDialog = new SaveFileDialog();
            if (saveFileDialog.ShowDialog() == true)
            {
                SampleManager.Instance.SaveToFile(saveFileDialog.FileName, vm.IsTaught);
                vm.AreChangesUnsaved = false;
                vm.FileName = saveFileDialog.FileName;
            }
                
        }

        private void ReadFile_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                SampleManager.Instance.ReadFromFile(openFileDialog.FileName);
                vm.FileName = openFileDialog.FileName;
                vm.LoadNewImageSquareList();
                DetermineButtonsState();
                AddToSamples.IsEnabled = false;
                RemoveFromCollection.IsEnabled = false;
                vm.LoadImageToCanvas();
            }
        }

        private void TeachProgram_Click(object sender, RoutedEventArgs e)
        {
            alg = new Algorithm(vm.FileName);
            RemoveNoise.IsEnabled = true;
            vm.IsTaught = true;
        }

        private void RemoveNoise_Click(object sender, RoutedEventArgs e)
        {
            vm.CanvasSquareList = alg.RemoveNoise(vm.CanvasSquareList);
        }
    }
}
