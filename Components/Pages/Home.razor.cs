using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;
using Syncfusion.Blazor;
using Syncfusion.Blazor.Charts;
using System.Globalization;
using System.Net.NetworkInformation;
using Szakdolgozat.Data;
using Szakdolgozat.Data.Models;
using Szakdolgozat.Interfaces;
using Szakdolgozat.Services;

namespace Szakdolgozat.Components.Pages
{
    public partial class Home
    {
        [Inject]
        private IMembersService _memberService {  get; set; }
        [Inject]
        private IMembershipService _membershipService {  get; set; }
        [Inject]
        private IExpensesService _expensesService {  get; set; }
        [Inject]
        private IPurchasesService _purchasesService {  get; set; }
        [Inject]
        private ITodoService _todoService {  get; set; }
        [Inject]
        public UserManager<ApplicationUser> UserManager { get; set; }

        private List<Data.Models.Members> membersList = new List<Szakdolgozat.Data.Models.Members>();
        private List<MonthlyNewMembers> monthlyData;

        protected override async void OnInitialized()
        {
            base.OnInitialized();
            membersList = await _memberService.GetAllMembers();
            monthlyData = GetMonthlyNewMembers(membersList);
        }
        public async void GetMembers()
        {
            //var Users = UserManager.Users;
            //var Members = await _memberService.GetAllMembers();
            //var Memberships = await _membershipService.GetAllMemberships();
            //var Expenses = await _expensesService.GetAllExpenses();
            //var Purchases = await _purchasesService.GetAllPurchases();
            //var Todos = await _todoService.GetAllTodos();
            ;
        }

        public List<MonthlyNewMembers> GetMonthlyNewMembers(List<Data.Models.Members> members)
        {
            var hungarianCulture = new CultureInfo("hu-HU");

            return members
                .Where(m => m.JoinedDate != null)
                .GroupBy(m => new { m.JoinedDate.Year, m.JoinedDate.Month })
                .Select(g => new MonthlyNewMembers
                {
                    Month = new DateTime(g.Key.Year, g.Key.Month, 1)
                        .ToString("yyyy MMM", hungarianCulture), // Format as "2024 január"
                    NewMembersCount = g.Count()
                })
                .OrderBy(m => DateTime.ParseExact(m.Month, "yyyy MMM", hungarianCulture))
                .ToList();
        }

        public class MonthlyNewMembers
        {
            public string Month { get; set; }
            public int NewMembersCount { get; set; }
        }
    }
}
