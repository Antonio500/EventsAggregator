using EventsApp.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using static EventsApp.SqlConnectionClass;

namespace EventsApp.DataBase
{
    public class SqlConnectoinEvents
    {
        // Получаем весь список событий
        public static List<BasicEvent> GetAllEventsFromDB()
        {
            try
            {
                List<BasicEvent> Events = new List<BasicEvent>();
                MyConnection.Open();
                SqlCommand command = new SqlCommand("SELECT Id, Title, Date FROM Events", MyConnection);
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    BasicEvent tempEvent = new BasicEvent((int)reader.GetValue(0), (string)reader.GetValue(1), (DateTime)reader.GetValue(2));
                    Events.Add(tempEvent);
                }
                reader.Close();
                return Events;
            }
            catch (Exception e)
            {
                return new List<BasicEvent>();
            }
            finally
            {
                if (MyConnection.State == System.Data.ConnectionState.Open)
                    MyConnection.Close();
            }
        }

        // Получаем событие со всеми полями
        public static FullEvent GetEventFromDB(int id)
        {
            try
            {
                MyConnection.Open();
                // Получаем Id, Title, Date, Description, LocationId
                SqlCommand command = new SqlCommand("SELECT Id, Title, Date, Description, LocationId FROM Events WHERE Id=" + id + "", MyConnection);
                SqlDataReader reader = command.ExecuteReader();
                reader.Read();
                FullEvent tempEvent = new FullEvent((int)reader.GetValue(0), (string)reader.GetValue(1), (DateTime)reader.GetValue(2), (string)reader.GetValue(3), new Location((int)reader.GetValue(4)));
                reader.Close();

                // Получаем координаты в Location
                command.CommandText = "SELECT Latitude, Longitude, Name FROM Locations WHERE Id =" + tempEvent.Location.Id;
                reader = command.ExecuteReader();
                reader.Read();
                tempEvent.Location.Latitude = (float)reader.GetValue(0);
                tempEvent.Location.Longitude = (float)reader.GetValue(1);
                tempEvent.Location.Name = (string)reader.GetValue(2);
                reader.Close();

                // Получаем список тегов для каждого события
                command.CommandText = "SELECT Name FROM Tags WHERE Id = ANY(SELECT IdTag FROM EventsTags WHERE IdEvent=" + tempEvent.Id + ")";
                reader = command.ExecuteReader();
                List<string> tags = new List<string>();
                while (reader.Read())
                {
                    tags.Add((string)reader.GetValue(0));
                }
                reader.Close();
                tempEvent.Tags = tags.ToArray();
                return tempEvent;
            }
            catch (Exception e)
            {
                return new FullEvent();
            }
            finally
            {
                if (MyConnection.State == System.Data.ConnectionState.Open)
                    MyConnection.Close();
            }
        }

        // Добавляем событие в базу данных
        public static int PostEventToDB(AddEvent @event)
        {
            try
            {
                // Заполняем координаты
                MyConnection.Open();
                var commandSql = "INSERT into Events VALUES('" + @event.Title + "', '" + @event.Date + "', '" + @event.Description + "', " + @event.LocationId + ") SELECT cast(  scope_identity() as int)";
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

        // Удаляем событие 
        public static bool DeleteEventInDB(int id)
        {
            try
            {
                MyConnection.Open();
                SqlCommand command = new SqlCommand("DELETE FROM Events WHERE Id=" + id, MyConnection);
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