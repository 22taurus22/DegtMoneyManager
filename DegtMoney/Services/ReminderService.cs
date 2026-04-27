using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using DegtMoney.Interfaces;


namespace DegtMoney.Services
{
    public class ReminderService : INotificationService, IDisposable
    {
        private readonly AppDbContext _context;
        private System.Timers.Timer _timer;
        private int _currentUserId;

        public ReminderService(AppDbContext context)
        {
            _context = context;
        }

        public void StartForUser(int userId)
        {
            _currentUserId = userId;
            _timer = new System.Timers.Timer(TimeSpan.FromHours(24).TotalMilliseconds);
            _timer.Elapsed += CheckReminders;
            _timer.Start();
            CheckReminders(null, null);
        }

        private void CheckReminders(object sender, System.Timers.ElapsedEventArgs e)
        {
            var today = DateTime.Today;

            var lastTransaction = _context.Transactions
                .Where(t => t.UserId == _currentUserId)
                .OrderByDescending(t => t.Date)
                .FirstOrDefault();
            if (lastTransaction != null && (today - lastTransaction.Date).Days > 3)
                ShowToast("Давно не добавляли расходы! Заполните данные.");

            var upcomingGoals = _context.SavingGoals
                .Where(g => g.UserId == _currentUserId && !g.IsAchieved && g.Deadline <= today.AddDays(7))
                .ToList();
            foreach (var goal in upcomingGoals)
                ShowToast($"Цель «{goal.Name}» истекает {goal.Deadline:d}");

            var budgetExceeded = CheckBudgetExceeded();
            foreach (var msg in budgetExceeded)
                ShowToast(msg);
        }

        private List<string> CheckBudgetExceeded()
        {
            var result = new List<string>();
            var startOfMonth = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            var endOfMonth = startOfMonth.AddMonths(1).AddDays(-1);

            var budgets = _context.BudgetLimits
                .Where(b => b.UserId == _currentUserId)
                .Include(b => b.Category)
                .ToList();

            foreach (var budget in budgets)
            {
                var spent = _context.Transactions
                    .Where(t => t.UserId == _currentUserId && t.CategoryId == budget.CategoryId && t.Date >= startOfMonth && t.Date <= endOfMonth && !t.Category.IsIncome)
                    .Sum(t => t.Amount);
                if (spent > budget.MonthlyLimit)
                    result.Add($"Превышен бюджет по категории {budget.Category.Name}: {spent:F2} / {budget.MonthlyLimit:F2}");
            }
            return result;
        }

        public void ShowToast(string message)
        {
            System.Windows.MessageBox.Show(message, "Напоминание", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Information);
        }

        public void ScheduleReminder(DateTime time, string message)
        {
            var delay = time - DateTime.Now;
            if (delay.TotalMilliseconds > 0)
            {
                var timer = new System.Timers.Timer(delay.TotalMilliseconds);
                timer.Elapsed += (s, e) => ShowToast(message);
                timer.AutoReset = false;
                timer.Start();
            }
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
