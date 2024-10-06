using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;
using Szakdolgozat.Data;
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

        protected override void OnInitialized()
        {
            base.OnInitialized();
            //GetMembers();
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
    }
}
