namespace ConstructionSite.Services.Authentication.Application.DTOs;

public record UserDto(Ulid Id, string UserName, string Email, string Name, string Surname);
