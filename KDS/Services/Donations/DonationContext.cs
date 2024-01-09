using Microsoft.EntityFrameworkCore;

namespace KDS.Services.Donations;

public class DonationContext : DbContext
{
    public DonationContext(DbContextOptions<DonationContext> options)
        : base(options)
    {
    }

    public DbSet<Donation> Donations { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Donation>().ToTable("Donation");
    }
}