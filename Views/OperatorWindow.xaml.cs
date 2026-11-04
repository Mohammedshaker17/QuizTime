using System.Windows;

namespace QuizTime.Views
{
    // بهالمرحلة هالنافذة "ميتة" -- ما فيها أي منطق بعد.
    // احنا بس نبني الشكل (الواجهة) اللي بيشوفها المستخدم.
    // بعدين، لما نوصل لمرحلة الـ Backend والربط، بنضيف هنا الكود اللي يشغّل الأزرار فعليًا.
    public partial class OperatorWindow : Window
    {
        public OperatorWindow()
        {
            InitializeComponent();
        }

        private void StartReviewButton_Click(object sender, RoutedEventArgs e)
        { 

        }

        private void StartPlayButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void PreviousButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void NextButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void CategoryComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {

        }
    }
}