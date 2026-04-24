using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tournament_Master.Models
{
    public enum TournamentType
    {
        SingleElimination, // Плей-офф (виліт після першої поразки)
        DoubleElimination, // Плей-офф (виліт після другої поразки)
        RoundRobin,        // Кожен з кожним (кругова система)
        SwissSystem        // Швейцарська система
    }

    public class Tournament
    {
        public string Title { get; set; }
        public string Date { get; set; }
        public string Icon { get; set; } // Наприклад "🏆" або "🎮"
        public string Info { get; set; } // "Гравців: 16"

        public ObservableCollection<Match> Matches { get; set; } = new ObservableCollection<Match>();
    }
}
