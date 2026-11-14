using Microsoft.AspNetCore.Mvc;
using RaceDayMVC.Models;
using RaceDayMVC.Services;

namespace RaceDayMVC.Controllers
{
    public class ParticipantController : Controller
    {
        private readonly ApiService _apiService;

        public ParticipantController(ApiService apiService)
        {
            _apiService = apiService;
        }

        [HttpPost]
        public async Task<IActionResult> Enrol(int eventId, int categoryId)
        {
            if (HttpContext.Session.GetString("Role") != "Participant")
            {
                return RedirectToAction("Login", "Account");
            }

            var response = await _apiService.PostAsync($"api/events/{eventId}/enrol", new { categoryId });

            if (response.IsSuccessStatusCode)
            {
                TempData["Success"] = "Successfully enrolled!";
            }
            else
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                TempData["Error"] = $"Failed to enrol: {errorContent}";
            }

            return RedirectToAction("Details", "Home", new { id = eventId });
        }
        [HttpGet]
        public async Task<IActionResult> MyEnrolments()
        {
            if (HttpContext.Session.GetString("Role") != "Participant")
            {
                return RedirectToAction("Login", "Account");
            }

            var enrolments = await _apiService.GetAsync<List<MyEnrolmentViewModel>>("api/enrolments");
            return View(enrolments ?? new List<MyEnrolmentViewModel>());
        }

        [HttpGet]
        public async Task<IActionResult> MyResults()
        {
            if (HttpContext.Session.GetString("Role") != "Participant")
            {
                return RedirectToAction("Login", "Account");
            }

            var results = await _apiService.GetAsync<List<MyResultViewModel>>("api/my-results");
            return View(results ?? new List<MyResultViewModel>());
        }

        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            if (HttpContext.Session.GetString("Role") != "Participant")
                return RedirectToAction("Login", "Account");

            var profile = await _apiService.GetAsync<UpdateParticipantProfileDto>("api/participant/profile");
            return View(profile ?? new UpdateParticipantProfileDto());
        }

        [HttpPost]
        public async Task<IActionResult> Profile(UpdateParticipantProfileDto model)
        {
            if (HttpContext.Session.GetString("Role") != "Participant")
                return RedirectToAction("Login", "Account");

            var response = await _apiService.PutAsync("api/participant/profile", model);
            if (response.IsSuccessStatusCode)
            {
                TempData["Success"] = "Profile updated successfully.";
                return RedirectToAction("Profile");
            }

            ModelState.AddModelError("", "Failed to update profile.");
            return View(model);
        }
    }
}