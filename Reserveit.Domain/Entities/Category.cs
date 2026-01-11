namespace Reserveit.Domain.Entities;

public class Category
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;

    public ICollection<Business> Businesses { get; set; } = new List<Business>();
}
