namespace KDS.Services.ChannelManager;

public class ChannelPointRewards
{
    /// <summary>
    /// Twitch Channel Point Reward ID
    /// </summary>
    public string RewardId { get; set; }
    
    /// <summary>
    /// Twitch Channel Point Reward Name
    /// </summary>
    public string RewardName { get; set; }
    
    /// <summary>
    /// Twitch Channel Point Reward Prompt
    /// </summary>
    public string RewardPrompt { get; set; }
    
    /// <summary>
    /// Twitch Channel Point Reward Cost
    /// </summary>
    public int RewardCost { get; set; }
    
    /// <summary>
    /// Twitch Channel Point Reward Value (ksp money)
    /// </summary>
    public int RewardValue { get; set; }
    
    /// <summary>
    /// Twitch Channel Point Reward Is Enabled
    /// </summary>
    public bool RewardIsEnabled { get; set; }
    
}