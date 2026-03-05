using Microsoft.AspNetCore.Mvc;
using biblioteca.Services;
using biblioteca.Models;

namespace biblioteca.Controllers
{
    public class BookController : Controller
    {
        private readonly ExcelBookService _excelService;
        private readonly ExcelCategoriaService _categoriaService;


        public BookController(ExcelBookService excelService, ExcelCategoriaService excelServiceCat)
        {
            _excelService = excelService;
            _categoriaService = excelServiceCat;
        }

        public IActionResult Index(int? categoria)
        {
            var books = _excelService.GetBooks();

            var listacategoria = _categoriaService.GetCategorias();

            ViewBag.Categorias = listacategoria;

            if (categoria != null)
            {
                books = books.Where(h => h.CategoriaId == categoria).ToList();
            }
            return View(books);
        }


        public IActionResult New()
        {
            var listacategoria = _categoriaService.GetCategorias();

            ViewBag.Categorias = listacategoria;
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
            else
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors);
            }

           
            return View(book);
        }

        public IActionResult Edit(int id)
        {
            var listacategoria = _categoriaService.GetCategorias();

            ViewBag.Categorias = listacategoria;

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