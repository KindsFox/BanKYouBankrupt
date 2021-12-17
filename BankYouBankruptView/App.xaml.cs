using BankYouBankruptBusinessLogic.BusinessLogic;
using BankYouBankruptBusinessLogic.HelperModels;
using BankYouBankruptBusinessLogic.Interfaces;
using BankYouBankruptBusinessLogic.ViewModels;
using BankYouBankruptDatabaseImplement.Implements;
using System;
using System.Configuration;
using System.Windows;
using Unity;
using Unity.Lifetime;

namespace BankYouBankruptView
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static UserViewModel Executor { get; set; }
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            var container = BuildUnityContainer();
            MailLogic.MailConfig(new MailConfig
            {
                SmtpClientHost = ConfigurationManager.AppSettings["SmtpClientHost"],
                SmtpClientPort = Convert.ToInt32(ConfigurationManager.AppSettings["SmtpClientPort"]),
                MailLogin = ConfigurationManager.AppSettings["MailLogin"],
                MailPassword = ConfigurationManager.AppSettings["MailPassword"],
                MailName = ConfigurationManager.AppSettings["MailName"]
            });
            var authWindow = container.Resolve<AuthorizationWindow>();
            authWindow.ShowDialog();
        }

        private static IUnityContainer BuildUnityContainer()
        {
            var currentContainer = new UnityContainer();
            currentContainer.RegisterType<IApplicationStorage, ApplicationsStorage>(new HierarchicalLifetimeManager());
            currentContainer.RegisterType<IBillsStorage, BillsStorage>(new HierarchicalLifetimeManager());
            currentContainer.RegisterType<ICardsStorage, CardsStorage>(new HierarchicalLifetimeManager());
            currentContainer.RegisterType<ICashWithdrawalStorage, CashWithdrawalStorage>(new HierarchicalLifetimeManager());
            currentContainer.RegisterType<IMoneyTransferStorage, MoneyTransferStorage>(new HierarchicalLifetimeManager());
            currentContainer.RegisterType<IOperationsStorage, OperationsStorage>(new HierarchicalLifetimeManager());
            currentContainer.RegisterType<IUserStorage, UserStorage>(new HierarchicalLifetimeManager());
            currentContainer.RegisterType<IReportStorage, ReportStorage>(new HierarchicalLifetimeManager());

            currentContainer.RegisterType<ApplicationLogic>(new HierarchicalLifetimeManager());
            currentContainer.RegisterType<BillsLogic>(new HierarchicalLifetimeManager());
            currentContainer.RegisterType<CardsLogic>(new HierarchicalLifetimeManager());
            currentContainer.RegisterType<CashWithdrawalLogic>(new HierarchicalLifetimeManager());
            currentContainer.RegisterType<MoneyTransferLogic>(new HierarchicalLifetimeManager());
            currentContainer.RegisterType<OperationsLogic>(new HierarchicalLifetimeManager());
            currentContainer.RegisterType<UserLogic>(new HierarchicalLifetimeManager());
            currentContainer.RegisterType<ReportLogicExecutor>(new HierarchicalLifetimeManager());
            return currentContainer;
        }
    }
}
