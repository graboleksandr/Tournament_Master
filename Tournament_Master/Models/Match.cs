using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tournament_Master.Models
{
    public class Match
    {
        public string Round { get; set; }
        public string Team1 { get; set; } // Використовуємо Team1
        public string Team2 { get; set; } // Використовуємо Team2
        public int Score1 { get; set; }
        public int Score2 { get; set; }

        // Додаткове поле для статусу, якщо потрібно
        public string Result => $"{Score1} : {Score2}";
    }
}
