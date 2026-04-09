using biblioteca.Models;
using Microsoft.AspNetCore.Mvc;

namespace biblioteca.Controllers
{
    public class ReservaController : Controller
    {
        public IActionResult Index()
        {
            Funcionario func = new Funcionario();

            func.função = "Recepcionista";
            func.Nome = "Ana";
            func.Nif = "999999999";


            List<Book> newlist = new List<Book>();

            Book book1 = new Book();

            book1.Title = "livro 1";
            book1.Autor = "autor 1";


            Book book2 = new Book();

            book2.Title = "livro 2";
            book2.Autor = "autor 2";

            newlist.Add(book1);
            newlist.Add(book2);


            //Reserva(List <Book> livro, Utilizador user, int TempoTotal, double custo)

            Reserva reserva1 = new Reserva(newlist, func, 5, 4);


            return View(reserva1);
        }
    }
}
