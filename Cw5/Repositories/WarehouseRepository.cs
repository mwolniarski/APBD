using Cw5.Models;
using System.Data.SqlClient;

namespace Cw5.Repositories
{
    public class WarehouseRepository : IWarehouseRepository
    {
        private readonly string DbUrl = @"Server=5R58803\SQLEXPRESS;Database=Warehouse;Trusted_Connection=True";
        public bool ProductExistById(int id)
        {
            string query = "SELECT * FROM [Product] WHERE IdProduct = " + id;
            using (SqlConnection connection = new SqlConnection(DbUrl))
            {
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    return reader.HasRows;
                }
            }
        }

        public bool WarehouseExistById(int id)
        {
            string query = "SELECT * FROM [Warehouse] WHERE IdWarehouse = " + id;
            using (SqlConnection connection = new SqlConnection(DbUrl))
            {
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    return reader.HasRows;
                }
            }
        }

        public int OrderExistByProductId(int id)
        {
            string query = "SELECT * FROM [Order] WHERE IdProduct = " + id + " AND CreatedAt < getdate()";
            using (SqlConnection connection = new SqlConnection(DbUrl))
            {
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if(!reader.HasRows) return -1;
                    reader.Read();
                    return (int) reader[0];
                }
            }
        }

        public bool ProductWarehouseExistByOrderId(int id)
        {
            string query = "SELECT * FROM [Product_Warehouse] WHERE IdOrder = " + id;
            using (SqlConnection connection = new SqlConnection(DbUrl))
            {
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    return reader.HasRows;
                }
            }
        }
        
        public void UpdateFullFilledDateOnOrder(int id)
        {
            string query = "UPDATE [Order] SET [FulfilledAt] = getdate()  WHERE IdOrder = " + id;
            using (SqlConnection connection = new SqlConnection(DbUrl))
            {
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();
                command.ExecuteReader();
            }
        }

        public int CreateProductWearhouses(RequestModel requestModel, int orderId, decimal price)
        {
            string query = @$"INSERT INTO [Product_Warehouse]([IdWarehouse],[IdProduct],[IdOrder],[Amount],[Price],[CreatedAt]) VALUES({requestModel.IdWarehouse},{requestModel.IdProduct},{orderId},{requestModel.Amount},{price.ToString().Replace(",",".")},getdate()); GO; SELECT @@IDENTITY AS 'Identity';";
            using (SqlConnection connection = new SqlConnection(DbUrl))
            {
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();
                IAsyncResult result = command.BeginExecuteReader();
                using (SqlDataReader reader = command.EndExecuteReader(result))
                {
                    Console.WriteLine("NewRecordId: {0}", reader[0]);
                }
                return 1;
            }
        }

        public Product GetProductbyId(int id)
        {
            string query = "SELECT * FROM [Product] WHERE IdProduct = " + id;
            using (SqlConnection connection = new SqlConnection(DbUrl))
            {
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    reader.Read();
                    if (!reader.HasRows) return null;
                    return new Product 
                    { 
                        IdProduct = (int) reader[0],
                        Name = (string) reader[1],
                        Description = (string) reader[2],
                        Price = (decimal) reader[3]
                    };
                }
            }
        }
    }
}
