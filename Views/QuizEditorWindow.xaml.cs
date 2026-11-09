using System.Windows;

namespace QuizTime.Views
{
    // Alleen de voorkant: de knoppen openen het vraag-scherm, er wordt nog niets opgeslagen.
    public partial class QuizEditorWindow : Window
    {
        public QuizEditorWindow()
        {
            InitializeComponent();
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
    }
}
    