using GalaSoft.MvvmLight;
using RobotArmAPP.Models;
using RobotArmAPP.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace RobotArmAPP.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        public MainViewModel()
        {
            MenuItems = new ObservableCollection<MenuItem>(GetMenuItems());
            SelectedMenuItem = MenuItems.FirstOrDefault();
            
        }

        public ObservableCollection<MenuItem> MenuItems { get; set; }

        private MenuItem selectedMenuItem;

        public MenuItem SelectedMenuItem
        {
            get { return selectedMenuItem; }
            set { selectedMenuItem = value; RaisePropertyChanged(); }
        }

        private List<MenuItem> GetMenuItems()
        {
            List<MenuItem> menuItems = new List<MenuItem>
            {
                new MenuItem() { Title = "Home", SymbolIcon = Symbol.Home, NavigateTo = typeof(Home) },
                new MenuItem() { Title = "Connection", SymbolIcon = Symbol.Sync, NavigateTo = typeof(Conexao) },
                new MenuItem() { Title = "Control", SymbolIcon = Symbol.Play, NavigateTo = typeof(Controle), },
                new MenuItem() { Title = "Settings", SymbolIcon = Symbol.Setting, NavigateTo = typeof(Settings) }
            };
            
            return menuItems;
        }
    }
}
