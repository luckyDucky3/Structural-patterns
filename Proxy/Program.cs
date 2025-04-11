using Microsoft.EntityFrameworkCore;

namespace Proxy;

public class AppDbContext : DbContext
{
    public DbSet<User?> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(Constants.DATABASE);
    }
}

public interface IUserService
{
    User? GetUserById(int id);
}

public class UserService : IUserService
{
    private readonly AppDbContext _db;

    public UserService(AppDbContext db)
    {
        _db = db;
    }

    public User? GetUserById(int id)
    {
        Console.WriteLine($"[Database] Запрос пользователя с ID {id}");
        return _db.Users.FirstOrDefault(u => u.Id == id);
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

    public User? GetUserById(int id)
    {
        if (_cache.TryGetValue(id, out var cachedUser))
        {
            Console.WriteLine($"[Proxy] Пользователь {id} взят из кэша");
            return cachedUser;
        }

        var user = _userService.GetUserById(id);
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
    private static void GetUser(CachingUserServiceProxy proxy, int userId)
    {
        Console.WriteLine($"User {userId}: {proxy.GetUserById(userId)?.Name}");
        Console.WriteLine();
    }
    
    
    
    static void Main()
    {
        using var db = new AppDbContext();
        
        // db.Database.EnsureCreated();
        //
        // db.Users.Add(new User { Id = 1, Name = "Alice", Email = "alice@example.com" });
        // db.Users.Add(new User { Id = 2, Name = "Bob", Email = "bob@example.com" });
        // db.SaveChanges();
        
        var realService = new UserService(db);
        var cachingProxy = new CachingUserServiceProxy(realService);
        
        GetUser(cachingProxy, 1);
        GetUser(cachingProxy, 1);
        GetUser(cachingProxy, 2);
    }
}
