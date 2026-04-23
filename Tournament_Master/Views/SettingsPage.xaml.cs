using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Tournament_Master.Views
{
    public partial class SettingsPage : Page
    {
        private List<string> _themes = new List<string> { "Світла / Light", "Темна / Dark" };
        private List<string> _languages = new List<string> { "Українська", "English" };
        private bool _isInitialized = false;

        public SettingsPage()
        {
            InitializeComponent();
            ThemeSelector.ItemsSource = _themes;
            LanguageSelector.ItemsSource = _languages;
            SetCurrentState();
            _isInitialized = true;
        }

        private void SetCurrentState()
        {
            // Звертаємося до глобальних ресурсів додатка
            var mergedDicts = Application.Current.Resources.MergedDictionaries;

            var currentTheme = mergedDicts.FirstOrDefault(d => d.Source != null && d.Source.OriginalString.Contains("Themes/"));
            if (currentTheme != null)
                ThemeSelector.SelectedIndex = currentTheme.Source.OriginalString.Contains("DarkTheme") ? 1 : 0;

            var currentLang = mergedDicts.FirstOrDefault(d => d.Source != null && d.Source.OriginalString.Contains("Languages/"));
            if (currentLang != null)
                LanguageSelector.SelectedIndex = currentLang.Source.OriginalString.Contains("en-US") ? 1 : 0;
        }

        private void ThemeSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!_isInitialized) return;
            string themePath = ThemeSelector.SelectedIndex == 1
                ? "Resources/Themes/DarkTheme.xaml"
                : "Resources/Themes/LightTheme.xaml";

            ApplyResource("Themes/", themePath);
        }

        private void LanguageSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!_isInitialized) return;
            string langPath = LanguageSelector.SelectedIndex == 1
                ? "Resources/Languages/en-US.xaml"
                : "Resources/Languages/uk-UA.xaml";

            ApplyResource("Languages/", langPath);
        }

        private void ApplyResource(string folderFilter, string fullPath)
        {
            try
            {
                var uri = new Uri(fullPath, UriKind.Relative);
                ResourceDictionary newDict = Application.LoadComponent(uri) as ResourceDictionary;

                // КРИТИЧНО: Використовуємо Application.Current.Resources
                var mergedDicts = Application.Current.Resources.MergedDictionaries;

                var oldDict = mergedDicts.FirstOrDefault(d => d.Source != null && d.Source.OriginalString.Contains(folderFilter));

                if (oldDict != null)
                {
                    int index = mergedDicts.IndexOf(oldDict);
                    mergedDicts[index] = newDict; // Заміна словника у всій програмі
                }
                else
                {
                    mergedDicts.Add(newDict);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Помилка: {ex.Message}");
            }
        }
    }
}