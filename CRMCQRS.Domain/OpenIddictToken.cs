using OpenIddict.EntityFrameworkCore.Models;

namespace CRMCQRS.Domain;

public class OpenIddictToken: OpenIddictEntityFrameworkCoreToken<Guid, OpenIddictApplication, OpenIddictAuthorization>
{
}