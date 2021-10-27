using Prism.Ioc;
using Prism.Modularity;
using System.Windows;
using WmsPrism.ViewModels;
using WmsPrism.ViewModels.BillCheck;
using WmsPrism.ViewModels.StoreBillIofLading;
using WmsPrism.Views;
using WmsPrism.Views.BillArrive;
using WmsPrism.Views.BillCheck;
using WmsPrism.Views.BingCargo;
using WmsPrism.Views.BuildBarCode;
using WmsPrism.Views.Login;
using WmsPrism.Views.Print;
using WmsPrism.Views.StoreBillIofLading;

namespace WmsPrism
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        //protected override void InitializeShell(Window shell)
        //{
        //    Login loginView = new Login();
        //    if (loginView.ShowDialog() == true)
        //    {
        //        var shellVM = shell.DataContext as MainWindowViewModel;
        //        shellVM.InitData();
        //        base.InitializeShell(shell);
        //    }
        //    else
        //    {
        //        Application.Current.Shutdown(-1);
        //    }
        //}

        protected override Window CreateShell()
        {
            //Login
            return Container.Resolve<MainWindow>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            //注册导航
            containerRegistry.RegisterForNavigation<BillArriveHistory>();
            containerRegistry.RegisterForNavigation<StoreBillIofLadingList>();
            containerRegistry.RegisterForNavigation<BillSearch>();
            containerRegistry.RegisterForNavigation<WayBillSearch>();
            containerRegistry.RegisterForNavigation<BillBulkInStored>();
            containerRegistry.RegisterForNavigation<PrintConsignment>();
            containerRegistry.RegisterForNavigation<BuildLabelBarCode>();
            containerRegistry.RegisterForNavigation<BingWayBillNo>();
            containerRegistry.RegisterForNavigation<BingWeight>();
            containerRegistry.RegisterForNavigation<BarCodeSearch>();
            containerRegistry.RegisterForNavigation<BillOutPosition>();

            //消息框注册 
            containerRegistry.RegisterDialog<BillofLadingDetails, BillofLadingDetailsViewModel>();
            containerRegistry.RegisterDialog<BarCodeDialog, BarCodeDialogViewModel>();

        }

        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
            //moduleCatalog.AddModule<NavigationModule>();
            //moduleCatalog.AddModule<MvvmModule>();
            //moduleCatalog.AddModule<ModuleModule>();
            //moduleCatalog.AddModule<RegionModule>();
            //base.ConfigureModuleCatalog(moduleCatalog);
        }

        protected override void OnInitialized()
        {
            var login = Container.Resolve<Login>();
            var result = login.ShowDialog();
            if (result.Value == true)
            {
                
                base.OnInitialized();
            }
            else
            {
                Application.Current.Shutdown();
            }
        }
    }
}
