namespace Reserveit.Application.Common.DTOs.ServiceDtos;

public sealed class UpdateServiceDto
{
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public decimal? Price { get; set; }
    public int DurationMinutes { get; set; }
    public bool IsActive { get; set; } = true; 
}
