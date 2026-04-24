using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Media;
using Tournament_Master.Models;

namespace Tournament_Master.Views
{
    public partial class LeaderboardPage : Page
    {
        public LeaderboardPage()
        {
            InitializeComponent();
            LoadLeaderboard();
        }

        private void LoadLeaderboard()
        {
            var players = DataStorage.AllParticipants.Select(p => new PlayerStatsDisplay
            {
                Name = $"{p.FirstName} {p.LastName}"
            }).ToList();

            foreach (var match in DataStorage.AllMatches)
            {
                var p1 = players.FirstOrDefault(p => p.Name == match.Team1);
                var p2 = players.FirstOrDefault(p => p.Name == match.Team2);

                if (p1 != null && p2 != null)
                {
                    p1.MatchesPlayed++; p2.MatchesPlayed++;
                    p1.GoalsScored += match.Score1; p1.GoalsConceded += match.Score2;
                    p2.GoalsScored += match.Score2; p2.GoalsConceded += match.Score1;

                    if (match.Score1 > match.Score2) { p1.Points += 3; }
                    else if (match.Score1 < match.Score2) { p2.Points += 3; }
                    else { p1.Points += 1; p2.Points += 1; }
                }
            }

            var sorted = players.OrderByDescending(p => p.Points)
                                .ThenByDescending(p => p.GoalsScored - p.GoalsConceded)
                                .ToList();

            // Призначаємо позиції та кольори
            for (int i = 0; i < sorted.Count; i++)
            {
                sorted[i].Position = i + 1;
                if (i == 0) { sorted[i].PosBackground = new SolidColorBrush(Color.FromRgb(255, 177, 12)); sorted[i].PosForeground = Brushes.White; } // Золото/Помаранчевий
                else if (i < 3) { sorted[i].PosBackground = new SolidColorBrush(Color.FromRgb(45, 45, 45)); sorted[i].PosForeground = Brushes.White; } // Топ-3
                else { sorted[i].PosBackground = Brushes.Transparent; sorted[i].PosForeground = Brushes.Gray; }
            }

            // ВИПРАВЛЕНО: тепер звертаємося до LeaderboardList
            LeaderboardList.ItemsSource = sorted;
        }
    }

    public class PlayerStatsDisplay
    {
        public int Position { get; set; }
        public string Name { get; set; }
        public int MatchesPlayed { get; set; }
        public int GoalsScored { get; set; }
        public int GoalsConceded { get; set; }
        public int Points { get; set; }
        public string GoalsDiff => $"{GoalsScored}:{GoalsConceded}";
        public Brush PosBackground { get; set; }
        public Brush PosForeground { get; set; }
    }
}