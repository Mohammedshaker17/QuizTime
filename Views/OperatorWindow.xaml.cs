using System.Windows;
using QuizTime.Models;
using QuizTime.Services;

namespace QuizTime.Views
{
    // Dit venster is het "bedieningspaneel" voor de quizmaster (operator).
    // Vanuit hier kies je de modus (spelen/nakijken) en stuur je aan welke
    // vraag op het speelscherm (PlayWindow) te zien is.
    public partial class OperatorWindow : Window
    {
        // De service die de quizdata voor ons inlaadt vanuit het JSON-bestand.
        private readonly QuizService _quizService = new();

        // De volledige, ingeladen quiz (titel + alle vragen).
        private Quiz _quiz = new();

        // Houdt bij bij welke vraag we op dit moment zijn.
        // Index 0 = eerste vraag (lijsten in C# beginnen bij 0).
        private int _currentQuestionIndex = 0;

        // Referentie naar het scherm dat aan het publiek getoond wordt.
        // "?" betekent dat deze waarde ook null (leeg) mag zijn, bijvoorbeeld
        // voordat de operator een modus heeft gekozen.
        private PlayWindow? _playWindow;

        // Onthoudt welke modus actief is, zodat we weten hoe we het
        // speelscherm moeten laten reageren (met of zonder timer).
        private bool _isReviewMode = false;

        public OperatorWindow()
        {
            InitializeComponent();

            // Zodra dit venster opent, laden we meteen de quizvragen in.
            _quiz = _quizService.LoadQuiz();
            StatusText.Text = $"Quiz geladen: \"{_quiz.Title}\" ({_quiz.Questions.Count} vragen).";
        }

        // Wordt aangeroepen wanneer de operator op "Start Quiz Spelen" klikt.
        private void StartPlayButton_Click(object sender, RoutedEventArgs e)
        {
            _isReviewMode = false;
            _currentQuestionIndex = 0;
            RevealAnswerButton.Visibility = Visibility.Collapsed;
            OpenPlayWindow();
        }

        // Wordt aangeroepen wanneer de operator op "Start Quiz Nakijken" klikt.
        private void StartReviewButton_Click(object sender, RoutedEventArgs e)
        {
            _isReviewMode = true;
            _currentQuestionIndex = 0;
            RevealAnswerButton.Visibility = Visibility.Visible;
            OpenPlayWindow();
        }

        // Opent het speelscherm (als het nog niet open is) en toont daarin de huidige vraag.
        private void OpenPlayWindow()
        {
            if (_playWindow == null || !_playWindow.IsLoaded)
            {
                _playWindow = new PlayWindow();
                _playWindow.Show();
            }

            ShowCurrentQuestion();
        }

        // Stuurt de huidige vraag door naar het speelscherm, passend bij de gekozen modus.
        private void ShowCurrentQuestion()
        {
            if (_playWindow == null || _quiz.Questions.Count == 0)
                return;

            var question = _quiz.Questions[_currentQuestionIndex];

            // In spelen-modus: vraag + timer, antwoord blijft verborgen.
            // In nakijk-modus: vraag zonder timer, antwoord verschijnt pas na een klik.
            _playWindow.ShowQuestion(question, _isReviewMode, showAnswer: false);

            StatusText.Text = $"Vraag {_currentQuestionIndex + 1} van {_quiz.Questions.Count}.";
        }

        // Ga naar de volgende vraag, als die er is.
        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            if (_currentQuestionIndex < _quiz.Questions.Count - 1)
            {
                _currentQuestionIndex++;
                ShowCurrentQuestion();
            }
        }

        // Ga terug naar de vorige vraag, als die er is.
        private void PreviousButton_Click(object sender, RoutedEventArgs e)
        {
            if (_currentQuestionIndex > 0)
            {
                _currentQuestionIndex--;
                ShowCurrentQuestion();
            }
        }

        // Alleen zinvol in nakijk-modus: toont het juiste antwoord op het speelscherm.
        private void RevealAnswerButton_Click(object sender, RoutedEventArgs e)
        {
            if (_playWindow == null || _quiz.Questions.Count == 0)
                return;

            var question = _quiz.Questions[_currentQuestionIndex];
            _playWindow.ShowQuestion(question, _isReviewMode, showAnswer: true);
        }
    }
}