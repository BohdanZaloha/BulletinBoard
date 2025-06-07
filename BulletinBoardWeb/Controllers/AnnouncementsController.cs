using Microsoft.AspNetCore.Mvc;
using BulletinBoardWeb.Models;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BulletinBoardWeb.Controllers
{
    public class AnnouncementsController : Controller
    {
        private string baseUrl = "https://localhost:7023/";
        public async Task<IActionResult> Index(int? categoryId, int? subCategoryId)
        {
            // 1) Отримуємо всі оголошення з API
            List<Announcement> announcements = new();
            using (var _httpClient = new HttpClient())
            {
                _httpClient.BaseAddress = new Uri(baseUrl);
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

            // 4) Завантажуємо список усіх категорій і підкатегорій (щоб наповнити дропдауни)
            List<Category> allCategories = new();
            List<SubCategory> allSubCategories = new();
            using (var _httpClient = new HttpClient())
            {
                _httpClient.BaseAddress = new Uri(baseUrl);
                _httpClient.DefaultRequestHeaders.Accept.Clear();
                _httpClient.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json")
                );

                // 4a) Отримуємо всі категорії
                var catResponse = await _httpClient.GetAsync("api/Category/GetCategories");
                if (catResponse.IsSuccessStatusCode)
                {
                    var jsonCat = await catResponse.Content.ReadAsStringAsync();
                    allCategories = JsonConvert.DeserializeObject<List<Category>>(jsonCat)!;
                }

                // 4b) Отримуємо всі підкатегорії
                var subCatResponse = await _httpClient.GetAsync("api/SubCategory/GetSubCategories");
                if (subCatResponse.IsSuccessStatusCode)
                {
                    var jsonSubCat = await subCatResponse.Content.ReadAsStringAsync();
                    allSubCategories = JsonConvert.DeserializeObject<List<SubCategory>>(jsonSubCat)!;
                }
            }

            // 5) Формуємо SelectList для дропдаунів і передаємо їх через ViewBag
            ViewBag.CategoryList = new SelectList(
                items: allCategories,
                dataValueField: "CategoryId",
                dataTextField: "Name",
                selectedValue: categoryId ?? 0
            );

            ViewBag.SubCategoryList = new SelectList(
                items: allSubCategories,
                dataValueField: "SubCategoryId",
                dataTextField: "Name",
                selectedValue: subCategoryId ?? 0
            );

            // 6) Передаємо обрані зараз значення у ViewBag (щоб відобразити їх як вибрані)
            ViewBag.SelectedCategoryId = categoryId;
            ViewBag.SelectedSubCategoryId = subCategoryId;

            // 7) Повертаємо у View уже відфільтрований список оголошень
            return View(announcements);
        }


        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateAnnouncement(Announcement announcement)
        {
            List<Announcement> announcements = new List<Announcement>();

            using (var _httpClient = new HttpClient())
            {
                _httpClient.BaseAddress = new Uri(baseUrl + "api/Announcement");
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

        public async Task<IActionResult> Details(int Id)
        {
            Announcement announcementDetails = new Announcement();

            using (var _httpClient = new HttpClient())
            {
                _httpClient.BaseAddress = new Uri(baseUrl + "api/Announcement/");
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

        public async Task<IActionResult> UpdateAnnouncementDetails(int Id, Announcement announcement)
        {
            List<Announcement> announcements = new List<Announcement>();

            using (var _httpClient = new HttpClient())
            {
                _httpClient.BaseAddress = new Uri(baseUrl + "api/Announcement");
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

        public async Task<IActionResult> Delete(int Id)
        {
            Announcement announcementDetails = new Announcement();

            using (var _httpClient = new HttpClient())
            {
                _httpClient.BaseAddress = new Uri(baseUrl + "api/Announcement/");
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

        public async Task<IActionResult> DeleteAnnouncement(int Id)
        {

            using (var _httpClient = new HttpClient())
            {
                _httpClient.BaseAddress = new Uri(baseUrl + "api/Announcement");
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
