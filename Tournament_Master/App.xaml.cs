using System.Windows;
using Tournament_Master.Views;

namespace Tournament_Master
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Це не дасть програмі закритися миттєво після закриття AuthWindow
            this.ShutdownMode = ShutdownMode.OnExplicitShutdown;

            Tournament_Master.Views.AuthWindow auth = new Tournament_Master.Views.AuthWindow();

            if (auth.ShowDialog() == true)
            {
                MainWindow main = new MainWindow();

                // Повертаємо стандартний режим закриття для головного вікна
                this.ShutdownMode = ShutdownMode.OnLastWindowClose;

                main.Show();
            }
            else
            {
                Shutdown();
            }
        }

        public static void ChangeTheme(string themeName)
        {
            var dict = new ResourceDictionary();
            dict.Source = new Uri($"Themes/{themeName}.xaml", UriKind.Relative);

            // Видаляємо стару тему (якщо вона була першою в списку)
            Application.Current.Resources.MergedDictionaries.Clear();
            Application.Current.Resources.MergedDictionaries.Add(dict);
        }
    }
}
