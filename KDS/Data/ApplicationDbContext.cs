using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace KDS.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : IdentityDbContext<ApplicationUser>(options)
{
    public DbSet<TwitchAuth> TwitchAuths { get; set; } = null!;
    
    public DbSet<Donation> Donations { get; set; } = null!;
    
    public DbSet<ChannelPointRewards> ChannelPointRewards { get; set; } = null!;
    
    public DbSet<ChannelConfig> ChannelConfigs { get; set; } = null!;
    
    public DbSet<ApiAuth> ApiAuths { get; set; } = null!;
    
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        
        builder.ApplyConfiguration(new ApplicationUserConfig());
    }
}