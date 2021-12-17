using BankYouBankruptBusinessLogic.BindingModels;
using BankYouBankruptBusinessLogic.BusinessLogic;
using BankYouBankruptBusinessLogic.ViewModels;
using NLog;
using System;
using System.Collections.Generic;
using System.Windows;
using Unity;

namespace BankYouBankruptView
{
    /// <summary>
    /// Логика взаимодействия для BillsCashWithdrawalWindow.xaml
    /// </summary>
    public partial class BillsCashWithdrawalWindow : Window
    {
        [Dependency]
        public IUnityContainer Container { get; set; }
        public int Id { set { id = value; } }
        private readonly BillsLogic logicB;
        private readonly CashWithdrawalLogic logicCW;
        private readonly ApplicationLogic logicA;
        private readonly Logger logger;
        private BillsViewModel billWiew;
        private Dictionary<int, bool> currentCashWithdrawal;
        private int id;

        public BillsCashWithdrawalWindow(BillsLogic logicB, CashWithdrawalLogic logicCW, ApplicationLogic logicA)
        {
            InitializeComponent();
            this.logicB = logicB;
            this.logicCW = logicCW;
            this.logicA = logicA;
            logger = LogManager.GetCurrentClassLogger();
        }

        private void BillsCashWithdrawalWindow_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                var listBills = logicB.Read(null);
                comboBoxBills.ItemsSource = listBills;
                comboBoxBills.SelectedItem = null;
                var listCashW = logicCW.Read(null);
                listBoxAllWithdrawal.ItemsSource = listCashW;
            }
            catch (Exception ex)
            {
                logger.Error("Ошибка загрузки данных : " + ex.Message);
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ReloadList()
        {
            listBoxCurrentWithdrawal.Items.Clear();
            foreach (var cSP in currentCashWithdrawal)
            {
                var application = logicA.Read(new ApplicationsBindingModels { Id = cSP.Key })?[0];
                    if (application != null)
                {
                    listBoxCurrentWithdrawal.Items.Add(new CashWithdrawalViewModel { AplicationsId = cSP.Key, ApplicationNumber = application.AplicationNumber });
                }
            }
        }

        private void LoadData()
        {
            try
            {
                var bill = logicB.Read(new BillsBindingModels
                {
                    Id = (int)comboBoxBills.SelectedValue
                })?[0];
                if (bill != null)
                {
                    billWiew = bill;
                    currentCashWithdrawal = bill.BillCashWithdrawalId;
                }
                else
                {
                    currentCashWithdrawal = new Dictionary<int, bool>();
                }
                ReloadList();
            }
            catch (Exception ex)
            {
                logger.Error("Ошибка загрузки данных счета : " + ex.Message);
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            if (comboBoxBills.SelectedValue != null)
            {
                if (!currentCashWithdrawal.ContainsKey((int)listBoxAllWithdrawal.SelectedValue))
                {
                    currentCashWithdrawal.Add((int)listBoxAllWithdrawal.SelectedValue, (listBoxAllWithdrawal.SelectedItem as CashWithdrawalViewModel).AvailabilityApplication);
                    ReloadList();
                }
            }
            else
            {
                MessageBox.Show("Выберите счет", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void ButtonDel_Click(object sender, RoutedEventArgs e)
        {
            if (comboBoxBills.SelectedValue != null)
            {
                if (listBoxCurrentWithdrawal.SelectedItems.Count == 1)
                {
                    MessageBoxResult result = (MessageBoxResult)MessageBox.Show("Удалить запись", "Вопрос", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (result == MessageBoxResult.Yes)
                    {
                        try
                        {
                            currentCashWithdrawal.Remove((int)listBoxCurrentWithdrawal.SelectedValue);
                            ReloadList();
                        }
                        catch (Exception ex)
                        {
                            logger.Error("Ошибка удаления наличия заявки из списка : " + ex.Message);
                            MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Выберите счет", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            if (comboBoxBills.SelectedValue == null)
            {
                MessageBox.Show("Выберите счет", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            try
            {
                logicB.CreateOrUpdate(new BillsBindingModels
                {
                    Id = billWiew.Id,
                    BillsNumber = billWiew.BillsNumber,
                    BillsBalance = billWiew.BillsBalance,
                    BillCashWithdrawalId = currentCashWithdrawal,
                    BillsMoneyTransfer = billWiew.MoneyTransferId
                });
                MessageBox.Show("Сохранение прошло успешно", "Сообщение", MessageBoxButton.OK, MessageBoxImage.Information);
                LoadData();
            }
            catch (Exception ex)
            {
                logger.Error("Ошибка сохранения данных : " + ex.Message);
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void comboBoxBills_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            LoadData();
        }

        private void ButtonCansel_Click(object sender, RoutedEventArgs e)
        {
            var view = Container.Resolve<MainWindow>();
            Close();
            view.ShowDialog();
        }
    }
}