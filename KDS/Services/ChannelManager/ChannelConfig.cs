using System.Collections.Generic;

namespace KDS.Services.ChannelManager;

public class ChannelConfig
{
    /// <summary>
    /// Whether or not the channel is configured for use
    /// </summary>
    public bool IsEnabled { get; set; }
    
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
    public string ChannelId { get; set; }
    
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
    public List<ChannelPointRewards> ChannelPointRewards { get; set; }
}