using BankAccount.API.Helpers;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace BankAccount.API.Controllers;

/// <summary>
/// Тестовые данные пользователя
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class AuthTestController(IHttpClientFactory httpClientFactory, AuthenticationConfiguration configuration)
    : ControllerBase
{
    /// <summary>
    /// Получить тестовый токен доступа
    /// </summary>
    /// <returns></returns>
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [HttpGet("token")]
    public async Task<IActionResult> GetAccessToken()
    {
        var client = httpClientFactory.CreateClient();

        var url = $"{configuration.UrlKeycloak}/protocol/openid-connect/token";

        var formData = new Dictionary<string, string>
        {
            { "client_id", configuration.ValidAudience },
            { "client_secret", configuration.SecretKey },
            { "grant_type", "password" },
            { "username", "test-user" },
            { "password", "test-user" }
        };

        var content = new FormUrlEncodedContent(formData);

        var response = await client.PostAsync(url, content);

        var responseString = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
            return BadRequest(responseString);

        var jsonDoc = JsonDocument.Parse(responseString);
        var accessToken = jsonDoc.RootElement.GetProperty("access_token").GetString();

        return Ok(new { access_token = accessToken });
    }
}
