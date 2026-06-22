public class Admin
{
    public Guid id {get; set;}
    public string? first_name {get; set;}
    public string? last_name {get; set;}
    public string? email {get; set;}
    public string? password {get; set;}
    public string? cep {get; set;}
    public string? street {get; set;}
    public string? neighborhood {get; set;}
    public string? complement {get; set;}
    public string? city {get; set;}
    public string? state {get; set;}
    public DateTime created_at {get; set;} = DateTime.UtcNow;
    public string? role {get; set;} = "Admin";
}