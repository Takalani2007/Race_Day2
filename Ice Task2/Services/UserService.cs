using Ice_Task2.Data;
using Ice_Task2.Model;
using Microsoft.EntityFrameworkCore;

namespace Ice_Task2.Services
{
    public class UserService
    {
        private readonly AppDbContext _db;
        public UserService(AppDbContext db)
        {
            _db = db;
        }

        public async Task<List<User>> GetAllUsersAsync() =>
            await _db.Users.AsNoTracking().ToListAsync();

        public async Task<User?> GetUserByIdAsync(int id) =>
            await _db.Users.FirstOrDefaultAsync(u => u.UserId == id);
        public async Task<User?> GetUserByEmailAsync (string email) =>
            await _db.Users.FirstOrDefaultAsync(u =>
                u.Email.ToLower() == email.ToLower());

        public async Task<User> RegisterAsync(User user)
        {
            if (string.IsNullOrWhiteSpace(user.Role))
                user.Role = "User";

            _db.Users.Add(user);
            await _db.SaveChangesAsync();
            return user;
        }

        public async Task<User?> AuthenticateAsync(string email, string password) =>
            await _db.Users.FirstOrDefaultAsync(u =>
                u.Email.ToLower() == email.ToLower() &&
                u.Password == password);

        public async Task<bool> UpdateAsync(int id, User updated)
        {
            var user = await _db.Users.FindAsync(id);
            if (user == null) return false;

            user.FullName = updated.FullName;
            user.Email = updated.Email;
            user.Role = updated.Role;

            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var user = await _db.Users.FindAsync(id);
            if (user == null) return false;

            _db.Users.Remove(user);
            await _db.SaveChangesAsync();
            return true;
        }
    }
}
