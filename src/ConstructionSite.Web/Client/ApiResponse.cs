namespace ConstructionSite.Web.Client;

public class ApiResponse<T>
{
    public bool Succeeded { get; set; }
    public T Data { get; set; } = default!;
    public List<string> Errors { get; set; } = [];
}
