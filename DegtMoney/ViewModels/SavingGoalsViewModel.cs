using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DegtMoney.Interfaces;
using DegtMoney.Models;
using DegtMoney.Services;
using Microsoft.VisualBasic;
using DegtMoney.Views;


namespace DegtMoney.ViewModels
{
    public partial class SavingGoalsViewModel : ObservableObject
    {
        private readonly AppDbContext _context;
        private readonly INotificationService _notification;
        [ObservableProperty] private List<SavingGoal> _goals = new();

        public SavingGoalsViewModel(AppDbContext context, INotificationService notification)
        {
            _context = context;
            _notification = notification;
            LoadGoals();
        }

        private void LoadGoals()
        {
            Goals = _context.SavingGoals.Where(g => g.UserId == SessionService.CurrentUser.Id).ToList();
        }

        [RelayCommand]
        private void AddGoal()
        {
            var dialog = new GoalDialog();
            if (dialog.ShowDialog() == true)
            {
                var goal = new SavingGoal
                {
                    Name = dialog.Name,
                    TargetAmount = dialog.TargetAmount,
                    Deadline = dialog.Deadline,
                    CurrentAmount = 0,
                    UserId = SessionService.CurrentUser.Id,
                    IsAchieved = false
                };
                _context.SavingGoals.Add(goal);
                _context.SaveChanges();
                LoadGoals();
            }
        }

        [RelayCommand]
        private void AddToGoal(SavingGoal goal)
        {
            var input = Interaction.InputBox("Сумма пополнения:", "Пополнить цель", "0");
            if (decimal.TryParse(input, out decimal amount) && amount > 0)
            {
                goal.CurrentAmount += amount;
                if (goal.CurrentAmount >= goal.TargetAmount)
                {
                    goal.IsAchieved = true;
                    _notification.ShowToast($"Поздравляем! Цель «{goal.Name}» достигнута!");
                }
                _context.SaveChanges();
                LoadGoals();
            }
        }

        [RelayCommand]
        private void DeleteGoal(SavingGoal goal)
        {
            _context.SavingGoals.Remove(goal);
            _context.SaveChanges();
            LoadGoals();
        }
    }
}
