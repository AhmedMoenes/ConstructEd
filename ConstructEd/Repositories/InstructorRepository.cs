using ConstructEd.Data;
using ConstructEd.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ConstructEd.Repositories
{
    public class InstructorRepository : IInstructorRepository
    {
        private readonly DataContext _dataContext;
        private readonly UserManager<ApplicationUser> _userManager;

        public InstructorRepository(DataContext dataContext, UserManager<ApplicationUser> userManager)
        {
            _dataContext = dataContext;
            _userManager = userManager;
        }

        public async Task<ICollection<ApplicationUser>> GetAllAsync()
        {
            var instructors = new List<ApplicationUser>();
            var users = await _dataContext.Users.ToListAsync();

            foreach (var user in users)
            {
                if (await _userManager.IsInRoleAsync(user, RoleNames.Instructor))
                {
                    instructors.Add(user);
                }
            }
            return instructors;
        }

        // Get an instructor by ID
        public async Task<ApplicationUser> GetByIdAsync(string id)
        {
            return await _dataContext.Users
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task UpdateAsync(ApplicationUser user)
        {
            _dataContext.Users.Update(user);
            await _dataContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(string id)
        {
            var user = await _dataContext.Users.FindAsync(id);
            if (user != null)
            {
                _dataContext.Users.Remove(user);
                await _dataContext.SaveChangesAsync();
            }
        }

        public async Task SaveAsync()
        {
            await _dataContext.SaveChangesAsync();
        }
    }
}