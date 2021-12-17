using BankYouBankruptBusinessLogic.BusinessLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.DataVisualization.Charting;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Unity;

namespace BankYouBankruptView
{
    /// <summary>
    /// Логика взаимодействия для Diagram.xaml
    /// </summary>
    public partial class Diagram : Window
    {
        [Dependency]
        public IUnityContainer Container { get; set; }
        private readonly ApplicationLogic logic;
        public Diagram (ApplicationLogic logic)
        {
            InitializeComponent();
            this.logic = logic;
        }
        private void LoadData()
        {
            ((ColumnSeries)mcChart.Series[0]).ItemsSource = logic.Read(null);
            
        }
        private void ButtonMake_Click(object sender, RoutedEventArgs e)
        {
            LoadData();
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            LoadData();
        }
    }
}