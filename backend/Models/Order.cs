namespace WEBAPIapp.Models;

public class Order
{
    public int Id { get; set; }
    
    public DateOnly Date { get; set; }

    public string? Title { get; set; }

    public int Price { get; set; }
}
