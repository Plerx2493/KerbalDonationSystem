using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KDS.Data;

public class ChannelPointReward
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity), Key]
    public ulong Id { get; set; }
    
    /// <summary>
    /// Id of the channel that the reward is for
    /// </summary>
    public ulong ChannelId { get; set; }
    
    /// <summary>
    /// Twitch Channel Point Reward ID
    /// </summary>
    public string RewardId { get; set; }
    
    /// <summary>
    /// Twitch Channel Point Reward Name
    /// </summary>
    public string Name { get; set; }
    
    /// <summary>
    /// Twitch Channel Point Reward Prompt
    /// </summary>
    public string Prompt { get; set; }
    
    /// <summary>
    /// Twitch Channel Point Reward Cost
    /// </summary>
    public int Cost { get; set; }
    
    /// <summary>
    /// Twitch Channel Point Reward Value (ksp money)
    /// </summary>
    public int Value { get; set; }
    
    /// <summary>
    /// Twitch Channel Point Reward Is Enabled
    /// </summary>
    public bool IsEnabled { get; set; }
    
    /// <summary>
    /// Timeout after reward is redeemed
    /// </summary>
    public int? Timeout { get; set; }
}