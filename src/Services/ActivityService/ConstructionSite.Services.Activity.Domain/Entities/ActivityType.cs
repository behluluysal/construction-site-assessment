namespace ConstructionSite.Services.Activity.Domain.Entities;

public class ActivityType
{
    public Ulid Id { get; set; } = Ulid.NewUlid();
    public string Name { get; set; } = string.Empty;
}
