using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Belbin_Real.Models
{
    public class AddPerson2
    {
        // Класс используется для передачи данных в представление после добавления сотрудника.
        public string Login { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName {get;set;}
        public string Mname { get; set; }
        
    }
}