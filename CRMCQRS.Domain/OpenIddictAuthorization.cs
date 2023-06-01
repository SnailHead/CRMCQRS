using OpenIddict.EntityFrameworkCore.Models;

namespace CRMCQRS.Domain;

public class OpenIddictAuthorization : OpenIddictEntityFrameworkCoreAuthorization<Guid, OpenIddictApplication, OpenIddictToken>
{
}