namespace API.Models;

public class Category
{
    public Guid id {get; set;}
    public string? name {get; set;}

    public Guid company_id {get; set;}
    public Company? Company {get; set;}
}