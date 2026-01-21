namespace Reserveit.Domain.Constants;

public sealed class SmtpOptions
{
    public string Host { get; set; } = null!;
    public int Port { get; set; } = 587;
    public string User { get; set; } = null!;
    public string Password { get; set; } = null!;
    public bool UseSsl { get; set; } = true;
}
