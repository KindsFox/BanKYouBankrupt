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
    /// Логика взаимодействия для OperationWindow.xaml
    /// </summary>
    public partial class OperationsWindow : Window
    {
        [Dependency]
        public IUnityContainer Container { get; set; }
        private readonly OperationsLogic logic;
        private readonly Logger logger;
        public OperationsWindow(OperationsLogic logic)
        {
            InitializeComponent();
            this.logic = logic;
            logger = LogManager.GetCurrentClassLogger();
        }
        private void OperationsWindow_Load(object sender, RoutedEventArgs e)
        {
            LoadData();
        }
        private void LoadData()
        {
            try
            {
                var list = logic.Read(new OperationsBimdingModels { UserId =  App.Executor.Id});
                if (list != null)
                {
                    dataGridOperations.ItemsSource = list;
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
            var form = Container.Resolve<OperationWindow>();
            if (form.ShowDialog() == true)
            {
                LoadData();
            }
        }

        private void ButtonUpd_Click(object sender, RoutedEventArgs e)
        {
            if (dataGridOperations.SelectedItems.Count == 1)
            {
                var form = Container.Resolve<OperationWindow>();
                form.Id = (dataGridOperations.SelectedItems[0] as OperationsViewModel).Id;
                if (form.ShowDialog() == true)
                {
                    LoadData();
                }
            }
        }

        private void ButtonDel_Click(object sender, RoutedEventArgs e)
        {
            if (dataGridOperations.SelectedItems.Count == 1)
            {
                MessageBoxResult result = MessageBox.Show("Удалить запись", "Вопрос", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    int id = (dataGridOperations.SelectedItems[0] as OperationsViewModel).Id;
                    try
                    {
                        logic.Delete(new OperationsBimdingModels { Id = id });
                    }
                    catch (Exception ex)
                    {
                        logger.Error("Ошибка удаления запчасти : " + ex.Message);
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
        private void ButtonChoose_Click(object sender, RoutedEventArgs e)
        {
            if (dataGridOperations.SelectedItems.Count == 1)
            {
                var form = Container.Resolve<OperationCardsWindow>();
                form.Id = (dataGridOperations.SelectedItems[0] as OperationsViewModel).Id;
                form.ShowDialog();
            }
        }
    }
}
