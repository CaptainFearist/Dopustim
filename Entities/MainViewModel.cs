using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using AMOGUSIK.Entities;

namespace AMOGUSIK.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private List<ServiceOrders> _orders;
        private string _searchQuery;

        public ICommand SearchCommand { get; }
        public ICommand SortByDateAscendingCommand { get; }
        public ICommand SortByDateDescendingCommand { get; }

        public List<ServiceOrders> Orders
        {
            get { return _orders; }
            set
            {
                _orders = value;
                OnPropertyChanged();
            }
        }

        public string SearchQuery
        {
            get { return _searchQuery; }
            set
            {
                _searchQuery = value;
                FilterOrders();
                OnPropertyChanged();
            }
        }

        public MainViewModel()
        {
            Orders = GetOrders();
            SearchCommand = new RelayCommand(Search);
            SortByDateAscendingCommand = new RelayCommand(SortByDateAscending);
            SortByDateDescendingCommand = new RelayCommand(SortByDateDescending);
        }

        public List<ServiceOrders> GetOrders()
        {
            using (var context = new AudiCenterusContext())
            {
                return context.ServiceOrders.ToList();
            }
        }

        private void FilterOrders()
        {
            if (string.IsNullOrWhiteSpace(SearchQuery))
            {
                Orders = GetOrders();
            }
            else
            {
                Orders = GetOrders().Where(order => order.Description.Contains(SearchQuery)).ToList();
            }
        }

        private void Search(object parameter)
        {
            FilterOrders();
        }

        private void SortByDateAscending(object parameter)
        {
            Orders = GetOrders().OrderBy(order => order.OrderDate).ToList();
        }

        private void SortByDateDescending(object parameter)
        {
            Orders = GetOrders().OrderByDescending(order => order.OrderDate).ToList();
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
