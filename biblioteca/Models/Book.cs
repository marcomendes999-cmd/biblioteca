using System.ComponentModel.DataAnnotations;

namespace biblioteca.Models
{
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Autor { get; set; }
        public string Categoria { get; set; }
        public int Quantidade { get; set; }
        public DateTime CreateAt { get; set; }
        public DateTime UpdateAt { get; set; }

    }
}
