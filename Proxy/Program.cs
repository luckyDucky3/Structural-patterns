using Microsoft.EntityFrameworkCore;

namespace Proxy;

public class AppDbContext : DbContext
{
    public DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(Constants.DATABASE);
    }
}

public interface IUserService
{
    Task<User?> GetUserByIdAsync(int id);
}

public class UserService : IUserService
{
    private readonly AppDbContext _db;

    public UserService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<User?> GetUserByIdAsync(int id)
    {
        Console.WriteLine($"[Database] Запрос пользователя с ID {id}");
        return await _db.Users.FirstOrDefaultAsync(u => u.Id == id);
    }
}

public class CachingUserServiceProxy : IUserService
{
    private readonly IUserService _userService;
    private readonly Dictionary<int, User> _cache = new();

    public CachingUserServiceProxy(IUserService userService)
    {
        _userService = userService;
    }

    public async Task<User?> GetUserByIdAsync(int id)
    {
        if (_cache.TryGetValue(id, out var cachedUser))
        {
            Console.WriteLine($"[Proxy] Пользователь {id} взят из кэша");
            return cachedUser;
        }

        var user = await _userService.GetUserByIdAsync(id);
        if (user != null)
        {
            _cache[id] = user;
            Console.WriteLine($"[Proxy] Пользователь {id} сохранён в кэш");
        }
        
        return user;
    }
}

class Program
{
    private static async Task GetUser(IUserService proxy, int userId)
    {
        var user = await proxy.GetUserByIdAsync(userId);
        Console.WriteLine($"User {userId}: {user?.Name}");
        Console.WriteLine();
    }
    
    
    
    static async Task Main()
    {
        await using var db = new AppDbContext();

        var realService = new UserService(db);
        var cachingProxy = new CachingUserServiceProxy(realService);
        
        await GetUser(cachingProxy, 1);
        await GetUser(cachingProxy, 1);
        await GetUser(cachingProxy, 2);
    }
}
