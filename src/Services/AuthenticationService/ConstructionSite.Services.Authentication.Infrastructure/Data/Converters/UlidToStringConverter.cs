using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace ConstructionSite.Services.Authentication.Infrastructure.Data.Converters;

public class UlidToStringConverter : ValueConverter<Ulid, string>
{
    public UlidToStringConverter()
        : base(
            v => v.ToString(), // Convert Ulid to string
            v => Ulid.Parse(v)  // Convert string to Ulid
        )
    {
    }
}
