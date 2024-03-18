using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using AMOGUSIK.Entities;

namespace AMOGUSIK.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private List<ServiceOrders> _orders;
        private string _searchQuery;

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

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
