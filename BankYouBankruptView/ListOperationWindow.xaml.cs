using BankYouBankruptBusinessLogic.BindingModels;
using BankYouBankruptBusinessLogic.BusinessLogic;
using BankYouBankruptBusinessLogic.ViewModels;
using Microsoft.Win32;
using NLog;
using System;
using System.Collections.Generic;
using System.Windows;
using Unity;


namespace BankYouBankruptView
{

	/// <summary>
	/// Логика взаимодействия для ListOperationWindow.xaml
	/// </summary>
	public partial class ListOperationWindow : Window
	{
		[Dependency]
		public IUnityContainer Container { get; set; }
		private readonly CardsLogic logicC;
		private readonly ReportLogicExecutor logicR;
		private readonly Logger logger;

		public ListOperationWindow(CardsLogic logicC, ReportLogicExecutor logicR)
		{
			InitializeComponent();
			this.logicC = logicC;
			this.logicR = logicR;
			logger = LogManager.GetCurrentClassLogger();

		}
		private void OperationsWindow_Loaded(object sender, RoutedEventArgs e)
		{
			LoadData();
		}

		private void LoadData()
		{
			try
			{
				var list = logicC.Read(new CardsBindingModels { UserId = App.Executor.Id });
				if (list != null)
				{
					dataGridCards.ItemsSource = list;
				}
			}
			catch (Exception ex)
			{
				logger.Error("Ошибка загрузки данных : " + ex.Message);
				MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK,
			   MessageBoxImage.Error);
			}
		}

		private void ButtonSaveToExcel_Click(object sender, RoutedEventArgs e)
		{
			if (dataGridCards.SelectedItem == null || dataGridCards.SelectedItems.Count == 0)
			{
				MessageBox.Show("Выберите карты", "Ошибка", MessageBoxButton.OK,
				   MessageBoxImage.Error);
				return;
			}
			SaveFileDialog dialog = new SaveFileDialog { Filter = "xlsx|*.xlsx" };
			if (dialog.ShowDialog() == true)
			{
				try
				{
					var cards = new List<CardsViewModel>();
					foreach (var work in dataGridCards.SelectedItems)
					{
						cards.Add(work as CardsViewModel);
					}
					logicR.SaveOperationCardToExcelFile(new ReportBindingModel
					{
						FileName = dialog.FileName,
						Cards = cards
					});
					MessageBox.Show("Выполнено", "Успех", MessageBoxButton.OK,
					MessageBoxImage.Information);
				}
				catch (Exception ex)
				{
					logger.Error("Ошибка формирования Excel файла : " + ex.Message);
					MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK,
				   MessageBoxImage.Error);
				}
			}
		}

		private void ButtonSaveToWord_Click(object sender, RoutedEventArgs e)
		{
			if (dataGridCards.SelectedItem == null || dataGridCards.SelectedItems.Count == 0)
			{
				MessageBox.Show("Выберите карты", "Ошибка", MessageBoxButton.OK,
				   MessageBoxImage.Error);
				return;
			}
			var dialog = new SaveFileDialog { Filter = "docx|*.docx" };
			try
			{
				if (dialog.ShowDialog() == true)
				{
					var list = new List<CardsViewModel>();
					foreach (var cards in dataGridCards.SelectedItems)
					{
						list.Add((CardsViewModel)cards);
					}
					logicR.SaveOperationCardToWordFile(new ReportBindingModel
					{
						FileName = dialog.FileName,
						Cards = list
					});
					MessageBox.Show("Выполнено", "Успех", MessageBoxButton.OK,
					MessageBoxImage.Information);
				}
			}
			catch (Exception ex)
			{
				logger.Error("Ошибка формирования Word файла : " + ex.Message);
				MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK,
			   MessageBoxImage.Error);
			}
		}

		private void ButtonCansel_Click(object sender, RoutedEventArgs e)
		{
			Close();
		}
	}
}

