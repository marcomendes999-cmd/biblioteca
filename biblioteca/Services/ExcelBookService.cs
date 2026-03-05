using ClosedXML.Excel;
using biblioteca.Models;

namespace biblioteca.Services
{
    public class ExcelBookService
    {
        private readonly string _filePath;
        private readonly ExcelCategoriaService _categoriaService;

        public ExcelBookService(IWebHostEnvironment env, ExcelCategoriaService categoriaService)
        {
            _filePath = Path.Combine(env.ContentRootPath, "books.xlsx");
            _categoriaService = categoriaService;
        }

        public List<Book> GetBooks()
        {
            var books = new List<Book>();

            if (!File.Exists(_filePath))
                return books;

            var categorias = _categoriaService.GetCategorias();

            using (var workbook = new XLWorkbook(_filePath))
            {
                var worksheet = workbook.Worksheet(1);
                var rows = worksheet.RowsUsed().Skip(1);

                foreach (var row in rows)
                {
                    DateTime createAt;
                    if (!DateTime.TryParse(row.Cell(6).GetString(), out createAt))
                        createAt = DateTime.Now;

                    DateTime updateAt;
                    if (!DateTime.TryParse(row.Cell(7).GetString(), out updateAt))
                        updateAt = DateTime.Now;

                    int categoriaId = row.Cell(4).TryGetValue<int>(out int catId) ? catId : 0;

                    var categoria = categorias.FirstOrDefault(c => c.Id == categoriaId);

                    books.Add(new Book
                    {
                        Id = row.Cell(1).TryGetValue<int>(out int id) ? id : 0,
                        Title = row.Cell(2).GetString(),
                        Autor = row.Cell(3).GetString(),
                        CategoriaId = categoriaId,
                        Categoria = categoria,
                        Quantidade = row.Cell(5).TryGetValue<int>(out int qtd) ? qtd : 0,
                        CreateAt = createAt,
                        UpdateAt = updateAt
                    });
                }
            }

            return books;
        }

        public void AddBook(Book book)
        {
            XLWorkbook workbook;
            IXLWorksheet worksheet;

            if (File.Exists(_filePath))
            {
                workbook = new XLWorkbook(_filePath);
                worksheet = workbook.Worksheet(1);
            }
            else
            {
                workbook = new XLWorkbook();
                worksheet = workbook.Worksheets.Add("Books");

                worksheet.Cell(1, 1).Value = "Id";
                worksheet.Cell(1, 2).Value = "Title";
                worksheet.Cell(1, 3).Value = "Autor";
                worksheet.Cell(1, 4).Value = "CategoriaId";
                worksheet.Cell(1, 5).Value = "Quantidade";
                worksheet.Cell(1, 6).Value = "CreateAt";
                worksheet.Cell(1, 7).Value = "UpdateAt";
            }

            var lastRow = worksheet.LastRowUsed()?.RowNumber() ?? 1;
            var newRow = lastRow + 1;

            int newId = 1;
            if (lastRow > 1)
            {
                var lastIdCell = worksheet.Cell(lastRow, 1);
                if (lastIdCell.TryGetValue<int>(out int lastId))
                    newId = lastId + 1;
            }

            worksheet.Cell(newRow, 1).Value = newId;
            worksheet.Cell(newRow, 2).Value = book.Title;
            worksheet.Cell(newRow, 3).Value = book.Autor;
            worksheet.Cell(newRow, 4).Value = book.CategoriaId;
            worksheet.Cell(newRow, 5).Value = book.Quantidade;
            worksheet.Cell(newRow, 6).Value = DateTime.Now;
            worksheet.Cell(newRow, 7).Value = DateTime.Now;

            workbook.SaveAs(_filePath);
        }

        public void UpdateBook(Book book)
        {
            if (!File.Exists(_filePath))
                throw new FileNotFoundException("O ficheiro Excel não existe.", _filePath);

            using (var workbook = new XLWorkbook(_filePath))
            {
                var worksheet = workbook.Worksheet(1);
                var rows = worksheet.RowsUsed().Skip(1);

                IXLRow rowToUpdate = null;

                foreach (var row in rows)
                {
                    if (row.Cell(1).TryGetValue<int>(out int rowId) && rowId == book.Id)
                    {
                        rowToUpdate = row;
                        break;
                    }
                }

                if (rowToUpdate == null)
                    throw new Exception($"Livro com Id {book.Id} não encontrado.");

                rowToUpdate.Cell(2).Value = book.Title;
                rowToUpdate.Cell(3).Value = book.Autor;
                rowToUpdate.Cell(4).Value = book.CategoriaId;
                rowToUpdate.Cell(5).Value = book.Quantidade;
                rowToUpdate.Cell(7).Value = DateTime.Now;

                workbook.SaveAs(_filePath);
            }
        }
    }
}