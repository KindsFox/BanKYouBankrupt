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
    /// Логика взаимодействия для ApplicationsWindow.xaml
    /// </summary>
    public partial class ApplicationsWindow : Window
    {
        [Dependency]
        public IUnityContainer Container { get; set; }
        private readonly ApplicationLogic logic;
        private readonly Logger logger;
        public ApplicationsWindow(ApplicationLogic logic)
        {
            InitializeComponent();
            this.logic = logic;
            logger = LogManager.GetCurrentClassLogger();
        }
        private void ApplicationsWindow_Load(object sender, RoutedEventArgs e)
        {
            LoadData();
        }
        private void LoadData()
        {
            try
            {
                var list = logic.Read(new ApplicationsBindingModels { UserId = App.Executor.Id});
                if (list != null)
                {
                    dataGridApplications.ItemsSource = list;
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
            var form = Container.Resolve<ApplicationWindow>();
            if (form.ShowDialog() == true)
            {
                LoadData();
            }
        }

        private void ButtonUpd_Click(object sender, RoutedEventArgs e)
        {
            if (dataGridApplications.SelectedItems.Count == 1)
            {
                var form = Container.Resolve<ApplicationWindow>();
                form.Id = (dataGridApplications.SelectedItems[0] as ApplicationsViewModel).Id;
                if (form.ShowDialog() == true)
                {
                    LoadData();
                }
            }
        }

        private void ButtonDel_Click(object sender, RoutedEventArgs e)
        {
            if (dataGridApplications.SelectedItems.Count == 1)
            {
                MessageBoxResult result = MessageBox.Show("Удалить запись", "Вопрос", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    int id = (dataGridApplications.SelectedItems[0] as ApplicationsViewModel).Id;
                    try
                    {
                        logic.Delete(new ApplicationsBindingModels { Id = id });
                    }
                    catch (Exception ex)
                    {
                        logger.Error("Ошибка удаления записи : " + ex.Message);
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

        private void ButtonAvailability_Click(object sender, RoutedEventArgs e)
        {
            if (dataGridApplications.SelectedItems.Count == 1)
            {
                var form = Container.Resolve<CashWithdrawalWindow>();
                form.Id = (dataGridApplications.SelectedItems[0] as ApplicationsViewModel).Id;
                form.ShowDialog();
            }
        }

        private void ButtonChoose_Click(object sender, RoutedEventArgs e)
        {
            if (dataGridApplications.SelectedItems.Count == 1)
            {
                var form = Container.Resolve<ApplicationCardsWindow>();
                form.Id = (dataGridApplications.SelectedItems[0] as ApplicationsViewModel).Id;
                form.ShowDialog();
            }
        }
    }
}
