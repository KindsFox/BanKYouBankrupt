using BankYouBankruptBusinessLogic.BusinessLogic;
using BankYouBankruptBusinessLogic.BindingModels;
using NLog;
using System;
using System.Text.RegularExpressions;
using System.Windows;
using Unity;

namespace BankYouBankruptView
{
    /// <summary>
    /// Логика взаимодействия для RegistrationWindow.xaml
    /// </summary>
    public partial class RegistrationWindow : Window
    {
        [Dependency]
        public IUnityContainer Container { get; set; }
        private readonly UserLogic logic;
        private readonly Logger logger;
        private readonly int _passwordMaxLength = 32;
        private readonly int _passwordMinLength = 5;

        public RegistrationWindow(UserLogic logic)
        {
            InitializeComponent();
            this.logic = logic;
            logger = LogManager.GetCurrentClassLogger();
        }
        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(textBoxFIO.Text))
            {
                MessageBox.Show("Заполните \"ФИО\"", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }            
            if (string.IsNullOrEmpty(textBoxEmail.Text))
            {
                MessageBox.Show("Заполните поле \"Почта\"", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (!Regex.IsMatch(textBoxEmail.Text, @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$"))
            {
                MessageBox.Show("Почта введена некорректно", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (string.IsNullOrEmpty(textBoxPassword.Text))
            {
                MessageBox.Show("Заполните поле \"пароль\"", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (textBoxPassword.Text.Length > _passwordMaxLength || textBoxPassword.Text.Length < _passwordMinLength)
            {
                MessageBox.Show($"Пароль должен быть длиной от {_passwordMinLength} до {_passwordMaxLength}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }            
            if (string.IsNullOrEmpty(textBoxNumber.Text))
            {
                MessageBox.Show("Заполните поле \"номер\"", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }            
            try
            {
                logic.CreateOrUpdate(new UserBindingModels
                {
                    FIO = textBoxFIO.Text,                   
                    Email = textBoxEmail.Text,
                    Password = textBoxPassword.Text,
                    Number = textBoxNumber.Text
                   });
                MessageBox.Show("Сохранение прошло успешно", "Сообщение", MessageBoxButton.OK, MessageBoxImage.Information);
                DialogResult = true;
                Close();
            }
            catch (Exception ex)
            {
                logger.Error("Ошибка сохранения данных : " + ex.Message);
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {                      
            DialogResult = false;
            Close();          
        }
    }
}
