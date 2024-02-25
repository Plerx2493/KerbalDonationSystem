using KDS.Data;
using KDS.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using IResult = Microsoft.AspNetCore.Http.IResult;

namespace KDS.Components.Donations;

internal static class DonationEndpointRouteBuilderExtensions
{
    public static IEndpointConventionBuilder MapAdditionalDonationEndpoints(this IEndpointRouteBuilder endpoints)
    {
        ArgumentNullException.ThrowIfNull(endpoints);
        var donationsGroup = endpoints.MapGroup("/Donations/").AllowAnonymous();

        donationsGroup.MapGet("/{channelId}",
            async (
            HttpContext context,
            [FromServices] DonationService service,
            [FromServices] ApiAuthService apiAuth,
            [FromRoute] ulong channelId,
            [FromQuery] DateTime? after
            ) =>
            {
                context.Request.Headers.TryGetValue("x-api-key", out var authHeader);

                if (authHeader.Count != 1)
                    return Results.Forbid();

                var apiKey = authHeader[0]!;
                var isAuthed = await apiAuth.CheckApiKeyForChannelAsync(channelId, apiKey);

                if (!isAuthed)
                    return Results.Forbid();

                IReadOnlyList<Donation> donations;

                if (after is null)
                {
                    donations = await service.GetDonationsByChannelAsync(channelId);
                    return Results.Ok(donations);
                }

                donations = await service.GetDonationsAfterAsync(channelId, after.Value);
                return Results.Ok(donations);
            });

        donationsGroup.MapGet("/{channelId}/test",
            async Task<IResult> (HttpContext context, [FromServices] DonationService service,
            [FromServices] ApiAuthService apiAuth, [FromRoute] ulong channelId) =>
            {
                context.Request.Headers.TryGetValue("x-api-key", out var authHeader);

                if (authHeader.Count != 1)
                    return Results.Forbid();

                var apiKey = authHeader[0]!;
                var isAuthed = await apiAuth.CheckApiKeyForChannelAsync(channelId, apiKey);

                if (!isAuthed)
                    return Results.Forbid();

                await service.AddDonationAsync("plerxTest", 123, channelId, 100, 1);
                return Results.Ok();
            });

        return donationsGroup;
    }

    public static IEndpointConventionBuilder MapAdditionalChannelRewardsEndpoints(this IEndpointRouteBuilder endpoints)
    {
        ArgumentNullException.ThrowIfNull(endpoints);
        var channelRewardsGroup = endpoints.MapGroup("/Donations/").RequireAuthorization();
        
        channelRewardsGroup.MapGet("/{channelId}/rewards",
            async (
            HttpContext context,
            [FromServices] ChannelManagerService service,
            [FromServices] UserManager<ApplicationUser> userManager,
            [FromRoute] ulong channelId
            ) =>
            {
                var user = context.User;
                var appUser = await userManager.GetUserAsync(user);
                if (appUser is null)
                    return Results.Forbid();
                
                if (appUser.TwitchId != channelId)
                    return Results.Forbid();

                var rewards = await service.GetChannelPointRewardsAsync(channelId);
                return Results.Ok(rewards);
            });
        
        channelRewardsGroup.MapPost("/{channelId}/rewards",
            async (
            HttpContext context,
            [FromServices] ChannelManagerService service,
            [FromServices] UserManager<ApplicationUser> userManager,
            [FromRoute] ulong channelId,
            [FromForm] ChannelPointReward reward
            ) =>
            {
                var user = context.User;
                var appUser = await userManager.GetUserAsync(user);
                if (appUser is null)
                    return Results.Forbid();
                
                if (appUser.TwitchId != channelId)
                    return Results.Forbid();

                await service.CreateRewardAsync(channelId, reward.Name, reward.Prompt, reward.Cost, reward.Value, reward.Timeout);
                return Results.Ok();
            });
        
        
        
        return channelRewardsGroup;
    }

}