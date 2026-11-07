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

            
            var playWindow = new Views.PlayWindow();
            var operatorWindow = new Views.OperatorWindow(playWindow);

            operatorWindow.Show();
            playWindow.Show();
        }
    }
}
