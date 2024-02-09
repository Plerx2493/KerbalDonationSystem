using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;

namespace KDS.Data;

public class TwitchAuth
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public ulong Id { get; set; }
    
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
    public DateTimeOffset ExpiresAt { get; set; }
    
    
    public ulong ChannelId { get; set; }

    public static TwitchAuth FromAuthTokens(IEnumerable<AuthenticationToken> tokens, ulong channelId)
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

        auth.ChannelId = channelId;
        return auth;
    }
}