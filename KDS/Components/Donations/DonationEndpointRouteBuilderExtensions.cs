using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using KDS.Services.Donations;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

namespace KDS.Components.Donations;

internal static class DonationEndpointRouteBuilderExtensions{

     public static IEndpointConventionBuilder MapAdditionalDonationEndpoints(this IEndpointRouteBuilder endpoints)
    {
        ArgumentNullException.ThrowIfNull(endpoints);
        var donationsGroup = endpoints.MapGroup("/Donations");

        donationsGroup.MapGet("/{channel}/after", [AllowAnonymous] async ([FromServices] DonationService service, [FromRoute] string channel, [FromBody] DateTime time) =>
        {
            var donations = await service.GetDonationsAfterAsync(channel, time);
            return new ActionResult<IEnumerable<Donation>>(donations);
        });

        return donationsGroup;
    }
}