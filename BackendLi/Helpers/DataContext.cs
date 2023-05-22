using BackendLi.Entities;
using Microsoft.EntityFrameworkCore;

namespace BackendLi.Helpers;


public class DataContext : DbContext
{
    private readonly string connectionString;
    
    public DbSet<User> Users { get; set; }



    public DataContext(IConfiguration configuration) { }

    public DataContext(string connectionString)
    {
        this.connectionString = connectionString;
    }
    
    public DataContext()
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
    } 
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // modelBuilder.Entity<Device>(entity =>
        // {
        //     entity.HasOne(e => e.User)
        //         .WithMany(g => g.Devices)
        //         .HasForeignKey(s => s.UserId)
        //         .HasConstraintName("FK_Devices_Users_UserId");
        //
        //     entity.Property(e => e.UserId)
        //         .IsRequired(false);
        // });
        //
        // modelBuilder.Entity<EnergyConsumptionMapping>(entity =>
        // {
        //     entity.HasOne(e => e.Device)
        //         .WithMany(e => e.EnergyConsumptionMappings)
        //         .HasForeignKey(e => e.DeviceId)
        //         .HasConstraintName("FK_EnergyConsumptionMappins_Devices_DeviceId");
        // });
    }
}