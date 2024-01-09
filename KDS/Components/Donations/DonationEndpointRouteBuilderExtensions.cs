using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using KDS.Services.Donations;

namespace KDS.Components.Donations;

internal static class DonationEndpointRouteBuilderExtensions{

     public static IEndpointConventionBuilder MapAdditionalDonationEndpoints(this IEndpointRouteBuilder endpoints)
    {
        ArgumentNullException.ThrowIfNull(endpoints);
        var donationsGroup = endpoints.MapGroup("/Donations");

        donationsGroup.MapGet("/{channel}/after", [AllowAnonymous, ] ([FromServices] DonationService service, string channel, DateTime time) => {
            var donations = await service.GetDonationsAfterAsync(channel, time);
            var donationDtos = donations.Select(x => x.ToDto()).ToList();
            return new ActionResult<IEnumerable<Donation>>(donationDtos);
        });

        return donationsGroup;
    }
}