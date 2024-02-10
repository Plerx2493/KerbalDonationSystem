using KDS.Data;
using Microsoft.EntityFrameworkCore;

namespace KDS.Services;

public class DonationService
{
    private IDbContextFactory<ApplicationDbContext> _dbContextFactory;

    public DonationService(IDbContextFactory<ApplicationDbContext> contextFactory)
    {
        _dbContextFactory = contextFactory;
    }

    public async Task<IReadOnlyList<Donation>> GetDonationsAfterAsync(ulong id, DateTime time)
    {
        await using var db = await _dbContextFactory.CreateDbContextAsync();
        
        return db.Donations.Where(x => x.ChannelId == id).Where(y => y.CreatedAt > time).ToList();
    }

    public async Task<IReadOnlyList<Donation>> GetDonationsByChannelAsync(ulong id)
    {
        await using var db = await _dbContextFactory.CreateDbContextAsync();
        
        return db.Donations.Where(x => x.ChannelId == id).ToList();
    }
    
    public async Task<IReadOnlyList<Donation>> GetDonationsByUserAsync(ulong id, ulong? channelId = null)
    {
        await using var db = await _dbContextFactory.CreateDbContextAsync();
        
        if (channelId.HasValue)
        {
            return db.Donations.Where(x => x.UserId == id && x.ChannelId == channelId).ToList();
        }
        
        return db.Donations.Where(x => x.UserId == id).ToList();
    }
    
    public async Task RemoveAllDonationsFromUserAsync(ulong id)
    {
        await using var db = await _dbContextFactory.CreateDbContextAsync();
        
        var donations = db.Donations.Where(x => x.UserId == id);
        db.Donations.RemoveRange(donations);
        await db.SaveChangesAsync();
    }
    
    public async Task RemoveAllDonationsFromChannelAsync(ulong id)
    {
        await using var db = await _dbContextFactory.CreateDbContextAsync();
        
        var donations = db.Donations.Where(x => x.ChannelId == id);
        db.Donations.RemoveRange(donations);
        await db.SaveChangesAsync();
    }

    public async Task AddDonationAsync(string username, ulong userId, ulong channelId, int value, int amount)
    {
        await using var db = await _dbContextFactory.CreateDbContextAsync();
        
        var donation = new Donation()
        {
            Username = username,
            UserId = userId,
            ChannelId = channelId,
            Value = value,
            Amount = amount,
            CreatedAt = DateTime.UtcNow
        };
        
        await db.Donations.AddAsync(donation);
        await db.SaveChangesAsync();
    }
}