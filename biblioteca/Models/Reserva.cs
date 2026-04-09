namespace biblioteca.Models
{
    public class Reserva
    {
        private int tempoDias;
        private double custoUn; //valor referente ao aluguer diário.
        private double custoTotal; //valor referente ao aluguer diário.

        public List<Book> Livro { get; set; }
        public Funcionario User { get; set; }

        public DateTime DataReserva { get; set; }

        public DateTime DataFim => DataReserva.AddDays(tempoDias);


        public Reserva(List<Book> livro, Funcionario user, int TempoTotal, double custo)
        {
            this.Livro = livro;
            this.User = user;
            this.TempoDias = TempoTotal;
            this.CustoUn = custo;
            this.DataReserva = DateTime.Now;
            this.CustoTotal = 25;

        }
        public int TempoDias
        {
            get { return tempoDias; }
            set
            {
                if (value <= 0) { 
                    throw new Exception(" Não é possivel alugar com menos de 1 dia.");
                }
                else
                {
                    tempoDias = value; 
                }

            }
        }

        public double CustoUn
        {

            get { return custoUn; }

            set
            {
                if (value < 0) { 
                    throw new Exception("Não é possivel valore negativos.");
                }
                else
                {
                    custoUn = value;
                }
            }
        }
        public double CustoTotal
        {

            get { return custoTotal; }

            set
            {
                if (this.custoUn < 0)
                {
                    throw new Exception("Não é possivel valore negativos.");
                }
                else
                {
                    custoTotal = this.custoUn * this.TempoDias;
                }
            }
        }


    }
}
