using Szakdolgozat.Data.Models;

namespace Szakdolgozat.Interfaces
{
    public interface IMembershipService
    {
        public Task CreateMembership(Membership membership);
        public Task<Membership?> GetMembershipById(Guid id);
        public Task<List<Membership>> GetAllMemberships();
        public Task UpdateMembership(Membership updatedMembership);
        public Task DeleteMembership(Guid id);
    }
}
