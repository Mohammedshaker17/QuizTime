using System.Collections.Generic;
using System.Linq;
using System.Windows;
using QuizTime.Models;
using QuizTime.Services;

namespace QuizTime.Views
{
    // Dit venster is het "bedieningspaneel" voor de quizmaster (operator).
    // Vanuit hier kies je een categorie, de modus (spelen/nakijken), en stuur je
    // aan welke vraag op het speelscherm (PlayWindow) te zien is.
    public partial class OperatorWindow : Window
    {
        private readonly QuizService _quizService = new();

        // Alle vragen uit het JSON-bestand, ongefilterd.
        private Quiz _quiz = new();

        // De vragen die op dit moment "actief" zijn: alleen de vragen van de
        // gekozen categorie (of alle vragen, als "Alle categorieën" gekozen is).
        // Dit is de lijst waar de operator daadwerkelijk doorheen navigeert.
        private List<Question> _activeQuestions = new();

        private int _currentQuestionIndex = 0;
        private PlayWindow? _playWindow;
        private bool _isReviewMode = false;

        // Vaste tekst voor de optie die ALLE categorieën samen toont.
        private const string AllCategoriesOption = "Alle categorieën";

        public OperatorWindow()
        {
            InitializeComponent();

            _quiz = _quizService.LoadQuiz();
            StatusText.Text = $"Quiz geladen: \"{_quiz.Title}\" ({_quiz.Questions.Count} vragen).";

            FillCategoryComboBox();
        }

        // Vult de ComboBox met alle unieke categorieën uit de quiz, plus "Alle categorieën".
        private void FillCategoryComboBox()
        {
            // Distinct() haalt dubbele categorieën eruit (anders staat "Dieren" er twee keer in).
            var categories = _quiz.Questions
                .Select(q => q.Category)
                .Distinct()
                .OrderBy(c => c)
                .ToList();

            var items = new List<string> { AllCategoriesOption };
            items.AddRange(categories);

            CategoryComboBox.ItemsSource = items;
            CategoryComboBox.SelectedIndex = 0; // standaard: alle categorieën
        }

        // Filtert _quiz.Questions op basis van de categorie die in de ComboBox is gekozen,
        // en zet het resultaat in _activeQuestions.
        private void ApplyCategoryFilter()
        {
            string? selected = CategoryComboBox.SelectedItem as string;

            if (string.IsNullOrEmpty(selected) || selected == AllCategoriesOption)
            {
                _activeQuestions = _quiz.Questions;
            }
            else
            {
                _activeQuestions = _quiz.Questions
                    .Where(q => q.Category == selected)
                    .ToList();
            }

            _currentQuestionIndex = 0;
        }

        private void StartPlayButton_Click(object sender, RoutedEventArgs e)
        {
            _isReviewMode = false;
            ApplyCategoryFilter();
            RevealAnswerButton.Visibility = Visibility.Collapsed;
            OpenPlayWindow();
        }

        private void StartReviewButton_Click(object sender, RoutedEventArgs e)
        {
            _isReviewMode = true;
            ApplyCategoryFilter();
            RevealAnswerButton.Visibility = Visibility.Visible;
            OpenPlayWindow();
        }

        private void OpenPlayWindow()
        {
            // Als de gekozen categorie geen vragen bevat, waarschuwen we en stoppen we hier.
            if (_activeQuestions.Count == 0)
            {
                StatusText.Text = "Geen vragen gevonden voor deze categorie.";
                return;
            }

            if (_playWindow == null || !_playWindow.IsLoaded)
            {
                _playWindow = new PlayWindow();
                _playWindow.Show();
            }

            ShowCurrentQuestion();
        }

        private void ShowCurrentQuestion()
        {
            if (_playWindow == null || _activeQuestions.Count == 0)
                return;

            var question = _activeQuestions[_currentQuestionIndex];
            _playWindow.ShowQuestion(question, _isReviewMode, showAnswer: false);

            StatusText.Text = $"Vraag {_currentQuestionIndex + 1} van {_activeQuestions.Count}.";
        }

        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            if (_currentQuestionIndex < _activeQuestions.Count - 1)
            {
                _currentQuestionIndex++;
                ShowCurrentQuestion();
            }
        }

        private void PreviousButton_Click(object sender, RoutedEventArgs e)
        {
            if (_currentQuestionIndex > 0)
            {
                _currentQuestionIndex--;
                ShowCurrentQuestion();
            }
        }

        private void RevealAnswerButton_Click(object sender, RoutedEventArgs e)
        {
            if (_playWindow == null || _activeQuestions.Count == 0)
                return;

            var question = _activeQuestions[_currentQuestionIndex];
            _playWindow.ShowQuestion(question, _isReviewMode, showAnswer: true);
        }
    }
}