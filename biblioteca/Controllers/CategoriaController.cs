using biblioteca.Models;
using biblioteca.Services;
using Microsoft.AspNetCore.Mvc;

namespace biblioteca.Controllers
{
    public class CategoriaController : Controller
    {
        private readonly ExcelCategoriaService _categoriaService;

        public CategoriaController(ExcelCategoriaService excelService)
        {
            _categoriaService = excelService;
        }
        public IActionResult Index()
        {
            var categorias = _categoriaService.GetCategorias();
         


          //  Lista.Add(cat2);

            return View(categorias);
        }

        public IActionResult New()
        {
            
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Categoria categoria)
        {


            if (ModelState.IsValid)
            {
                // Define datas automáticas se não vierem preenchidas
                categoria.createAt = DateTime.Now;
                categoria.updateAt = DateTime.Now;

                _categoriaService.AddCategoria(categoria); // chama serviço para gravar no Excel

                return RedirectToAction("Index");
            }
            else
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors);
            }


            return View(categoria);
        }

        public IActionResult Edit(int id)
        {


            var categoria = _categoriaService.GetCategorias().FirstOrDefault(b => b.Id == id);
            if (categoria == null)
                return NotFound();

            return View(categoria);
        }

        // POST: Book/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Categoria categoria)
        {
            if (ModelState.IsValid)
            {
                _categoriaService.UpdateCategoria(categoria);
                return RedirectToAction("Index");
            }

            return View(categoria);
        }


    }
}
