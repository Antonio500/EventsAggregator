using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EventsApp.DataBase;
using EventsApp.Models;
using System.Data.SqlClient;
using static EventsApp.SqlConnectionClass;
using System.Globalization;

namespace EventsApp.DataBase
{
    public class SqlConnectionLocations
    {
        // Получаем весь список локаций
        public static List<Location> GetAllLocationsFromDB()
        {
            try
            {
                List<Location> locations = new List<Location>();
                MyConnection.Open();
                SqlCommand command = new SqlCommand("SELECT Id, Latitude, Longitude, Name FROM Locations", MyConnection);
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Location location = new Location((int)reader.GetValue(0), (float)reader.GetValue(1), (float)reader.GetValue(2), (string)reader.GetValue(3));
                    locations.Add(location);
                }
                reader.Close();
                return locations;
            }
            catch (Exception e)
            {
                return new List<Location>();
            }
            finally
            {
                if (MyConnection.State == System.Data.ConnectionState.Open)
                    MyConnection.Close();
            }
        }

        public static Location GetLocationFromDB(int id)
        {
            try
            {
                MyConnection.Open();
                SqlCommand command = new SqlCommand("SELECT Id, Latitude, Longitude, Name FROM Locations WHERE Id =" + id, MyConnection);
                SqlDataReader reader = command.ExecuteReader();

                reader.Read();
                Location location = new Location((int)reader.GetValue(0), (float)reader.GetValue(1), (float)reader.GetValue(2), (string)reader.GetValue(3));
                reader.Close();
                return location;
            }
            catch (Exception e)
            {
                return new Location();
            }
            finally
            {
                if (MyConnection.State == System.Data.ConnectionState.Open)
                    MyConnection.Close();
            }
        }

        public static Location GetLocationFromDB(float latitude, float longitude)
        {
            try
            {
                MyConnection.Open();
                SqlCommand command = new SqlCommand("SELECT Id, Latitude, Longitude, Name  FROM Locations WHERE Latitude =" + latitude + " AND Longitude =" + longitude, MyConnection);
                SqlDataReader reader = command.ExecuteReader();

                reader.Read();
                Location location = new Location((int)reader.GetValue(0), (float)reader.GetValue(1), (float)reader.GetValue(2), (string)reader.GetValue(3));
                reader.Close();
                return location;
            }
            catch (Exception e)
            {
                return new Location();
            }
            finally
            {
                if (MyConnection.State == System.Data.ConnectionState.Open)
                    MyConnection.Close();
            }
        }

        public static int PostLocationToDB(AddLocation location)
        {
            try
            {
                // Заполняем координаты
                MyConnection.Open();
                CultureInfo ci = new CultureInfo("en-us");
                var commandSql = "INSERT into Locations(Latitude, Longitude, Name) VALUES(" + location.Latitude.ToString("F", CultureInfo.InvariantCulture) + ", " + location.Longitude.ToString("F", CultureInfo.InvariantCulture) + ",'" + location.Name + "') SELECT cast(  scope_identity() as int) ";
                SqlCommand command = new SqlCommand(commandSql, MyConnection);
                SqlDataReader reader = command.ExecuteReader();
                reader.Read();
                int LocationsId = (int)reader.GetValue(0);
                reader.Close();
                return LocationsId;
            }
            catch (Exception e)
            {
                return 0;
            }
            finally
            {
                if (MyConnection.State == System.Data.ConnectionState.Open)
                    MyConnection.Close();
            }
        }

        // Удаляем локацию 
        public static bool DeleteLocationInDB(int id)
        {
            try
            {
                MyConnection.Open();
                SqlCommand command = new SqlCommand("DELETE FROM Locations WHERE Id=" + id, MyConnection);
                command.ExecuteNonQuery();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
            finally
            {
                if (MyConnection.State == System.Data.ConnectionState.Open)
                    MyConnection.Close();
            }
        }
    }
}

