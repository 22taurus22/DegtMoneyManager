using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DegtMoney.Interfaces
{
    public interface INotificationService
    {
        void ShowToast(string message);
        void ScheduleReminder(DateTime time, string message);
    }
}
