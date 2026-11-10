using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using QuizTime.Models;

namespace QuizTime.Views
{
    // Alleen de voorkant: nog geen logica.
    public partial class QuestionEditorWindow : Window
    {
        public QuestionEditorWindow()
        {
            InitializeComponent();
        }

        // Het Id van de vraag die wordt bewerkt 
        private readonly int _questionId;

        // Tweede constructor: een bestaande vraag bewerken.
        public QuestionEditorWindow(Question question) : this()
        {
            _questionId = question.Id;

            QuestionTextBox.Text = question.Text;
            TimeTextBox.Text = question.TimeLimitSeconds.ToString();

            TextBox[] answerBoxes = { OptionTextBox0, OptionTextBox1, OptionTextBox2, OptionTextBox3 };
            RadioButton[] correctRadios = { CorrectRadio0, CorrectRadio1, CorrectRadio2, CorrectRadio3 };

            for (int i = 0; i < question.Answers.Count && i < 4; i++)
            {
                answerBoxes[i].Text = question.Answers[i].Text;
                correctRadios[i].IsChecked = question.Answers[i].IsCorrect;
            }
        }



        // De vraag die is ingevuld; pas gevuld na "Opslaan".
        public Question? SavedQuestion { get; private set; }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            // De 4 tekstvakken, de 4 rondjes en de letters, in dezelfde volgorde.
            TextBox[] answerBoxes = { OptionTextBox0, OptionTextBox1, OptionTextBox2, OptionTextBox3 };
            RadioButton[] correctRadios = { CorrectRadio0, CorrectRadio1, CorrectRadio2, CorrectRadio3 };
            string[] labels = { "A", "B", "C", "D" };

            string questionText = QuestionTextBox.Text.Trim();
            if (questionText == "")
            {
                MessageBox.Show("Vul de vraag in.");
                return;
            }

            // Tekst omzetten naar een getal; lukt dat niet (of is het 0 of minder), dan stoppen.
            if (!int.TryParse(TimeTextBox.Text, out int seconds) || seconds <= 0)
            {
                MessageBox.Show("Vul een geldig aantal seconden in.");
                return;
            }

            var answers = new List<Answer>();
            bool hasCorrectAnswer = false;

            for (int i = 0; i < 4; i++)
            {
                string answerText = answerBoxes[i].Text.Trim();
                if (answerText == "")
                {
                    MessageBox.Show("Vul alle 4 de antwoorden in.");
                    return;
                }

                bool isCorrect = correctRadios[i].IsChecked == true;
                if (isCorrect)
                {
                    hasCorrectAnswer = true;
                }

                answers.Add(new Answer { Label = labels[i], Text = answerText, IsCorrect = isCorrect });
            }

            if (!hasCorrectAnswer)
            {
                MessageBox.Show("Kies welk antwoord juist is.");
                return;
            }

            SavedQuestion = new Question
            {
                Id = _questionId,
                Text = questionText,
                TimeLimitSeconds = seconds,
                Answers = answers
            };
            DialogResult = true;
        }

    }
}
