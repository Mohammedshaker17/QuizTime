using System.Collections.Generic;
using System.Linq;
using System.Windows;
using QuizTime.Models;
using QuizTime.Services;

namespace QuizTime.Views
{
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
                Title = editor.QuizTitle,
                Questions = editor.Questions
            };

            _quizzes.Add(newQuiz);
            AssignIds(newQuiz);
            _quizService.SaveQuizzes(_quizzes);

            // Lijst opnieuw tonen, zodat de nieuwe quiz erin staat.
            QuizListBox.Items.Refresh();
        }

        private void EditQuizButton_Click(object sender, RoutedEventArgs e)
        {
            // Welke quiz is er aangeklikt in de lijst?
            if (QuizListBox.SelectedItem is not Quiz quiz)
            {
                MessageBox.Show("Kies eerst een quiz uit de lijst.");
                return;
            }

            var editor = new QuizEditorWindow(quiz);
            editor.Owner = this;

            if (editor.ShowDialog() != true)
            {
                return;
            }

            quiz.Title = editor.QuizTitle;
            quiz.Questions = editor.Questions;
            AssignIds(quiz);
            _quizService.SaveQuizzes(_quizzes);

            QuizListBox.Items.Refresh();
        }

        private void DeleteQuizButton_Click(object sender, RoutedEventArgs e)
        {
            // Welke quiz is er aangeklikt in de lijst?
            if (QuizListBox.SelectedItem is not Quiz quiz)
            {
                MessageBox.Show("Kies eerst een quiz uit de lijst.");
                return;
            }

            // Eerst vragen of het echt de bedoeling is.
            MessageBoxResult answer = MessageBox.Show(
                $"Weet je zeker dat je \"{quiz.Title}\" wilt verwijderen?",
                "Verwijderen",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (answer != MessageBoxResult.Yes)
            {
                return;
            }

            _quizzes.Remove(quiz);
            _quizService.SaveQuizzes(_quizzes);

            QuizListBox.Items.Refresh();
        }

        // Geeft nieuwe vragen en antwoorden (Id = 0) een uniek Id,
        // en zet QuizId en QuestionId goed.
        private void AssignIds(Quiz quiz)
        {
            List<Question> allQuestions = _quizzes.SelectMany(q => q.Questions).ToList();
            int nextQuestionId = allQuestions.Select(q => q.Id).DefaultIfEmpty(0).Max() + 1;
            int nextAnswerId = allQuestions.SelectMany(q => q.Answers).Select(a => a.Id).DefaultIfEmpty(0).Max() + 1;

            foreach (Question question in quiz.Questions)
            {
                question.QuizId = quiz.Id;
                if (question.Id == 0)
                {
                    question.Id = nextQuestionId;
                    nextQuestionId++;
                }

                foreach (Answer answer in question.Answers)
                {
                    answer.QuestionId = question.Id;
                    if (answer.Id == 0)
                    {
                        answer.Id = nextAnswerId;
                        nextAnswerId++;
                    }
                }
            }
        }
    }
}