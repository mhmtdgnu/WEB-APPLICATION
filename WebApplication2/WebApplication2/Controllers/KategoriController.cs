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
    public class KategoriController : Controller
    {
        public ActionResult Index()
        {
            var httpClient = new HttpClient();
            var request = httpClient.GetAsync("https://localhost:1433/api/kategori").Result;
            var response = request.Content.ReadAsStringAsync().Result;
            var value = JsonConvert.DeserializeObject<List<TBLKATEGORI>>(response);
            var degerler = value.ToList();
            return View(degerler);
        }
        [HttpGet]
        public ActionResult KategoriEkle()
        {
            return View();
        }
        [HttpPost]
        public ActionResult KategoriEkle(TBLKATEGORI p)
        {
            p.DURUM = true;
            using (var httpClient = new HttpClient())
            {
                var json = JsonConvert.SerializeObject(p);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var task = httpClient.PostAsync("https://localhost:1433/api/kategori/ekle", content);
                task.Wait(); // İstek tamamlanana kadar burada bekler
                var response = task.Result; // İstek sonucunu alır
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    return View();
                }
            }
        }
        public ActionResult KategoriSil(int id)
        {
            using (var httpClient = new HttpClient())
            {
                var task = httpClient.DeleteAsync($"https://localhost:1433/api/kategori/sil{id}");
                task.Wait(); // İstek tamamlanana kadar burada bekler
                var response = task.Result; // İstek sonucunu alır
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    return View();
                }
            }
        }
        public ActionResult KategoriGetir(int id)
        {
            var httpClient = new HttpClient();
            var request = httpClient.GetAsync($"https://localhost:1433/api/kategori/kategorigetir{id}").Result;
            var response = request.Content.ReadAsStringAsync().Result;
            if (!request.IsSuccessStatusCode)
            {
                return View();
            }
            var kategori = JsonConvert.DeserializeObject<TBLKATEGORI>(response);
            return View("KategoriGetir", kategori);
        }
        public ActionResult KategoriGuncelle(TBLKATEGORI p)
        {
            int id = p.ID;
            p.DURUM = true;
            using (var httpClient = new HttpClient())
            {
                var url = $"https://localhost:1433/api/kategori/guncelle{id}";
                var json = JsonConvert.SerializeObject(p);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var responseTask = httpClient.PutAsync(url, content);
                responseTask.Wait(); // İstek tamamlanana kadar burada bekleyin
                var response = responseTask.Result; // İstek sonucunu alın
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    return View("KategoriGetir");
                }
            }

        }
    }
}