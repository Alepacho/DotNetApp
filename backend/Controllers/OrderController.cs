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

    public OrderController(ILogger<OrderController> logger)
    {
        _logger = logger;
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
                return Enumerable.Range(1, 5).Select(index => new Order
                {
                    Id    = index,
                    Date  = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                    Title = Titles[Random.Shared.Next(Titles.Length)],
                    Price = Random.Shared.Next(99, 1999)
                })
                .ToArray();
            }));
        } catch (Exception e) {
            return StatusCode(StatusCodes.Status500InternalServerError, $"{e.Message}");
        }
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult> GetByID(int id) // 
    {
        try {
            return Ok(await Task.Run(() => {
                return new Order
                {
                    Id    = 0,
                    Date  = DateOnly.FromDateTime(DateTime.Now.AddDays(0)),
                    Title = Titles[Random.Shared.Next(Titles.Length)],
                    Price = Random.Shared.Next(99, 1999)
                };
            }));
        } catch (Exception e) {
            return StatusCode(StatusCodes.Status500InternalServerError, $"{e.Message}");
        }
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult<Order>> Delete(int id)
    {
        // _logger.LogInformation("Deleting order {}", orderId);
        try {
            return NotFound($"Order with ID {id} not found");
        } catch (Exception e) {
            return StatusCode(StatusCodes.Status500InternalServerError, $"{e.Message}");
        }
    }
}
