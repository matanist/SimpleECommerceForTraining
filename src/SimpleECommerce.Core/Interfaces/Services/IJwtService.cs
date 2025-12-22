using SimpleECommerce.Core.Entities;

namespace SimpleECommerce.Core.Interfaces.Services;

public interface IJwtService
{
    string GenerateToken(User user);
    int? ValidateToken(string token);
}
