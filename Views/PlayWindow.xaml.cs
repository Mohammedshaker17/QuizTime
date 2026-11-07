using System;
using System.Linq;
using System.Windows;
using System.Windows.Threading;
using QuizTime.Models;

namespace QuizTime.Views
{
    // Speelscherm: toont één vraag tegelijk, met een eigen timer per vraag.
    public partial class PlayWindow : Window
    {
        // Deze timer "tikt" elke seconde.
        private readonly DispatcherTimer _timer = new() { Interval = TimeSpan.FromSeconds(1) };

        private Quiz? _quiz;
        private int _currentIndex;
        private int _secondsLeft;

        public PlayWindow()
        {
            InitializeComponent();
            _timer.Tick += Timer_Tick;
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

        private void Timer_Tick(object? sender, EventArgs e)
        {
            _secondsLeft--;

            if (_secondsLeft <= 0)
            {
                _timer.Stop();
                TimerText.Text = "Tijd is om!";
                return;
            }

            TimerText.Text = _secondsLeft.ToString();
        }
    }
}