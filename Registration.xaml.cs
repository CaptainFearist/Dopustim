using AMOGUSIK.Entities;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace AMOGUSIK
{
    public partial class Registration : Window
    {
        private AudiCenterusContext dbContext;

        public Registration()
        {
            InitializeComponent();
            dbContext = new AudiCenterusContext();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            MainWindow Ent = new MainWindow();
            Ent.Show();
            this.Close();
        }

        private void RegisterClick(object sender, RoutedEventArgs e)
        {
            string connectionString = "Data Source=DESKTOP-IPI4ON8\\SQLExpress;" +
                "Initial Catalog=AudiCenterus;Integrated Security=True;MultipleActiveResultSets=True;" +
                "TrustServerCertificate=True";

            string firstName = Name1.Text;
            string lastName = Surname1.Text;
            string username = Login1.Text;
            string password = PSD2.Password;
            string confirmPassword = PSD3.Password;

            if (password != confirmPassword)
            {
                MessageBox.Show("Пароли не совпадают.");
                return;
            }

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // SQL-запрос для вставки данных в базу данных
                    string sqlQuery = "INSERT INTO dbo.Customers (FirstName, LastName, Username, Password, RoleID) VALUES (@FirstName, @LastName, @Login, @Password, 3)";

                    using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                    {
                        command.Parameters.AddWithValue("@FirstName", firstName);
                        command.Parameters.AddWithValue("@LastName", lastName);
                        command.Parameters.AddWithValue("@Login", username);
                        command.Parameters.AddWithValue("@Password", password);

                        // Выполните SQL-запрос
                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            // Данные успешно добавлены
                            MessageBox.Show("Данные успешно добавлены в базу данных.");
                        }
                        else
                        {
                            // Что-то пошло не так
                            MessageBox.Show("Произошла ошибка при добавлении данных в базу данных.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка: {ex.Message}");
            }
        }

        private void Login1_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}