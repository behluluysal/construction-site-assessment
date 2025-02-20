namespace ConstructionSite.Services.Activity.Domain.Entities;

public class Activity
{
    public Ulid Id { get; set; } = Ulid.NewUlid();
    public DateTime ActivityDate { get; set; }
    public virtual Ulid ActivityTypeId { get; set; }
    public virtual ActivityType ActivityType { get; set; } = default!;
    public string Description { get; set; } = string.Empty;
    public string Worker { get; set; } = string.Empty;
}
