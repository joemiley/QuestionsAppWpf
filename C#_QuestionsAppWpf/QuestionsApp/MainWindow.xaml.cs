using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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
using System.Windows.Threading;

namespace QuestionsApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DispatcherTimer dispatcherTimer = new DispatcherTimer();
        Stopwatch stopwatch = new Stopwatch();
        List<string> questions = File.ReadLines(@"..\..\Questions.txt").ToList();
        int questionsStartCount;
        Random rng = new Random();

        public MainWindow()
        {
            InitializeComponent();
            questionsStartCount = questions.Count;
        }

        private void ButtonStart_Click(object sender, RoutedEventArgs e)
        {
            ButtonStart.Content = "Next";
            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, 100);
            stopwatch.Start();
            dispatcherTimer.Start();
            ChangeWindowColour();
            ChangeQuestion();
        }

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            var fiveMins = new TimeSpan(00, 05, 00);

            if (stopwatch.Elapsed > fiveMins)
            {
                ChangeQuestion();
                stopwatch.Restart();
            }
        }

        private void ChangeQuestion()
        {
            var questionIndex = rng.Next(0, questions.Count - 1);
            LabelQuestion.Text = questions[questionIndex] + $"\n {questions.Count}/{questionsStartCount}";

            if(questions.Count == 1)
            {
                ButtonStart.IsEnabled = false;
            }
            questions.RemoveAt(questionIndex);
        }

        private void ChangeWindowColour()
        {
            var randomColour = new Color();
            randomColour.R = (byte)rng.Next(150, 255);
            randomColour.G = (byte)rng.Next(150, 255);
            randomColour.B = (byte)rng.Next(150, 255);
            randomColour.A = 100;
            var brush = new SolidColorBrush(randomColour);
            Panel.Background = brush;
        }

        private void Reset()
        {
            Panel.Background = Brushes.White;
            LabelQuestion.Text = "Q's";
            ButtonStart.IsEnabled = true;
            dispatcherTimer.Stop();
            stopwatch.Reset();
            questions = File.ReadLines(@"..\..\Questions.txt").ToList();
        }

        private void ButtonStop_Click(object sender, RoutedEventArgs e)
        {
            ButtonStop.Content = "Stop";
            Reset();
        }
    }
}
