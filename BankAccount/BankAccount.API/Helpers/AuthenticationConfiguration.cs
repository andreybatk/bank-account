// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global Используется для Configuration.Bind который устанавливает значения через сеттеры
namespace BankAccount.API.Helpers;

public class AuthenticationConfiguration
{
    public string UrlKeycloak { get; set; } = string.Empty;
    public string ValidIssuer { get; set; } = string.Empty;
    public string ValidAudience { get; set; } = string.Empty;
    public string SecretKey { get; set; } = string.Empty;
}
