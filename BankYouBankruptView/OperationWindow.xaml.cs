using NLog;
using BankYouBankruptBusinessLogic.BindingModels;
using BankYouBankruptBusinessLogic.BusinessLogic;
using System;
using System.Windows;
using Unity;

namespace BankYouBankruptView
{
    /// <summary>
    /// Логика взаимодействия для OperationWindow.xaml
    /// </summary>
    public partial class OperationWindow : Window
    {
        [Dependency]
        public IUnityContainer Container { get; set; }
        public int Id { set { id = value; } }
        private readonly OperationsLogic logic;
        private int? id;
        private readonly Logger logger;       

        public OperationWindow(OperationsLogic logic)
        {
            InitializeComponent();
            this.logic = logic;
            logger = LogManager.GetCurrentClassLogger();
        }

        private void OperationWindow_Load(object sender, RoutedEventArgs e)
        {
            if (id.HasValue)
            {
                try
                {
                    var view = logic.Read(new OperationsBimdingModels { Id = id })?[0];
                    if (view != null)
                    {
                        dateOperation.SelectedDate = view.OperationDate;
                        textBoxOperationType.Text = view.OperationType.ToString();
                        textBoxOperationNumber.Text = view.OperationNumber.ToString();
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
            if (textBoxOperationType.Text == null)
            {
                MessageBox.Show("Заполните вид операции", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                logger.Warn("Вы не записали какую операцию планируете провести");
                return;
            }
            if (textBoxOperationNumber.Text == null)
            {
                MessageBox.Show("Заполните номер операции", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                logger.Warn("Не заполнен номер операции");
                return;
            }
            if (dateOperation.SelectedDate == null)
            {
                MessageBox.Show("Выберите время операции", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                logger.Warn("Не выбрано время операции");
                return;
            }
            try
            {
                logic.CreateOrUpdate(new OperationsBimdingModels
                {
                    Id = id,
                    OperationType = textBoxOperationType.Text,  
                    OperationNumber = Convert.ToInt32(textBoxOperationNumber.Text),
                    OperationDate = (DateTime)dateOperation.SelectedDate,
                    UserId = App.Executor.Id
                });
                MessageBox.Show("Сохранение прошло успешно", "Сообщение", MessageBoxButton.OK, MessageBoxImage.Information);
                this.DialogResult = true;
                Close();
            }
            catch (Exception ex)
            {
                logger.Error("Ошибка сохранение операций : " + ex.Message);
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