using Microsoft.AspNetCore.Mvc;
using RaceDayMVC.Models;
using RaceDayMVC.Services;

namespace RaceDayMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApiService _apiService;

        public HomeController(ApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task<IActionResult> Index()
        {
            var events = await _apiService.GetAsync<List<EventListViewModel>>("api/events");
            return View(events ?? new List<EventListViewModel>());
        }

        public async Task<IActionResult> Details(int id)
        {
            var eventDetail = await _apiService.GetAsync<EventDetailViewModel>($"api/events/{id}");

            if (eventDetail == null)
            {
                return NotFound();
            }

            return View(eventDetail);
        }
        [HttpGet]
        public async Task<IActionResult> GetImage(string path)
        {
            if (string.IsNullOrEmpty(path)) return NotFound();

            var imageBytes = await _apiService.GetBytesAsync(path);
            if (imageBytes == null) return NotFound();

            string contentType = path.EndsWith(".png") ? "image/png" : "image/jpeg";

            return File(imageBytes, contentType);
        }
    }
}