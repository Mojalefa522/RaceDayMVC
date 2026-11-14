using Microsoft.AspNetCore.Mvc;
using RaceDayMVC.Models;
using RaceDayMVC.Services;
using System.Text.Json;

namespace RaceDayMVC.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApiService _apiService;

        public AccountController(ApiService apiService)
        {
            _apiService = apiService;
        }

        [HttpGet]
        public IActionResult Login()
        {
            if (HttpContext.Session.GetString("Role") != null)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var response = await _apiService.PostAsync("api/auth/login", model);

            if (response.IsSuccessStatusCode)
            {
                if (response.Headers.TryGetValues("Set-Cookie", out var cookieValues))
                {
                    var sessionCookie = cookieValues.FirstOrDefault(c => c.StartsWith(".AspNetCore.Session="));
                    if (sessionCookie != null)
                    {
                        var cookieValue = sessionCookie.Split(';')[0].Replace(".AspNetCore.Session=", "");
                        HttpContext.Session.SetString("ApiSessionCookie", cookieValue);
                    }
                }

                var content = await response.Content.ReadAsStringAsync();
                var user = JsonDocument.Parse(content).RootElement;

                string role = user.GetProperty("role").GetString();
                string fullName = user.GetProperty("fullName").GetString();

                HttpContext.Session.SetString("Role", role);
                HttpContext.Session.SetString("FullName", fullName);

                if (role == "Organiser")
                    return RedirectToAction("Dashboard", "Organiser");
                else
                    return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError("", "Invalid email or password.");
            return View(model);
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var response = await _apiService.PostAsync("api/auth/register", model);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Login");
            }

            var error = await response.Content.ReadAsStringAsync();
            ModelState.AddModelError("", $"API Error: {error}");
            return View(model);
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }
    }
}"// Account Controller for MVC" 
