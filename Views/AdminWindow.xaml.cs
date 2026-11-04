using System.Windows;

namespace QuizTime.Views
{
    // Alleen de voorkant: de knoppen openen het volgende scherm, er wordt nog niets opgeslagen.
    public partial class AdminWindow : Window
    {
        public AdminWindow()
        {
            InitializeComponent();
        }

        private void OpenQuizEditor_Click(object sender, RoutedEventArgs e)
        {
            new QuizEditorWindow { Owner = this }.ShowDialog();
        }
    }
}
