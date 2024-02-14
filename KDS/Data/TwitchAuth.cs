using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Authentication;

namespace KDS.Data;

public class TwitchAuth
{
    
    private TwitchAuth()
    {
    }
    
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public ulong Id { get; set; }
    
    public string AccessToken { get; set; } = null!;
    public string RefreshToken { get; set; } = null!;
    public bool IsElevated { get; set; }
    public bool IsExpired => ExpiresAt < DateTimeOffset.UtcNow;
    public DateTimeOffset ExpiresAt { get; set; }
    public ulong ChannelId { get; set; }

    public static TwitchAuth FromAuthTokens(IEnumerable<AuthenticationToken> tokens, ulong channelId, bool isElevated = false)
    {
        var auth = new TwitchAuth();
        foreach (var token in tokens)
        {
            if (token.Name == "access_token")
            {
                auth.AccessToken = token.Value;
            }
            else if (token.Name == "refresh_token")
            {
                auth.RefreshToken = token.Value;
            }
            else if (token.Name == "expires_at")
            {
                auth.ExpiresAt = DateTimeOffset.TryParse(token.Value, out var expiresAt)
                    ? expiresAt
                    : throw new InvalidOperationException("Invalid expires_at value");
            }
        }

        auth.IsElevated = isElevated;
        auth.ChannelId = channelId;
        return auth;
    }
}