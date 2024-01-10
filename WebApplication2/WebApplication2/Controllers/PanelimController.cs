using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using WebApplication2.Models.Entity;

namespace WebApplication2.Controllers
{
    public class PanelimController : Controller
    {
        [HttpGet]
        [Authorize]
        public ActionResult Index()
        {
            var uyemail = (string)Session["Mail"];
            var httpClient = new HttpClient();
            var request = httpClient.GetAsync($"https://localhost:1433/api/uye/uyegetirmail/{uyemail}").Result;
            var response = request.Content.ReadAsStringAsync().Result;
            if (!request.IsSuccessStatusCode)
            {
                return View();
            }
            var degerler = JsonConvert.DeserializeObject<TBLUYELER>(response);
            return View(degerler);
        }
        [HttpPost]
        public ActionResult Index2(TBLUYELER p)
        {
            var kullanici = (string)Session["Mail"];
            var httpClient = new HttpClient();
            var request = httpClient.GetAsync($"https://localhost:1433/api/uye/uyegetirmail/{kullanici}").Result;
            var response = request.Content.ReadAsStringAsync().Result;
            if (!request.IsSuccessStatusCode)
            {
                return View();
            }
            var uye = JsonConvert.DeserializeObject<TBLUYELER>(response);
            uye.SIFRE = p.SIFRE;
            uye.KULLANICIADI = p.KULLANICIADI;
            using (var httpClient1 = new HttpClient())
            {
                var url = $"https://localhost:1433/api/uye/guncelle{uye.ID}";
                var json = JsonConvert.SerializeObject(uye);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var responseTask = httpClient.PutAsync(url, content);
                responseTask.Wait(); // İstek tamamlanana kadar burada bekleyin
                var response1 = responseTask.Result; // İstek sonucunu alın
                if (response1.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    return View();
                }
            }
        }

        public ActionResult Kitaplarım()
        {
            var kullanici = (string)Session["Mail"];
            var httpClient = new HttpClient();
            var request = httpClient.GetAsync($"https://localhost:1433/api/uye/uyegetirmail/{kullanici}").Result;
            var response = request.Content.ReadAsStringAsync().Result;
            var degerler = JsonConvert.DeserializeObject<TBLUYELER>(response);
            var hareketrequest = httpClient.GetAsync($"https://localhost:1433/api/hareket/uyehareket{degerler.ID}").Result;
            var hareketresponse = hareketrequest.Content.ReadAsStringAsync().Result;
            var value = JsonConvert.DeserializeObject<List<TBLHAREKET>>(hareketresponse);
            var kitaprequest = httpClient.GetAsync($"https://localhost:1433/api/kitap/hepsi").Result;
            var kitapresponse = kitaprequest.Content.ReadAsStringAsync().Result;
            var kitap = JsonConvert.DeserializeObject<List<TBLKITAP>>(kitapresponse);
            ViewBag.ktp = kitap.ToList();
            return View(value.ToList());
        }

        public ActionResult Duyurular()
        {
            var httpClient = new HttpClient();
            var request = httpClient.GetAsync("https://localhost:1433/api/duyuru").Result;
            var response = request.Content.ReadAsStringAsync().Result;
            var value = JsonConvert.DeserializeObject<List<TBLDUYURULAR>>(response);
            var degerler = value.ToList();
            return View(degerler);
        }
        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("GirisYap", "Login");
        }
    }
}