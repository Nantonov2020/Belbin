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
    public class HR_managerController : Controller
    {
        public ActionResult Add_person()
        {
            // Читаем куки для сотрудника отдела кадров. Только в части опреледения Id.
            string v1 = "0";
            HttpCookie cookie = Request.Cookies.Get("Cook1");
            if (cookie != null)
            {
                v1 = cookie.Value;
                var v0 = Convert.ToInt32(v1);
                

                using (var contex = new MyDbContext())
                {
                    var collection0 = contex.HR_Managers.Where(item => item.Id == v0);
                    var ppp = collection0.First();
                    var DepID = ppp.FirmId;
                    

                    var collection = contex.Departments.Where(item => item.FirmId == DepID);// Выбираем все подразделения, которые есть в фирме данного сотрудника отдела кадров.
                    if (collection != null)
                    {
                        List<string> NamDep= new List<string>();
                        List<int> IdDep = new List<int>();
                        
                        foreach (Department iii in collection)
                        {
                            var v5 = iii.Name;
                            var v6 = iii.Id;

                            NamDep.Add(v5);
                            IdDep.Add(v6);
                         }

                         AddPerson addPerson = new AddPerson()
                            {
                                Name = NamDep,
                                Id = IdDep
                            };
                       
                       return View(addPerson);// Передаем в представление лист с наименованием подразделений и их номерами. Только подразделения фирмы, в которой работает сотрудник ОК.

                    }
                    else
                    {

                    }

                }
                 return View();
            }
            else
            {
                return View("HR_man");
            }
            
        }

        public ActionResult HR_man()
        {
            return View();
        }

        public ActionResult Add_person_processing()
        {
            return View("HR_man");
        }

        public ActionResult Rez_department_processing()
        {
            return View("HR_man");
        }
        public ActionResult Del_department()
        {
           return View();
        }
        public ActionResult Analiz_candidates()
        {
            return View();
        }
        public ActionResult Rename_department()
        {
            return View();
        }

        public int[,] ResultForExit { get; private set; }


        [HttpPost]
        public ActionResult Rez_department_processing(int IdDep)
        {
            // Читаем куки для сотрудника отдела кадров.
            string v1 = "0";
            string v2 = "";
            string NamDep="";
            int PresenceBoss = 0;
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

            using (var contex = new MyDbContext())
            {
                var v0 = Convert.ToInt32(v1);
                string ErrorString = "";
                List<int> SpisokOfWorkers = new List<int>();// В этот список вносим номера сотрудников из нужного департамента.
                List<string> SpisokOfResults = new List<string>();// В этот список помещаем строки с результатами тестирования.
                List<string> FIO_for_exit = new List<string>();// В этот список помещаем ФИО сотрудников по которым нужно выдать итоговую форму.
                string CommentString = ""; // Строка с комментариями по сформированной команде.
                var check = contex.HR_Managers.Where(item => item.Id == v0 && item.Password == v2);// Проверяем соответствие Id и пароля сотрудника отдела кадров.
                if (check.Count() != 0) // Если Id соответсвует паролю, то все нормально.
                {
                    var check1 = contex.Workers.Where(item=>item.DepartmentId==IdDep).OrderByDescending(item=>item.Boss);
                    if (check1.Count() == 0) { ErrorString += " В данном подразделении нет сотрудников. "; } else
                    {
                        var check5 = contex.Departments.Where(item => item.Id == IdDep);
                        NamDep = check5.First().Name;
                                                                       
                        foreach (Worker iii in check1)
                        {
                            SpisokOfWorkers.Add(iii.Id);
                        }

                        if (SpisokOfWorkers.Count == 0) { ErrorString += " В данном подразделении нет сотрудников. "; }
                        else
                        {
                            //var check2 = contex.Results.Select;
                            List<int> IdWorkersForDelete = new List<int>();// Список с Id сотрудников, которые не прошли тестирование.
                            foreach (int iii in SpisokOfWorkers)
                            {
                                var check2 = contex.Results.Where(item => item.WorkerId == iii);
                                if (check2.Count() != 0)
                                {
                                    var check3 = check2.FirstOrDefault();
                                    SpisokOfResults.Add(check3.Results);
                                }
                                else
                                {
                                    IdWorkersForDelete.Add(iii);// Записываем на удаление Id тех, кто не прошел тестирование.
                                }

                            }

                            if (IdWorkersForDelete.Count != 0)
                            {
                                foreach(int iii in IdWorkersForDelete)
                                {
                                    SpisokOfWorkers.Remove(iii);// Удаляем из списка сотрудников тех кто не прошел тестирование.
                                }

                            }

                            foreach (int iii in SpisokOfWorkers)
                            {
                                var check8 = contex.Workers.Where(item => item.Id == iii);
                                var check9 = check8.First();
                                v1 = check9.Lastname + " " + check9.Firstname[0] + "." + check9.Mname[0] + ".";
                                if (check9.Boss==1) { PresenceBoss = 1; } // Если у начальника заполненная анкета.
                                FIO_for_exit.Add(v1);
                            }
                            var prom88 = SpisokOfResults.Count; // Количество заполненных анкет в подразделении.
                            if (prom88 == 0) { ErrorString += " Ни один сотрудник данного подразделения не прошел тестирование. "; } else
                            {
                                ResultForExit = new int[prom88, 8];
                                int[] prom99 = new int[8];

                                for (int iii=0;iii< prom88; iii++)
                                {
                                    prom99 = AnalizRes(SpisokOfResults[iii]);

                                    for (int ii1 = 0; ii1 < 8; ii1++)
                                    {
                                        ResultForExit[iii, ii1] = prom99[ii1];
                                    }

                                }
                                
                                // Начало формирования строки с комментариями.
                                if (prom88 < 3) { CommentString += " Коллектив не сформирован (менее 3 человек). "; }
                                

                                int[,] VeryBigRoleArray2 = new int[prom88, 8]; // Массив с очень сильными ролями.
                                int[,] BigRoleArray2 = new int[prom88, 8]; // Массив с сильными ролями.
                                int[,] WeakRoleArray2 = new int[prom88, 8]; // Массив со слабыми ролями.
                                int[] VeryBigRoleArray = new int[8]; // Массив с количеством сотрудников с очень сильными ролями.
                                int[] BigRoleArray = new int[8]; // Массив с количеством сотрудников с сильными ролями.
                                int[] WeakRoleArray = new int[8]; // Массив с количеством сотрудников со слабыми ролями.

                                for (int iii = 0; iii < prom88; iii++)
                                {
                                    for (int ii1 = 0; ii1 < 8; ii1++)
                                    {
                                        if (ii1 == 0)// Подсчет для РП
                                        {
                                            if (ResultForExit[iii, ii1] < 7) { WeakRoleArray2[iii, ii1] = 1; WeakRoleArray[ii1] += 1; }
                                            if (ResultForExit[iii, ii1] >16) { VeryBigRoleArray2[iii, ii1] = 1; VeryBigRoleArray[ii1] += 1; }
                                            if ((ResultForExit[iii, ii1] < 17)&&(ResultForExit[iii, ii1] > 11)) { BigRoleArray2[iii, ii1] = 1; BigRoleArray[ii1] += 1; }
                                        }
                                        if (ii1 == 1)// Подсчет для РК
                                        {
                                            if (ResultForExit[iii, ii1] < 7) { WeakRoleArray2[iii, ii1] = 1; WeakRoleArray[ii1] += 1; }
                                            if (ResultForExit[iii, ii1] > 13) { VeryBigRoleArray2[iii, ii1] = 1; VeryBigRoleArray[ii1] += 1; }
                                            if ((ResultForExit[iii, ii1] < 14) && (ResultForExit[iii, ii1] > 10)) { BigRoleArray2[iii, ii1] = 1; BigRoleArray[ii1] += 1; }
                                        }
                                        if (ii1 == 2)// Подсчет для МТ
                                        {
                                            if (ResultForExit[iii, ii1] < 9) { WeakRoleArray2[iii, ii1] = 1; WeakRoleArray[ii1] += 1; }
                                            if (ResultForExit[iii, ii1] > 17) { VeryBigRoleArray2[iii, ii1] = 1; VeryBigRoleArray[ii1] += 1; }
                                            if ((ResultForExit[iii, ii1] < 18) && (ResultForExit[iii, ii1] > 13)) { BigRoleArray2[iii, ii1] = 1; BigRoleArray[ii1] += 1; }
                                        }
                                        if (ii1 == 3)// Подсчет для ГИ
                                        {
                                            if (ResultForExit[iii, ii1] < 5) { WeakRoleArray2[iii, ii1] = 1; WeakRoleArray[ii1] += 1; }
                                            if (ResultForExit[iii, ii1] > 12) { VeryBigRoleArray2[iii, ii1] = 1; VeryBigRoleArray[ii1] += 1; }
                                            if ((ResultForExit[iii, ii1] < 13) && (ResultForExit[iii, ii1] > 8)) { BigRoleArray2[iii, ii1] = 1; BigRoleArray[ii1] += 1; }
                                        }
                                        if (ii1 == 4)// Подсчет для СН
                                        {
                                            if (ResultForExit[iii, ii1] < 7) { WeakRoleArray2[iii, ii1] = 1; WeakRoleArray[ii1] += 1; }
                                            if (ResultForExit[iii, ii1] > 11) { VeryBigRoleArray2[iii, ii1] = 1; VeryBigRoleArray[ii1] += 1; }
                                            if ((ResultForExit[iii, ii1] < 12) && (ResultForExit[iii, ii1] > 9)) { BigRoleArray2[iii, ii1] = 1; BigRoleArray[ii1] += 1; }
                                        }
                                        if (ii1 == 5)// Подсчет для АН
                                        {
                                            if (ResultForExit[iii, ii1] < 6) { WeakRoleArray2[iii, ii1] = 1; WeakRoleArray[ii1] += 1; }
                                            if (ResultForExit[iii, ii1] > 12) { VeryBigRoleArray2[iii, ii1] = 1; VeryBigRoleArray[ii1] += 1; }
                                            if ((ResultForExit[iii, ii1] < 13) && (ResultForExit[iii, ii1] > 9)) { BigRoleArray2[iii, ii1] = 1; BigRoleArray[ii1] += 1; }
                                        }
                                        if (ii1 == 6)// Подсчет для ВД
                                        {
                                            if (ResultForExit[iii, ii1] < 9) { WeakRoleArray2[iii, ii1] = 1; WeakRoleArray[ii1] += 1; }
                                            if (ResultForExit[iii, ii1] > 16) { VeryBigRoleArray2[iii, ii1] = 1; VeryBigRoleArray[ii1] += 1; }
                                            if ((ResultForExit[iii, ii1] < 17) && (ResultForExit[iii, ii1] > 12)) { BigRoleArray2[iii, ii1] = 1; BigRoleArray[ii1] += 1; }
                                        }
                                        if (ii1 == 7)// Подсчет для КН
                                        {
                                            if (ResultForExit[iii, ii1] < 4) { WeakRoleArray2[iii, ii1] = 1; WeakRoleArray[ii1] += 1; }
                                            if (ResultForExit[iii, ii1] > 9) { VeryBigRoleArray2[iii, ii1] = 1; VeryBigRoleArray[ii1] += 1; }
                                            if ((ResultForExit[iii, ii1] < 10) && (ResultForExit[iii, ii1] > 6)) { BigRoleArray2[iii, ii1] = 1; BigRoleArray[ii1] += 1; }
                                        }
                                    }

                                }
                                if (CommentString == "") // Если количество сотрудников три и более.
                                {
                                    if (prom88 > 9) { CommentString += " В коллективе более 9 сотрудников. Это затрудняет руководство подразделением. "; }
                                    if (WeakRoleArray[0] == prom88) CommentString += " В подразделении отсутствуют исполнители (РП), которые могут выполнять основной объем работы. ";
                                    if (WeakRoleArray[7] == prom88) CommentString += " В подразделении отсутствуют сотрудники (КТ), способные доводить работы до завершения. ";
                                    if (VeryBigRoleArray[3] > 1) CommentString += " Наличие двух и более генераторов идей (ГИ) может привести к непродуктивной работе данных сотрудников. ";
                                    if ((VeryBigRoleArray[3] + BigRoleArray[3]) == 0) CommentString += " Остутвуют сотрудники, способные генерировать новые идеи(ГИ). ";
                                    if ((PresenceBoss==1)&&(VeryBigRoleArray2[0,7])==1) CommentString += " Руководитель подразделения склонен к роли Контролера(КН), что не является оптимальным для руководителя подразделения. ";
                                }
                                // Окончание формирования строки 
                            }
                        }
                    }
                    
                    if (ErrorString == "")
                    { // Если ошибок нет, то выводим страницу с данными по подразделению.


                        Rez_depatment rez_Depatment = new Rez_depatment()
                        {
                            Boss= PresenceBoss,
                            NameDepatment = NamDep,
                            FIO = FIO_for_exit.ToArray(),
                            Results = ResultForExit,
                            Conclusion = CommentString
                        };
                        
                        return View(rez_Depatment);
                    }
                    else
                    {
                        ViewBag.Message = ErrorString;
                        return View("Hr_man");
                    }
                    
                }
                else
                {
                    return View("Hr_man");
                }
            }
             
        }

        public ActionResult Rez_department()
        {
            // Читаем куки для сотрудника отдела кадров. Только в части опреледения Id.
            string v1 = "0";
            HttpCookie cookie = Request.Cookies.Get("Cook1");
            if (cookie != null)
            {
                v1 = cookie.Value;
                var v0 = Convert.ToInt32(v1);


                using (var contex = new MyDbContext())
                {
                    var collection0 = contex.HR_Managers.Where(item => item.Id == v0);
                    var ppp = collection0.First();
                    var DepID = ppp.FirmId;


                    var collection = contex.Departments.Where(item => item.FirmId == DepID);// Выбираем все подразделения, которые есть в фирме данного сотрудника отдела кадров.
                    if (collection != null)
                    {
                        List<string> NamDep = new List<string>();
                        List<int> IdDep = new List<int>();

                        foreach (Department iii in collection)
                        {
                            var v5 = iii.Name;
                            var v6 = iii.Id;

                            NamDep.Add(v5);
                            IdDep.Add(v6);
                        }

                        AddPerson addPerson = new AddPerson()
                        {
                            Name = NamDep,
                            Id = IdDep
                        };

                        return View(addPerson);// Передаем в представление лист с наименованием подразделений и их номерами. Только подразделения фирмы, в которой работает сотрудник ОК.

                    }
                    else
                    {

                    }

                }
                return View();
            }
            else
            {
                return View("HR_man");
            }


        }


        [HttpPost]
        public ActionResult Add_person_processing(string LastName, string FirsName, string Mname, string Email, Boolean EmailYes, int IdDep, Boolean Boss, Boolean Candidate)
        {// Обработка формы внесения нового сотрудника.

            // Читаем куки для сотрудника отдела кадров.
            string v1 = "0";
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

            using (var contex = new MyDbContext())
            {
                var v0 = Convert.ToInt32(v1);
                var check = contex.HR_Managers.Where(item => item.Id == v0 && item.Password == v2);// Проверяем соответствие Id и пароля сотрудника отдела кадров.
                if (check != null) // Если Id соответсвует паролю, то все нормально.
                {
                    HR_Manager check1 = check.First();
                    var Firm = check1.FirmId;

                    string Password = "";
                    int iii5;
                    
                    var rnd = new Random();
                    for (int iii = 0; iii < 9; iii++)
                    {
                        if (iii % 2 == 0) { iii5 = rnd.Next(97, 122); }
                        else
                        {
                            iii5 = rnd.Next(65, 90);
                        }
                        var iii6 = (char)iii5;
                        Password+= iii6.ToString();
                    }

                    var PasswordForBd = GetHashString(Password);

                    int BossForBd, CandidateForBd;
                    if (Boss){BossForBd = 1;}
                    else{BossForBd = 0;}

                    if (Candidate) {CandidateForBd = 1; }
                    else {CandidateForBd = 0; }

                    Worker worker = new Worker()
                    {
                        DepartmentId= IdDep,
                        Boss= BossForBd,
                        Candidate= CandidateForBd,
                        Firstname= FirsName,
                        Lastname= LastName,
                        Mname= Mname,
                        Email= Email,
                        Login= Email,
                        Password= PasswordForBd
                    };

                    contex.Workers.Add(worker);
                    contex.SaveChanges();
                    ViewBag.Message = "Сотрудник добавлен";

                    AddPerson2 addPerson2 = new AddPerson2()
                    {
                        FirstName = FirsName,
                        LastName = LastName,
                        Mname = Mname,
                        Login = Email,
                        Password = Password
                    };
                    return View("Add_person_processing",addPerson2);

                }
                else
                {// Если Id не соответствует паролю.
                    ViewBag.Message = " ";
                    return View("HR_man");
                }
            }

            
        }

        public ActionResult Add_department()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Add_depatment_processing(string Name)
        {
            // Читаем куки для сотрудника отдела кадров.
            string v1 = "0";
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

            using (var contex = new MyDbContext())
            {
                var v0= Convert.ToInt32(v1);
                var check = contex.HR_Managers.Where(item=>item.Id==v0&&item.Password==v2);// Проверяем соответствие Id и пароля сотрудника отдела кадров.
                if (check!=null) // Если Id соответсвует паролю, то все нормально.
                {
                    HR_Manager check1 = check.First();
                    var Firm = check1.FirmId;

                    Department department = new Department()
                    {
                        FirmId = Firm,
                        Name = Name
                    };

                    contex.Departments.Add(department);
                    contex.SaveChanges();
                    ViewBag.Message = "Подразделение добавлено.";
                    return View("HR_man");

                }
                else
                {// Если Id не соответствует паролю.
                    ViewBag.Message = " ";
                    return View("HR_man");
                }
            }
                       
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

        public int[] AnalizRes(string s)
        {// Метод получает строку результата и возвращает расшифрованный массив значений.
            int[] ArrayRes = new int[8];
            string [] NachZnach = new string[56];
            int[] NachZnach1 = new int[56];
            NachZnach = s.Split('+');
            for (int iii = 0; iii < 56; iii++)
            {
                NachZnach1[iii]= Convert.ToInt32(NachZnach[iii]);
            }

            ArrayRes[0] = NachZnach1[6]+ NachZnach1[8]+ NachZnach1[23]+ NachZnach1[27]+ NachZnach1[33]+ NachZnach1[45]+ NachZnach1[52];
            ArrayRes[1] = NachZnach1[3] + NachZnach1[9] + NachZnach1[16] + NachZnach1[31] + NachZnach1[37] + NachZnach1[42] + NachZnach1[54];
            ArrayRes[2] = NachZnach1[5] + NachZnach1[12] + NachZnach1[18] + NachZnach1[25] + NachZnach1[35] + NachZnach1[46] + NachZnach1[48]; 
            ArrayRes[3] = NachZnach1[2] + NachZnach1[14] + NachZnach1[19] + NachZnach1[28] + NachZnach1[39] + NachZnach1[40] + NachZnach1[53];
            ArrayRes[4] = NachZnach1[0] + NachZnach1[10] + NachZnach1[21] + NachZnach1[30] + NachZnach1[36] + NachZnach1[47] + NachZnach1[51];
            ArrayRes[5] = NachZnach1[7] + NachZnach1[11] + NachZnach1[22] + NachZnach1[26] + NachZnach1[32] + NachZnach1[44] + NachZnach1[49];
            ArrayRes[6] = NachZnach1[1] + NachZnach1[13] + NachZnach1[20] + NachZnach1[24] + NachZnach1[34] + NachZnach1[41] + NachZnach1[55];
            ArrayRes[7] = NachZnach1[4] + NachZnach1[15] + NachZnach1[17] + NachZnach1[29] + NachZnach1[38] + NachZnach1[43] + NachZnach1[50];

            return ArrayRes;
        }
    }
}