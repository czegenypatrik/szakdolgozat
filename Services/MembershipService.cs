using Szakdolgozat.Data.Models;
using Szakdolgozat.Data;
using Szakdolgozat.Interfaces;

namespace Szakdolgozat.Services
{
    public class MembershipService : IMembershipService
    {
        private readonly ApplicationDbContext _context;

        public MembershipService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task CreateMembership(Membership membership)
        {
            membership.Id = Guid.NewGuid();
            _context.Membership.Add(membership);
            await _context.SaveChangesAsync();
        }

        public async Task<Membership?> GetMembershipById(Guid id)
        {
            return await _context.Membership.FindAsync(id);
        }

        public async Task<List<Membership>> GetAllMemberships()
        {
            return _context.Membership.ToList();
        }

        public async Task UpdateMembership(Membership updatedMembership)
        {

            var membership = await _context.Membership.FindAsync(updatedMembership.Id);
            if (membership != null)
            {
                membership.Type = updatedMembership.Type;
                membership.Price = updatedMembership.Price;
                membership.Description = updatedMembership.Description;

                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteMembership(Guid id)
        {

            var membership = await _context.Membership.FindAsync(id);
            if (membership != null)
            {
                _context.Membership.Remove(membership);
                await _context.SaveChangesAsync();
            }
        }

    }
}
