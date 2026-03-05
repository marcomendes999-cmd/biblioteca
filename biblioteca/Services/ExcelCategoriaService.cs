using ClosedXML.Excel;
using biblioteca.Models;

namespace biblioteca.Services
{
    public class ExcelCategoriaService
    {
        private readonly string _filePath;

        public ExcelCategoriaService(IWebHostEnvironment env)
        {
            _filePath = Path.Combine(env.ContentRootPath, "categorias.xlsx");
        }

        public List<Categoria> GetCategorias()
        {
            var categorias = new List<Categoria>();

            if (!File.Exists(_filePath))
                return categorias;

            using (var workbook = new XLWorkbook(_filePath))
            {
                var worksheet = workbook.Worksheet(1);
                var rows = worksheet.RowsUsed().Skip(1);

                foreach (var row in rows)
                {
                    DateTime createAt;
                    if (!DateTime.TryParse(row.Cell(4).GetString(), out createAt))
                        createAt = DateTime.Now;

                    DateTime updateAt;
                    if (!DateTime.TryParse(row.Cell(5).GetString(), out updateAt))
                        updateAt = DateTime.Now;

                    categorias.Add(new Categoria
                    {
                        Id = row.Cell(1).TryGetValue<int>(out int id) ? id : 0,
                        Nome = row.Cell(2).GetString(),
                        Descricao = row.Cell(3).GetString(),
                        createAt = createAt,
                        updateAt = updateAt
                    });
                }
            }

            return categorias;
        }

        public Categoria GetById(int id)
        {
            return GetCategorias().FirstOrDefault(c => c.Id == id);
        }

        public void AddCategoria(Categoria categoria)
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
                worksheet = workbook.Worksheets.Add("Categorias");

                worksheet.Cell(1, 1).Value = "Id";
                worksheet.Cell(1, 2).Value = "Nome";
                worksheet.Cell(1, 3).Value = "Descricao";
                worksheet.Cell(1, 4).Value = "CreateAt";
                worksheet.Cell(1, 5).Value = "UpdateAt";
            }

            var lastRow = worksheet.LastRowUsed()?.RowNumber() ?? 1;
            var newRow = lastRow + 1;

            int newId = 1;
            if (lastRow > 1)
            {
                if (worksheet.Cell(lastRow, 1).TryGetValue<int>(out int lastId))
                    newId = lastId + 1;
            }

            worksheet.Cell(newRow, 1).Value = newId;
            worksheet.Cell(newRow, 2).Value = categoria.Nome;
            worksheet.Cell(newRow, 3).Value = categoria.Descricao;
            worksheet.Cell(newRow, 4).Value = DateTime.Now;
            worksheet.Cell(newRow, 5).Value = DateTime.Now;

            workbook.SaveAs(_filePath);
        }

        public void UpdateCategoria(Categoria categoria)
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
                    if (row.Cell(1).TryGetValue<int>(out int rowId) && rowId == categoria.Id)
                    {
                        rowToUpdate = row;
                        break;
                    }
                }

                if (rowToUpdate == null)
                    throw new Exception($"Categoria com Id {categoria.Id} não encontrada.");

                rowToUpdate.Cell(2).Value = categoria.Nome;
                rowToUpdate.Cell(3).Value = categoria.Descricao;

                // Mantém CreateAt original
                var originalCreateAt = rowToUpdate.Cell(4).GetString();
                rowToUpdate.Cell(4).Value = originalCreateAt;

                rowToUpdate.Cell(5).Value = DateTime.Now; // UpdateAt

                workbook.SaveAs(_filePath);
            }
        }
        public void DeleteCategoria(int id)
        {
            if (!File.Exists(_filePath))
                throw new FileNotFoundException("O ficheiro Excel não existe.", _filePath);

            using (var workbook = new XLWorkbook(_filePath))
            {
                var worksheet = workbook.Worksheet(1);
                var rows = worksheet.RowsUsed().Skip(1); // ignora cabeçalho

                IXLRow rowToDelete = null;

                foreach (var row in rows)
                {
                    if (row.Cell(1).TryGetValue<int>(out int rowId) && rowId == id)
                    {
                        rowToDelete = row;
                        break;
                    }
                }

                if (rowToDelete == null)
                    throw new Exception($"Categoria com Id {id} não encontrada.");

                rowToDelete.Delete();

                workbook.SaveAs(_filePath);
            }
        }
    }
}