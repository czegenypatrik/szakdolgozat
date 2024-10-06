using Szakdolgozat.Data.Models;
using Szakdolgozat.Data;
using Microsoft.EntityFrameworkCore;
using Szakdolgozat.Interfaces;

namespace Szakdolgozat.Services
{
    public class MembersService : IMembersService
    {
        private readonly ApplicationDbContext _context;

        public MembersService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task CreateMember(Members member)
        {
            member.Id = Guid.NewGuid();
            _context.Members.Add(member);
            await _context.SaveChangesAsync();
        }

        public async Task<Members?> GetMemberById(Guid id)
        {
            return await _context.Members.FindAsync(id);
        }

        public async Task<List<Members>> GetAllMembers()
        {
            return _context.Members.ToList();
        }

        public async Task UpdateMember(Members updatedMember)
        {

            var member = await _context.Members.FindAsync(updatedMember.Id);
            if (member != null)
            {
                member.Name = updatedMember.Name;
                member.Email = updatedMember.Email;
                member.Birthday = updatedMember.Birthday;
                member.JoinedDate = updatedMember.JoinedDate;
                member.Gender = updatedMember.Gender;

                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteMembers(Guid id)
        {

            var members = await _context.Members.FindAsync(id);
            if (members != null)
            {
                _context.Members.Remove(members);
                await _context.SaveChangesAsync();
            }
        }

    }
}
