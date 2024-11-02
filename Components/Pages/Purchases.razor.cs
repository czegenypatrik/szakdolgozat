using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using MudBlazor;
using System.Globalization;
using System.Net.Http;
using System.Security.Claims;
using Szakdolgozat.Data;
using Szakdolgozat.Data.Models;
using Szakdolgozat.Interfaces;
using Szakdolgozat.Services;
using static MudBlazor.CategoryTypes;

namespace Szakdolgozat.Components.Pages
{
    public partial class Purchases
    {
        [Inject]
        IPurchasesService _purchasesService { get; set; }
        [Inject]
        IMembersService _membersService { get; set; }
        [Inject]
        IMembershipService _membershipService { get; set; }
        [Inject]
        public UserManager<ApplicationUser> UserManager { get; set; }
        [Inject]
        private AuthenticationStateProvider AuthenticationStateProvider { get; set; }

        private List<ApplicationUser> Users = new List<ApplicationUser>();
        private List<Data.Models.Members> Members = new List<Data.Models.Members>();
        private List<Data.Models.Membership> Memberships = new List<Data.Models.Membership>();
        private List<Data.Models.Purchases> PurchasesList = new List<Data.Models.Purchases>();
        private Data.Models.Purchases purchase = new Data.Models.Purchases();
        private string? _searchString;
        private string? userId;
        private Data.Models.Members newBuyer = new Data.Models.Members();

        protected override async Task OnInitializedAsync() 
        {
            PurchasesList = await GetPurchases();
            Users = UserManager.Users.ToList();
            Members = await _membersService.GetAllMembers();
            Memberships = await _membershipService.GetAllMemberships();
            var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;
            if (user.Identity.IsAuthenticated)
            {
                userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            }
        }

        public async Task<List<Data.Models.Purchases>> GetPurchases() => await _purchasesService.GetAllPurchases();

        public string GetUsernameById(string id)
        {
            var user = Users.FirstOrDefault(u => u.Id == id);
            if (user != null)
            {
                return user?.UserName;
            }
            else
            {
                return "Ismeretlen Felhasználó";
            }
        }

        public string GetMemberById(Guid id)
        {
            var member = Members.FirstOrDefault(m => m.Id == id);
            if (member != null)
            {
                return member?.Name;
            }
            else
            {
                return "Ismeretlen Személy";
            }
        }

        public string GetMembershipById(Guid id)
        {
            var membership = Memberships.FirstOrDefault(m => m.Id == id);
            if (membership != null)
            {
                return membership?.Type;
            }
            else
            {
                return "Ismeretlen";
            }
        }

        void CreateDummyData()
        {
            List<Data.Models.Purchases> dummyList = new List<Data.Models.Purchases>();

            DateTime startDate = new DateTime(2023, 1, 1);
            for (int i = 0; i <= 100; i++)
            {
                Random random = new Random();
                int randomMembershipIndex = random.Next(Memberships.Count);
                Data.Models.Purchases dummy = new Data.Models.Purchases();
                dummy.Id = Guid.NewGuid();
                dummy.BuyerId = Members[random.Next(Members.Count)].Id;
                dummy.PassId = Memberships[randomMembershipIndex].Id;
                dummy.Price = Memberships[randomMembershipIndex].Price;
                dummy.CreatorId = Users[random.Next(Users.Count)].Id;
                dummy.CreatedDate = startDate.AddDays(random.Next((DateTime.Now - startDate).Days));
                PurchasesList.Add(dummy);
                dummyList.Add(dummy);
            }
            _purchasesService.CreateManyPurchases(dummyList);
            StateHasChanged();
        }

        void HandleValidSubmit(Data.Models.Purchases item)
        {
            try
            {
                if (userId == null)
                {
                    Snackbar.Add($"HIBA! Csak érvényes felhasználók tudnak vásárlást létrehozni!", Severity.Error, c => c.SnackbarVariant = Variant.Outlined);
                }
                else
                {
                    item.Id = Guid.NewGuid();
                    item.BuyerId = newBuyer.Id;
                    item.Price = Memberships.Where(m => m.Id == item.PassId).Select(m => m.Price).FirstOrDefault();
                    item.CreatorId = userId;
                    item.CreatedDate = DateTime.Now;
                    _purchasesService.CreatePurchase(item);
                    PurchasesList.Add(item);
                    purchase = new Data.Models.Purchases();
                    newBuyer = new Data.Models.Members();
                    StateHasChanged();
                    Snackbar.Add($"Új vásárlás sikeresen hozzáadva!", Severity.Success, c => c.SnackbarVariant = Variant.Outlined);
                }
            }
            catch (Exception e)
            {
                Snackbar.Add($"HIBA! {e}", Severity.Error, c => c.SnackbarVariant = Variant.Outlined);
                throw;
            }
        }

        private async Task OnCreatedDateChanged(Data.Models.Purchases purchase, DateTime? newDate)
        {
            if (newDate.HasValue)
            {
                try
                {
                    purchase.CreatedDate = newDate.Value;
                    await _purchasesService.UpdatePurchase(purchase);
                    Snackbar.Add($"Kiadáshoz tartozó dátum sikeresen frissítve.", Severity.Success, c => c.SnackbarVariant = Variant.Outlined);
                }
                catch (Exception e)
                {
                    Snackbar.Add($"HIBA! {e}", Severity.Error, c => c.SnackbarVariant = Variant.Outlined);
                    throw;
                }
            }
        }

        void DeletePurchase(Data.Models.Purchases purchase)
        {
            try
            {
                _purchasesService.DeletePurchase(purchase.Id);
                PurchasesList.Remove(purchase);
                StateHasChanged();
                Snackbar.Add($"A kiválasztott kiadás sikeresen törölve!", Severity.Success, c => c.SnackbarVariant = Variant.Outlined);
            }
            catch (Exception e)
            {
                Snackbar.Add($"HIBA! {e}", Severity.Error, c => c.SnackbarVariant = Variant.Outlined);
                throw;
            }
        }

        private Func<Data.Models.Purchases, bool> _quickFilter => x =>
        {
            if (string.IsNullOrWhiteSpace(_searchString))
                return true;

            if (GetUsernameById(x.CreatorId).Contains(_searchString, StringComparison.OrdinalIgnoreCase))
                return true;
            if (GetMembershipById(x.PassId).Contains(_searchString, StringComparison.OrdinalIgnoreCase))
                return true;
            if (GetMemberById(x.BuyerId).Contains(_searchString, StringComparison.OrdinalIgnoreCase))
                return true;

            return false;
        };

        private async Task<IEnumerable<Data.Models.Members>> SearchBuyer(string value, CancellationToken token)
        {
            IEnumerable<Data.Models.Members> membersList = await _membersService.GetAllMembers();

            if (string.IsNullOrEmpty(value))
                return membersList;

            return membersList.Where(x => x.Name.Contains(value, StringComparison.InvariantCultureIgnoreCase));
        }

        private async Task<IEnumerable<Data.Models.Membership>> SearchPass(string value, CancellationToken token)
        {
            IEnumerable<Data.Models.Membership> membershipList = await _membershipService.GetAllMemberships();

            if (string.IsNullOrEmpty(value))
                return membershipList;

            return membershipList.Where(x => x.Type.Contains(value, StringComparison.InvariantCultureIgnoreCase));
        }
    }
}
