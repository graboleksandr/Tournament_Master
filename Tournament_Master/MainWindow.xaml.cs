using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Tournament_Master
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void AddTeam_Click(object sender, RoutedEventArgs e)
        {
            string teamName = TeamNameInput.Text.Trim();
            if (!string.IsNullOrEmpty(teamName))
            {
                TeamsListBox.Items.Add(teamName);
                TeamNameInput.Clear();
            }
        }
    }
}