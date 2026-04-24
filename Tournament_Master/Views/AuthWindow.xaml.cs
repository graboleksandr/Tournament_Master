using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Tournament_Master.Models;

namespace Tournament_Master.Views
{
    public partial class AuthWindow : Window
    {
        private const string UsersFile = "users.json";

        public AuthWindow()
        {
            InitializeComponent();
        }

        // --- МЕТОДИ ДЛЯ ПРИХОВУВАННЯ ПОМИЛОК ПРИ ВВОДІ ---
        private void Input_Changed(object sender, EventArgs e)
        {
            if (StatusLabel != null) StatusLabel.Text = "";
        }

        private void RegInput_Changed(object sender, EventArgs e)
        {
            if (StatusLabelReg != null) StatusLabelReg.Text = "";
        }

        // --- ЛОГІКА ВХОДУ ---
        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            var users = LoadUsers();
            string login = LoginField.Text;
            string passwordHash = DataStorage.HashPassword(PasswordField.Password);

            var user = users.FirstOrDefault(u => u.Login == login && u.PasswordHash == passwordHash);

            if (user != null)
            {
                StatusLabel.Foreground = Brushes.Green;
                StatusLabel.Text = "Вхід успішний!";

                DataStorage.CurrentUser = user.Login;
                DataStorage.LoadAll();

                // ВАЖЛИВО: DialogResult каже App.xaml.cs, що все ОК
                this.DialogResult = true;
                this.Close();
            }
            else
            {
                StatusLabel.Foreground = Brushes.Red;
                StatusLabel.Text = "Невірний логін або пароль";
            }
        }

        // --- ЛОГІКА РЕЄСТРАЦІЇ ---
        private void BtnDoRegister_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(RegLogin.Text) || RegPassword.Password.Length < 4)
            {
                StatusLabelReg.Foreground = Brushes.Orange;
                StatusLabelReg.Text = "Перевірте логін та пароль (мін. 4 симв.)";
                return;
            }

            var users = LoadUsers();
            if (users.Any(u => u.Login == RegLogin.Text))
            {
                StatusLabelReg.Foreground = Brushes.Red;
                StatusLabelReg.Text = "Цей логін вже зайнятий";
                return;
            }

            users.Add(new UserData
            {
                Login = RegLogin.Text,
                PasswordHash = DataStorage.HashPassword(RegPassword.Password),
                FirstName = RegFirstName.Text,
                LastName = RegLastName.Text,
                Gender = (RegGender.SelectedItem as ComboBoxItem)?.Content.ToString()
            });

            File.WriteAllText(UsersFile, JsonSerializer.Serialize(users, new JsonSerializerOptions { WriteIndented = true }));

            StatusLabelReg.Foreground = Brushes.Green;
            StatusLabelReg.Text = "Успіх! Тепер увійдіть";
        }

        private void ShowRegister_Click(object sender, RoutedEventArgs e)
        {
            LoginPanel.Visibility = Visibility.Collapsed;
            RegisterPanel.Visibility = Visibility.Visible;
        }

        private void ShowLogin_Click(object sender, RoutedEventArgs e)
        {
            RegisterPanel.Visibility = Visibility.Collapsed;
            LoginPanel.Visibility = Visibility.Visible;
        }

        private List<UserData> LoadUsers()
        {
            if (!File.Exists(UsersFile)) return new List<UserData>();
            try { return JsonSerializer.Deserialize<List<UserData>>(File.ReadAllText(UsersFile)) ?? new List<UserData>(); }
            catch { return new List<UserData>(); }
        }
    }
}