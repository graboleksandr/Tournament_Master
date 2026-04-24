using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Tournament_Master.Models;

namespace Tournament_Master.Views
{
    public partial class SchedulePage : Page
    {
        // Поточний турнір, з яким працює сторінка
        private Tournament _currentTournament;

        // Конструктор для створення НОВОГО турніру (якщо перейшли через меню)
        public SchedulePage()
        {
            InitializeComponent();
            _currentTournament = null;
            RefreshList();
        }

        // Конструктор для ВІДКРИТТЯ існуючого турніру (з головної сторінки)
        public SchedulePage(Tournament tournament)
        {
            InitializeComponent();
            _currentTournament = tournament;

            // Якщо ми відкрили існуючий турнір, підтягуємо його назву в UI
            // Припускаємо, що у вас в XAML є TextBlock з іменем TournamentNameHeader
            // TournamentNameHeader.Text = _currentTournament.Title;

            RefreshList();
        }

        private void RefreshList()
        {
            MatchesList.ItemsSource = null;

            // Якщо ми всередині конкретного турніру — беремо його матчі, 
            // якщо ні — показуємо глобальний список
            if (_currentTournament != null && _currentTournament.Matches != null)
            {
                MatchesList.ItemsSource = _currentTournament.Matches;
            }
            else
            {
                MatchesList.ItemsSource = DataStorage.AllMatches;
            }
        }

        private void BtnGenerateSchedule_Click(object sender, RoutedEventArgs e)
        {
            List<string> opponents = new List<string>();

            if (DataStorage.IsTeamMode)
            {
                opponents = DataStorage.AllTeams.Select(t => t.TeamName).ToList();
            }
            else
            {
                opponents = DataStorage.AllParticipants.Select(p => $"{p.FirstName} {p.LastName}").ToList();
            }

            if (opponents.Count < 2)
            {
                MessageBox.Show("Недостатньо учасників для генерації розкладу!");
                return;
            }

            // Рандомізація (Fisher-Yates)
            Random rng = new Random();
            int n = opponents.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                string value = opponents[k];
                opponents[k] = opponents[n];
                opponents[n] = value;
            }

            // Очищуємо список матчів (або глобальний, або конкретного турніру)
            if (_currentTournament != null)
            {
                if (_currentTournament.Matches == null)
                    _currentTournament.Matches = new System.Collections.ObjectModel.ObservableCollection<Match>();
                _currentTournament.Matches.Clear();
            }
            else
            {
                DataStorage.AllMatches.Clear();
            }

            // Генерація пар
            for (int i = 0; i < opponents.Count; i += 2)
            {
                if (i + 1 < opponents.Count)
                {
                    var newMatch = new Match
                    {
                        Team1 = opponents[i],
                        Team2 = opponents[i + 1],
                        Score1 = 0,
                        Score2 = 0,
                        Round = "Раунд 1"
                    };

                    if (_currentTournament != null)
                        _currentTournament.Matches.Add(newMatch);
                    else
                        DataStorage.AllMatches.Add(newMatch);
                }
            }

            DataStorage.SaveAll();
            RefreshList();
        }

        private void BtnDeleteMatch_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var matchToRemove = button?.DataContext as Match;

            if (matchToRemove != null)
            {
                var result = MessageBox.Show("Ви впевнені, що хочете видалити цей матч?", "Підтвердження", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    if (_currentTournament != null)
                        _currentTournament.Matches.Remove(matchToRemove);
                    else
                        DataStorage.AllMatches.Remove(matchToRemove);

                    DataStorage.SaveAll();
                    RefreshList();
                }
            }
        }

        private void BtnSaveResults_Click(object sender, RoutedEventArgs e)
        {
            // Якщо ми вже в існуючому турнірі, просто зберігаємо зміни
            if (_currentTournament != null)
            {
                _currentTournament.Info = $"Матчів: {_currentTournament.Matches.Count}";
                DataStorage.SaveAll();
                MessageBox.Show("Зміни в турнірі збережено!");
            }
            else // Якщо це створення нового турніру з "чистого листа"
            {
                var tournamentEntry = new Tournament
                {
                    Title = DataStorage.CurrentTournamentName,
                    Date = DateTime.Now.ToString("dd.MM.yyyy HH:mm"),
                    Icon = "", // Додаємо іконку кубка за замовчуванням
                    Info = $"Матчів: {DataStorage.AllMatches.Count}",
                    // Копіюємо згенеровані матчі в новий об'єкт
                    Matches = new System.Collections.ObjectModel.ObservableCollection<Match>(DataStorage.AllMatches)
                };

                DataStorage.SavedTournaments.Add(tournamentEntry);
                DataStorage.SaveAll();
                MessageBox.Show("Турнір збережено в архів!");
            }
        }

        private void TxtSearchMatch_TextChanged(object sender, TextChangedEventArgs e)
        {
            string filter = TxtSearchMatch.Text.ToLower();
            var source = (_currentTournament != null) ? _currentTournament.Matches : DataStorage.AllMatches;

            if (string.IsNullOrEmpty(filter))
            {
                MatchesList.ItemsSource = source;
            }
            else
            {
                MatchesList.ItemsSource = source
                    .Where(m => m.Team1.ToLower().Contains(filter) || m.Team2.ToLower().Contains(filter))
                    .ToList();
            }
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            if (this.NavigationService.CanGoBack)
                this.NavigationService.GoBack();
        }
    }
}