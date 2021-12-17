using BankYouBankruptBusinessLogic.BindingModels;
using BankYouBankruptBusinessLogic.BusinessLogic;
using BankYouBankruptBusinessLogic.HelperModels;
using Microsoft.Win32;
using NLog;
using System;
using System.Windows;
using Unity;

namespace BankYouBankruptView
{
    /// <summary>
    /// Логика взаимодействия для ReportCardsWindow.xaml
    /// </summary>
    public partial class ReportCardsWindow : Window
    {
        [Dependency]
        public new IUnityContainer Container { get; set; }
        private readonly ReportLogicExecutor logic;
        private readonly Logger logger;
        public ReportCardsWindow(ReportLogicExecutor logic)
        {
            InitializeComponent();
            logger = LogManager.GetCurrentClassLogger();
            this.logic = logic;
        }
        private void ButtonMake_Click(object sender, RoutedEventArgs e)
        {
            if (DatePikerTo.SelectedDate == null || DatePikerFrom.SelectedDate == null)
            {
                MessageBox.Show("Выберите даты", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (DatePikerFrom.SelectedDate >= DatePikerTo.SelectedDate)
            {
                MessageBox.Show("Дата начала должна быть меньше даты окончания", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            try
            {
                var dataSource = logic.GetNumberCardsActoins(new ReportBindingModel
                {
                    DateFrom = DatePikerFrom.SelectedDate,
                    DateTo = DatePikerTo.SelectedDate,
                    UserId = App.Executor.Id
                });
                dataGridSpareParts.ItemsSource = dataSource;
                textBoxDateFrom.Content = DatePikerFrom.SelectedDate.Value.ToLongDateString();
                textBoxDateTo.Content = DatePikerTo.SelectedDate.Value.ToLongDateString();
            }
            catch (Exception ex)
            {
                logger.Error("Ошибка формирования отчета : " + ex.Message);
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK,
               MessageBoxImage.Error);
            }
        }

        private void ButtonToPdf_Click(object sender, RoutedEventArgs e)
        {
            if (DatePikerTo.SelectedDate == null || DatePikerFrom.SelectedDate == null)
            {
                MessageBox.Show("Выберите даты", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (DatePikerFrom.SelectedDate >= DatePikerTo.SelectedDate)
            {
                MessageBox.Show("Дата начала должна быть меньше даты окончания", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            var dialog = new SaveFileDialog { Filter = "pdf|*.pdf" };
            {
                if (dialog.ShowDialog() == true)
                {
                    try
                    {
                        logic.SaveNumberCardsActionsToPdf(new ReportBindingModel
                        {
                            FileName = dialog.FileName,
                            DateFrom = DatePikerFrom.SelectedDate,
                            DateTo = DatePikerTo.SelectedDate,
                            UserId = App.Executor.Id
                        });
                        MessageBox.Show("Выполнено", "Успех", MessageBoxButton.OK,
                       MessageBoxImage.Information);
                    }
                    catch (Exception ex)
                    {
                        logger.Error("Ошибка сохранения отчета : " + ex.Message);
                        MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK,
                       MessageBoxImage.Error);
                    }
                }
            }

        }

        private void ButtonPDFToEmail_Click(object sender, RoutedEventArgs e)
        {
            if (DatePikerTo.SelectedDate == null || DatePikerFrom.SelectedDate == null)
            {
                MessageBox.Show("Выберите даты", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                logger.Warn("Не выбраны даты для отчета");
                return;
            }

            if (DatePikerFrom.SelectedDate >= DatePikerTo.SelectedDate)
            {
                MessageBox.Show("Дата начала должна быть меньше даты окончания", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                logger.Warn("Выбранная дата начала больше, чем дата окончания");
                return;
            }
            try
            {
                var fileName = "Отчет.pdf";
                logic.SaveNumberCardsActionsToPdf(new ReportBindingModel
                {
                    FileName = fileName,
                    DateFrom = DatePikerFrom.SelectedDate,
                    DateTo = DatePikerTo.SelectedDate,
                    UserId = App.Executor.Id
                });
                MailLogic.MailSend(new MailSendInfo
                {
                    MailAddress = App.Executor.Email,
                    Subject = "Отчет по действиям на картах",
                    Text = "Отчет по действиям от " + DatePikerFrom.SelectedDate.Value.ToShortDateString() + " по " + DatePikerTo.SelectedDate.Value.ToShortDateString(),
                    FileName = fileName
                });
                MessageBox.Show("Выполнено", "Успех", MessageBoxButton.OK,
                MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                logger.Error("Ошибка отправки отчета: " + ex.Message);
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK,
                MessageBoxImage.Error);
            }
        }
    }
}