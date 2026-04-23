using System;
using System.Collections.Generic;
using System.Linq;

namespace Tournament_Master.Models
{
    public class Team
    {
        // Основна інформація про команду
        public string TeamName { get; set; }

        // Список гравців, що входять до команди (Сценарій 2)
        public List<Participant> Members { get; set; } = new List<Participant>();

        // Властивість для відображення кількості гравців у інтерфейсі TeamsPage
        // Виправлено: прибрано помилкові символи ";ч"
        public string PlayersCountText => $"Гравців у команді: {Members?.Count ?? 0}";

        // --- Статистика команди (для відображення у LeaderboardPage) ---

        public int MatchesPlayed { get; set; }
        public int Wins { get; set; }
        public int Draws { get; set; }
        public int Losses { get; set; }
        public int GoalsScored { get; set; }
        public int GoalsConceded { get; set; }
        public int Points { get; set; }

        // Властивість для виведення назви команди в універсальних списках
        public string DisplayName => TeamName;

        // Властивість для виведення різниці м'ячів (наприклад, "10:4")
        public string GoalsDiff => $"{GoalsScored}:{GoalsConceded}";
    }
}