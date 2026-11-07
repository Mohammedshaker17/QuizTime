using System.Collections.Generic;
using System.Linq;
using System.Windows;
using QuizTime.Models;
using QuizTime.Services;

namespace QuizTime.Views
{
    public partial class OperatorWindow : Window
    {
        // Het speelscherm dat deze operator bestuurt.
        private readonly PlayWindow _playWindow;

        // Alle quizzen uit quiz.json. Elke quiz is één categorie (de Title).
        private readonly List<Quiz> _quizzes;

        public OperatorWindow(PlayWindow playWindow)
        {
            InitializeComponent();
            _playWindow = playWindow;

            _quizzes = new QuizService().LoadQuizzes();

            if (_quizzes.Count == 0)
            {
                StatusText.Text = "Geen quiz gevonden in quiz.json.";
                return;
            }

            // De keuzelijst vullen met de quizzen; we tonen alleen de Title.
            CategoryComboBox.ItemsSource = _quizzes;
            CategoryComboBox.DisplayMemberPath = "Title";
            CategoryComboBox.SelectedIndex = 0;
        }

        private void StartPlayButton_Click(object sender, RoutedEventArgs e)
        {
            if (CategoryComboBox.SelectedItem is not Quiz quiz)
            {
                StatusText.Text = "Kies eerst een categorie.";
                return;
            }

            if (quiz.Questions.Count == 0)
            {
                StatusText.Text = "Deze categorie heeft geen vragen.";
                return;
            }

            _playWindow.StartQuiz(quiz);
            StatusText.Text = _playWindow.StatusText;
        }

        private void PreviousButton_Click(object sender, RoutedEventArgs e)
        {
            _playWindow.PreviousQuestion();
            StatusText.Text = _playWindow.StatusText;
        }

        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            _playWindow.NextQuestion();
            StatusText.Text = _playWindow.StatusText;
        }

        private void AdminButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}