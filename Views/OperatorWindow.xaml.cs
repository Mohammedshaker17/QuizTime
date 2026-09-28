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
    }
}