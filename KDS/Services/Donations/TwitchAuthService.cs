using System.Collections.Concurrent;
using KDS.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using TwitchLib.Api;

namespace KDS.Services.Donations;

public class TwitchAuthService
{
    private IDbContextFactory<ApplicationDbContext> _contextFactory;
    private TwitchAPI _twitchApi;
    private string _clientId;
    private string _clientSecret;
    
    private ConcurrentDictionary<ulong, TwitchAPI> _twitchApis = new();

    public TwitchAuthService(IDbContextFactory<ApplicationDbContext> contextFactory, TwitchAPI twitchApi, IConfiguration configuration)
    {
        _contextFactory = contextFactory;
        _twitchApi = twitchApi;
        
        _clientId = configuration["Authentication:Twitch:ClientId"] ??
                    throw new InvalidOperationException("Twitch ClientId not found.");
        
        _clientSecret = configuration["Authentication:Twitch:ClientSecret"] ?? 
                        throw new InvalidOperationException("Twitch ClientSecret not found.");
    }
    
    public async Task<TwitchAuth> GetAuth(ulong channelId)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();
        var auth = await context.TwitchAuths.FirstOrDefaultAsync(a => a.ChannelId == channelId);
        
        if (auth is null)
        {
            throw new KeyNotFoundException("TwitchAuth not found");
        }
        
        if (auth.ExpiresAt < DateTimeOffset.UtcNow)
        {
            var token = await _twitchApi.Auth.RefreshAuthTokenAsync(auth.RefreshToken, _clientSecret , _clientId);
            auth.AccessToken = token!.AccessToken;
            auth.RefreshToken = token!.RefreshToken;
            auth.ExpiresAt = DateTimeOffset.UtcNow.AddSeconds(token.ExpiresIn);
            await context.SaveChangesAsync();
        }

        return auth;
    }
    
    public async Task SetAuth(ulong channelId, TwitchAuth auth)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();
        var existentAuth = await context.TwitchAuths.FirstOrDefaultAsync(a => a.ChannelId == channelId);
        
        if (existentAuth is null)
        {
            existentAuth = auth;
            await context.TwitchAuths.AddAsync(existentAuth);
        }
        else
        {
            existentAuth.AccessToken = auth.AccessToken;
            existentAuth.RefreshToken = auth.RefreshToken;
            existentAuth.ExpiresAt = auth.ExpiresAt;
        }
        
        await context.SaveChangesAsync();
    }
    
    public async Task SetAuth(ulong channelId, IEnumerable<AuthenticationToken> tokens)
    {
        var auth = TwitchAuth.FromAuthTokens(tokens, channelId);
        await SetAuth(channelId, auth);
    }
    
    public TwitchAPI GetTwitchApi(ulong channelId)
    {
        if (_twitchApis.TryGetValue(channelId, out var api))
        {
            return api;
        }
        
        var auth = GetAuth(channelId).Result;
        var newApi = new TwitchAPI();
        newApi.Settings.AccessToken = auth.AccessToken;
        _twitchApis[channelId] = newApi;
        return newApi;
    }
}