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
    }
}
