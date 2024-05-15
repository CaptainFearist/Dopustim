using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using AMOGUSIK.Entities;
using AMOGUSIK.ViewModels;
using Castle.Core.Resource;
using Microsoft.EntityFrameworkCore;

namespace AMOGUSIK
{
    public partial class Specialists : Window
    {
        private List<ServiceOrders> _orders;

        public Specialists()
        {
            InitializeComponent();
            DataContext = new MainViewModel();
            _orders = GetOrders();
            RefreshOrders();
        }

        public List<ServiceOrders> GetOrders()
        {
            using (var context = new AudiCenterusContext())
            {
                return context.ServiceOrders.ToList();
            }
        }
        private void RefreshOrders()
        {
            ordersListBox.ItemsSource = null;
            ordersListBox.ItemsSource = ((MainViewModel)DataContext).Orders;
        }

        private void SortByDateAscending_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is MainViewModel viewModel)
            {
                viewModel.SortByDateDescendingCommand.Execute(null);
                _orders = viewModel.Orders.ToList();
                RefreshOrders();
            }
        }

        private void SortByDateDescending_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is MainViewModel viewModel)
            {
                viewModel.SortByDateAscendingCommand.Execute(null);
                _orders = viewModel.Orders.ToList();
                RefreshOrders();
            }
        }

        public void UpdateSortedOrders()
        {
            if (_orders != null)
            {
                if (((MainViewModel)DataContext).IsSortedAscending)
                {
                    ((MainViewModel)DataContext).SortByDateAscendingCommand.Execute(null);
                }
                else
                {
                    ((MainViewModel)DataContext).SortByDateDescendingCommand.Execute(null);
                }
                _orders = ((MainViewModel)DataContext).Orders.ToList();
                RefreshOrders();
            }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            ServiceOrderDialog dialog = new ServiceOrderDialog();

            if (dialog.ShowDialog() == true)
            {
                ServiceOrders newOrder = dialog.Order;

                using (var context = new AudiCenterusContext())
                {
                    var lastCustomer = context.Customers.OrderByDescending(c => c.CustomerID).FirstOrDefault();

                    if (lastCustomer != null)
                    {
                        Cars newCar = new Cars
                        {
                            CustomerID = lastCustomer.CustomerID,
                            Model = "Model",
                            Year = 2022,
                            VIN = "VIN1234567890"
                        };
                        context.Cars.Add(newCar);
                        newOrder.Car = newCar;
                    }
                    else
                    {
                        MessageBox.Show("Не удалось найти клиентов. Добавьте клиента перед созданием заказа.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    var serviceType = context.ServiceTypes.FirstOrDefault(st => st.TypeName == "");
                    if (serviceType == null)
                    {
                        serviceType = new ServiceTypes { TypeName = "Название типа" };
                        context.ServiceTypes.Add(serviceType);
                        context.SaveChanges();
                    }

                    newOrder.ServiceTypeID = serviceType.ServiceTypeID;
                    context.ServiceOrders.Add(newOrder);
                    context.SaveChanges();
                }
                // Добавляем заказ и в ViewModel
                MainViewModel viewModel = DataContext as MainViewModel;
                if (viewModel != null)
                {
                    viewModel.Orders.Add(newOrder);
                    viewModel.UpdateSortedOrders(); // Обновляем сортировку и фильтрацию
                }

                RefreshOrders();
            }
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            ServiceOrders selectedOrder = (ServiceOrders)ordersListBox.SelectedItem;

            if (selectedOrder != null)
            {
                ServiceOrderDialog dialog = new ServiceOrderDialog(selectedOrder);
                dialog.ShowDialog();

                RefreshOrders();
            }
            else
            {
                MessageBox.Show("Выберите заказ для редактирования.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            ServiceOrders selectedOrder = (ServiceOrders)ordersListBox.SelectedItem;

            if (selectedOrder != null)
            {
                MessageBoxResult result = MessageBox.Show("Вы уверены, что хотите удалить этот заказ?", "Подтверждение удаления", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    using (var context = new AudiCenterusContext())
                    {
                        context.ServiceOrders.Remove(selectedOrder);
                        context.SaveChanges();
                    }

                    MainViewModel viewModel = DataContext as MainViewModel;
                    if (viewModel != null)
                    {
                        viewModel.Orders.Remove(selectedOrder);
                        viewModel.UpdateSortedOrders(); // Обновляем сортировку, если она активна
                    }

                    RefreshOrders();
                }
            }
            else
            {
                MessageBox.Show("Выберите заказ для удаления.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string searchQuery = SearchTextBox.Text;
            List<ServiceOrders> filteredOrders = _orders.Where(order => order.Description.Contains(searchQuery)).ToList();
            ordersListBox.ItemsSource = filteredOrders;
        }


        private void Button_ClickVK(object sender, RoutedEventArgs e)
        {

        }

        private void Button_ClickTG(object sender, RoutedEventArgs e)
        {

        }

    }
}
