using BackendLi.Entities;
using Microsoft.EntityFrameworkCore;

namespace BackendLi.Helpers;

public class DataContext : DbContext
{
    private readonly string connectionString;

    public DataContext(IConfiguration configuration)
    {
    }

    public DataContext(string connectionString)
    {
        this.connectionString = connectionString;
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Photo> Images { get; set; }
    public DbSet<Token> Token { get; set; }
    public DbSet<Follow> Follow { get; set; }
    public DbSet<Gallery> Gallery { get; set; }
    public DbSet<ChatMessage> Chat { get; set; }
    public DbSet<ChatMessageSender> ChatSender { get; set; }
    public DbSet<Location> Location { get; set; }
    public DbSet<Notification> Notifications { set; get; }
    public DbSet<Report> Reports{ set; get; }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
    }
}