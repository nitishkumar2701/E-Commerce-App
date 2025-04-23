using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MVCTest.Models;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace MVCTest.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly HttpClient _httpClient;

    public HomeController(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient();
    }

    // public HomeController(ILogger<HomeController> logger)
    // {
    //     _logger = logger;
    // }

    // public IActionResult Index()
    // {
    //     return View();
    // }

    public IActionResult Privacy()
    {
        return View();
    }
    public IActionResult About()
    {
        return View();
    }

     public async Task<IActionResult> Index()
    {
        try
        {
            // Make the API request
            HttpResponseMessage response = await _httpClient.GetAsync("https://fakestoreapi.com/products");
            
            if (response.IsSuccessStatusCode)
            {
                // Read and deserialize the response
                string jsonResponse = await response.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };
                var data = JsonSerializer.Deserialize<List<Product>>(jsonResponse, options);
                
                // Pass the data to your view
                return View(data);
            }
            else
            {
                // Handle error cases
                return View("Error", new ErrorViewModel { Message = $"API returned {response.StatusCode}" });
            }
        }
        catch (Exception ex)
        {
            return View("Error", new ErrorViewModel { Message = ex.Message });
        }
    }
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
