namespace PD.BL.Helpers.GeminiHelper;

public static class ApiKey
{
    public static string Gemini => Environment.GetEnvironmentVariable("ApiKey");
}