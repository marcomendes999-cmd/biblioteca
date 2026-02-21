using Microsoft.AspNetCore.Mvc;
using biblioteca.Services;
using biblioteca.Models;

namespace biblioteca.Controllers
{
    public class BookController : Controller
    {
        private readonly ExcelBookService _excelService;

        public BookController(ExcelBookService excelService)
        {
            _excelService = excelService;
        }

        public IActionResult Index()
        {
            var books = _excelService.GetBooks();
            return View(books);
        }


        public IActionResult New()
        {
   
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Book book)
        {
            if (ModelState.IsValid)
            {
                // Define datas automáticas se não vierem preenchidas
                book.CreateAt = DateTime.Now;
                book.UpdateAt = DateTime.Now;

                _excelService.AddBook(book); // chama serviço para gravar no Excel

                return RedirectToAction("Index");
            }

            return View(book);
        }

        public IActionResult Edit(int id)
        {
            var book = _excelService.GetBooks().FirstOrDefault(b => b.Id == id);
            if (book == null)
                return NotFound();

            return View(book);
        }

        // POST: Book/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Book book)
        {
            if (ModelState.IsValid)
            {
                _excelService.UpdateBook(book);
                return RedirectToAction("Index");
            }

            return View(book);
        }
    }
}