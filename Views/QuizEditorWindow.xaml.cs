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
    }
}
