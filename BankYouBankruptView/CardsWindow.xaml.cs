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
    /// Логика взаимодействия для CardsWindow.xaml
    /// </summary>
    public partial class CardsWindow : Window
    {
        [Dependency]
        public IUnityContainer Container { get; set; }
        private readonly CardsLogic logic;
        private readonly Logger logger;
        public CardsWindow(CardsLogic logic)
        {
            InitializeComponent();
            this.logic = logic;
            logger = LogManager.GetCurrentClassLogger();
        }
        private void CardsWindow_Load(object sender, RoutedEventArgs e)
        {
            LoadData();
        }
        private void LoadData()
        {
            try
            {
                var list = logic.Read(new CardsBindingModels { UserId = App.Executor.Id });
                if (list != null)
                {
                    dataGridCards.ItemsSource = list;
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
            var form = Container.Resolve<CardWindow>();
            if (form.ShowDialog() == true)
            {
                LoadData();
            }
        }

        private void ButtonUpd_Click(object sender, RoutedEventArgs e)
        {
            if (dataGridCards.SelectedItems.Count == 1)
            {
                var form = Container.Resolve<CardWindow>();
                form.Id = (dataGridCards.SelectedItems[0] as CardsViewModel).Id;
                if (form.ShowDialog() == true)
                {
                    LoadData();
                }
            }
        }

        private void ButtonDel_Click(object sender, RoutedEventArgs e)
        {

            if (dataGridCards.SelectedItems.Count == 1)
            {
                MessageBoxResult result = MessageBox.Show("Удалить запись", "Вопрос", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    int id = (dataGridCards.SelectedItems[0] as CardsViewModel).Id;
                    try
                    {
                        logic.Delete(new CardsBindingModels { Id = id });
                    }
                    catch (Exception ex)
                    {
                        logger.Error("Ошибка удаления карты : " + ex.Message);
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

