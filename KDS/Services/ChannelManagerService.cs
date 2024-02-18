using KDS.Data;
using Microsoft.EntityFrameworkCore;
using TwitchLib.Api.Helix.Models.ChannelPoints.CreateCustomReward;
using TwitchLib.Api.Helix.Models.ChannelPoints.UpdateCustomReward;

namespace KDS.Services;

public class ChannelManagerService
{
    private readonly IDbContextFactory<ApplicationDbContext> _dbContextFactory;
    private readonly TwitchAuthService _twitchAuthService;
    
    public ChannelManagerService(IDbContextFactory<ApplicationDbContext> contextFactory, TwitchAuthService twitchAuthService)
    {
        _dbContextFactory = contextFactory;
        _twitchAuthService = twitchAuthService;
    }
    
    public async Task<IReadOnlyList<ChannelPointReward>> GetChannelPointRewardsAsync(ulong channelId)
    {
        await using var db = await _dbContextFactory.CreateDbContextAsync();
        
        return db.ChannelPointRewards.Where(x => x.ChannelId == channelId).ToList();
    }
    
    #region rewards

    public async Task CreateRewardAsync(ulong channelId, string name, string prompt, int cost, int value, int? timeout)
    {
        var api = await _twitchAuthService.GetTwitchApi(channelId);
        var request = new CreateCustomRewardsRequest
        {
            Title = name,
            Cost = cost,
            Prompt = prompt,
            IsEnabled = true
        };
        
        if (timeout.HasValue)
        {
            request.IsGlobalCooldownEnabled = true;
            request.GlobalCooldownSeconds = timeout;
        }
        
        var result = await api.Helix.ChannelPoints.CreateCustomRewardsAsync(channelId.ToString(), request);

        var reward = result?.Data.FirstOrDefault();
        if (reward is null)
        {
            throw new InvalidOperationException("Failed to create reward");
        }
        
        await using var db = await _dbContextFactory.CreateDbContextAsync();

        db.ChannelPointRewards.Add(new ChannelPointReward
        {
            ChannelId = channelId,
            RewardId = reward.Id,
            Cost = cost,
            Value = value,
            IsEnabled = true,
            Name = name,
            Prompt = prompt
        });

        await db.SaveChangesAsync();
    }
    
    public async Task UpdateRewardAsync(ulong channelId, string rewardId, string? name, string? prompt, int? cost, int? timeout, bool? enabled)
    {
        await using var db = await _dbContextFactory.CreateDbContextAsync();
        var reward = db.ChannelPointRewards.FirstOrDefault(x => x.ChannelId == channelId && x.RewardId == rewardId);
        if (reward is null)
        {
            throw new KeyNotFoundException("Reward not found");
        }
        
        var api = await _twitchAuthService.GetTwitchApi(channelId);
        var request = new UpdateCustomRewardRequest();
        
        if (name is not null)
        {
            request.Title = name;
        }
        
        if (prompt is not null)
        {
            request.Prompt = prompt;
        }
        
        if (cost.HasValue)
        {
            request.Cost = cost;
        }
        
        if (timeout.HasValue)
        {
            request.IsGlobalCooldownEnabled = true;
            request.GlobalCooldownSeconds = timeout;
        }
        
        await api.Helix.ChannelPoints.UpdateCustomRewardAsync(channelId.ToString(), rewardId, request);

        if (cost is not null)
        {
            reward.Cost = cost.Value;
        }
        
        if (name is not null)
        {
            reward.Name = name;
        }
        
        reward.IsEnabled = true;
        
        await db.SaveChangesAsync();
    }
    

    #endregion
    
}