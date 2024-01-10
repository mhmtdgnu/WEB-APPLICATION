using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication2.Models.Entity
{
    public class TBLKITAP
    {
        public int ID { get; set; }
        [Required(ErrorMessage = "Ad alanı boş bırakılamaz")]
        public string AD { get; set; }
        [Required(ErrorMessage = "Kategori alanı boş bırakılamaz")]
        public Nullable<byte> KATEGORI { get; set; }
        [Required(ErrorMessage = "Yazar alanı boş bırakılamaz")]
        public Nullable<int> YAZAR { get; set; }
        [Required(ErrorMessage = "Basımylı alanı boş bırakılamaz")]
        public string BASIMYILI { get; set; }
        [Required(ErrorMessage = "Yayınevi alanı boş bırakılamaz")]
        public string YAYINEVI { get; set; }
        [Required(ErrorMessage = "sayfa alanı boş bırakılamaz")]
        public string SAYFA { get; set; }
        public Nullable<bool> DURUM { get; set; }
        public string KITAPRESIM { get; set; }
        public virtual ICollection<TBLHAREKET> TBLHAREKET { get; set; }
        public virtual TBLKATEGORI TBLKATEGORI { get; set; }
        public virtual TBLYAZAR TBLYAZAR { get; set; }
    }
}