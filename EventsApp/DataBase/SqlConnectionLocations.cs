using System;
using System.Collections.Generic;
using EventsApp.Models;
using System.Data.SqlClient;
using System.Globalization;

namespace EventsApp.DataBase
{
    public class SqlConnectionLocations
    {
        // Получаем весь список локаций
        public  List<Location> GetAllLocationsFromDB()
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
                {
                    MyConnection.Close();
                }
            }
        }

        public Location GetLocationFromDB(int id)
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

        public Location GetLocationFromDB(float latitude, float longitude)
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

        public int PostLocationToDB(AddLocation location)
        {
            try
            {
                // Заполняем координаты
                MyConnection.Open();
                var commandSql = "INSERT into Locations(Latitude, Longitude, Name) VALUES(" +
                                 location.Latitude.ToString("F", CultureInfo.InvariantCulture) + ", " +
                                 location.Longitude.ToString("F", CultureInfo.InvariantCulture) + ",'" + location.Name +
                                 "') SELECT cast(  scope_identity() as int) ";
                SqlCommand command = new SqlCommand(commandSql, MyConnection);
                SqlDataReader reader = command.ExecuteReader();
                reader.Read();
                int LocationId = (int)reader.GetValue(0);
                reader.Close();
                return LocationId;
            }
            catch (Exception e)
            {
                throw  new Exception("", e);
            }
            finally
            {
                if (MyConnection.State == System.Data.ConnectionState.Open)
                    MyConnection.Close();
            }
        }

        // Удаляем локацию 
        public bool DeleteLocationInDB(int id)
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

