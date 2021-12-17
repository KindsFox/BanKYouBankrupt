using NLog;
using BankYouBankruptBusinessLogic.BindingModels;
using BankYouBankruptBusinessLogic.BusinessLogic;
using System;
using System.Windows;
using Unity;


namespace BankYouBankruptView
{   
    /// <summary>
    /// Логика взаимодействия для AuthorizationWindow.xaml
    /// </summary>
    public partial class AuthorizationWindow : Window
    {
        [Dependency]
        public IUnityContainer Container { get; set; }
        private readonly UserLogic logic;
        private readonly Logger logger;

        public AuthorizationWindow(UserLogic logic)
        {
            InitializeComponent();
            this.logic = logic;
            logger = LogManager.GetCurrentClassLogger();
        }

        private void ButtonLogin_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(textBoxEmail.Text))
            {
                MessageBox.Show("Введите почту", "Ошибка", MessageBoxButton.OK,
               MessageBoxImage.Error);
                return;
            }
            if (string.IsNullOrEmpty(passwordBox.Password))
            {
                MessageBox.Show("Введите пароль", "Ошибка", MessageBoxButton.OK,
               MessageBoxImage.Error);
                return;
            }
            try
            {
                var users = logic.Read(new UserBindingModels
                {
                    Email = textBoxEmail.Text,
                    Password = passwordBox.Password
                });
                if (users != null && users.Count > 0)
                {           
                        App.Executor = users[0];
                        var MainWindow = Container.Resolve<MainWindow>();                        
                        MainWindow.Show();
                        Close();                                       
                }
                else
                {
                    MessageBox.Show("Неверно введен пароль или логин", "Ошибка", MessageBoxButton.OK,
                   MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                logger.Error("Ошибка загрузки данных : " + ex.Message);
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ButtonRegistration_Click(object sender, RoutedEventArgs e)
        {
            var form = Container.Resolve<RegistrationWindow>();
            form.ShowDialog();
        }
    }
}
