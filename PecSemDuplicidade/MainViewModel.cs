using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Win32;
using System.Diagnostics;
using System.Windows;

namespace PecSemDuplicidade
{
    public partial class MainViewModel(
        ServicoChecarDuplicacoes servicoChecarDuplicacoes
    ) : ObservableObject
    {
        private const string FiltroSeletorDeArquivos = "Arquivo CSV|*.csv|Todos os arquivos|*.*";

        private readonly ServicoChecarDuplicacoes _servicoChecarDuplicacoes = servicoChecarDuplicacoes;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(
            nameof(AbrirPlanilhaCommand),
            nameof(SalvarPossiveisDuplicacoesCommand)
        )]
        public partial bool ChecandoDuplicacoes { get; set; } = false;

        [RelayCommand(CanExecute = nameof(AbrirPlanilhaCanExecute))]
        private async Task AbrirPlanilha()
        {
            var ofd = new OpenFileDialog
            {
                Filter = FiltroSeletorDeArquivos
            };
            if (ofd.ShowDialog() == true)
            {
                try
                {
                    ChecandoDuplicacoes = true;
                    await Task.Run(() => _servicoChecarDuplicacoes.LerPlanilhaPec(ofd.FileName));

                    if (_servicoChecarDuplicacoes.PlanilhaLida)
                    {
                        MostrarMensagemInfo("Planilha lida com sucesso. Você já pode gerar o relatório das possíveis duplicações.");
                        SalvarPossiveisDuplicacoesCommand.NotifyCanExecuteChanged();
                    }
                }
                catch (Exception e)
                {
                    MostrarMensagemErro($"Não foi possível abrir o arquivo.\n\n{e.Message}");
                }
                finally
                {
                    ChecandoDuplicacoes = false;
                }
            }
        }

        [RelayCommand(CanExecute = nameof(SalvarPossiveisDuplicacoesCanExecute))]
        private void SalvarPossiveisDuplicacoes()
        {
            var sfd = new SaveFileDialog
            {
                FileName = "Relatório",
                Filter = FiltroSeletorDeArquivos,
                DefaultExt = ".csv",
            };

            if (sfd.ShowDialog() == true)
            {
                try
                {
                    _servicoChecarDuplicacoes.SalvarRelatorioDuplicacoes(sfd.FileName);

                    MostrarMensagemInfo("Relatório salvo com sucesso. Você pode abrir o arquivo do relatório em um programa como Excel ou Google Planilhas.");
                }
                catch (Exception e)
                {
                    MostrarMensagemErro($"Não foi possível salvar o arquivo.\n\n{e.Message}");
                }
            }
        }

        [RelayCommand]
        private static void SobreECodigoFonte()
        {
            Process.Start(new ProcessStartInfo(Config.UrlGithub) { UseShellExecute = true });
        }

        private bool AbrirPlanilhaCanExecute()
        {
            return !ChecandoDuplicacoes;
        }

        private bool SalvarPossiveisDuplicacoesCanExecute()
        {
            return (!ChecandoDuplicacoes) && _servicoChecarDuplicacoes.PlanilhaLida;
        }

        private static void MostrarMensagemErro(string texto)
        {
            MessageBox.Show(texto, "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private static void MostrarMensagemInfo(string texto)
        {
            MessageBox.Show(texto, "Informação", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
