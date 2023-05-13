namespace MVCapp.Models;

public class OrderViewModel
{
    public int Id { get; set; }
    
    public DateOnly Date { get; set; }

    public string? Title { get; set; }

    public int Price { get; set; }
}
