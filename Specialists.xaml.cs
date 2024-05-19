using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using AMOGUSIK.Entities;
using AMOGUSIK.ViewModels;
using Castle.Core.Resource;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using Xceed.Document.NET;
using Xceed.Words.NET;

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
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
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

        private void CreateWordDocument_Click(object sender, RoutedEventArgs e)
        {
            ServiceOrders selectedOrder = (ServiceOrders)ordersListBox.SelectedItem;

            if (selectedOrder != null)
            {
                string fileName = "ServiceOrder";
                string fileExtension = ".docx";
                string filePath = GetUniqueFilePath(fileName, fileExtension);

                // Path to the template file
                string templatePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "Template.docx");

                if (!File.Exists(templatePath))
                {
                    MessageBox.Show("Шаблонный файл не найден.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // Load the template document
                using (DocX document = DocX.Load(templatePath))
                {
                    // Insert data into the template
                    document.ReplaceText("{OrderID}", selectedOrder.OrderID.ToString());
                    document.ReplaceText("{CarID}", selectedOrder.CarID.ToString());
                    document.ReplaceText("{ServiceTypeID}", selectedOrder.ServiceTypeID.ToString());
                    document.ReplaceText("{OrderDate}", selectedOrder.OrderDate.ToString("d"));
                    document.ReplaceText("{Description}", selectedOrder.Description);
                    document.ReplaceText("{Cost}", selectedOrder.Cost.ToString("N0"));

                    // Save the document as a new file
                    document.SaveAs(filePath);
                }

                MessageBox.Show($"Документ Word создан: {filePath}", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Выберите заказ для создания документа.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CreateExcelDocument_Click(object sender, RoutedEventArgs e)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            ServiceOrders selectedOrder = (ServiceOrders)ordersListBox.SelectedItem;

            if (selectedOrder != null)
            {
                string fileName = "ServiceOrder";
                string fileExtension = ".xlsx";
                string filePath = GetUniqueFilePath(fileName, fileExtension);

                using (ExcelPackage package = new ExcelPackage())
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Order Details");

                    worksheet.Cells[1, 1].Value = "Номер заказа";
                    worksheet.Cells[1, 2].Value = selectedOrder.OrderID;
                    worksheet.Cells[2, 1].Value = "ID автомобиля";
                    worksheet.Cells[2, 2].Value = selectedOrder.CarID;
                    worksheet.Cells[3, 1].Value = "ID типа услуги";
                    worksheet.Cells[3, 2].Value = selectedOrder.ServiceTypeID;
                    worksheet.Cells[4, 1].Value = "Дата заказа";
                    worksheet.Cells[4, 2].Value = selectedOrder.OrderDate.ToString("d");
                    worksheet.Cells[5, 1].Value = "Описание";
                    worksheet.Cells[5, 2].Value = selectedOrder.Description;
                    worksheet.Cells[6, 1].Value = "Стоимость";
                    worksheet.Cells[6, 2].Value = selectedOrder.Cost;

                    package.SaveAs(new FileInfo(filePath));
                }

                MessageBox.Show($"Документ Excel создан: {filePath}", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Выберите заказ для создания документа.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private string GetUniqueFilePath(string baseFileName, string fileExtension)
        {
            string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string filePath = Path.Combine(folderPath, baseFileName + fileExtension);
            int count = 1;

            while (File.Exists(filePath))
            {
                string tempFileName = $"{baseFileName} ({count++})";
                filePath = Path.Combine(folderPath, tempFileName + fileExtension);
            }

            return filePath;
        }


        private void Button_ClickVK(object sender, RoutedEventArgs e)
        {

        }

        private void Button_ClickTG(object sender, RoutedEventArgs e)
        {

        }

    }
}
