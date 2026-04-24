using System.Windows;
using Tournament_Master.Views;

namespace Tournament_Master
{
    public partial class MainWindow : Window
    {
        // Створюємо екземпляри сторінок один раз
        private HomePage _homePage = new HomePage();
        private TeamsPage _teamsPage = new TeamsPage();
        private ParticipantsPage _participantsPage = new ParticipantsPage();
        private SchedulePage _schedulePage = new SchedulePage();
        private SettingsPage _settingsPage = new SettingsPage();

        public MainWindow()
        {
            InitializeComponent();
            MainFrame.Navigate(_homePage);
        }

        private void BtnHome_Click(object sender, RoutedEventArgs e) => MainFrame.Navigate(_homePage);
        private void BtnTeams_Click(object sender, RoutedEventArgs e) => MainFrame.Navigate(_teamsPage);
        private void BtnParticipants_Click(object sender, RoutedEventArgs e) => MainFrame.Navigate(_participantsPage);
        private void BtnSchedule_Click(object sender, RoutedEventArgs e) => MainFrame.Navigate(_schedulePage);
        private void BtnNavigateLeaderboard_Click(object sender, RoutedEventArgs e)
        {
            // MainFrame — це ім'я вашого елемента <Frame />
            MainFrame.Navigate(new Tournament_Master.Views.LeaderboardPage());
        }
        private void BtnSettings_Click(object sender, RoutedEventArgs e) => MainFrame.Navigate(_settingsPage);
    }
}