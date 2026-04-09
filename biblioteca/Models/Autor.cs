using System.Threading.RateLimiting;

namespace biblioteca.Models
{
    public class Autor
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public DateTime DataNascimento { get; set; }

        public DateTime CreateAt { get; set; }

        public DateTime UpdateAt { get; set; }
    }
}
