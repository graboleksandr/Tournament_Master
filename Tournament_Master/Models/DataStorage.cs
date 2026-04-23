using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Security.Cryptography;
using Tournament_Master.Models;

namespace Tournament_Master
{
    public static class DataStorage
    {
        // 1. Списки даних
        public static ObservableCollection<Participant> AllParticipants { get; set; } = new ObservableCollection<Participant>();
        public static ObservableCollection<Team> AllTeams { get; set; } = new ObservableCollection<Team>();
        public static ObservableCollection<Match> AllMatches { get; set; } = new ObservableCollection<Match>();

        // НОВЕ: Список збережених турнірів для Головної сторінки
        public static ObservableCollection<Tournament> SavedTournaments { get; set; } = new ObservableCollection<Tournament>();

        // 2. Налаштування турніру
        public static string CurrentUser { get; set; }
        public static string CurrentTournamentName { get; set; } = "Мій Турнір";
        public static bool IsTeamMode { get; set; } = false;

        // Назви файлів
        private const string ParticipantsFile = "participants_data.json";
        private const string TeamsFile = "teams_data.json";
        private const string MatchesFile = "matches_data.json";
        private const string SettingsFile = "settings_data.json";
        private const string ArchiveFile = "tournaments_archive.json"; // Файл для збережених турнірів

        public static void SaveAll()
        {
            try
            {
                var options = new JsonSerializerOptions { WriteIndented = true };

                File.WriteAllText(ParticipantsFile, JsonSerializer.Serialize(AllParticipants, options));
                File.WriteAllText(TeamsFile, JsonSerializer.Serialize(AllTeams, options));
                File.WriteAllText(MatchesFile, JsonSerializer.Serialize(AllMatches, options));

                // Зберігаємо архів турнірів
                File.WriteAllText(ArchiveFile, JsonSerializer.Serialize(SavedTournaments, options));

                var settings = new Dictionary<string, string>
                {
                    { "TournamentName", CurrentTournamentName },
                    { "IsTeamMode", IsTeamMode.ToString() }
                };
                File.WriteAllText(SettingsFile, JsonSerializer.Serialize(settings, options));
            }
            catch (Exception ex) { System.Diagnostics.Debug.WriteLine(ex.Message); }
        }

        public static void LoadAll()
        {
            try
            {
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

                // Завантаження учасників
                if (File.Exists(ParticipantsFile))
                {
                    var data = JsonSerializer.Deserialize<ObservableCollection<Participant>>(File.ReadAllText(ParticipantsFile), options);
                    if (data != null) AllParticipants = data;
                }

                // Завантаження команд
                if (File.Exists(TeamsFile))
                {
                    var data = JsonSerializer.Deserialize<ObservableCollection<Team>>(File.ReadAllText(TeamsFile), options);
                    if (data != null) AllTeams = data;
                }

                // Завантаження матчів
                if (File.Exists(MatchesFile))
                {
                    var data = JsonSerializer.Deserialize<ObservableCollection<Match>>(File.ReadAllText(MatchesFile), options);
                    if (data != null) AllMatches = data;
                }

                // НОВЕ: Завантаження архіву турнірів для Головної
                if (File.Exists(ArchiveFile))
                {
                    var data = JsonSerializer.Deserialize<ObservableCollection<Tournament>>(File.ReadAllText(ArchiveFile), options);
                    if (data != null) SavedTournaments = data;
                }

                // Завантаження налаштувань
                if (File.Exists(SettingsFile))
                {
                    var settings = JsonSerializer.Deserialize<Dictionary<string, string>>(File.ReadAllText(SettingsFile));
                    if (settings != null)
                    {
                        if (settings.ContainsKey("TournamentName")) CurrentTournamentName = settings["TournamentName"];
                        if (settings.ContainsKey("IsTeamMode")) IsTeamMode = bool.Parse(settings["IsTeamMode"]);
                    }
                }
            }
            catch (Exception ex) { System.Diagnostics.Debug.WriteLine(ex.Message); }
        }

        public static string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(bytes);
            }
        }
    }
}