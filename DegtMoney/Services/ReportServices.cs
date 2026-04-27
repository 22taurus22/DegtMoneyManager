using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvHelper;
using System.Globalization;
using iTextSharp.text;
using iTextSharp.text.pdf;
using DegtMoney.Interfaces;

namespace DegtMoney.Services
{
    public class ReportService : IReportService
    {
        private readonly AppDbContext _context;
        private readonly IEncryptionService _encryption;

        public ReportService(AppDbContext context, IEncryptionService encryption)
        {
            _context = context;
            _encryption = encryption;
        }

        public async Task<byte[]> GenerateReportCsvAsync(int userId, DateTime start, DateTime end)
        {
            var transactions = await _context.Transactions
                .Where(t => t.UserId == userId && t.Date >= start && t.Date <= end)
                .Include(t => t.Category)
                .ToListAsync();

            using (var memoryStream = new MemoryStream())
            using (var writer = new StreamWriter(memoryStream, Encoding.UTF8))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csv.WriteRecords(transactions.Select(t => new
                {
                    t.Date,
                    Category = t.Category.Name,
                    t.Amount,
                    Note = _encryption.Decrypt(t.NoteEncrypted)
                }));
                writer.Flush();
                return memoryStream.ToArray();
            }
        }

        public async Task<byte[]> GenerateReportPdfAsync(int userId, DateTime start, DateTime end)
        {
            var csvData = await GenerateReportCsvAsync(userId, start, end);
            var csvString = Encoding.UTF8.GetString(csvData);
            var lines = csvString.Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries).Skip(1);

            using (var memoryStream = new MemoryStream())
            {
                var document = new iTextSharp.text.Document();
                try
                {
                    iTextSharp.text.pdf.PdfWriter.GetInstance(document, memoryStream);
                    document.Open();
                    document.Add(new iTextSharp.text.Paragraph($"Финансовый отчёт с {start:d} по {end:d}"));
                    document.Add(new iTextSharp.text.Paragraph(" "));

                    var table = new iTextSharp.text.pdf.PdfPTable(4);
                    table.AddCell("Дата");
                    table.AddCell("Категория");
                    table.AddCell("Сумма");
                    table.AddCell("Примечание");

                    foreach (var line in lines)
                    {
                        if (string.IsNullOrWhiteSpace(line)) continue;
                        var parts = line.Split(',');
                        if (parts.Length >= 4)
                        {
                            table.AddCell(parts[0]);
                            table.AddCell(parts[1]);
                            table.AddCell(parts[2]);
                            table.AddCell(parts[3]);
                        }
                    }
                    document.Add(table);
                }
                finally
                {
                    document.Close();
                }
                return memoryStream.ToArray();
            }
        }
    }
}