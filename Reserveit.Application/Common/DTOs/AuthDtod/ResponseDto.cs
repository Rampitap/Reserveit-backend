namespace Reserveit.Application.Common.DTOs.AuthDtod;

public class ResponseDto
{
    public Guid Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public IList<string> Roles { get; set; } = new List<string>();
}
