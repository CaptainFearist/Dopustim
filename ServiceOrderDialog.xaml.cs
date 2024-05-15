using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Data.SqlClient;
using AMOGUSIK.Entities;

namespace AMOGUSIK
{
    public partial class ServiceOrderDialog : Window
    {
        public ServiceOrders Order { get; set; }

        private AudiCenterusContext audiContext;

        public ServiceOrderDialog()
        {
            InitializeComponent();

            Order = new ServiceOrders();
            DataContext = Order;

            using (audiContext = new AudiCenterusContext())
            {
                audiContext.Database.EnsureCreated();
                audiContext.SaveChanges();
            }
        }

        public ServiceOrderDialog(ServiceOrders order)
        {
            InitializeComponent();

            Order = order;
            DataContext = Order;

            using (audiContext = new AudiCenterusContext())
            {
                audiContext.Database.EnsureCreated();
                audiContext.SaveChanges();
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            ServiceOrders serviceOrder;

            if (Order == null)
            {
                serviceOrder = new ServiceOrders();
            }
            else
            {
                serviceOrder = Order;
            }

            serviceOrder.OrderDate = orderDatePicker.SelectedDate.Value;
            serviceOrder.Description = descriptionTextBox.Text;
            var cost = costTextBox.Text;
            serviceOrder.Cost = Convert.ToDecimal(cost);

            using (audiContext = new AudiCenterusContext())
            {
                if (Order == null)
                {
                    Cars newCar = new Cars
                    {
                        Model = "Model",
                        Year = 2022,
                        VIN = "VIN1234567890"
                    };
                    audiContext.Cars.Add(newCar);
                    serviceOrder.Car = newCar;
                }
                else
                {
                    var existingOrder = audiContext.ServiceOrders.Find(serviceOrder.OrderID);
                    if (existingOrder != null)
                    {
                        existingOrder.OrderDate = serviceOrder.OrderDate;
                        existingOrder.Description = serviceOrder.Description;
                        existingOrder.Cost = serviceOrder.Cost;
                    }
                }

                audiContext.SaveChanges();
            }

            DialogResult = true;
            Close();
        }


        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}