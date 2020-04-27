using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Belbin_Real.Models
{
    // Данная модель предназначена для передачи данных от контролера в представление по выводу данных по подразделению.
    public class Rez_depatment
    {
        public int Boss { get; set; } // Равна единице, если начальник подразделения прошел тестирование.
        public string NameDepatment { get; set; } // Наименование подразделения

        public string[] FIO { get; set; }// Массив с ФИО сотрудников

        public int[,] Results { get; set; }// Двумерный массив с результатами тестов сотрудников. В каждой строке выводится данные по одному сотруднику.

        public string Conclusion { get; set; }// Вывод рекомендаций по подразделению.

    }
}