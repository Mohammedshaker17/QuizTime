using System.Windows;
using System.Collections.Generic;
using QuizTime.Models;

namespace QuizTime.Views
{
    // Alleen de voorkant: de knoppen openen het vraag-scherm, er wordt nog niets opgeslagen.
    public partial class QuizEditorWindow : Window
    {

        public List<Question> Questions { get; } = new();

        public QuizEditorWindow()
        {
            InitializeComponent();

            QuestionListBox.ItemsSource = Questions;
            QuestionListBox.DisplayMemberPath = "Text";
        }

        public QuizEditorWindow(Quiz quiz) : this()
        {
            TitleTextBox.Text = quiz.Title;

            Questions.AddRange(quiz.Questions);
            QuestionListBox.Items.Refresh();
        }

        private void OpenQuestionEditor_Click(object sender, RoutedEventArgs e)
        {
            new QuestionEditorWindow { Owner = this }.ShowDialog();
        }

        // De titel die de gebruiker heeft ingevuld (zonder spaties ervoor/erna).
        public string QuizTitle => TitleTextBox.Text.Trim();

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (QuizTitle == "")
            {
                MessageBox.Show("Vul een titel in.");
                return;
            }

            DialogResult = true;
        }


        private void AddQuestionButton_Click(object sender, RoutedEventArgs e)
        {
            var editor = new QuestionEditorWindow();
            editor.Owner = this;

            if (editor.ShowDialog() != true || editor.SavedQuestion == null)
            {
                return;
            }

            Questions.Add(editor.SavedQuestion);
            QuestionListBox.Items.Refresh();
        }

    }
}
    