using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KDS.Data;

// Add profile data for application users by adding properties to the ApplicationUser class
public class ApplicationUser : IdentityUser
{
    public ulong TwitchId { get; set; }
    
    public TwitchAuth? TwitchAuth { get; set; }
}

public class ApplicationUserConfig : IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        builder.HasOne(x => x.TwitchAuth)
            .WithOne()
            .HasForeignKey<TwitchAuth>(x => x.ChannelId)
            .HasPrincipalKey<ApplicationUser>(x => x.TwitchId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}