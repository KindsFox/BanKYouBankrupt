using BankYouBankruptBusinessLogic.BindingModels;
using BankYouBankruptBusinessLogic.BusinessLogic;
using NLog;
using System;
using System.Windows;
using Unity;

namespace BankYouBankruptView
{
    /// <summary>
    /// Логика взаимодействия для CashWithdrawalWindow.xaml
    /// </summary>
    public partial class CashWithdrawalWindow : Window
    {
        [Dependency]
        public IUnityContainer Container { get; set; }
        public int Id { get; set; }
        private readonly CashWithdrawalLogic logic;
        private readonly Logger logger;
        public CashWithdrawalWindow(CashWithdrawalLogic logic)
        {
            InitializeComponent();
            this.logic = logic;
            logger = LogManager.GetCurrentClassLogger();
        }
        private void CashWithdrawalWindow_Load(object sender, RoutedEventArgs e)
        {
            if (Id != 0)
            {
                try
                {
                    var view = logic.Read(new CashWithdrawalBindingModels { AplicationId = Id })?[0];
                    if (view != null)
                    {
                        if (view.AvailabilityApplication)
                        {
                            textBoxAvailabilityApplication.Text = "Да";
                        }
                        else
                        {
                            textBoxAvailabilityApplication.Text = "Нет";
                        }
                    }
                }
                catch (Exception ex)
                {
                    logger.Error("Ошибка загрузки данных : " + ex.Message);
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

        }
        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {

            if (string.IsNullOrEmpty(textBoxAvailabilityApplication.Text))
            {
                MessageBox.Show("Заполните наличие заявки", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (textBoxAvailabilityApplication.Text != "Да" && textBoxAvailabilityApplication.Text != "Нет")
            {
                MessageBox.Show("Введите \"Да\" или \"Нет\"", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (Id != 0)
                try
                {
                    bool availbility = false;
                    if (textBoxAvailabilityApplication.Text.Equals("Да")) { availbility = true; }

                    logic.CreateOrUpdate(new CashWithdrawalBindingModels
                    {
                        AplicationId = Id,
                        AvailabilityApplication = availbility
                    });
                    MessageBox.Show("Сохранение прошло успешно", "Сообщение", MessageBoxButton.OK, MessageBoxImage.Information);
                    this.DialogResult = true;
                    Close();
                }
                catch (Exception ex)
                {
                    logger.Error("Ошибка сохранение выдачи наличных : " + ex.Message);
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            var view = Container.Resolve<BillsCashWithdrawalWindow>();
            view.ShowDialog();
        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}

