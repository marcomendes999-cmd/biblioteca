using ClosedXML.Excel;
using biblioteca.Models;

namespace biblioteca.Services
{
    public class ExcelBookService
    {
        private readonly string _filePath;

        public ExcelBookService(IWebHostEnvironment env)
        {
            _filePath = Path.Combine(env.ContentRootPath, "books.xlsx");
        }

        public List<Book> GetBooks()
        {
            var books = new List<Book>();

            if (!File.Exists(_filePath))
                return books;

            using (var workbook = new XLWorkbook(_filePath))
            {
                var worksheet = workbook.Worksheet(1);
                var rows = worksheet.RowsUsed().Skip(1);

                foreach (var row in rows)
                {
                    DateTime createAt;
                    if (!DateTime.TryParse(row.Cell(5).GetString(), out createAt))
                    {
                        createAt = DateTime.Now;
                    }

                    DateTime updateAt;
                    if (!DateTime.TryParse(row.Cell(6).GetString(), out updateAt))
                    {
                        updateAt = DateTime.Now;
                    }

                    books.Add(new Book
                    {
                        Id = row.Cell(1).TryGetValue<int>(out int id) ? id : 0,
                        Title = row.Cell(2).GetString(),
                        Autor = row.Cell(3).GetString(),
                        Categoria = row.Cell(4).GetString(),
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

                // Cabeçalhos
                worksheet.Cell(1, 1).Value = "Id";
                worksheet.Cell(1, 2).Value = "Title";
                worksheet.Cell(1, 3).Value = "Autor";
                worksheet.Cell(1, 4).Value = "Categoria";
                worksheet.Cell(1, 5).Value = "Quantidade";
                worksheet.Cell(1, 6).Value = "CreateAt";
                worksheet.Cell(1, 7).Value = "UpdateAt";
            }

            var lastRow = worksheet.LastRowUsed()?.RowNumber() ?? 1;
            var newRow = lastRow + 1;

            // Calcula Id automaticamente
            int newId = 1;
            if (lastRow > 1) // se já tiver linhas de dados
            {
                var lastIdCell = worksheet.Cell(lastRow, 1);
                if (lastIdCell.TryGetValue<int>(out int lastId))
                {
                    newId = lastId + 1;
                }
            }

            // Preenche os valores
            worksheet.Cell(newRow, 1).Value = newId;
            worksheet.Cell(newRow, 2).Value = book.Title;
            worksheet.Cell(newRow, 3).Value = book.Autor;
            worksheet.Cell(newRow, 4).Value = book.Categoria;
            worksheet.Cell(newRow, 5).Value = book.Quantidade;
            worksheet.Cell(newRow, 6).Value = book.CreateAt;
            worksheet.Cell(newRow, 7).Value = book.UpdateAt;

            workbook.SaveAs(_filePath);
        }

        public void UpdateBook(Book book)
        {
            if (!File.Exists(_filePath))
                throw new FileNotFoundException("O ficheiro Excel não existe.", _filePath);

            using (var workbook = new XLWorkbook(_filePath))
            {
                var worksheet = workbook.Worksheet(1);

                // Procurar linha pelo Id
                var rows = worksheet.RowsUsed().Skip(1); // ignora cabeçalho
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

                // Atualiza os valores
                rowToUpdate.Cell(2).Value = book.Title;
                rowToUpdate.Cell(3).Value = book.Autor;
                rowToUpdate.Cell(4).Value = book.Categoria;
                rowToUpdate.Cell(5).Value = book.Quantidade;
                rowToUpdate.Cell(6).Value = DateTime.Now; // UpdateAt
                rowToUpdate.Cell(7).Value = rowToUpdate.Cell(6).Value; // mantém CreateAt ou ajusta conforme necessidade

                workbook.SaveAs(_filePath);
            }
        }
    }
}