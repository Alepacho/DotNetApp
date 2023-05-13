using Microsoft.AspNetCore.Mvc;
using WEBAPIapp.Models;

namespace WEBAPIapp.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class OrderController : ControllerBase
{
    private static readonly string[] Titles = new[]
    {
        "Суп", "Суши", "Роллы", "Вок", "Салат", "Бургер", "Сашими"
    };

    private readonly ILogger<OrderController> _logger;
    private readonly OrderContext _db;

    public OrderController(ILogger<OrderController> logger, OrderContext db)
    {
        _logger = logger;
        _db     = db;
    }

    [HttpPost()]
    public async Task<ActionResult> Post([FromBody] string value) 
    {
        try {
            return Ok(await Task.Run(() => {
                var newOrder = new Order
                {
                    Id    = 0,
                    Date  = DateOnly.FromDateTime(DateTime.Now.AddDays(0)),
                    Title = Titles[Random.Shared.Next(Titles.Length)],
                    Price = Random.Shared.Next(99, 1999)
                };

                _db.Orders.Add(newOrder);
                _db.SaveChanges();
                return newOrder;
            }));
        } catch (Exception e) {
            return StatusCode(StatusCodes.Status500InternalServerError, $"{e.Message}");
        }
    }

    // [HttpGet(Name = "GetOrders")]
    [HttpGet]
    public async Task<ActionResult> GetAll() // 
    {
        try {
            return Ok(await Task.Run(() => {
                return _db.Orders.ToList();
                // return Enumerable.Range(1, 5).Select(index => new Order
                // {
                //     Id    = index,
                //     Date  = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                //     Title = Titles[Random.Shared.Next(Titles.Length)],
                //     Price = Random.Shared.Next(99, 1999)
                // })
                // .ToArray();
            }));
        } catch (Exception e) {
            return StatusCode(StatusCodes.Status500InternalServerError, $"{e.Message}");
        }
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult> GetByID(int id) // 
    {
        try {
            var data = _db.Orders.First(item => item.Id == id);
            if (data != null) {
                return Ok(await Task.Run(() => { return data; }));
            } else return NotFound($"Order with ID {id} not found");
        } catch (Exception e) {
            return StatusCode(StatusCodes.Status500InternalServerError, $"{e.Message}");
        }
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult<Order>> Delete(int id)
    {
        _logger.LogInformation("Deleting order {}", id);
        try {
            var data = _db.Orders.First(item => item.Id == id);
            if (data != null) {
                _db.Orders.Remove(data);
                _db.SaveChanges();
                return Ok(await Task.Run(() => { return data; }));
            } else return NotFound($"Order with ID {id} not found");
            
            
        } catch (Exception e) {
            return StatusCode(StatusCodes.Status500InternalServerError, $"{e.Message}");
        }
    }
}
