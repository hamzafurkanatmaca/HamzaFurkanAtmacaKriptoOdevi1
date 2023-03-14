using HamzaFurkanAtmacaKriptoOdevi1.Models;
using Newtonsoft.Json;
using System;
using System.Collections;
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
                string cipherText = Encipher(req.plainText, 1);
                var refHarfAnaliziDictionary= HarfSiklikAnaliziBackend(new harfSiklikAnaliziRequestModel { cipherText= req.referansMetin.ToLower() });
                var cipherHarfAnaliziDictionary= HarfSiklikAnaliziBackend(new harfSiklikAnaliziRequestModel { cipherText= cipherText.ToLower() });
                char[] xValues = { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z' };
                var refMetinHarfHaritasiDictionary = new Dictionary<char, int>();
                var cipherTextHarfHaritasiDictionary = new Dictionary<char, int>();
                foreach (var x in xValues)
                {
                    refMetinHarfHaritasiDictionary.Add(x, 0);
                    cipherTextHarfHaritasiDictionary.Add(x, 0);
                }
                foreach (var x in xValues)
                {
                    foreach (KeyValuePair<char, int> entry in refHarfAnaliziDictionary)
                    {
                        if (entry.Key == x)
                        {
                            refMetinHarfHaritasiDictionary[x]+=entry.Value;
                        }
                    }

                }

                foreach (var x in xValues)
                {
                    foreach (KeyValuePair<char, int> entry in cipherHarfAnaliziDictionary)
                    {
                        if (entry.Key == x)
                        {
                            cipherTextHarfHaritasiDictionary[x] += entry.Value;
                        }
                    }

                }



                var sortedRefMetinDict = (from entry in refMetinHarfHaritasiDictionary orderby entry.Value descending select entry).ToList();
                var sortedPlainDict = (from entry in cipherTextHarfHaritasiDictionary orderby entry.Value descending select entry).ToList();

                    Dictionary<char,char> cozmeMap = new Dictionary<char,char>();

                for (int i = 0; i < 26; i++)
                {
                     
                    cozmeMap.Add( sortedRefMetinDict[i].Key, sortedPlainDict[i].Key); //sıra önemli.
                }

                string cozulmus = "";
                foreach (var p in cipherText.ToLower())
                {
                    foreach (KeyValuePair<char, char> entry in cozmeMap)
                    {
                        if(entry.Value == p)
                        {
                        cozulmus += entry.Key;
                        }
                    }
                }
                
 
                return Json(new { Message = cozulmus, JsonRequestBehavior.AllowGet });
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

        public Dictionary<char,int> HarfSiklikAnaliziBackend(harfSiklikAnaliziRequestModel req)
        {
            try
            {
                var freqs = req.cipherText
                    .Where(c => Char.IsLetter(c))
                    .GroupBy(c => c)
                    .ToDictionary(g => g.Key, g => g.Count());

                return freqs;
            }
            catch (Exception ex)
            {
                return null;
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


        public ActionResult BasariHesaplamasi(BasariHesaplamasiRequestModel req)
        {
            try
            {
                var fark = Compute(req.referansOrijinalMetin, req.referansCozulmusMetin);
                double basariYuzdesi = (double)req.referansOrijinalMetin.Length - fark;
                double basariYuzdesi2 = (double)basariYuzdesi / req.referansOrijinalMetin.Length;
                double basariYuzdesi3 = (double)basariYuzdesi2 * 100;
                int basariYuzdesi4 = Convert.ToInt32(basariYuzdesi3);
                return Json(new { Message = fark +" Hata. Başarı: %"+basariYuzdesi4, JsonRequestBehavior.AllowGet });
            }
            catch (Exception ex)
            {
                return Json(new { Message = "Bir hata oldu.", JsonRequestBehavior.AllowGet });
                throw;
            }

        }

        public ActionResult TahminYap(TahminYapRequestModel req)
        {
            try
            {
                if (req.cipherText.Length % 3 != 0)
                {
                    req.cipherText = req.cipherText.Substring(0,req.cipherText.Length - req.cipherText.Length%3);
                }
                var parcalanmisString = Split(req.cipherText, 3).ToArray();

                var q = (from x in parcalanmisString
                        group x by x into g
                        let count = g.Count()
                        orderby count descending
                        select new { Value = g.Key, Count = count }).ToArray();

                 

                 
                return Json(new { Message = q[0].Value +" 'the' olabilir. "+q[1].Value+" 'and' olabilir.", JsonRequestBehavior.AllowGet });
            }
            catch (Exception ex)
            {
                return Json(new { Message = "Bir hata oldu.", JsonRequestBehavior.AllowGet });
                throw;
            }

        }

        static IEnumerable<string> Split(string str, int chunkSize)
        {
            return Enumerable.Range(0, str.Length / chunkSize)
                .Select(i => str.Substring(i * chunkSize, chunkSize));
        }

        public static int Compute(string s, string t)
        {
            int n = s.Length;
            int m = t.Length;
            int[,] d = new int[n + 1, m + 1];

            // Step 1
            if (n == 0)
            {
                return m;
            }

            if (m == 0)
            {
                return n;
            }

            // Step 2
            for (int i = 0; i <= n; d[i, 0] = i++)
            {
            }

            for (int j = 0; j <= m; d[0, j] = j++)
            {
            }

            // Step 3
            for (int i = 1; i <= n; i++)
            {
                //Step 4
                for (int j = 1; j <= m; j++)
                {
                    // Step 5
                    int cost = (t[j - 1] == s[i - 1]) ? 0 : 1;

                    // Step 6
                    d[i, j] = Math.Min(
                        Math.Min(d[i - 1, j] + 1, d[i, j - 1] + 1),
                        d[i - 1, j - 1] + cost);
                }
            }
            // Step 7
            return d[n, m];
        }
    }
}