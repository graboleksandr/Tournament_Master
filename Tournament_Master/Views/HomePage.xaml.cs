using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Tournament_Master.Models;

namespace Tournament_Master.Views
{
    public partial class HomePage : Page
    {
        public HomePage()
        {
            InitializeComponent();
            RecentTournamentsList.ItemsSource = DataStorage.SavedTournaments;
        }

        // ПЕРЕХІД НА РОЗКЛАД
        private void RecentTournamentsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (RecentTournamentsList.SelectedItem is Tournament selectedTournament)
            {
                var schedulePage = new SchedulePage(selectedTournament);
                this.NavigationService.Navigate(schedulePage);
                RecentTournamentsList.SelectedItem = null;
            }
        }

        // ВИДАЛЕННЯ ТУРНІРУ
        private void BtnDeleteTournament_Click(object sender, RoutedEventArgs e)
        {
            // Зупиняємо подію, щоб не спрацював SelectionChanged (перехід на іншу сторінку)
            var button = sender as Button;
            var tournament = button?.DataContext as Tournament;

            if (tournament != null)
            {
                var result = MessageBox.Show($"Ви впевнені, що хочете видалити турнір \"{tournament.Title}\"?",
                                           "Підтвердження", MessageBoxButton.YesNo, MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                {
                    DataStorage.SavedTournaments.Remove(tournament);
                    DataStorage.SaveAll(); // Оновлюємо файл на диску
                }
            }
        }

        // РЕДАГУВАННЯ НАЗВИ
        private void EditTitle_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                SaveTitle(sender as TextBox);
                Keyboard.ClearFocus();
                RecentTournamentsList.SelectedItem = null;
            }
        }

        private void EditTitle_LostFocus(object sender, RoutedEventArgs e)
        {
            SaveTitle(sender as TextBox);
        }

        private void SaveTitle(TextBox textBox)
        {
            if (textBox != null && textBox.DataContext is Tournament tournament)
            {
                tournament.Title = string.IsNullOrWhiteSpace(textBox.Text) ? "Без назви" : textBox.Text;
                DataStorage.SaveAll();
            }
        }
    }
}