using System.Collections.Generic;
using System.Linq;
using System.Windows;
using QuizTime.Models;
using QuizTime.Services;

namespace QuizTime.Views
{
    // Alleen de voorkant: de knoppen openen het volgende scherm, er wordt nog niets opgeslagen.
    public partial class AdminWindow : Window
    {
        private readonly QuizService _quizService = new();
        private readonly List<Quiz> _quizzes;

        public AdminWindow()
        {
            InitializeComponent();

            _quizzes = _quizService.LoadQuizzes();
            QuizListBox.ItemsSource = _quizzes;
        }

        private void NewQuizButton_Click(object sender, RoutedEventArgs e)
        {
            var editor = new QuizEditorWindow();
            editor.Owner = this;

            // ShowDialog geeft true terug als er op "Opslaan" is gedrukt.
            if (editor.ShowDialog() != true)
            {
                return;
            }

            var newQuiz = new Quiz
            {
                Id = _quizzes.Count == 0 ? 1 : _quizzes.Max(q => q.Id) + 1,
                Title = editor.QuizTitle
            };

            _quizzes.Add(newQuiz);
            _quizService.SaveQuizzes(_quizzes);

            // Lijst opnieuw tonen, zodat de nieuwe quiz erin staat.
            QuizListBox.Items.Refresh();
        }


        private void OpenQuizEditor_Click(object sender, RoutedEventArgs e)
        {
            new QuizEditorWindow { Owner = this }.ShowDialog();
        }
    }
}
