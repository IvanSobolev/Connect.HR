namespace AuthMicroservice.Models.Dtos;

public class AuthOptions
{
    public string Issuer { get; set; }
    public string Audience { get; set; }
    public string Key { get; set; }

    public AuthOptions(string authOptionsRaw)
    {
        if (authOptionsRaw == null)
            throw new InvalidOperationException("AUTH_OPTIONS env var is missing");

        var authOptions = authOptionsRaw
            .Split(';', StringSplitOptions.RemoveEmptyEntries)
            .Select(part => part.Split('='))
            .ToDictionary(pair => pair[0], pair => pair[1]);

        Issuer = authOptions["Issuer"];
        Audience = authOptions["Audience"];
        Key = authOptions["Secret"];
    }
}