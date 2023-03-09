using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HamzaFurkanAtmacaKriptoOdevi1.Models
{
    public class CozForRefRequestModel
    {
        public string cipherText { get; set; }
        public string plainText { get; set; }
        public int key { get; set; }
        public string referansMetin { get; set; }
    }
}