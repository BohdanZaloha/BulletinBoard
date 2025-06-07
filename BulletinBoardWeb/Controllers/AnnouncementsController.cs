using BulletinBoardWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace BulletinBoardWeb.Controllers
{
    /// <summary>
    /// Handles CRUD operations and filtering for announcements in the web UI,
    /// communicating with the back-end API.
    /// </summary>
    public class AnnouncementsController : Controller
    {
        private readonly string _baseUrl;
        public AnnouncementsController(IConfiguration config)
        {
            _baseUrl = config["ApiBaseUrl"];
        }

        /// <summary>
        /// Loads all categories and (optionally filtered) subcategories into the <see cref="ViewBag"/>
        /// for use in cascading dropdowns.
        /// </summary>
        private async Task LoadCategoryData(int? categoryId = null, int? subCategoryId = null)
        {
            var allCategories = new List<Category>();
            var allSubCategories = new List<SubCategory>();

            using var client = new HttpClient();
            client.BaseAddress = new Uri(_baseUrl);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            // --- 1) Категорії ---
            var catResp = await client.GetAsync("api/Category/GetCategories");
            if (catResp.IsSuccessStatusCode)
            {
                var jsonCat = await catResp.Content.ReadAsStringAsync();
                allCategories = JsonConvert.DeserializeObject<List<Category>>(jsonCat)!;
            }

            // --- 2) Підкатегорії ---
            HttpResponseMessage subResp;
            if (categoryId.HasValue && categoryId.Value > 0)
            {
                subResp = await client
                   .GetAsync($"api/SubCategory/GetSubCategoresByCategoryId/{categoryId.Value}");
            }
            else
            {
                subResp = await client.GetAsync("api/SubCategory/GetSubCategories");
            }
            if (subResp.IsSuccessStatusCode)
            {
                var jsonSub = await subResp.Content.ReadAsStringAsync();
                allSubCategories = JsonConvert.DeserializeObject<List<SubCategory>>(jsonSub)!;
            }

            // --- 3) Готові SelectList з вибраними елементами ---
            ViewBag.CategoryList = new SelectList(allCategories, "CategoryId", "Name", categoryId ?? 0);
            ViewBag.SubCategoryList = new SelectList(allSubCategories, "SubCategoryId", "Name", subCategoryId ?? 0);
        }

        /// <summary>
        /// Displays a list of announcements, optionally filtered by category and/or subcategory.
        /// </summary>
        public async Task<IActionResult> Index(int? categoryId, int? subCategoryId)
        {
            // 1) Отримуємо всі оголошення з API
            List<Announcement> announcements = new();
            using (var _httpClient = new HttpClient())
            {
                _httpClient.BaseAddress = new Uri(_baseUrl);
                _httpClient.DefaultRequestHeaders.Accept.Clear();
                _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var response = await _httpClient.GetAsync("api/Announcement");
                if (response.IsSuccessStatusCode)
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    announcements = JsonConvert.DeserializeObject<List<Announcement>>(jsonString)!;
                }
                else
                {
                    // Якщо помилка звернення до API
                    return View("ErrorPage");
                }
            }

            // 2) Якщо передано categoryId, відфільтровуємо список оголошень
            if (categoryId.HasValue && categoryId.Value > 0)
            {
                announcements = announcements
                    .Where(a => a.CategoryId == categoryId.Value)
                    .ToList();
            }

            // 3) Якщо передано subCategoryId, фільтруємо далі
            if (subCategoryId.HasValue && subCategoryId.Value > 0)
            {
                announcements = announcements
                    .Where(a => a.SubCategoryId == subCategoryId.Value)
                    .ToList();
            }

            await LoadCategoryData(categoryId, subCategoryId);

            return View(announcements);
        }

        /// <summary>
        /// Shows the form for creating a new announcement, optionally preselecting a category.
        /// </summary>
        public async Task<IActionResult> Create(int? categoryId)
        {
            if (!ModelState.IsValid)
            {
                await LoadCategoryData();
                return BadRequest(ModelState);
            }

            await LoadCategoryData(categoryId);

            var model = new Announcement
            {
                CategoryId = categoryId.GetValueOrDefault(),
                SubCategoryId = 0
            };

            return View(model);
        }

        /// <summary>
        /// Handles the POST of a new announcement to the API.
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateAnnouncement(Announcement announcement)
        {
            if (!ModelState.IsValid)
            {
                await LoadCategoryData(announcement.CategoryId, announcement.SubCategoryId);
                return View("Create", announcement);
            }

            using (var _httpClient = new HttpClient())
            {
                _httpClient.BaseAddress = new Uri(_baseUrl + "api/Announcement");
                _httpClient.DefaultRequestHeaders.Accept.Clear();
                _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage getData = await _httpClient.PostAsJsonAsync("", announcement);

                if (getData.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    return View("ErrorPage");
                }
            }
        }

        /// <summary>
        /// Displays the details of a specific announcement.
        /// </summary>
        public async Task<IActionResult> Details(int Id)
        {
            Announcement announcementDetails = new Announcement();

            using (var _httpClient = new HttpClient())
            {
                _httpClient.BaseAddress = new Uri(_baseUrl + "api/Announcement/");
                _httpClient.DefaultRequestHeaders.Accept.Clear();
                _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage getData = await _httpClient.GetAsync($"GetAnnouncementById/{Id}");

                if (getData.IsSuccessStatusCode)
                {
                    string result = getData.Content.ReadAsStringAsync().Result;
                    announcementDetails = JsonConvert.DeserializeObject<Announcement>(result);
                }
                else
                {
                    return View("ErrorPage");
                }
            }
            return View(announcementDetails);
        }

        /// <summary>
        /// Updates an existing announcement via the API.
        /// </summary>
        public async Task<IActionResult> UpdateAnnouncementDetails(int Id, Announcement announcement)
        {

            using (var _httpClient = new HttpClient())
            {
                _httpClient.BaseAddress = new Uri(_baseUrl + "api/Announcement");
                _httpClient.DefaultRequestHeaders.Accept.Clear();
                _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage getData = await _httpClient.PutAsJsonAsync($"Announcement/UpdateAnnouncement/{Id}", announcement);

                if (getData.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    return View("ErrorPage");
                }
            }
        }

        /// <summary>
        /// Displays the confirmation view for deleting an announcement.
        /// </summary>
        public async Task<IActionResult> Delete(int Id)
        {
            Announcement announcementDetails = new Announcement();

            using (var _httpClient = new HttpClient())
            {
                _httpClient.BaseAddress = new Uri(_baseUrl + "api/Announcement/");
                _httpClient.DefaultRequestHeaders.Accept.Clear();
                _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage getData = await _httpClient.GetAsync($"GetAnnouncementById/{Id}");

                if (getData.IsSuccessStatusCode)
                {
                    string result = getData.Content.ReadAsStringAsync().Result;
                    announcementDetails = JsonConvert.DeserializeObject<Announcement>(result);
                }
                else
                {
                    return View("ErrorPage");
                }
            }
            await LoadCategoryData(announcementDetails.CategoryId, announcementDetails.SubCategoryId);
            return View(announcementDetails);
        }

        /// <summary>
        /// Deletes an announcement via the API.
        /// </summary>
        public async Task<IActionResult> DeleteAnnouncement(int Id)
        {
            if (!ModelState.IsValid)
            {
                // if binding failed (e.g. missing or invalid Id), redirect back to confirmation
                return RedirectToAction(nameof(Delete), new { Id });
            }

            using (var _httpClient = new HttpClient())
            {
                _httpClient.BaseAddress = new Uri(_baseUrl + "api/Announcement");
                _httpClient.DefaultRequestHeaders.Accept.Clear();
                _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage getData = await _httpClient.DeleteAsync($"Announcement/DeleteAnnouncement/{Id}");


                if (getData.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    return View("ErrorPage");
                }
            }
        }
        public IActionResult ErrorPage()
        {
            return View();
        }
    }
}
