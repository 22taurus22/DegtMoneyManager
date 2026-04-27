using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DegtMoney.Interfaces
{
    public interface IDialogService
    {
        bool ShowConfirmation(string message);
        void ShowMessage(string message);
    }
}
