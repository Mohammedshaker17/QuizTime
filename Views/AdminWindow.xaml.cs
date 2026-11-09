using System.Collections.Generic;
using System.Windows;
using QuizTime.Models;
using QuizTime.Services;

namespace QuizTime.Views
{
    // Alleen de voorkant: de knoppen openen het volgende scherm, er wordt nog niets opgeslagen.
    public partial class AdminWindow : Window
    {
        public AdminWindow()
        {
            InitializeComponent();

            // Alle quizzen uit quiz.json inlezen en in de lijst tonen.
            List<Quiz> quizzes = new QuizService().LoadQuizzes();
            QuizListBox.ItemsSource = quizzes;
        }

        private void OpenQuizEditor_Click(object sender, RoutedEventArgs e)
        {
            new QuizEditorWindow { Owner = this }.ShowDialog();
        }
    }
}
