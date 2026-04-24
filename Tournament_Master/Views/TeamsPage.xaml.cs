using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Tournament_Master.Models;

namespace Tournament_Master.Views
{
    public partial class TeamsPage : Page
    {
        public TeamsPage()
        {
            InitializeComponent();
            Refresh();
        }

        private void Refresh()
        {
            ListPlayersSelector.ItemsSource = null;
            ListPlayersSelector.ItemsSource = DataStorage.AllParticipants;
            TeamsDisplay.ItemsSource = null;
            TeamsDisplay.ItemsSource = DataStorage.AllTeams;
        }

        private void BtnCreateManual_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TxtTeamName.Text)) return;
            var sel = DataStorage.AllParticipants.Where(p => p.IsSelected).ToList();
            if (sel.Count == 0) return;

            DataStorage.AllTeams.Add(new Team { TeamName = TxtTeamName.Text, Members = new List<Participant>(sel) });
            foreach (var p in DataStorage.AllParticipants) p.IsSelected = false;

            DataStorage.SaveAll();
            TxtTeamName.Clear();
            Refresh();
        }

        private void BtnAutoGenerate_Click(object sender, RoutedEventArgs e)
        {
            if (!int.TryParse(TxtPlayersPerTeam.Text, out int n) || n < 1) return;
            var rnd = DataStorage.AllParticipants.OrderBy(p => Guid.NewGuid()).ToList();

            DataStorage.AllTeams.Clear();
            for (int i = 0, count = 1; i < rnd.Count; i += n)
            {
                var g = rnd.Skip(i).Take(n).ToList();
                if (g.Any()) DataStorage.AllTeams.Add(new Team { TeamName = $"Команда {count++}", Members = g });
            }
            DataStorage.SaveAll();
            Refresh();
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            if ((sender as Button)?.DataContext is Team t)
            {
                DataStorage.AllTeams.Remove(t);
                DataStorage.SaveAll();
                Refresh();
            }
        }
    }
}