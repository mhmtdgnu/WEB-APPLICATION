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
    public class OduncController : Controller
    {
        public ActionResult Index()
        {
            var httpClient = new HttpClient();
            var request = httpClient.GetAsync("https://localhost:1433/api/hareket").Result;
            var response = request.Content.ReadAsStringAsync().Result;
            var value = JsonConvert.DeserializeObject<List<TBLHAREKET>>(response);
            var degerler = value.ToList();
            var uyerequest = httpClient.GetAsync("https://localhost:1433/api/uye").Result;
            var uyeresponse = uyerequest.Content.ReadAsStringAsync().Result;
            var uyevalue = JsonConvert.DeserializeObject<List<TBLUYELER>>(uyeresponse);
            ViewBag.uye = uyevalue.ToList();
            var personelrequest = httpClient.GetAsync("https://localhost:1433/api/personel").Result;
            var personelresponse = personelrequest.Content.ReadAsStringAsync().Result;
            var personelvalue = JsonConvert.DeserializeObject<List<TBLPERSONEL>>(personelresponse);
            ViewBag.personel = personelvalue.ToList();
            var kitaprequest = httpClient.GetAsync("https://localhost:1433/api/kitap/hepsi").Result;
            var kitapresponse = kitaprequest.Content.ReadAsStringAsync().Result;
            var kitapvalue = JsonConvert.DeserializeObject<List<TBLKITAP>>(kitapresponse);
            ViewBag.kitap = kitapvalue.ToList();
            return View(degerler);
        }
        [HttpGet]
        public ActionResult OduncVer()
        {
            var httpClient = new HttpClient();
            var request = httpClient.GetAsync("https://localhost:1433/api/uye").Result;
            var response = request.Content.ReadAsStringAsync().Result;
            var value = JsonConvert.DeserializeObject<List<TBLUYELER>>(response);
            var degerler = value.ToList();
            List<SelectListItem> deger1 = (from x in value.ToList()
                                           select new SelectListItem
                                           {
                                               Text = x.AD + " " + x.SOYAD,
                                               Value = x.ID.ToString()
                                           }).ToList();
            var request2 = httpClient.GetAsync("https://localhost:1433/api/kitap/true").Result;
            var response2 = request2.Content.ReadAsStringAsync().Result;
            var value2 = JsonConvert.DeserializeObject<List<TBLKITAP>>(response2);
            List<SelectListItem> deger2 = (from y in value2.ToList()
                                           select new SelectListItem
                                           {
                                               Text = y.AD,
                                               Value = y.ID.ToString()
                                           }).ToList();
            var request1 = httpClient.GetAsync("https://localhost:1433/api/personel").Result;
            var response1 = request1.Content.ReadAsStringAsync().Result;
            var value1 = JsonConvert.DeserializeObject<List<TBLPERSONEL>>(response1);
            List<SelectListItem> deger3 = (from z in value1.ToList()
                                           select new SelectListItem
                                           {
                                               Text = z.PERSONEL,
                                               Value = z.ID.ToString()
                                           }).ToList();
            ViewBag.dgr1 = deger1;
            ViewBag.dgr2 = deger2;
            ViewBag.dgr3 = deger3;
            return View();
        }
        [HttpPost]
        public ActionResult OduncVer(TBLHAREKET p)
        {
            p.UYE = p.TBLUYELER.ID;
            p.KITAP = p.TBLKITAP.ID;
            p.PERSONEL = p.TBLPERSONEL.ID;
            using (var httpClient = new HttpClient())
            {
                var json = JsonConvert.SerializeObject(p);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var task = httpClient.PostAsync("https://localhost:1433/api/hareket/ekle", content);
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
        public ActionResult Odunciade(TBLHAREKET p)
        {
            var httpClient = new HttpClient();
            var request = httpClient.GetAsync($"https://localhost:1433/api/hareket/getir{p.ID}").Result;
            var response = request.Content.ReadAsStringAsync().Result;
            if (!request.IsSuccessStatusCode)
            {
                return View();
            }
            var odn = JsonConvert.DeserializeObject<TBLHAREKET>(response);
            var request1 = httpClient.GetAsync($"https://localhost:1433/api/kitap/kitapgetir{odn.KITAP}").Result;
            var response1 = request1.Content.ReadAsStringAsync().Result;
            if (!request.IsSuccessStatusCode)
            {
                return View();
            }
            var kitap = JsonConvert.DeserializeObject<TBLKITAP>(response1);
            var uyerequest = httpClient.GetAsync($"https://localhost:1433/api/uye/{odn.UYE}").Result;
            var uyeresponse = uyerequest.Content.ReadAsStringAsync().Result;
            if (!request.IsSuccessStatusCode)
            {
                return View();
            }
            var uye = JsonConvert.DeserializeObject<TBLUYELER>(uyeresponse);
            var prsrequest = httpClient.GetAsync($"https://localhost:1433/api/personel/getir{odn.PERSONEL}").Result;
            var prsresponse = prsrequest.Content.ReadAsStringAsync().Result;
            if (!request.IsSuccessStatusCode)
            {
                return View();
            }
            var prs = JsonConvert.DeserializeObject<TBLPERSONEL>(prsresponse);
            DateTime d1 = DateTime.Parse(odn.IADETARIHI.ToString());
            DateTime d2 = Convert.ToDateTime(DateTime.Now.ToShortTimeString());
            TimeSpan d3 = d2 - d1;
            ViewBag.dgr = d3.TotalDays;
            ViewBag.ktp = kitap.AD;
            ViewBag.prs = prs.PERSONEL;
            ViewBag.uye = uye.AD + " " + uye.SOYAD;
            return View("Odunciade", odn);
        }
        public ActionResult OduncGuncelle(TBLHAREKET p)
        {
            var httpClient = new HttpClient();
            var request = httpClient.GetAsync($"https://localhost:1433/api/hareket/getir{p.ID}").Result;
            var response = request.Content.ReadAsStringAsync().Result;
            if (!request.IsSuccessStatusCode)
            {
                return View();
            }
            var hareket = JsonConvert.DeserializeObject<TBLHAREKET>(response);
            hareket.UYEGETIRTARIH = p.UYEGETIRTARIH;
            hareket.ISLEMDURUM = true;
            using (var httpClient1 = new HttpClient())
            {
                var url = $"https://localhost:1433/api/hareket/guncelle{p.ID}";
                var json = JsonConvert.SerializeObject(hareket);
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
    }
}