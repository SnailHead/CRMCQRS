using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;

namespace CRMCQRS.Identity.Application.Dto;

public class TokenDto
{
    [JsonPropertyName("grant_type")]
    [FromForm(Name = "grant_type")]
    public string GrantType { get; set; }
    [JsonPropertyName("scope")]
    [FromForm(Name = "scope")]
    public List<string> Scope { get; set; }
    [JsonPropertyName("username")]
    [FromForm(Name = "username")]
    public string UserName { get; set; }
    [JsonPropertyName("password")]
    [FromForm(Name = "password")]
    public string Password { get; set; }
    [JsonPropertyName("client_id")]
    [FromForm(Name = "client_id")]
    public string ClientId { get; set; }
    [JsonPropertyName("client_secret")]
    [FromForm(Name = "client_secret")]
    public string ClientSecret { get; set; }
}