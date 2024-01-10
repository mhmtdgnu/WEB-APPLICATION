using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using System.Web.WebPages;
using WebApplication2.Models.Entity;

namespace WebApplication2.Controllers
{
    public class KitapController : Controller
    {

        public ActionResult Index(string p)
        {
            var httpClient = new HttpClient();
            var request = httpClient.GetAsync($"https://localhost:1433/api/kitap/hepsi").Result;
            var response = request.Content.ReadAsStringAsync().Result;
            var value = JsonConvert.DeserializeObject<List<TBLKITAP>>(response);
            var degerler = value.ToList();
            if (!string.IsNullOrEmpty(p))
            {
                var request1 = httpClient.GetAsync($"https://localhost:1433/api/kitap/{p}").Result;
                var response1 = request1.Content.ReadAsStringAsync().Result;
                value = JsonConvert.DeserializeObject<List<TBLKITAP>>(response1);
                degerler = value.ToList();
            }
            var requestyazar = httpClient.GetAsync("https://localhost:1433/api/yazar").Result;
            var responseyazar = requestyazar.Content.ReadAsStringAsync().Result;
            var valueyazar = JsonConvert.DeserializeObject<List<TBLYAZAR>>(responseyazar);
            List<TBLYAZAR> yazar = valueyazar.ToList();
            var kategorirequest = httpClient.GetAsync("https://localhost:1433/api/kategori").Result;
            var kategoriresponse = kategorirequest.Content.ReadAsStringAsync().Result;
            var value1 = JsonConvert.DeserializeObject<List<TBLKATEGORI>>(kategoriresponse);
            List<TBLKATEGORI> kategori = value1.ToList();
            ViewBag.yazar = yazar;
            ViewBag.kategori = kategori;
            return View(degerler);
        }

        [HttpGet]
        public ActionResult KitapEkle()
        {
            var httpClient = new HttpClient();
            var request = httpClient.GetAsync("https://localhost:1433/api/kategori").Result;
            var response = request.Content.ReadAsStringAsync().Result;
            var value = JsonConvert.DeserializeObject<List<TBLKATEGORI>>(response);
            List<TBLKATEGORI> listkategori = new List<TBLKATEGORI>();
            listkategori = value.ToList();
            ViewBag.ktgl = listkategori;
            var request1 = httpClient.GetAsync("https://localhost:1433/api/yazar").Result;
            var response1 = request1.Content.ReadAsStringAsync().Result;
            var value1 = JsonConvert.DeserializeObject<List<TBLYAZAR>>(response1);
            List<TBLYAZAR> listyazar = value1.ToList();
            ViewBag.yzrl = listyazar;
            return View("KitapEkle");
        }
        [HttpPost]
        public ActionResult KitapEkle(TBLKITAP p)
        {
            var httpClient = new HttpClient();
            var request = httpClient.GetAsync($"https://localhost:1433/api/kategori/kategorigetir{p.KATEGORI}").Result;
            var response = request.Content.ReadAsStringAsync().Result;
            var ktg = JsonConvert.DeserializeObject<TBLKATEGORI>(response);
            var request1 = httpClient.GetAsync($"https://localhost:1433/api/yazar/{p.YAZAR}").Result;
            var response1 = request1.Content.ReadAsStringAsync().Result;
            var yzr = JsonConvert.DeserializeObject<TBLYAZAR>(response1);
            p.DURUM = true;
            p.KITAPRESIM = " ";
            p.YAZAR = yzr.ID;
            p.KATEGORI = ktg.ID;
            using (var httpClient2 = new HttpClient())
            {
                var json = JsonConvert.SerializeObject(p);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var task = httpClient2.PostAsync("https://localhost:1433/api/kitap/ekle", content);
                task.Wait(); // İstek tamamlanana kadar burada bekler
                var response2 = task.Result; // İstek sonucunu alır
                if (response2.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    return View("KitapEkle");
                }
            }
        }

        public ActionResult KitapSil(int id)
        {
            using (var httpClient = new HttpClient())
            {
                var task = httpClient.DeleteAsync($"https://localhost:1433/api/kitap/kitapsil{id}");
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

        public ActionResult KitapGetir(int id)
        {
            var httpClient = new HttpClient();
            var request = httpClient.GetAsync($"https://localhost:1433/api/kitap/kitapgetir{id}").Result;
            var response = request.Content.ReadAsStringAsync().Result;
            if (!request.IsSuccessStatusCode)
            {
                return View();
            }
            var kitap = JsonConvert.DeserializeObject<TBLKITAP>(response);
            var kategorirequest = httpClient.GetAsync("https://localhost:1433/api/kategori").Result;
            var kategoriresponse = kategorirequest.Content.ReadAsStringAsync().Result;
            var value = JsonConvert.DeserializeObject<List<TBLKATEGORI>>(kategoriresponse);
            var degerler = value.ToList();
            List<TBLKATEGORI> kategori = value.ToList();
            ViewBag.dgr3 = kategori;
            var requestyazar = httpClient.GetAsync("https://localhost:1433/api/yazar").Result;
            var responseyazar = requestyazar.Content.ReadAsStringAsync().Result;
            var value1 = JsonConvert.DeserializeObject<List<TBLYAZAR>>(responseyazar);
            List<TBLYAZAR> yazar = value1.ToList();
            ViewBag.dgr4 = yazar;
            return View("KitapGetir", kitap);
        }

        public ActionResult KitapGuncelle(TBLKITAP p)
        {
            var httpClient = new HttpClient();
            var request = httpClient.GetAsync($"https://localhost:1433/api/kitap/kitapgetir{p.ID}").Result;
            var response = request.Content.ReadAsStringAsync().Result;
            if (!request.IsSuccessStatusCode)
            {
                return View("KitapGetir");
            }
            var kitap = JsonConvert.DeserializeObject<TBLKITAP>(response);
            kitap.AD = p.AD;
            kitap.BASIMYILI = p.BASIMYILI;
            kitap.SAYFA = p.SAYFA;
            kitap.YAYINEVI = p.YAYINEVI;
            kitap.DURUM = true;
            using (var httpClient1 = new HttpClient())
            {
                var url = $"https://localhost:1433/api/kitap/guncelle{p.ID}";
                var json = JsonConvert.SerializeObject(kitap);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var responseTask = httpClient1.PutAsync(url, content);
                responseTask.Wait(); // İstek tamamlanana kadar burada bekleyin
                var response1 = responseTask.Result; // İstek sonucunu alın
                if (response1.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    return View("KitapGetir");
                }
            }


        }
    }
}