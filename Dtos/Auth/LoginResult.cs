using Microsoft.IdentityModel.Tokens;

namespace F_LocalBrand.Dtos.Auth;

public class LoginResult
{
    public bool Authenticated { get; set; }
    public SecurityToken? Token { get; set; }
}