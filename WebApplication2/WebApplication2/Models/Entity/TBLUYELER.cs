using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication2.Models.Entity
{
    public class TBLUYELER
    {
        public int ID { get; set; }
        [Required(ErrorMessage = "Üye adı alanı boş bırakılamaz")]
        public string AD { get; set; }
        [Required(ErrorMessage = "Üye soyadı alanı boş bırakılamaz")]
        public string SOYAD { get; set; }
        [Required(ErrorMessage = "E-mail adresi alanı boş bırakılamaz")]
        public string MAIL { get; set; }
        [Required(ErrorMessage = "Kullanıcı adı alanı boş bırakılamaz")]
        public string KULLANICIADI { get; set; }
        [Required(ErrorMessage = "Şifre alanı boş bırakılamaz")]
        public string SIFRE { get; set; }
        public string FOTOGRAF { get; set; }
        [Required(ErrorMessage = "Telefon alanı boş bırakılamaz")]
        public string TELEFON { get; set; }
        [Required(ErrorMessage = "Okul alanı boş bırakılamaz")]
        public string OKUL { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TBLCEZALAR> TBLCEZALAR { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TBLHAREKET> TBLHAREKET { get; set; }
    }
}