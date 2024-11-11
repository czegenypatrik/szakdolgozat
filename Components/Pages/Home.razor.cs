using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;
using System.Globalization;
using Szakdolgozat.Data;
using Szakdolgozat.Interfaces;

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

        private List<Data.Models.Members> membersList = new List<Data.Models.Members>();
        private List<Data.Models.Membership> membershipList = new List<Data.Models.Membership>();
        private List<Data.Models.Expenses> expensesList = new List<Data.Models.Expenses>();
        private List<Data.Models.Purchases> purchasesList = new List<Data.Models.Purchases>();
        private List<MonthlyNewMembers> monthlyData;
        private List<GenderCount> genderDistribution;
        private List<MonthlyExpense> monthlyExpenseData;
        private List<MonthlyProfit> monthlyProfitData;
        private List<PassPurchaseSummary> purchaseData;

        protected override async void OnInitialized()
        {
            base.OnInitialized();
            membersList = await _memberService.GetAllMembers();
            membershipList = await _membershipService.GetAllMemberships();
            expensesList = await _expensesService.GetAllExpenses();
            purchasesList = await _purchasesService.GetAllPurchases();

            monthlyData = GetMonthlyNewMembers(membersList);
            genderDistribution = GetGenderDistribution(membersList);
            monthlyExpenseData = GetMonthlyExpenses(expensesList);
            monthlyProfitData = GetMonthlyPurchases(purchasesList);
            purchaseData = await GetPurchaseDataAsync(purchasesList);
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

        public List<MonthlyExpense> GetMonthlyExpenses(List<Data.Models.Expenses> expenses)
        {
            var hungarianCulture = new CultureInfo("hu-HU");

            return expenses
            .Where(e => e.CreatedDate.HasValue)
            .GroupBy(e => new { e.CreatedDate.Value.Year, e.CreatedDate.Value.Month })
            .Select(g => new MonthlyExpense
            {
                Month = new DateTime(g.Key.Year, g.Key.Month, 1).ToString("yyyy MMM", hungarianCulture),
                TotalExpense = g.Sum(e => e.Price)
            })
            .OrderBy(m => DateTime.ParseExact(m.Month, "yyyy MMM", hungarianCulture))
            .ToList();
        }
        public List<MonthlyProfit> GetMonthlyPurchases(List<Data.Models.Purchases> purchases)
        {
            var hungarianCulture = new CultureInfo("hu-HU");

            return purchases
            .Where(e => e.CreatedDate != null)
            .GroupBy(e => new { e.CreatedDate.Year, e.CreatedDate.Month })
            .Select(g => new MonthlyProfit
            {
                Month = new DateTime(g.Key.Year, g.Key.Month, 1).ToString("yyyy MM", hungarianCulture),
                TotalProfit = g.Sum(e => e.Price)
            })
            .OrderBy(m => DateTime.ParseExact(m.Month, "yyyy MM", hungarianCulture))
            .ToList();
        }
        public async Task<List<PassPurchaseSummary>> GetPurchaseDataAsync(List<Data.Models.Purchases> purchases)
        {
            var hungarianCulture = new CultureInfo("hu-HU");

            var passPurchaseSummary = new List<PassPurchaseSummary>();

            foreach (var group in purchases.GroupBy(p => p.PassId))
            {
                var membership = await _membershipService.GetMembershipById(group.Key);

                passPurchaseSummary.Add(new PassPurchaseSummary
                {
                    PassId = membership?.Type ?? "Ismeretlen", // Use a specific property or ToString() if Name is not available
                    TotalAmount = group.Sum(p => p.Price),
                    Count = group.Count()
                });
            }

            return passPurchaseSummary;
        }

        public List<GenderCount> GetGenderDistribution(List<Data.Models.Members> members)
        {
            var hungarianCulture = new CultureInfo("hu-HU");

            return members
            .Where(m => !string.IsNullOrEmpty(m.Gender)) // Ensure Gender is not null or empty
            .GroupBy(m => m.Gender)
            .Select(g => new GenderCount
            {
                Gender = g.Key == "Férfi" ? "Férfi" : "Nő",
                Count = g.Count()
            })
            .ToList();
        }

        public class MonthlyNewMembers
        {
            public string Month { get; set; }
            public int NewMembersCount { get; set; }
        }
        public class MonthlyExpense
        {
            public string Month { get; set; }
            public int TotalExpense { get; set; }
        }
        public class MonthlyProfit
        {
            public string Month { get; set; }
            public int TotalProfit { get; set; }
        }
        public class GenderCount
        {
            public string Gender { get; set; }
            public int Count { get; set; }
        }
        public class PassPurchaseSummary
        {
            public string PassId { get; set; }
            public int TotalAmount { get; set; }
            public int Count { get; set; }
        }
    }
}
