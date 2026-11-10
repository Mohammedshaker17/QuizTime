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

        private void EditQuestionButton_Click(object sender, RoutedEventArgs e)
        {
            // Welke vraag is er aangeklikt in de lijst?
            if (QuestionListBox.SelectedItem is not Question question)
            {
                MessageBox.Show("Kies eerst een vraag uit de lijst.");
                return;
            }

            var editor = new QuestionEditorWindow(question);
            editor.Owner = this;

            if (editor.ShowDialog() != true || editor.SavedQuestion == null)
            {
                return;
            }

            // De oude vraag vervangen door de nieuwe, op dezelfde plek in de lijst.
            int index = Questions.IndexOf(question);
            Questions[index] = editor.SavedQuestion;
            QuestionListBox.Items.Refresh();
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


        private void DeleteQuestionButton_Click(object sender, RoutedEventArgs e)
        {
            // Welke vraag is er aangeklikt in de lijst?
            if (QuestionListBox.SelectedItem is not Question question)
            {
                MessageBox.Show("Kies eerst een vraag uit de lijst.");
                return;
            }

            // Eerst vragen of het echt de bedoeling is.
            MessageBoxResult answer = MessageBox.Show(
                $"Weet je zeker dat je \"{question.Text}\" wilt verwijderen?",
                "Vraag verwijderen",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (answer != MessageBoxResult.Yes)
            {
                return;
            }

            Questions.Remove(question);
            QuestionListBox.Items.Refresh();
        }

    }
}
    