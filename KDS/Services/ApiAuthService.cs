using System.Collections.Concurrent;
using KDS.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace KDS.Services;

public class ApiAuthService
{
    private ConcurrentDictionary<string, ApiAuth> ApiAuths { get; } = new();
    private UserManager<ApplicationUser> UserManager { get; }
    private IDbContextFactory<ApplicationDbContext> ContextFactory { get; }
    
    public ApiAuthService(UserManager<ApplicationUser> userManager, IDbContextFactory<ApplicationDbContext> contextFactory)
    {
        UserManager = userManager;
        ContextFactory = contextFactory;
    }
    
    public async Task<ApiAuth?> GetApiAuthAsync(ApplicationUser user)
    {
        if (ApiAuths.TryGetValue(user.Id, out var cachedApiAuth))
            return cachedApiAuth;
        
        await using var context = await ContextFactory.CreateDbContextAsync();
       
        var apiAuth = await context.ApiAuths
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.UserId == user.Id);
        
        if (apiAuth is null)
            return null;
        
        ApiAuths[user.Id] = apiAuth;
        return apiAuth;
    }
    
    public async Task<ApiAuth> CreateApiAuthAsync(ApplicationUser user)
    {
        var apiAuth = new ApiAuth
        {
            UserId = user.Id,
            ApiKey = Guid.NewGuid().ToString().Replace("-", ""),
            CreatedAt = DateTime.UtcNow
        };
        
        await using var context = await ContextFactory.CreateDbContextAsync();
        
        var old = context.ApiAuths.FirstOrDefault(x => x.UserId == user.Id);
        if (old is not null)
            context.ApiAuths.Remove(old);
        
        context.ApiAuths.Add(apiAuth);
        await context.SaveChangesAsync();
        
        ApiAuths[user.Id] = apiAuth;
        return apiAuth;
    }
    
    public async Task<bool> DeleteApiAuthAsync(ApplicationUser user)
    {
        await using var context = await ContextFactory.CreateDbContextAsync();
        
        var apiAuth = await context.ApiAuths
            .FirstOrDefaultAsync(x => x.UserId == user.Id);
        
        if (apiAuth is null)
            return false;
        
        context.ApiAuths.Remove(apiAuth);
        await context.SaveChangesAsync();
        
        ApiAuths.TryRemove(user.Id, out _);
        return true;
    }
    
    public async Task<bool> CheckApiKeyForChannelAsync(ulong channelId, string apiKey)
    {
        await using var context = await ContextFactory.CreateDbContextAsync();
        
        var user = await context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.TwitchId == channelId);
        
        if (user is null)
            return false;
        
        var apiAuth = await context.ApiAuths
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.UserId == user.Id);
        
        if (apiAuth is null)
            return false;
        
        return apiAuth.ApiKey == apiKey;
    }
}