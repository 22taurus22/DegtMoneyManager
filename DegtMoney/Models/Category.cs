using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DegtMoney.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public bool IsIncome { get; set; }
        public int? ParentCategoryId { get; set; }
        public int UserId { get; set; }
        public virtual User User { get; set; } = null!;
    }
}
