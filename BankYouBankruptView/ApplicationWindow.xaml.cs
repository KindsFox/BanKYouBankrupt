using System;
using System.Collections.Generic;
using NLog;
using BankYouBankruptBusinessLogic.BindingModels;
using BankYouBankruptBusinessLogic.BusinessLogic;
using System.Windows;
using Unity;

namespace BankYouBankruptView
{
    /// <summary>
    /// Логика взаимодействия для ApplicationWindow.xaml
    /// </summary>
    public partial class ApplicationWindow : Window
    {        
            [Dependency]
            public IUnityContainer Container { get; set; }
            public int Id { set { id = value; } }
            private readonly ApplicationLogic logic;
            private int? id;
            private readonly Logger logger;
            private Dictionary<int, (string, string)>  applicationsMoneyTransfer;

            public ApplicationWindow(ApplicationLogic logic)
            {
                InitializeComponent();
                this.logic = logic;
                logger = LogManager.GetCurrentClassLogger();
            }

            private void ApplicationWindow_Load(object sender, RoutedEventArgs e)
            {
                if (id.HasValue)
                 {
                     try
                     {
                         var view = logic.Read(new ApplicationsBindingModels { Id = id })?[0];
                         if (view != null)
                         {
                            textBoxAplicationSum.Text = view.AplicationSum.ToString();
                            dateApplicationPass.Text = view.AplicationDate.ToString();
                            textBoxAplicationNumber.Text = view.AplicationNumber.ToString();
                            applicationsMoneyTransfer = view.ApplicationMoneyTransfer;
                         }
                     }
                    catch (Exception ex)
                     {
                    logger.Error("Ошибка загрузки данных : " + ex.Message);
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                     }
                }
                else
                {
                applicationsMoneyTransfer = new Dictionary<int, (string, string)>();
                }
            }
            private void ButtonSave_Click(object sender, RoutedEventArgs e)
            {
                
                if (textBoxAplicationSum.Text == null)
                {
                    MessageBox.Show("Заполните сумму", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    logger.Warn("Не заполнено сумма");
                    return;
                }
                if (dateApplicationPass.SelectedDate == null)
                {
                    MessageBox.Show("Заполните дату", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    logger.Warn("Не заполнена дата заявки");
                    return;
                }
                if (textBoxAplicationNumber.Text == null)
                {
                    MessageBox.Show("Заполните номер", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    logger.Warn("Не заполнен номер заявки");
                    return;
                }
                try
                {
                    logic.CreateOrUpdate(new ApplicationsBindingModels
                    {
                        Id = id,
                        AplicationSum = Convert.ToDecimal(textBoxAplicationSum.Text),
                        AplicationDate = (DateTime)dateApplicationPass.SelectedDate,
                        AplicationNumber = Convert.ToInt32(textBoxAplicationNumber.Text),                        
                        ApplicationMoneyTransfer = applicationsMoneyTransfer,
                        UserId = App.Executor.Id
                    });
                    MessageBox.Show("Сохранение прошло успешно", "Сообщение", MessageBoxButton.OK, MessageBoxImage.Information);
                    this.DialogResult = true;
                    Close();
                }
                catch (Exception ex)
                {
                    logger.Error("Ошибка сохранение заявок : " + ex.Message);
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