using System.IO;
using System.Text;

namespace PecSemDuplicidade
{
    public class ServicoChecarDuplicacoes(Config config)
    {
        private readonly Config _config = config;
        private string _linhaCabecalho = "";
        private List<List<string>> _pacientes = [];

        public bool PlanilhaLida { get => _pacientes.Count != 0; }

        public void LerPlanilhaPec(string caminhoArquivoPlanilha)
        {
            using var sr = new StreamReader(caminhoArquivoPlanilha, Encoding.GetEncoding(_config.CodigoEncoding));

            for (int i = 0; i < _config.LinhaInicialDados - 1; i++)
            {
                sr.ReadLine();
            }

            _linhaCabecalho = sr.ReadLine() ?? "";

            _pacientes = [];

            var linha = sr.ReadLine();
            while (!string.IsNullOrEmpty(linha))
            {
                var dados = linha.Split(';');

                foreach (var paciente in _pacientes)
                {
                    foreach (var duplicacao in paciente)
                    {
                        if (EPossivelDuplicacao(dados, duplicacao.Split(';')))
                        {
                            paciente.Add(linha);
                            break;
                        }
                    }
                }

                _pacientes.Add([linha]);

                linha = sr.ReadLine();
            }

            _pacientes = [.. _pacientes.Where(duplicados => duplicados.Count > 1)];
        }

        public void SalvarRelatorioDuplicacoes(string caminhoArquivoRelatorio)
        {
            using var sw = new StreamWriter(caminhoArquivoRelatorio, false, Encoding.GetEncoding(_config.CodigoEncoding));

            sw.WriteLine($"Total de possíveis pacientes com cadastro duplicado: {_pacientes.Count}");
            sw.WriteLine($"Total de possíveis cadastros duplicados: {_pacientes.Sum(cadastro => cadastro.Count)}\n");

            sw.WriteLine($"{_linhaCabecalho}");
            foreach (var paciente in _pacientes)
            {
                foreach (var duplicacao in paciente)
                {
                    sw.WriteLine(duplicacao);
                }
                sw.WriteLine();
            }
        }

        public bool EPossivelDuplicacao(string[] dadosPaciente1, string[] dadosPaciente2)
        {
            var nomePaciente1 = dadosPaciente1[_config.ColunaNome];
            var dataNascimentoPaciente1 = dadosPaciente1[_config.ColunaDataNascimento];

            var nomePaciente2 = dadosPaciente2[_config.ColunaNome];
            var dataNascimentoPaciente2 = dadosPaciente2[_config.ColunaDataNascimento];

            if (LevenshteinDistance.Calculate(dataNascimentoPaciente1, dataNascimentoPaciente2) <= _config.DistanciaMaximaDataNascimento)
            {
                if (LevenshteinDistance.Calculate(nomePaciente1, nomePaciente2) <= _config.DistanciaMaximaNome)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
