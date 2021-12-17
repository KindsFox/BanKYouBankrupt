using System.Windows;
using Unity;

namespace BankYouBankruptView
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        [Dependency]
        public IUnityContainer Container { get; set; }
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ButtonCards_Click(object sender, RoutedEventArgs e)
        {           
            var window = Container.Resolve<CardsWindow>();
            window.ShowDialog();
        }

        private void ButtonApplications_Click(object sender, RoutedEventArgs e)
        {
            var window = Container.Resolve<ApplicationsWindow>();
            window.ShowDialog();
        }

        private void ButtonOperations_Click(object sender, RoutedEventArgs e)
        {
            var window = Container.Resolve<OperationsWindow>();
            window.ShowDialog();
        }

        private void ButtonReport_Click(object sender, RoutedEventArgs e)
        {
            var window = Container.Resolve<ReportCardsWindow>();
            window.ShowDialog();
        }
        private void ButtonListOperations_Click(object sender, RoutedEventArgs e)
        {
            var window = Container.Resolve<ListOperationWindow>();
            window.ShowDialog();
        }

        private void ButtonExit_Click(object sender, RoutedEventArgs e)
        {
            var window = Container.Resolve<AuthorizationWindow>();
            Close();
            window.ShowDialog();
        }

        private void ButtonMoneyTransfers_Click(object sender, RoutedEventArgs e)
        {
            var window = Container.Resolve<MoneyTransfersWindow>();
            window.ShowDialog();
        }

        private void ButtonDiagram_Click(object sender, RoutedEventArgs e)
        {
            var window = Container.Resolve<Diagram>();
            window.ShowDialog();
        }
    }
}
