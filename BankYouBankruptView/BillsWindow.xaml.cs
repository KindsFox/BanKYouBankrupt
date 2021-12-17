using BankYouBankruptBusinessLogic.BindingModels;
using BankYouBankruptBusinessLogic.BusinessLogic;
using NLog;
using System;
using System.Collections.Generic;
using System.Windows;
using Unity;

namespace BankYouBankruptView
{
    /// <summary>
    /// Логика взаимодействия для BillsWindow.xaml
    /// </summary>
    public partial class BillsWindow : Window
    {
        [Dependency]
        public IUnityContainer Container { get; set; }
        public int Id { set { id = value; } }
        private readonly BillsLogic logic;
        private int? id;
        private readonly Logger logger;
        private Dictionary<int, string> currentMoneyTransfers;
        private Dictionary<int, bool> currentBillCashWithdrawal;
        public BillsWindow(BillsLogic logic)
        {
            InitializeComponent();
            this.logic = logic;
            logger = LogManager.GetCurrentClassLogger();
        }
        private void BillsWindow_Load(object sender, RoutedEventArgs e)
        {
            if (id.HasValue)
            {
                try
                {
                    var view = logic.Read(new BillsBindingModels { Id = id })?[0];
                    if (view != null)
                    {
                        textBoxBillsNumber.Text = view.BillsNumber.ToString();
                        textBoxBillsBalance.Text = view.BillsBalance.ToString();
                        currentBillCashWithdrawal = view.BillCashWithdrawalId;
                        currentMoneyTransfers = view.MoneyTransferId;
                    }

                }
                catch (Exception ex)
                {
                    logger.Error("Ошибка загрузки данных : " + ex.Message);
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            currentBillCashWithdrawal = new Dictionary<int, bool>();
            currentMoneyTransfers = new Dictionary<int, string>();
        }
        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {

            if (textBoxBillsNumber.Text == null)
            {
                MessageBox.Show("Заполните счет", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                logger.Warn("Не заполнен номер счета");
                return;
            }
            if (textBoxBillsBalance.Text == null)
            {
                MessageBox.Show("Заполните остаток на счете", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                logger.Warn("Не заполнен остаток счета");
                return;
            }
            try
            {
                logic.CreateOrUpdate(new BillsBindingModels
                {
                    Id = id,
                    BillsNumber = textBoxBillsNumber.Text,
                    BillsBalance = Convert.ToInt32(textBoxBillsBalance.Text),
                    BillCashWithdrawalId = currentBillCashWithdrawal,
                    BillsMoneyTransfer = currentMoneyTransfers
                });
                MessageBox.Show("Сохранение прошло успешно", "Сообщение", MessageBoxButton.OK, MessageBoxImage.Information);
                this.DialogResult = true;               
            }
            catch (Exception ex)
            {
                logger.Error("Ошибка сохранение счета : " + ex.Message);
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }          
        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {            
            var view = Container.Resolve<BillsCashWithdrawalWindow>();
            Close();
            view.ShowDialog();
        }
    }
}