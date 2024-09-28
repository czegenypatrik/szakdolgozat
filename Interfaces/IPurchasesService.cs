using Szakdolgozat.Data.Models;

namespace Szakdolgozat.Interfaces
{
    public interface IPurchasesService
    {
        public Task CreatePurchase(Purchases purchase);
        public Task<Purchases?> GetPurchaseById(Guid id);
        public Task<List<Purchases>> GetAllPurchases();
        public Task UpdatePurchase(Purchases updatedPurchase);
        public Task DeletePurchase(Guid id);
    }
}
