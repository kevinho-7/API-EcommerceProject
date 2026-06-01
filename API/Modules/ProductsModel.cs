namespace API.Models;

public class Products
{
    public string Name {get; set;} = string.Empty;
    public string Description {get; set;} = string.Empty;
    public double Price {get; set;}
    public string Image_path {get; set;} = string.Empty;
    public int Qantity {get; set;}
}