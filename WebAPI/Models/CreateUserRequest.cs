namespace WebAPI.Models;

public class CreateUserRequest
{
    public required string Name { get; set; } = null!;

    public required string Email { get; set; } = null!;
}
