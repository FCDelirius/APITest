using System.Collections.Concurrent;
using WebApi.Models;

namespace WebApi.Repositories
{
    public class InMemoryUserRepository : IUserRepository
    {
        private readonly ConcurrentDictionary<Guid, User> _users = new();

        public InMemoryUserRepository()
        {
            // Seed one sample user
            var sample = new User
            {
                Id = Guid.NewGuid(),
                FirstName = "Sample",
                LastName = "User",
                Email = "sample@example.com",
                DateOfBirth = null
            };
            _users[sample.Id] = sample;
        }

        public Task<User> CreateAsync(User user, CancellationToken cancellationToken = default)
        {
            _users[user.Id] = user;
            return Task.FromResult(user);
        }

        public Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(_users.TryRemove(id, out _));
        }

        public Task<IEnumerable<User>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return Task.FromResult<IEnumerable<User>>(_users.Values.ToArray());
        }

        public Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            _users.TryGetValue(id, out var user);
            return Task.FromResult(user);
        }

        public Task<bool> UpdateAsync(User user, CancellationToken cancellationToken = default)
        {
            if (!_users.ContainsKey(user.Id))
            {
                return Task.FromResult(false);
            }

            _users[user.Id] = user;
            return Task.FromResult(true);
        }
    }
}