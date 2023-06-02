using System.Text.Json.Serialization;

namespace CRMCQRS.Application.Dto.Account;

public class TokenDto
{
    [JsonPropertyName("access_token")]
    public string AccessToken { get; set; }
    [JsonPropertyName("refresh_token")]
    public string RefreshToken { get; set; }
}