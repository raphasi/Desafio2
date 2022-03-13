using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Web.Mvc;
using System.Web.Security;

namespace Students.Controllers
{
    public class AccountController : Controller
    {
        public ActionResult Login()
        {
            FormsAuthentication.SetAuthCookie("admin", false);
            return RedirectToAction("Index", "Courses");
            //return View();
        }

        [HttpPost]
        public ActionResult Login(string password)
        {
            if (Cifrar(password) == "lMzqTTxilD8taKu0gV6B0A==")
            {
                FormsAuthentication.SetAuthCookie("admin", false);
                return RedirectToAction("Index", "Courses");
            }
            ViewBag.Message = "Senha Incorreta , tente novamente !";
            return View();
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }

        public string Cifrar(string textoPuro)
        {
            string chaveCifragem = "MACVS2014XYW";
            byte[] bytesLimpos = Encoding.Unicode.GetBytes(textoPuro);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(chaveCifragem, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(bytesLimpos, 0, bytesLimpos.Length);
                        cs.Close();
                    }
                    textoPuro = Convert.ToBase64String(ms.ToArray());
                }
            }
            return textoPuro;
        }

        public string Decifrar(string textoCifrado)
        {
            string chaveCifragem = "MACVS2014XYW";
            byte[] bytesCifrados = Convert.FromBase64String(textoCifrado);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(chaveCifragem, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(bytesCifrados, 0, bytesCifrados.Length);
                        cs.Close();
                    }
                    textoCifrado = Encoding.Unicode.GetString(ms.ToArray());
                }
            }
            return textoCifrado;
        }
    }
}
