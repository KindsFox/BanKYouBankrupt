using BankYouBankruptBusinessLogic.BindingModels;
using BankYouBankruptBusinessLogic.BusinessLogic;
using BankYouBankruptBusinessLogic.ViewModels;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Unity;

namespace BankYouBankruptView
{
    /// <summary>
    /// Логика взаимодействия для ApplicationCardsWindow.xaml
    /// </summary>
    public partial class ApplicationCardsWindow : Window
    {
        [Dependency]
        public IUnityContainer Container { get; set; }
        public int Id { set { id = value; } }
        private readonly CardsLogic logicC;
        private readonly ApplicationLogic logicA;
        private readonly Logger logger;
        private Dictionary<int, string> newListCards;
        private List<CardsViewModel> currentCards;
        private int id;

        public ApplicationCardsWindow(ApplicationLogic logicA, CardsLogic logicC)
        {
            InitializeComponent();
            this.logicC = logicC;
            this.logicA = logicA;
            logger = LogManager.GetCurrentClassLogger();
        }

        private void ApplicationCardsWindow_Load(object sender, RoutedEventArgs e)
        {
            LoadData();
        }

        private void LoadData()
        {
            var currentCardsList = logicC.Read(new CardsBindingModels { ApplicationId = id });
            if (currentCardsList != null)
            {
                newListCards = currentCardsList.ToDictionary(rec => rec.Id, rec => rec.CardsNumber);
            }
            currentCards = currentCardsList;
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
                    if (view != null && view[0].CardsAplications.ContainsKey(id))
                    {
                        view?[0].CardsAplications.Remove(id);
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
                    if (view != null && !view[0].CardsAplications.ContainsKey(id))
                    {
                        view?[0].CardsAplications.Add(id, (decimal)logicA.Read(new ApplicationsBindingModels { Id = id})?[0].AplicationSum);
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

