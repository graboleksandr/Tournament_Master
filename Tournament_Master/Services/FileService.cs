using System;
using System.IO;
using System.Text.Json;
using System.Collections.Generic;
using Tournament_Master.Models;

namespace Tournament_Master.Services
{
    public static class FileService
    {
        private static readonly string FileName = "database.json";

        public static void SaveData()
        {
            try
            {
                var data = new
                {
                    Participants = DataStorage.AllParticipants,
                    Teams = DataStorage.AllTeams
                };

                // Додамо невелике форматування (WriteIndented), щоб файл було легко читати
                var options = new JsonSerializerOptions { WriteIndented = true };
                string json = JsonSerializer.Serialize(data, options);

                File.WriteAllText(FileName, json);
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Помилка збереження: {ex.Message}");
            }
        }

        public static void LoadData()
        {
            if (!File.Exists(FileName)) return;

            try
            {
                string json = File.ReadAllText(FileName);
                var data = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(json);

                if (data != null)
                {
                    // Десеріалізація учасників
                    if (data.ContainsKey("Participants"))
                    {
                        var participants = JsonSerializer.Deserialize<List<Participant>>(data["Participants"].GetRawText());
                        DataStorage.AllParticipants.Clear();
                        foreach (var p in participants) DataStorage.AllParticipants.Add(p);
                    }

                    // Десеріалізація команд
                    if (data.ContainsKey("Teams"))
                    {
                        var teams = JsonSerializer.Deserialize<List<Team>>(data["Teams"].GetRawText());
                        DataStorage.AllTeams.Clear();
                        foreach (var t in teams) DataStorage.AllTeams.Add(t);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Помилка завантаження: {ex.Message}");
            }
        }
    }
}