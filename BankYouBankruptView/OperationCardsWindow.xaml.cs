using NLog;
using BankYouBankruptBusinessLogic.BindingModels;
using BankYouBankruptBusinessLogic.BusinessLogic;
using BankYouBankruptBusinessLogic.ViewModels;
using System;
using System.Collections.Generic;
using System.Windows;
using Unity;
using System.Linq;

namespace BankYouBankruptView
{
    /// <summary>
    /// Логика взаимодействия для OperationCardsWindow.xaml
    /// </summary>
    public partial class OperationCardsWindow : Window
    {
        [Dependency]
        public IUnityContainer Container { get; set; }
        public int Id { set { id = value; } }
        private readonly CardsLogic logicC;
        private readonly OperationsLogic logicO;
        private readonly Logger logger;
        private Dictionary<int, string> newListCards;
        private List<CardsViewModel> currentCards;
        private int id;

        public OperationCardsWindow(OperationsLogic logicO, CardsLogic logicC)
        {
            InitializeComponent();
            this.logicC = logicC;
            this.logicO = logicO;
            logger = LogManager.GetCurrentClassLogger();
        }

        private void OperationsCardsWindow_Load(object sender, RoutedEventArgs e)
        {
            LoadData();
        }

        private void LoadData()
        {
            var currentOperationCards = logicC.Read(new CardsBindingModels { OperationId = id });
            if (currentOperationCards != null)
            {
                newListCards = currentOperationCards.ToDictionary(rec => rec.Id, rec => rec.CardsNumber);
            }
            currentCards = currentOperationCards;
            var FullCardsList = logicC.Read(null);
            if (FullCardsList != null)
            {
                listBoxAllCards.ItemsSource = FullCardsList;
            }
            ReloadList();
        }

        private void ReloadList()
        {
            listBoxCurrentCards.Items.Clear();
            foreach (var cSP in newListCards)
            {
                listBoxCurrentCards.Items.Add(new CardsViewModel { Id = cSP.Key, CardsNumber = cSP.Value });
            }
        }


        private void ButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            if (!newListCards.ContainsKey((int)listBoxAllCards.SelectedValue))
            {
                newListCards.Add((int)listBoxAllCards.SelectedValue, (listBoxAllCards.SelectedItem as CardsViewModel).CardsNumber);
                ReloadList();
            }
        }

        private void ButtonDel_Click(object sender, RoutedEventArgs e)
        {
            if (listBoxCurrentCards.SelectedItems.Count == 1)
            {
                MessageBoxResult result = MessageBox.Show("Удалить запись", "Вопрос", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        newListCards.Remove((int)listBoxCurrentCards.SelectedValue);
                        ReloadList();
                    }
                    catch (Exception ex)
                    {
                        logger.Error("Ошибка удаления запчасти из списка : " + ex.Message);
                        MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }

        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                currentCards.RemoveAll(rec => newListCards.ContainsKey(rec.Id));
                foreach (var card in currentCards)
                {
                    var view = logicC.Read(new CardsBindingModels { Id = card.Id });
                    if (view != null && view[0].CardsOperations.ContainsKey(id))
                    {
                        view?[0].CardsOperations.Remove(id);
                        logicC.CreateOrUpdate(new CardsBindingModels
                        {
                            Id = view[0].Id,
                            CardsNumder = view[0].CardsNumber,
                            SecurityCode = view[0].SecurityCode,
                            ServiceEndDate = view[0].ServiceEndDate,
                            UserId = view[0].UserId,
                            CardsAplications = view[0].CardsAplications,
                            CardsOperations = view[0].CardsOperations
                        });
                    }
                }

                foreach (var card in newListCards)
                {
                    var view = logicC.Read(new CardsBindingModels { Id = card.Key });
                    if (view != null && !view[0].CardsOperations.ContainsKey(id))
                    {
                        view?[0].CardsOperations.Add(id, (string)logicO.Read(new OperationsBimdingModels { Id = id })?[0].OperationType);
                        logicC.CreateOrUpdate(new CardsBindingModels
                        {
                            Id = view[0].Id,
                            CardsNumder = view[0].CardsNumber,
                            SecurityCode = view[0].SecurityCode,
                            ServiceEndDate = view[0].ServiceEndDate,
                            UserId = view[0].UserId,
                            CardsAplications = view[0].CardsAplications,
                            CardsOperations = view[0].CardsOperations
                        });
                    }
                }
                MessageBox.Show("Сохранение прошло успешно", "Сообщение", MessageBoxButton.OK, MessageBoxImage.Information);
                LoadData();
            }
            catch (Exception ex)
            {
                logger.Error("Ошибка сохранения данных : " + ex.Message);
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ButtonCansel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}