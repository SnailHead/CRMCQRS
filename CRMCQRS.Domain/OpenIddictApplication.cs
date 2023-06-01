using OpenIddict.EntityFrameworkCore.Models;

namespace CRMCQRS.Domain;

public class OpenIddictApplication : OpenIddictEntityFrameworkCoreApplication<Guid, OpenIddictAuthorization, OpenIddictToken>
{
}