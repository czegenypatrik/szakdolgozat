using Szakdolgozat.Data.Models;

namespace Szakdolgozat.Interfaces
{
    public interface IMembersService
    {
        public Task CreateMember(Members member);
        public Task<Members?> GetMemberById(Guid id);
        public Task<List<Members>> GetAllMembers();
        public Task UpdateMember(Members updatedMember);
        public Task DeleteMembers(Guid id);
    }
}
