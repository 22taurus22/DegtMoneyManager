using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DegtMoney.Interfaces
{
    public interface IReportService
    {
        Task<byte[]> GenerateReportCsvAsync(int userId, DateTime start, DateTime end);
        Task<byte[]> GenerateReportPdfAsync(int userId, DateTime start, DateTime end);
    }
}
