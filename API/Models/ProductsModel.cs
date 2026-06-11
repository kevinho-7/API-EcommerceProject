using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models;

public class Product
{
    public Guid id {get; set;}
    public string? title {get; set;}
    public string? description {get; set;}
    public decimal price {get; set;}
    public string? image_path {get; set;}
    public int quantity {get; set;}

    public Guid category_id {get; set;}
    public Category? Category {get; set;}

    public Guid company_id {get; set;}
    public Company? Company {get; set;}

}