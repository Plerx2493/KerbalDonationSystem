

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KDS.Services.Donations;
using Microsoft.EntityFrameworkCore;

public class DonationService
{
    private IDbContextFactory<DonationContext> _dbContextFactory;

    public DonationService(IDbContextFactory<DonationContext> contextFactory)
    {
        _dbContextFactory = contextFactory;
    }

    public async Task<IEnumerable<Donation>> GetDonationsAfterAsync(string channelName, DateTime time)
    {
        await using var db = await _dbContextFactory.CreateDbContextAsync();
        
        return db.Donations.Where(x => x.ChannelName == channelName && x.CreatedAt > time).AsEnumerable();
    }

    public async Task<IEnumerable<Donation>> GetDonationsByChannelAsync(string channelName)
    {
        await using var db = await _dbContextFactory.CreateDbContextAsync();
        
        return db.Donations.Where(x => x.ChannelName == channelName).AsEnumerable();
    }
    
    public async Task<IEnumerable<Donation>> GetDonationsByUserAsync(string username)
    {
        await using var db = await _dbContextFactory.CreateDbContextAsync();
        
        return db.Donations.Where(x => x.User == username).AsEnumerable();
    }

    public async Task AddDonationAsync(string user, string channelname, int amount)
    {
        await using var db = await _dbContextFactory.CreateDbContextAsync();
        
        var donation = new Donation()
        {
            Amount = amount,
            ChannelName = channelname,
            User = user,
            CreatedAt = DateTime.UtcNow
        };
        
        await db.Donations.AddAsync(donation);
        await db.SaveChangesAsync();
    }
}