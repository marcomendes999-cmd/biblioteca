using biblioteca.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using biblioteca.Services;

namespace biblioteca.Controllers
{
    public class HomeController : Controller
    {

        private readonly ExcelBookService _excelService;

        public HomeController(ExcelBookService excelService)
        {
            _excelService = excelService;
        }

        public IActionResult Index()
        {

            var books = _excelService.GetBooks();

            // Total de livros considerando a Quantidade de cada livro
            int totalLivros = books.Sum(b => b.Quantidade);

            ViewData["TotalLivros"] = totalLivros;

            return View();


        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
