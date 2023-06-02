using Blazored.LocalStorage;

namespace CRMCQRS.UI.Application;

public static class SetHeader
{
    public static async Task SetBearerAuth(this HttpClient httpClient, ILocalStorageService storage)
    {
        string value = await storage.GetItemAsStringAsync("access_token");
        httpClient.DefaultRequestHeaders.Authorization = new ("bearer", value.Replace("\"", ""));
    }
}