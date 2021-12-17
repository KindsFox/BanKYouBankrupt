using NLog;
using BankYouBankruptBusinessLogic.BindingModels;
using BankYouBankruptBusinessLogic.BusinessLogic;
using BankYouBankruptBusinessLogic.ViewModels;
using System;
using System.Windows;
using Unity;

namespace BankYouBankruptView
{
    /// <summary>
    /// Логика взаимодействия для MoneyTransfersWindow.xaml
    /// </summary>
    public partial class MoneyTransfersWindow : Window
    {
        [Dependency]
        public IUnityContainer Container { get; set; }
        private readonly MoneyTransferLogic logic;
        private readonly Logger logger;
        public MoneyTransfersWindow(MoneyTransferLogic logic)
        {
            InitializeComponent();
            this.logic = logic;
            logger = LogManager.GetCurrentClassLogger();
        }

        private void MoneyTransfersWindow_Load(object sender, RoutedEventArgs e)
        {
            LoadData();
        }

        private void LoadData()
        {
            try
            {
                var list = logic.Read(new MoneyTransferBindingModels { Sender = App.Executor.FIO });
                if (list != null)
                {
                    dataGridMoneyTransfers.ItemsSource = list;
                }
            }
            catch (Exception ex)
            {
                logger.Error("Ошибка загрузки данных : " + ex.Message);
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void ButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            var form = Container.Resolve<MoneyTransferWindow>();
            if (form.ShowDialog() == true)
            {
                LoadData();
            }
        }

        private void ButtonUpd_Click(object sender, RoutedEventArgs e)
        {
            if (dataGridMoneyTransfers.SelectedItems.Count == 1)
            {
                var form = Container.Resolve<MoneyTransferWindow>();
                form.Id = (dataGridMoneyTransfers.SelectedItems[0] as MoneyTransferViewModel).Id;
                if (form.ShowDialog() == true)
                {
                    LoadData();
                }
            }
        }

        private void ButtonDel_Click(object sender, RoutedEventArgs e)
        {

            if (dataGridMoneyTransfers.SelectedItems.Count == 1)
            {
                MessageBoxResult result = MessageBox.Show("Удалить запись", "Вопрос", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    int id = (dataGridMoneyTransfers.SelectedItems[0] as MoneyTransferViewModel).Id;
                    try
                    {
                        logic.Delete(new MoneyTransferBindingModels { Id = id });
                    }
                    catch (Exception ex)
                    {
                        logger.Error("Ошибка удаления перевода : " + ex.Message);
                        MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    LoadData();
                }
            }

        }
        private void ButtonRef_Click(object sender, RoutedEventArgs e)
        {
            LoadData();
        }

        private void ButtonCansel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
