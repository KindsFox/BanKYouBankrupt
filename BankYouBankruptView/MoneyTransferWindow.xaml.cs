using BankYouBankruptBusinessLogic.BindingModels;
using BankYouBankruptBusinessLogic.BusinessLogic;
using BankYouBankruptBusinessLogic.ViewModels;
using NLog;
using System;
using System.Windows;
using Unity;

namespace BankYouBankruptView
{
    /// <summary>
    /// Логика взаимодействия для MoneyTransferWindow.xaml
    /// </summary>
    public partial class MoneyTransferWindow : Window
    {
        [Dependency]
        public IUnityContainer Container { get; set; }
        public int Id { set { id = value; } }
        private readonly MoneyTransferLogic logic;
        private readonly CardsLogic cardLogic;
        private readonly OperationsLogic operationLogic;
        private readonly int _cardsNumber = 16;
        private int? id;
        private readonly Logger logger;

        public MoneyTransferWindow(MoneyTransferLogic logic, CardsLogic cardsLogic, OperationsLogic operationLogic)
        {
            InitializeComponent();
            this.logic = logic;
            this.cardLogic = cardsLogic;
            this.operationLogic = operationLogic;
            logger = LogManager.GetCurrentClassLogger();
        }

        private void MoneyTransferWindow_Load(object sender, RoutedEventArgs e)
        {
            var cardsList = cardLogic.Read(new CardsBindingModels { UserId = App.Executor.Id });
            comboBoxCards.ItemsSource = cardsList;
            var operationsList = operationLogic.Read(new OperationsBimdingModels { UserId = App.Executor.Id });
            comboBoxOperations.ItemsSource = operationsList;
            if (id.HasValue)
            {
                try
                {
                    var view = logic.Read(new MoneyTransferBindingModels { Id = id })?[0];
                    if (view != null)
                    {
                        comboBoxOperations.SelectedValue = operationLogic.Read(new OperationsBimdingModels { Id = view.OperationId })?[0].Id;
                        comboBoxCards.SelectedValue = cardLogic.Read(new CardsBindingModels { CardsNumder = view.SendersCard })?[0].Id;
                        textBoxFIO.Text = view.Recipient;
                        textBoxCardNumber.Text = view.RecipientsCard;
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
            if (comboBoxCards.SelectedItem == null)
            {
                MessageBox.Show("Выберите карту", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                logger.Warn("Не выбрана карта отправителя");
                return;
            }
            if (comboBoxOperations.SelectedItem == null)
            {
                MessageBox.Show("Выберите операцию", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                logger.Warn("Не выбрана операция");
                return;
            }
            if (textBoxFIO.Text == null)
            {
                MessageBox.Show("Заполните ФИО получателя", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                logger.Warn("Не заполнено ФИО получателя");
                return;
            }
            if (textBoxCardNumber.Text == null)
            {
                MessageBox.Show("Заполните номер карты", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                logger.Warn("Не заполнен номер карты");
                return;
            }
            if (textBoxCardNumber.Text.Length != _cardsNumber)
            {
                MessageBox.Show($"Номер карты должен быть длиной {_cardsNumber}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            try
            {
                logic.CreateOrUpdate(new MoneyTransferBindingModels
                {
                    Id = id,
                    Sender = App.Executor.FIO,
                    Recipient = textBoxFIO.Text,
                    SendersCard = (comboBoxCards.SelectedItem as CardsViewModel).CardsNumber,
                    RecipientsCard = textBoxCardNumber.Text,
                    OperationId = (int)comboBoxOperations.SelectedValue,
                });
                MessageBox.Show("Сохранение прошло успешно", "Сообщение", MessageBoxButton.OK, MessageBoxImage.Information);
                this.DialogResult = true;
                Close();
            }
            catch (Exception ex)
            {
                logger.Error("Ошибка сохранение карт : " + ex.Message);
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }
        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            Close();
        }
    }
}
