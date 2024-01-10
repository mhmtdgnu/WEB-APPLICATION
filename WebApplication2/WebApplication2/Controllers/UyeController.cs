﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication2.Models.Entity;
using PagedList;
using PagedList.Mvc;
using System.Net.Http;
using Newtonsoft.Json;
using System.Text;
using System.Collections;
namespace WebApplication2.Controllers
{
    public class UyeController : Controller
    {
        public ActionResult Index(int sayfa = 1)
        {
            var httpClient = new HttpClient();
            var request = httpClient.GetAsync("https://localhost:1433/api/uye").Result;
            var response = request.Content.ReadAsStringAsync().Result;
            var value = JsonConvert.DeserializeObject<List<TBLUYELER>>(response);
            var degerler = value.ToList().ToPagedList(sayfa, 10);
            return View(degerler);
        }
        [HttpGet]
        public ActionResult UyeEkle()
        {
            return View();
        }
        [HttpPost]
        public ActionResult UyeEkle(TBLUYELER yeniUye)
        {
            using (var httpClient = new HttpClient())
            {
                var json = JsonConvert.SerializeObject(yeniUye);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var task = httpClient.PostAsync("https://localhost:1433/api/uye/ekle", content);
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

        public ActionResult UyeSil(int id)
        {
            using (var httpClient = new HttpClient())
            {
                var task = httpClient.DeleteAsync($"https://localhost:1433/api/uye/sil{id}");
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
        public ActionResult UyeGetir(int id)
        {
            var httpClient = new HttpClient();
            var request = httpClient.GetAsync($"https://localhost:1433/api/uye/{id}").Result;
            var response = request.Content.ReadAsStringAsync().Result;
            var uye = JsonConvert.DeserializeObject<TBLUYELER>(response);
            return View("UyeGetir", uye);
        }
        public ActionResult UyeGuncelle(TBLUYELER guncellenmisUye)
        {
            if (string.IsNullOrEmpty(guncellenmisUye.FOTOGRAF))
            {
                guncellenmisUye.FOTOGRAF = " ";
            }
            int id = guncellenmisUye.ID;
            using (var httpClient = new HttpClient())
            {
                var url = $"https://localhost:1433/api/uye/guncelle{id}";
                var json = JsonConvert.SerializeObject(guncellenmisUye);
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
                    return View();
                }
            }
        }
        public ActionResult UyeKitapGecmis(int id)
        {
            var httpClient = new HttpClient();
            var request = httpClient.GetAsync($"https://localhost:1433/api/hareket/gecmis{id}").Result;
            var response = request.Content.ReadAsStringAsync().Result;
            var value = JsonConvert.DeserializeObject<List<TBLHAREKET>>(response);
            var ktpgcms = value.ToList();
            var uyerequest = httpClient.GetAsync($"https://localhost:1433/api/uye/{id}").Result;
            var uyeresponse = uyerequest.Content.ReadAsStringAsync().Result;
            var uye = JsonConvert.DeserializeObject<TBLUYELER>(uyeresponse);
            List<TBLKITAP> kitapadı = new List<TBLKITAP>();
            List<TBLPERSONEL> personeladı = new List<TBLPERSONEL>();
            foreach (var item in value)
            {
                var kitaprequest = httpClient.GetAsync($"https://localhost:1433/api/kitap/kitapgetir{item.KITAP}").Result;
                var kitapresponse = kitaprequest.Content.ReadAsStringAsync().Result;
                var kitap = JsonConvert.DeserializeObject<TBLKITAP>(kitapresponse);
                kitapadı.Add(kitap);
            }
            foreach (var item in value)
            {
                var prsrequest = httpClient.GetAsync($"https://localhost:1433/api/personel/getir{item.PERSONEL}").Result;
                var prsresponse = prsrequest.Content.ReadAsStringAsync().Result;
                var personel = JsonConvert.DeserializeObject<TBLPERSONEL>(prsresponse);
                personeladı.Add(personel);
            }
            var uyekit = uye.AD + " " + uye.SOYAD;
            ViewBag.u1 = uyekit;
            ViewBag.ktp = kitapadı;
            ViewBag.prs = personeladı;
            return View(ktpgcms);


        }
    }
}