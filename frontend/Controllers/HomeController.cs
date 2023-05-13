using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MVCapp.Models;
using Newtonsoft.Json;
// using System.Net.Http;
// using System.Net.Http.Json;

namespace MVCapp.Controllers;

// https://learn.microsoft.com/en-us/aspnet/mvc/overview/performance/using-asynchronous-methods-in-aspnet-mvc-4
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    // static  HttpClient client = new HttpClient();
    static  readonly string baseServerURL = "http://localhost:5091/";

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    // private async Task<List<OrderViewModel>> GetOrders()
    // {
    //     List<OrderViewModel> result = new List<OrderViewModel>();
    //     HttpClient client = new HttpClient();
    //     client.BaseAddress = new Uri(baseServerURL);
    //     client.DefaultRequestHeaders.Clear();

    //     HttpResponseMessage response = await client.GetAsync("order");
    //     if (response.IsSuccessStatusCode)
    //     {
    //         // product = await response.Content.ReadAsAsync<IList<OrderViewModel>>();
    //         var json = await response.Content.ReadAsStringAsync();
    //         _logger.LogInformation("Result: {}", json);

    //         // либо же использовать это:
    //         // https://stackoverflow.com/questions/63108280/has-httpcontent-readasasynct-method-been-superceded-in-net-core
    //         // (System.Text.Json)
    //         var jr = JsonConvert.DeserializeObject<List<OrderViewModel>>(json);
    //         if (jr != null) result = jr; else {
    //             _logger.LogInformation("JSON Parse Error: {}", json);
    //         }
    //     } else _logger.LogInformation("Server Error!");
    //     return result;
    // }

    // public async Task<IActionResult> Index()
    // {
    //     // лучше IEnumerable использовать
    //     var orders = await GetOrders();
    //     // ModelState.AddModelError
    //     return View(orders);
    // }

    public IActionResult Index()
    {
        // https://www.tutorialsteacher.com/webapi/consume-web-api-get-method-in-aspnet-mvc
        IEnumerable<OrderViewModel> orders = Enumerable.Empty<OrderViewModel>();

        using (var client = new HttpClient())
        {
            client.BaseAddress = new Uri(baseServerURL);
            var responseTask = client.GetAsync("order/getall");
            responseTask.Wait();

            var result = responseTask.Result;
            if (result.IsSuccessStatusCode)
            {
                var json = result.Content.ReadAsStringAsync();
                json.Wait();
                _logger.LogInformation("Result: {}", json);

                var jr = JsonConvert.DeserializeObject<List<OrderViewModel>>(json.Result);
                if (jr != null) orders = jr; else {
                    _logger.LogInformation("JSON Parse Error: {}", json);
                }
            } else {
                orders = Enumerable.Empty<OrderViewModel>();

                ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
            }
        }
        return View(orders);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    public IActionResult Delete(int id)
    {
        _logger.LogInformation("Deleting {}", id);

        using (var client = new HttpClient())
        {
            client.BaseAddress = new Uri(baseServerURL);
            var responseTask = client.DeleteAsync($"order/delete/{id}");
            responseTask.Wait();

            var result = responseTask.Result;
            if (result.IsSuccessStatusCode)
            {
                var json = result.Content.ReadAsStringAsync();
                json.Wait();
                _logger.LogInformation("Delete Result: {}", json);
            } else {
                _logger.LogInformation("Bad Delete");
                ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
            }
        }

        return RedirectToAction("Index", "");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
