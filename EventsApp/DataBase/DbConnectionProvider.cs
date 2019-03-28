using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using EventsApp.Models;


namespace EventsApp
{
    public static class DbConnectionProvider
    {
        private static string DefaultConnectionString => "Data Source=ANTONIO-PC;Initial Catalog=EventsBase;Integrated Security=True";

        //public static SqlConnection GetDbConnectionOpen()
        //{
        //    if (_connection == null)
        //    {
        //        _connection = new SqlConnection(ConnectionString);
        //    }

        //    return _connection;
        //}

        //private static SqlConnection _connection;

        public static SqlConnection GetDbConnectionOpen(string connectionString=null)
        {
            if (connectionString == null)
            {
                connectionString = DefaultConnectionString;
            }

            var result = new SqlConnection(connectionString);
            result.Open();
            return result;
        }
    }
}