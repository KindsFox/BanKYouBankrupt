using NLog;
using BankYouBankruptBusinessLogic.BindingModels;
using BankYouBankruptBusinessLogic.BusinessLogic;
using System;
using System.Windows;
using Unity;
using System.Collections.Generic;

namespace BankYouBankruptView
{
    /// <summary>
    /// Логика взаимодействия для CardWindow.xaml
    /// </summary>
    public partial class CardWindow : Window
    {
        [Dependency]
        public IUnityContainer Container { get; set; }
        public int Id { set { id = value; } }
        private readonly CardsLogic logic;
        private int? id;
        private readonly Logger logger;
        private readonly int _cardsNumber = 16;
        private readonly int _securityCode = 3;
        private Dictionary<int, decimal> cardsAplications;
        private Dictionary<int, string> cardsOperations;

        public CardWindow(CardsLogic logic)
        {
            InitializeComponent();
            this.logic = logic;
            logger = LogManager.GetCurrentClassLogger();
        }

        private void CardWindow_Load(object sender, RoutedEventArgs e)
        {
            if (id.HasValue)
            {
                try
                {
                    var view = logic.Read(new CardsBindingModels { Id = id })?[0];
                    if (view != null)
                    {
                        textBoxCardsNumder.Text = view.CardsNumber.ToString();
                        textBoxSecurityCode.Text = view.SecurityCode.ToString();
                        dateServiceEndDate.SelectedDate = view.ServiceEndDate;
                        cardsAplications = view.CardsAplications;
                        cardsOperations = view.CardsOperations;
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
                cardsAplications = new Dictionary<int, decimal>();
                cardsOperations = new Dictionary<int, string>();
            }            
        }
        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {            
            if (textBoxCardsNumder.Text == null)
            {
                MessageBox.Show("Заполните номер карты", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                logger.Warn("Не заполнен номер карты");
                return;
            }
            if (textBoxCardsNumder.Text.Length != _cardsNumber )
            {
                MessageBox.Show($"Номер карты должен быть длиной {_cardsNumber}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (textBoxSecurityCode.Text == null)
            {
                MessageBox.Show("Заполните код безопастности", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                logger.Warn("Не заполнен код безопастности");
                return;
            }
            if (textBoxSecurityCode.Text.Length != _securityCode)
            {
                MessageBox.Show($"Код безопасноти должен быть длиной {_securityCode}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (dateServiceEndDate.SelectedDate == null)
            {
                MessageBox.Show("Заполните дату окончания обслуживания", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                logger.Warn("Не заполнена дата окончания обслуживания");
                return;
            }
            try
            {
                logic.CreateOrUpdate(new CardsBindingModels
                {
                    Id = id,
                    CardsNumder = textBoxCardsNumder.Text,
                    SecurityCode = Convert.ToInt32(textBoxSecurityCode.Text),
                    ServiceEndDate = (DateTime)dateServiceEndDate.SelectedDate,
                    CardsAplications = cardsAplications,
                    CardsOperations = cardsOperations,
                    UserId = App.Executor.Id
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

   