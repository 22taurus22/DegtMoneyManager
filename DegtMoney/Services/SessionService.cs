using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DegtMoney.Models;

namespace DegtMoney.Services
{
    internal static class SessionService
    {
        public static User? CurrentUser { get; set; }

    }
}
