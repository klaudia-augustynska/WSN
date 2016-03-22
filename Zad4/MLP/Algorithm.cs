using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Zad4.MLP
{
    public class Algorithm
    {
        private string fileName = "weights";
        private double learningRate = 0.1;
        private int file = 1000;
        private Perceptron[][] perceptrons;
        private Example[] examples;

        public Algorithm()
        {
            InitializePerceptrons();
            if (File.Exists(fileName))
            {
                LoadWeightsFromFile();
            }
            else
            {
                Learn();
    //            SaveNewWeights();
            }
        }

        private void SaveNewWeights()
        {
            using (var sw = new StreamWriter(fileName))
            {
                for (int i = 0; i < perceptrons.Length; i++)
                {
                    for (int j = 0; j < perceptrons[i].Length; j++)
                    {
                        StringBuilder sb = new StringBuilder();
                        for (int k = 0; k < perceptrons[i][j].weights.Count; k++)
                        {
                            sb.Append(perceptrons[i][j].weights[k]);
                            sb.Append(' ');
                        }
                        sb.Append(perceptrons[i][j].bias);
                        sw.WriteLine(sb.ToString());
                    }
                }
            }
        }

        public Arm GiveAnswer(ref Point p)
        {
            NormalizePoint(ref p);
            Example t = new Example(p);
            GoForward(t);
            Angles angles = GetAngles();
            DenormalizeAngles(ref angles);
            Debug.WriteLine("alfa: {0}, beta: {1}", angles.alpha,angles.beta);
            return GetArmByAngles(ref angles);
        }

        private void InitializePerceptrons()
        {
            perceptrons = new Perceptron[Globals.LayerCount][];
            for (int i = 0; i < Globals.LayerCount; ++i)
            {
                perceptrons[i] = new Perceptron[Globals.Dimensions[i]];
                for (int j = 0; j < Globals.Dimensions[i]; ++j)
                    perceptrons[i][j] = new Perceptron(ref learningRate);
            }
        }

        private void Learn()
        {

            GenerateLearningExamples();
            int interval = Globals.Interval;
            file = 0;
            double ERROR = double.MaxValue;
            double minimum = double.MaxValue;

            for (int i = 0; i < Globals.LearningSteps; ++i)
            {
                Example t = GetRandomExample();
                GoForward(t);
                GoBack(t);
                UpdateWeights();
                
                if (i % interval == 0)
                {
                    ERROR = Error();
                    Debug.WriteLine("minimum: " + minimum);

                    if (ERROR < minimum)
                        minimum = ERROR;
                }
            }

            //Debug.WriteLine("ERROR: " + ERROR);
            //if (minimum > 200)
            //{
            //    Learn();
            //}
            
        }

        private double Error()
        {
            double err = 0;

            for (int n = 0; n < examples.Length; n++)
                err += E(n);

            return err * 0.5;
        }

        private double E(int n)
        {
            double result = 0;
            Angles t = examples[n].angles;
            int lastLayerIndex = perceptrons.Length - 1;

            GoForward(examples[n]);
            result += (Math.Pow(perceptrons[lastLayerIndex][0].sigmaX - t.alpha, 2));
            result += (Math.Pow(perceptrons[lastLayerIndex][1].sigmaX - t.beta, 2));

            return result;
        }

        private void UpdateWeights()
        {
            for (int i = 0; i < perceptrons.Length; i++)
            {
                for (int j = 0; j < perceptrons[i].Length; j++)
                {
                    perceptrons[i][j].UpdateWeights();
                }
            }
        }

        private void GoForward(Example t)
        {
            double[] input = { t.point.X, t.point.Y };
            for (int i = 0; i < perceptrons.Length; i++)
            {
                for (int j = 0; j < perceptrons[i].Length; j++)
                {
                    perceptrons[i][j].CalculateSigmaX(input);
                }

                // Debug.WriteLine("len:{0}", input.Length);
                SetInitialValueForSigmaX(ref input, t, i);
            }
        }
        
        private void GoBack(Example t)
        {
            double[] output = new double[] { t.angles.alpha, t.angles.beta };
            int lastLayerIndex = perceptrons.Length - 1;
            int secondLastLayerIndex = perceptrons.Length - 2;
            double tmp;

            for (int i = 0; i < perceptrons[lastLayerIndex].Length; ++i)
                perceptrons[lastLayerIndex][i].CalculateDelta(output[i]);

            output = new double[perceptrons[lastLayerIndex].Length];
            for (int i = 0; i < output.Length; i++)
            {
                output[i] = perceptrons[lastLayerIndex][i].delta;
            }

            for (int i = secondLastLayerIndex; i >= 0; --i)
            {
                for (int j = 0; j < perceptrons[i].Length; ++j)
                {
                    tmp = 0;
                    //tmp += (output[0] * perceptrons[i + 1][0].weights[0]);
                    //tmp += (output[1] * perceptrons[i + 1][1].weights[1]);
                    for (int k = 0; k < output.Length; ++k)
                        tmp += (output[k] * perceptrons[i + 1][k].weights[j]);
                    perceptrons[i][j].CalculateDelta2(tmp);
                }
            }
        }

        private Example GetRandomExample()
        {
            return examples[Globals.Random.Next(0, examples.Length)];
        }

        private void GenerateLearningExamples()
        {
            examples = new Example[Globals.LearningExamplesCount];
            for (int i = 0; i < examples.Length; ++i)
            {
                Example t = new Example(0, 0);
                Arm arm = GetArmByAngles(ref t.angles);
                //while (arm.hand.X < 0)
                //{
                //    t = new Example(0, 0);
                //    arm = GetArmByAngles(ref t.angles);
                //}
                t.point = arm.hand;
                NormalizePoint(ref t.point);
                NormalizeAngles(ref t.angles);
                examples[i] = t;
            }
        }

        private void LoadWeightsFromFile()
        {
            Example t = new Example(0, 0);
            GoForward(t);

            for (int i = 0; i < perceptrons.Length; ++i)
                for (int j = 0; j < perceptrons[i].Length; ++j)
                {
                    for (int k = 0; k < perceptrons[i][j].weights.Count; ++k)
                        perceptrons[i][j].weights[k] = 0;
                    perceptrons[i][j].bias = 0;
                }

            List<string> fileLines = new List<string>();
            using (var sr = new StreamReader(fileName))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                    fileLines.Add(line);
            }

            int z = 0;
            for (int i = 0; i < perceptrons.Length; ++i)
                for (int j = 0; j < perceptrons[i].Length; ++j)
                {
                    var splitedLine = fileLines[z].Split(' ');
                    ++z;
                    int k;
                    for (k = 0; k < perceptrons[i][j].weights.Count; ++k)
                        perceptrons[i][j].weights[k] = Convert.ToDouble(splitedLine[k]);
                    perceptrons[i][j].bias = Convert.ToDouble(splitedLine[k]);
                }
        }

        private void NormalizePoint(ref Point p)
        {
            p.X = ((p.X / Globals.Cols) * 0.8 + 0.1);
            p.Y = ((p.Y / Globals.Rows) * 0.8 + 0.1);
        }

        private void NormalizeAngles(ref Angles a)
        {
            a.alpha = ((a.alpha / 180.0) * 0.8 + 0.1);
            a.beta = ((a.beta / 180.0) * 0.8 + 0.1);
        }

        private void DenormalizeAngles(ref Angles a)
        {
            a.alpha = ((a.alpha - 0.1) / 0.8 * 180);
            a.beta = ((a.beta - 0.1) / 0.8 * 180);
            if (file > 0)
            a.beta = -180 + a.beta;
        }
        
        private void SetInitialValueForSigmaX(ref double[] input, Example t, int i)
        {
            //if (file > 0)
            //{
                input = new double[perceptrons[i].Length];
                for (int j = 0; j < input.Length; j++)
                {
                    input[j] = perceptrons[i][j].sigmaX;
                }
                return;
            //}

            // Debug.WriteLine("i: {0}", i);
            // Debug.WriteLine("len: {0}", input.Length);

            //for (int j = 0; j < input.Length; j++)
            //{
            //    input[j] = perceptrons[i][j].sigmaX;
            //}

            //input[0] = t.point.X;
            //input[1] = t.point.Y;
        }

        private Angles GetAngles()
        {
            int lastLayerIndex = perceptrons.Length - 1;
            return new Angles(perceptrons[lastLayerIndex][0].sigmaX, perceptrons[lastLayerIndex][1].sigmaX);
        }

        private Arm GetArmByAngles(ref Angles a)
        {
            Arm arm = new Arm();

            Point initialElbow = new Point(0, Globals.MountPoint.Y - Globals.ArmLength);
            arm.elbow = RotatePoint(initialElbow, Globals.MountPoint, a.alpha);
            
            Point initialHand = new Point(0, Globals.MountPoint.Y - 2 * Globals.ArmLength);
            var rotatedHand = RotatePoint(initialHand, Globals.MountPoint, a.alpha);
            arm.hand = RotatePoint(rotatedHand, arm.elbow, a.beta);

            return arm;
        }

        private Point RotatePoint(Point pointToRotate, Point centerPoint, double angleInDegrees)
        {
            double angleInRadians = angleInDegrees * (Math.PI / 180);
            double cosTheta = Math.Cos(angleInRadians);
            double sinTheta = Math.Sin(angleInRadians);
            return new Point
            {
                X =
                    (int)
                    (cosTheta * (pointToRotate.X - centerPoint.X) -
                    sinTheta * (pointToRotate.Y - centerPoint.Y) + centerPoint.X),
                Y =
                    (int)
                    (sinTheta * (pointToRotate.X - centerPoint.X) +
                    cosTheta * (pointToRotate.Y - centerPoint.Y) + centerPoint.Y)
            };
        }
    }
}
