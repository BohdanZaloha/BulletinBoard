using Microsoft.AspNetCore.Mvc;
using BulletinBoardWeb.Models;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.Runtime.CompilerServices;

namespace BulletinBoardWeb.Controllers
{
    public class AnnouncementsController : Controller
    {
        private string baseUrl = "https://localhost:7023/";
        public async Task<IActionResult> Index()
        {
            List<Announcement> announcements = new List<Announcement>();

            using (var _httpClient = new HttpClient())
            {
                _httpClient.BaseAddress = new Uri(baseUrl + "api/Announcement");
                _httpClient.DefaultRequestHeaders.Accept.Clear();
                _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage getData = await _httpClient.GetAsync("");

                if (getData.IsSuccessStatusCode)
                {
                    string result = getData.Content.ReadAsStringAsync().Result;
                    announcements= JsonConvert.DeserializeObject<List<Announcement>>(result);
                }
                else
                {
                    return View("ErrorPage");
                }
            }
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
