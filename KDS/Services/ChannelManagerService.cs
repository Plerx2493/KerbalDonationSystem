using KDS.Data;
using Microsoft.EntityFrameworkCore;

namespace KDS.Services;

public class ChannelManagerService
{
    IDbContextFactory<ApplicationDbContext> _dbContextFactory;
    
    public ChannelManagerService(IDbContextFactory<ApplicationDbContext> contextFactory)
    {
        _dbContextFactory = contextFactory;
    }
    
    public async Task<IReadOnlyList<ChannelPointRewards>> GetChannelPointRewardsAsync(ulong channelId)
    {
        await using var db = await _dbContextFactory.CreateDbContextAsync();
        
        return db.ChannelPointRewards.Where(x => x.ChannelId == channelId).ToList();
    }
    
}