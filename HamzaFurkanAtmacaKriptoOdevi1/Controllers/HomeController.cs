using HamzaFurkanAtmacaKriptoOdevi1.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HamzaFurkanAtmacaKriptoOdevi1.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Sifrele(sifreleRequestModel req)
        {
            try
            {
                string cipherText = Encipher(req.plainText, req.key);
                string message = cipherText;
                return Json(new { Message = message, JsonRequestBehavior.AllowGet });
            }
            catch (Exception ex)
            {
                return Json(new { Message = "Bir hata oldu.", JsonRequestBehavior.AllowGet });
                throw;
            }

        }

        public ActionResult Coz(cozRequestModel req)
        {
            try
            {
                string plainText = Decipher(req.cipherText, req.key);
                string message = plainText;
                return Json(new { Message = message, JsonRequestBehavior.AllowGet });
            }
            catch (Exception ex)
            {
                return Json(new { Message = "Bir hata oldu.", JsonRequestBehavior.AllowGet });
                throw;
            }

        }         
        
        public ActionResult CozForRef(CozForRefRequestModel req)
        {
            try
            {
                string plainText = Decipher(req.cipherText, req.key);
                string message = plainText;
                return Json(new { Message = message, JsonRequestBehavior.AllowGet });
            }
            catch (Exception ex)
            {
                return Json(new { Message = "Bir hata oldu.", JsonRequestBehavior.AllowGet });
                throw;
            }

        }      
        
        public ActionResult HarfSiklikAnalizi(harfSiklikAnaliziRequestModel req)
        {
            try
            {
                var freqs = req.cipherText
                    .Where(c => Char.IsLetter(c))
                    .GroupBy(c => c)
                    .ToDictionary(g => g.Key, g => g.Count());

                var freqsJson=JsonConvert.SerializeObject(freqs);
                return Json(new { Message = freqsJson, JsonRequestBehavior.AllowGet });
            }
            catch (Exception ex)
            {
                return Json(new { Message = "Bir hata oldu.", JsonRequestBehavior.AllowGet });
                throw;
            }

        }

        public static char cipher(char chr, int key)
        {
            if (!char.IsLetter(chr))
            {

                return chr;
            }

            char d = char.IsUpper(chr) ? 'A' : 'a';
            return (char)((((chr + key) - d) % 26) + d);


        }


        public static string Encipher(string input, int key)
        {
            string output = string.Empty;

            foreach (char ch in input)
                output += cipher(ch, key);

            return output;
        }

        public static string Decipher(string input, int key)
        {
            return Encipher(input, 26 - (key % 26));
        }
    }
}