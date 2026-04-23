using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;
using Tournament_Master.Models; // Обов'язково для Participant

namespace Tournament_Master.Views
{
    public partial class ParticipantsPage : Page
    {
        public ParticipantsPage()
        {
            InitializeComponent();
            UpdateStats();
        }

        private void BtnAddManual_Click(object sender, RoutedEventArgs e)
        {
            AddParticipantWindow addWin = new AddParticipantWindow();
            if (addWin.ShowDialog() == true)
            {
                UpdateStats();
            }
        }

        private void BtnImport_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Текстові файли (*.txt)|*.txt";

            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    var lines = File.ReadAllLines(openFileDialog.FileName);
                    foreach (var line in lines)
                    {
                        var parts = line.Split(' ');
                        if (parts.Length >= 2)
                        {
                            DataStorage.AllParticipants.Add(new Participant
                            {
                                FirstName = parts[0],
                                LastName = parts[1]
                            });
                        }
                    }
                    DataStorage.SaveAll();
                    UpdateStats();
                }
                catch (Exception ex) { MessageBox.Show(ex.Message); }
            }
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.DataContext is Participant p)
            {
                DataStorage.AllParticipants.Remove(p);
                DataStorage.SaveAll();
                UpdateStats();
            }
        }

        private void TxtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Перевірка на null для безпеки при ініціалізації
            if (ParticipantsCards == null) return;

            if (string.IsNullOrWhiteSpace(TxtSearch.Text))
            {
                ParticipantsCards.ItemsSource = DataStorage.AllParticipants;
            }
            else
            {
                var filter = TxtSearch.Text.ToLower();
                ParticipantsCards.ItemsSource = DataStorage.AllParticipants
                    .Where(p => p.FirstName.ToLower().Contains(filter) ||
                                p.LastName.ToLower().Contains(filter))
                    .ToList();
            }
        }

        private void UpdateStats()
        {
            if (LblStats != null)
            {
                int count = DataStorage.AllParticipants.Count;
                LblStats.Text = $"Гравців: {count} | Можливо створити команд: {count / 2}";
            }
        }
    }
}