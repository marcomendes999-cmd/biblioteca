using System.ComponentModel.DataAnnotations;

namespace biblioteca.Models
{
    public class Categoria
    {

        public int Id { get; set; }

        public string Nome { get; set; }

        public string Descricao  { get; set; }

        public int ativo { get; set; }

        public DateTime createAt  { get; set; }
        public DateTime updateAt  { get; set; }


    }
}
