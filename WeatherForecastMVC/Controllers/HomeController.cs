using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using WeatherForecastMVC.Models;
using WeatherForecastMVC.Services;

namespace WeatherForecastMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> logger;

        private readonly IWeatherForecast weatherForecast;

        public HomeController(IServiceProvider serviceProvider)
        {
            logger = serviceProvider.GetRequiredService<ILogger<HomeController>>();
            weatherForecast = serviceProvider.GetRequiredService<IWeatherForecast>();
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult WeatherForecast(string city)
        {
            CookieOptions cookieOptions = new() { };
            Response.Cookies.Append("City", city, cookieOptions);

            try
            {
                WeatherForecastModel model = weatherForecast.GetWeatherForecast(city);

                CookieOptions cookieOptions2 = new()
                {
                    Expires = DateTime.Now.AddDays(1)
                };
                Response.Cookies.Append("VistedCity", city, cookieOptions2);

                return View(model);
            }
            catch (ArgumentNullException ex)
            {
                TempData["ErrorMessage"] = $"400: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
            catch (ArgumentException ex)
            {
                TempData["ErrorMessage"] = $"404: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                TempData["ErrorMessage"] = $"500: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}