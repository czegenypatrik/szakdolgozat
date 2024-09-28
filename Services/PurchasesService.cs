using Szakdolgozat.Data.Models;
using Szakdolgozat.Data;
using Szakdolgozat.Interfaces;

namespace Szakdolgozat.Services
{
    public class PurchasesService : IPurchasesService
    {
        private readonly ApplicationDbContext _context;

        public PurchasesService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task CreatePurchase(Purchases purchase)
        {
            purchase.Id = Guid.NewGuid();
            _context.Purchases.Add(purchase);
            await _context.SaveChangesAsync();
        }

        public async Task<Purchases?> GetPurchaseById(Guid id)
        {
            return await _context.Purchases.FindAsync(id);
        }

        public async Task<List<Purchases>> GetAllPurchases()
        {
            return _context.Purchases.ToList();
        }

        public async Task UpdatePurchase(Purchases updatedPurchase)
        {

            var purchase = await _context.Purchases.FindAsync(updatedPurchase.Id);
            if (purchase != null)
            {
                purchase.BuyerId = updatedPurchase.BuyerId;
                purchase.PassId = updatedPurchase.PassId;
                purchase.Price = updatedPurchase.Price;
                purchase.CreatorId = updatedPurchase.CreatorId;
                purchase.CreatedDate = updatedPurchase.CreatedDate;

                await _context.SaveChangesAsync();
            }
        }

        public async Task DeletePurchase(Guid id)
        {

            var purchase = await _context.Purchases.FindAsync(id);
            if (purchase != null)
            {
                _context.Purchases.Remove(purchase);
                await _context.SaveChangesAsync();
            }
        }
    }
}
