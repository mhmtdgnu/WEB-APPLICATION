using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Mvc;
using WebApplication2.Models.Entity;
namespace WebApplication2.Controllers
{
    public class KayitOlController : Controller
    {
        [HttpGet]
        public ActionResult Kayit()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Kayit(TBLUYELER u)
        {
            u.FOTOGRAF = "null";
            using (var httpClient = new HttpClient())
            {
                var json = JsonConvert.SerializeObject(u);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var task = httpClient.PostAsync("https://localhost:1433/api/uye/ekle", content);
                task.Wait();
                var response = task.Result;
                if (response.IsSuccessStatusCode)
                {
                    return View();
                }
                else
                {
                    return View();
                }
            }
        }
    }
}