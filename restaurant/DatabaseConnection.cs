using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace restaurant
{
    internal class DatabaseConnection
    {
        internal string connectionString = "Data Source=SUFIYAN-LEGION-;Initial Catalog=restaurant;Integrated Security=True";
        internal SqlConnection connection;

        public void OpenConnection()
        {
            connection = new SqlConnection(connectionString);
            connection.Open();
        }

        public void CloseConnection()
        {
            if (connection != null && connection.State != ConnectionState.Closed)
            {
                connection.Close();
            }
        }

        public DataTable ExecuteQuery(string query)
        {
            DataTable dataTable = new DataTable();

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                using (SqlDataAdapter dataAdapter = new SqlDataAdapter(command))
                {
                    dataAdapter.Fill(dataTable);
                }
            }

            return dataTable;
        }

        public int ExecuteNonQuery(string query)
        {
            int rowsAffected = 0;

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                rowsAffected = command.ExecuteNonQuery();
            }

            return rowsAffected;
        }
    }
}
