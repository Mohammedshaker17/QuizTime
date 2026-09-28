using System.Configuration;
using System.Data;
using System.Windows;

namespace QuizTime
{
    public partial class App : Application
    {
        // OnStartup تشتغل أول ما البرنامج يبدأ، قبل أي نافذة تفتح.
        // هنا نتحكم إحنا بأنفسنا شنو يفتح، بدل ما نعتمد على StartupUri.
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // مؤقتاً (بمرحلة "الشكل بس")، نفتح النافذتين مع بعض عشان نشوفهم سوا.
            // بعدين، لما نوصل لمرحلة المنطق، بيصير: OperatorWindow تفتح لحالها تلقائيًا،
            // و PlayWindow تفتح فقط لما تضغط "Start Quiz" من الكود (مو تلقائيًا).
            var operatorWindow = new Views.OperatorWindow();
            var playWindow = new Views.PlayWindow();

            operatorWindow.Show();
            playWindow.Show();
        }
    }
}
