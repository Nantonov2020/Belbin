using Belbin_Real.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Belbin_Real.Controllers
{
    public class HomeController : Controller
    {

        public ActionResult Index()
        {

            Firm rez1 = new Firm();

            using (var contex = new MyDbContext())
            {

                var group = new Firm()
                {
                    Name = "Васюганнефтегаз",
                };

                contex.Firms.Add(group);
                contex.SaveChanges();

                var rezult = contex.Firms.Where(item=>item.Id==5);

                if (rezult.LongCount() > 0)
                {
                    rez1 = rezult.First();
                    //var rez2 = rez0.Name;
                }
               // ViewBag.Message = rezult.LongCount();
               
            }


            return View();
        }

        public ActionResult Login()
        {
            return View("Index");
        }

        [HttpPost]
        public ActionResult Login(string Login, string Password)
        {
            int rez3;
            string napr = "";
            //string napr2 = "";
            using (var contex = new MyDbContext())
            {
                var rez1= GetHashString(Password);
                var rezult = contex.HR_Managers.Where(item => item.Login == Login&&item.Password== rez1);

                if (rezult.LongCount() > 0)// Если осуществлен вход сотрудников отдела кадров.
                {
                    var rez2 = rezult.First();
                    rez3 = rez2.Id;
                    napr = "HR_man";
                    //napr2 = "HR_manager";

                    ViewBag.Message = "";

                    // Устанавливаем куки для сотрудников отдела кадров.
                    HttpCookie cookie = Request.Cookies.Get("Cook1");// В этот кук записывается Id сотрудника отдела кадров.
                    if (cookie == null)
                    {
                        cookie = new HttpCookie("Cook1");
                        cookie.Value = rez3.ToString();
                        Response.Cookies.Add(cookie);
                    }
                    HttpCookie cookie2 = Request.Cookies.Get("Cook2");// В этот кук записывается кэшированный пароль сотрудника отдела кадров.
                    if (cookie2 == null)
                    {
                        cookie2 = new HttpCookie("Cook2");
                        cookie2.Value = rez1;
                        Response.Cookies.Add(cookie2);
                    }

                }
                else
                {
                    rez3 = 0;
                    napr = "Index";
                }

                if (rez3 == 0)
                {
                    var rezult4 = contex.Workers.Where(item => item.Login == Login && item.Password == rez1);

                    if (rezult4.LongCount() > 0)// Если осуществлен вход сотрудником (не отдела кадров).
                    {
                        var rez2 = rezult4.First();
                        rez3 = rez2.Id;
                        napr = "Anketa0";
                        ViewBag.Message = "";

                        // Устанавливаем куки для сотрудников которые будут проходить тестирование.
                        HttpCookie cookie = Request.Cookies.Get("Cook3");// В этот кук записывается Id сотрудника.
                       // if (cookie == null)
                        //{
                            cookie = new HttpCookie("Cook3");
                            cookie.Value = rez3.ToString();
                            Response.Cookies.Add(cookie);
                        //}
                        HttpCookie cookie2 = Request.Cookies.Get("Cook4");// В этот кук записывается кэшированный пароль сотрудника.
                        //if (cookie2 == null)
                        //{
                            cookie2 = new HttpCookie("Cook4");
                            cookie2.Value = rez1;
                            Response.Cookies.Add(cookie2);
                        //}


                        //var rez2 = rez0.Name;
                    }
                    else
                    {
                        rez3 = 0;
                        napr = "Index";
                        ViewBag.Message = "Неверно введен логин или пароль.";
                    }

                }
                               
            }
            //ViewBag.Message = rez3;

                return View(napr);
            
        }

        public ActionResult Anketa0()
        {
            return View();
        }
        public ActionResult HR_man()
        {
            return View();
        }

        public ActionResult About()
        {
        
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Decryption_test()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Add_person()
        {
            // Читаем куки для сотрудника отдела кадров.
            string v1="";
            string v2 = "";
            HttpCookie cookie = Request.Cookies.Get("Cook1");
            if (cookie != null)
            {
                v1 = cookie.Value;
            }
            HttpCookie cookie2 = Request.Cookies.Get("Cook2");
            if (cookie2 != null)
            {
                v2 = cookie2.Value;
            }
            ViewBag.Message = v1+" - "+v2;

            return View();
        }
        public ActionResult Obrab()
        {
            return View("Index");
        }

        [HttpPost]
        public ActionResult Obrab(int p00, int p01,int p02, int p03, int p04, int p05, int p06, int p07, int p10, int p11, int p12, int p13, int p14, int p15, int p16, int p17, int p20, int p21, int p22, int p23, int p24, int p25, int p26, int p27, int p30, int p31, int p32, int p33, int p34, int p35, int p36, int p37, int p40, int p41, int p42, int p43, int p44, int p45, int p46, int p47, int p50, int p51, int p52, int p53, int p54, int p55, int p56, int p57, int p60, int p61, int p62, int p63, int p64, int p65, int p66, int p67)
        {
            //читаем куки сотрудника, который проходил тестирование.
            string v1 = "";
            string v2 = "";
            HttpCookie cookie = Request.Cookies.Get("Cook3");
            if (cookie != null)
            {
                v1 = cookie.Value;

            }
            HttpCookie cookie2 = Request.Cookies.Get("Cook4");
            if (cookie2 != null)
            {
                v2 = cookie2.Value;
            }

            if ((v1 != "") && (v2 != ""))
            {
                using (var contex = new MyDbContext())
                {
                    var v0 = Convert.ToInt32(v1);
                    var rezult = contex.Workers.Where(item => item.Id == v0 && item.Password == v2);
                    if (rezult != null) // Если в базе есть пользователь с нужным Id и его пароль соответсвует заданному, то записываем данные в базу.
                    {
                        Result result = new Result()
                        {
                            WorkerId = v0,
                            DateRez = "",
                            Results = (string)"" + p00 + "+" + p01 + "+" + p02 + "+" + p03 + "+" + p04 + "+" + p05 + "+" + p06 + "+" + p07
                                          + "+" + p10 + "+" + p11 + "+" + p12 + "+" + p13 + "+" + p14 + "+" + p15 + "+" + p16 + "+" + p17
                                          + "+" + p20 + "+" + p21 + "+" + p22 + "+" + p23 + "+" + p24 + "+" + p25 + "+" + p26 + "+" + p27
                                          + "+" + p30 + "+" + p31 + "+" + p32 + "+" + p33 + "+" + p34 + "+" + p35 + "+" + p36 + "+" + p37
                                          + "+" + p40 + "+" + p41 + "+" + p42 + "+" + p43 + "+" + p44 + "+" + p45 + "+" + p46 + "+" + p47
                                          + "+" + p50 + "+" + p51 + "+" + p52 + "+" + p53 + "+" + p54 + "+" + p55 + "+" + p56 + "+" + p57
                                          + "+" + p60 + "+" + p61 + "+" + p62 + "+" + p63 + "+" + p64 + "+" + p65 + "+" + p66 + "+" + p67
                        };
                        contex.Results.Add(result);
                        contex.SaveChanges();
                    }


                }

                }
              return View();
        }


        public ActionResult Anketa()
        {
            return View("Index");
        }

        [HttpPost]
        public ActionResult Anketa(string t)
        {
            ViewBag.Message = "Анкета";
            Anketa_V anketa_V = new Anketa_V();
            return View(anketa_V);
        }

        public string GetHashString(string s)
        {
            //метод хеширует строку.
            //переводим строку в байт-массим  
            byte[] bytes = Encoding.Unicode.GetBytes(s);

            //создаем объект для получения средст шифрования  
            MD5CryptoServiceProvider CSP =
                new MD5CryptoServiceProvider();

            //вычисляем хеш-представление в байтах  
            byte[] byteHash = CSP.ComputeHash(bytes);

            string hash = string.Empty;

            //формируем одну цельную строку из массива  
            foreach (byte b in byteHash)
                hash += string.Format("{0:x2}", b);

            return hash;
        }


    }
}