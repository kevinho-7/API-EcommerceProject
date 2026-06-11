
public class User
{
    public Guid id {get; set;}
    public string? first_name {get; set;}
    public string? last_name {get; set;}
    public string? email {get; set;}
    public string? password_hash {get; set;}
    public string? auth_provider {get; set;}
    public string? cep {get; set;}
    public string? street {get; set;}
    public string? neighborhood {get; set;}
    public string? complement {get; set;}
    public string? city {get; set;}
    public string? state {get; set;}
    public DateTime? created_at {get; set;}
    public string? user_role {get; set;}   
}