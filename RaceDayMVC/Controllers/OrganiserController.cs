using Microsoft.AspNetCore.Mvc;
using RaceDayMVC.Models;
using RaceDayMVC.Services;
using System.Net.Http.Headers;
using System.IO;

namespace RaceDayMVC.Controllers
{
    public class OrganiserController : Controller
    {
        private readonly ApiService _apiService;

        public OrganiserController(ApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task<IActionResult> Dashboard()
        {
            if (HttpContext.Session.GetString("Role") != "Organiser")
            {
                return RedirectToAction("Login", "Account");
            }

            var events = await _apiService.GetAsync<List<EventSummaryViewModel>>("api/events");

            return View(events ?? new List<EventSummaryViewModel>());
        }

        [HttpGet]
        public IActionResult CreateEvent()
        {
            if (HttpContext.Session.GetString("Role") != "Organiser")
            {
                return RedirectToAction("Login", "Account");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateEvent(CreateEventViewModel model, IFormFile imageFile)
        {
            if (HttpContext.Session.GetString("Role") != "Organiser")
            {
                return RedirectToAction("Login", "Account");
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                var formData = new MultipartFormDataContent();
                formData.Add(new StringContent(model.Name ?? ""), "Name");
                formData.Add(new StringContent(model.Description ?? ""), "Description");
                formData.Add(new StringContent(model.Location ?? ""), "Location");
                formData.Add(new StringContent(DateTime.Parse(model.StartDate).ToString("yyyy-MM-ddTHH:mm:ss")), "StartDate");
                formData.Add(new StringContent(DateTime.Parse(model.EndDate).ToString("yyyy-MM-ddTHH:mm:ss")), "EndDate");

                if (imageFile != null && imageFile.Length > 0)
                {
                    var fileContent = new StreamContent(imageFile.OpenReadStream());
                    fileContent.Headers.ContentType = new MediaTypeHeaderValue(imageFile.ContentType);
                    formData.Add(fileContent, "ImageFile", imageFile.FileName);
                }

                var response = await _apiService.PostFormDataAsync("api/events", formData);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Dashboard");
                }

                var errorContent = await response.Content.ReadAsStringAsync();
                ModelState.AddModelError("", $"API Error: {errorContent}");
                return View(model);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Error: {ex.Message}");
                return View(model);
            }
        }

        [HttpGet]
        public IActionResult AddCategory(int eventId)
        {
            if (HttpContext.Session.GetString("Role") != "Organiser")
            {
                return RedirectToAction("Login", "Account");
            }

            var model = new CreateCategoryViewModel { EventId = eventId };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddCategory(CreateCategoryViewModel model)
        {
            if (HttpContext.Session.GetString("Role") != "Organiser")
            {
                return RedirectToAction("Login", "Account");
            }

            if (!ModelState.IsValid) return View(model);

            var apiModel = new
            {
                Name = model.Name,
                Description = model.Description,
                Distance = model.Distance,
                MinAge = model.MinAge,
                MaxAge = model.MaxAge,
                MaxCapacity = model.MaxCapacity > 0 ? model.MaxCapacity : 100
            };

            var response = await _apiService.PostAsync($"api/events/{model.EventId}/categories", apiModel);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("ManageEvent", new { id = model.EventId });
            }

            var error = await response.Content.ReadAsStringAsync();
            ModelState.AddModelError("", $"API Error: {response.StatusCode} - {error}");
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Enrolments(int eventId)
        {
            if (HttpContext.Session.GetString("Role") != "Organiser")
            {
                return RedirectToAction("Login", "Account");
            }

            ViewBag.EventId = eventId;
            var enrolments = await _apiService.GetAsync<List<EventEnrolmentViewModel>>($"api/events/{eventId}/enrolments");
            return View(enrolments ?? new List<EventEnrolmentViewModel>());
        }

        [HttpGet]
        public async Task<IActionResult> Results(int eventId)
        {
            if (HttpContext.Session.GetString("Role") != "Organiser")
            {
                return RedirectToAction("Login", "Account");
            }

            var results = await _apiService.GetAsync<List<EnrolmentResultViewModel>>($"api/events/{eventId}/results");

            var model = new ResultEntryViewModel
            {
                EventId = eventId,
                Enrolments = results ?? new List<EnrolmentResultViewModel>()
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> SubmitResult(SubmitResultViewModel model)
        {
            if (HttpContext.Session.GetString("Role") != "Organiser")
            {
                return RedirectToAction("Login", "Account");
            }

            var response = await _apiService.PostAsync($"api/events/{model.EventId}/results", new
            {
                enrolmentId = model.EnrolmentId,
                finishTime = model.FinishTime,
                position = model.Position
            });

            if (response.IsSuccessStatusCode)
            {
                TempData["Success"] = "Result saved successfully.";
            }
            else
            {
                var error = await response.Content.ReadAsStringAsync();
                TempData["Error"] = $"Failed to save result: {error}";
            }

            return RedirectToAction("Results", new { eventId = model.EventId });
        }

        [HttpGet]
        public async Task<IActionResult> ManageEvent(int id)
        {
            if (HttpContext.Session.GetString("Role") != "Organiser")
                return RedirectToAction("Login", "Account");

            var eventDetail = await _apiService.GetAsync<EventDetailViewModel>($"api/events/{id}");

            if (eventDetail == null)
                return NotFound();

            return View(eventDetail);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteEvent(int id)
        {
            if (HttpContext.Session.GetString("Role") != "Organiser")
                return RedirectToAction("Login", "Account");

            await _apiService.DeleteAsync($"api/events/{id}");

            return RedirectToAction("Dashboard");
        }

        [HttpGet]
        public async Task<IActionResult> GetEventImage(string path)
        {
            if (string.IsNullOrEmpty(path)) return NotFound();

            var bytes = await _apiService.GetImageBytesAsync(path);
            if (bytes == null) return NotFound();

            var extension = Path.GetExtension(path).ToLower();
            var contentType = extension switch
            {
                ".png" => "image/png",
                ".jpg" or ".jpeg" => "image/jpeg",
                _ => "application/octet-stream"
            };

            return File(bytes, contentType);
        }

        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            if (HttpContext.Session.GetString("Role") != "Organiser")
                return RedirectToAction("Login", "Account");

            var profile = await _apiService.GetAsync<UpdateOrganiserProfileDto>("api/organiser/profile");
            return View(profile ?? new UpdateOrganiserProfileDto());
        }

        [HttpPost]
        public async Task<IActionResult> Profile(UpdateOrganiserProfileDto model)
        {
            if (HttpContext.Session.GetString("Role") != "Organiser")
                return RedirectToAction("Login", "Account");

            var response = await _apiService.PutAsync("api/organiser/profile", model);
            if (response.IsSuccessStatusCode)
            {
                TempData["Success"] = "Profile updated successfully.";
                return RedirectToAction("Profile");
            }

            ModelState.AddModelError("", "Failed to update profile.");
            return View(model);
        }
    }
}"// Organiser Controller for MVC" 
