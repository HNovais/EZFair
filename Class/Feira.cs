namespace EZFair.Class
{
    public class Feira
    {
        public int idFeira { get; set; }
        public int empresa { get; set; }
        public int categoria { get; set; }
        public string nomeFeira { get; set; }
        public int numParticipantes { get; set; }
        public DateTime dataInicio { get; set; }
        public DateTime dataFinal { get; set; }
        public string descricao { get; set; }

        public Feira(int idFeira, int empresa, int categoria, string nomeFeira, int numParticipantes, DateTime dataInicio, DateTime dataFinal, string descricao)
        {
            this.idFeira = idFeira;
            this.empresa = empresa;
            this.categoria = categoria;
            this.nomeFeira = nomeFeira;
            this.numParticipantes = numParticipantes;
            this.dataInicio = dataInicio;
            this.dataFinal = dataFinal;
            this.descricao = descricao;
        }
        public Feira(int empresa, string nomeFeira, DateTime inicio, DateTime final)
        {
            this.nomeFeira = nomeFeira;
            this.empresa = empresa;
            this.dataInicio = inicio;
            this.dataFinal = final;
        }

    }
}
