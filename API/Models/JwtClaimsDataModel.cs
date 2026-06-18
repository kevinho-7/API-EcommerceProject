public class JwtClaimsData
{
    public Guid id {get; set;}
    public string? email {get; set;}
    public string? password {get; set;}
    public Role? role {get; set;}
}