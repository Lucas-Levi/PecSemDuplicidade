using System.Configuration;
using System.Data;
using System.Text;
using System.Windows;

namespace PecSemDuplicidade
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            var config = new Config();
            var servicoChecarDuplicacoes = new ServicoChecarDuplicacoes(config);

            MainWindow = new MainWindow
            {
                DataContext = new MainViewModel(servicoChecarDuplicacoes)
            };

            MainWindow.Show();
        }
    }
}
