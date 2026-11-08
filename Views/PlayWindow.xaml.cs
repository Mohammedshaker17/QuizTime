using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;
using QuizTime.Models;

namespace QuizTime.Views
{
    // Speelscherm: toont één vraag tegelijk, met een eigen timer per vraag.
    public partial class PlayWindow : Window
    {
        // Deze timer "tikt" elke seconde.
        private readonly DispatcherTimer _timer = new() { Interval = TimeSpan.FromSeconds(1) };



        // Geluid dat de hele vraag door blijft spelen (in een lus),
        // en een apart geluid voor als de tijd om is.
        private readonly MediaPlayer _questionSound = new();
        private readonly MediaPlayer _timeUpSound = new();

        private Quiz? _quiz;
        private int _currentIndex;
        private int _secondsLeft;

        public PlayWindow()
        {
            InitializeComponent();
            _timer.Tick += Timer_Tick;
            
            // Is het vraaggeluid klaar? Dan opnieuw vanaf het begin: zo blijft het doorspelen.
            _questionSound.MediaEnded += (sender, e) =>
            {
                _questionSound.Position = TimeSpan.Zero;
                _questionSound.Play();
            };
        }

        public void StartQuiz(Quiz quiz)
        {
            _quiz = quiz;
            ShowQuestion(0);
        }

        public void ShowQuestion(int index)
        {
            if (_quiz == null || index < 0 || index >= _quiz.Questions.Count)
            {
                return;
            }

            _currentIndex = index;
            Question question = _quiz.Questions[index];

            CategoryText.Text = $"{_quiz.Title} - Vraag {index + 1}/{_quiz.Questions.Count}";
            QuestionText.Text = question.Text;
            AnswersList.ItemsSource = question.Answers.OrderBy(a => a.Label).ToList();
         
            // Elke vraag heeft zijn eigen tijd: opnieuw beginnen met aftellen.
            _secondsLeft = question.TimeLimitSeconds;
            TimerText.Text = _secondsLeft.ToString();
            _timer.Start();
            PlaySound(_questionSound, "question.wav");
        }

        public void NextQuestion()
        {
            ShowQuestion(_currentIndex + 1);
        }

        public void PreviousQuestion()
        {
            ShowQuestion(_currentIndex - 1);
        }

        public string StatusText =>
            _quiz == null ? "Nog geen quiz geladen." : $"Vraag {_currentIndex + 1} van {_quiz.Questions.Count}";

        // Speelt een geluid uit de map Sounds (naast het programma).
        // Bestaat het bestand niet, dan gebeurt er gewoon niets.
        private static void PlaySound(MediaPlayer player, string fileName)
        {
            string path = Path.Combine(AppContext.BaseDirectory, "Sounds", fileName);
            if (!File.Exists(path))
            {
                return;
            }

            player.Open(new Uri(path));
            player.Play();
        }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            _secondsLeft--;
            

            if (_secondsLeft <= 0)
            {
                _timer.Stop();
                _questionSound.Stop();
                PlaySound(_timeUpSound, "timeup.wav");
                TimerText.Text = "Tijd is om!";
                return;
            }

            TimerText.Text = _secondsLeft.ToString();
        }
    }
}