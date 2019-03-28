using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using EventsApp.Models;


namespace EventsApp
{
    public class SqlConnectionClass
    {
        public static string ConnectionString = "Data Source=ANTONIO-PC;Initial Catalog=EventsBase;Integrated Security=True";
        public static SqlConnection MyConnection = new SqlConnection(ConnectionString);
    }
}