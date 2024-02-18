using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KDS.Data;

public class ChannelConfig
{
    /// <summary>
    /// Unique Identifier
    /// </summary>
    [DatabaseGenerated(DatabaseGeneratedOption.Identity), Key]
    public ulong Id { get; set; }
    
    /// <summary>
    /// Whether or not moderators are allowed to configure the channel
    /// </summary>
    public bool ModeratorsAllowed { get; set; }
    
    /// <summary>
    /// Twitch Channel Name
    /// </summary>
    public string ChannelName { get; set; }
    
    /// <summary>
    /// Twitch Channel ID
    /// </summary>
    public ulong ChannelId { get; set; }
    
    /// <summary>
    /// Twitch Channel Access Token
    /// </summary>
    public string ChannelAccessToken { get; set; }
    
    /// <summary>
    /// Channel Access Token Expiration
    /// </summary>
    public string ChannelRefreshToken { get; set; }
    
    /// <summary>
    /// List of configured channel point rewards
    /// </summary>
    public List<ChannelPointReward> ChannelPointRewards { get; set; }
}

public class ChannelConfigDbConfig : IEntityTypeConfiguration<ChannelConfig>
{
    public void Configure(EntityTypeBuilder<ChannelConfig> builder)
    {
        builder.ToTable("ChannelConfigs");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.ChannelName).IsRequired();
        builder.Property(x => x.ChannelId).IsRequired();
        builder.Property(x => x.ChannelAccessToken).IsRequired();
        builder.Property(x => x.ChannelRefreshToken).IsRequired();
        builder.HasMany(x => x.ChannelPointRewards).WithOne().HasForeignKey(x => x.ChannelId);
    }
}