namespace PecSemDuplicidade
{
    public record Config
    {
        public const string UrlGithub = "https://github.com/Lucas-Levi/PecSemDuplicidade";

        private const int CodigoEncodingPadrao = 1252;

        private const int LinhaInicialDadosPadrao = 18;
        private const int ColunaNomePadrao = 5;
        private const int ColunaDataNascimentoPadrao = 9;

        private const int DistanciaMaximaNomePadrao = 4;
        private const int DistanciaMaximaDataNascimentoPadrao = 2;

        public int CodigoEncoding { get; set; } = CodigoEncodingPadrao;

        public int LinhaInicialDados { get; set; } = LinhaInicialDadosPadrao;
        public int ColunaNome { get; set; } = ColunaNomePadrao;
        public int ColunaDataNascimento { get; set; } = ColunaDataNascimentoPadrao;

        public int DistanciaMaximaNome { get; set; } = DistanciaMaximaNomePadrao;
        public int DistanciaMaximaDataNascimento { get; set; } = DistanciaMaximaDataNascimentoPadrao;
    }
}
