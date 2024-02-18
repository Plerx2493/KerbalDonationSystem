using KDS.Components.Donations.Shared;
using KDS.Data;
using KDS.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace KDS.Components.Donations;

internal static class DonationEndpointRouteBuilderExtensions
{
    public static IEndpointConventionBuilder MapAdditionalDonationEndpoints(this IEndpointRouteBuilder endpoints)
    {
        ArgumentNullException.ThrowIfNull(endpoints);
        var donationsGroup = endpoints.MapGroup("/Donations/").AllowAnonymous();

        donationsGroup.MapGet("/{channelId}",
            async (HttpContext context, [FromServices] DonationService service,
            [FromServices] ApiAuthService apiAuth, [FromRoute] ulong channelId, [FromQuery] DateTime? after) =>
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

        donationsGroup.MapGet("/{channelId}/edit",
            async (HttpContext context,
            [FromRoute] ulong channelId,
            [FromForm] UpdateChannelPointRewardForm form,
            [FromServices] UserManager<ApplicationUser> userManager,
            [FromServices] ChannelManagerService channelManagerService) =>
            {
                var user = await userManager.GetUserAsync(context.User);
                if (user is null)
                {
                    return Results.NotFound($"Unable to load user with ID '{userManager.GetUserId(context.User)}'.");
                }

                await channelManagerService.UpdateRewardAsync(channelId, "test", "test", "test", 1, 1, true);
                return Results.Accepted();
            });

        return donationsGroup;
    }
}