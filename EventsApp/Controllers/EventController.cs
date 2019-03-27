using EventsApp.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using static EventsApp.SqlConnectionClass;

namespace EventsApp.Controllers
{
    [RoutePrefix("api/events")]
    public class EventController : ApiController
    {
        //static List<FullEvent> Events = new List<FullEvent>() {
        //     new FullEvent(1,
        //            "Премьера фильма Стекло",
        //            new DateTime(2019, 1, 17, 18, 0, 0),
        //            "Стекло» — американский супергеройский фильм в жанре фантастического триллера режиссёра М. Найта Шьямалана. Третий фильм серии «Неуязвимый» и общий сиквел фильмов «Неуязвимый» и «Сплит».",
        //            new Location (1,12f, 61f, "16-й пер., 7, Таганрог")),
        //      new FullEvent(2,
        //            "Клуб гуманитарного самообразования",
        //            new DateTime(2019, 4, 15, 18, 30, 0),
        //            "Мы обсуждаем книги и фильмы. Читаем вслух. Делимся мнением о посещенных спектаклях, выставках и концертах.Рассматриваем со всех сторон: мастерство автора или режиссера, его замысел, исторический контекст, взгляды критиков, влияние произведения на общество и т.д.Также, можно обсудить связь искусства, культуры, истории и реальности.А ещё мы немного пишем по поводу прочитанного, просмотренного, подмеченного в реальной жизни.Приходите — будет интересно.",
        //             new Location (2,52f, 11f,"ул. Фрунзе, 82, Таганрог")),
        //     new FullEvent(3,
        //            "Taganrog Mobile Talks #1: Мобильный фарш",
        //            new DateTime(2019, 5, 16, 14, 0, 0),
        //            "Первый в 2019 митап, посвященный исключительно тематике мобильной разработки.Нас ждут доклады разделов дизайн, IOS, архитектура и профайлинг:• Михаил Никипелов (арт-директор, Distillery) «Ещё никто мне так приятно юиксом не делал»• Артём Ковалёв (ведущий мобильный разработчик, Mentalstack) «IOS и темплейтинг»• Денис Александров (старший Android разработчик, Arcadia) «LiveData — больше, чем шаблон проектирования»• Михаил Левченко (Android разработчик, Auto.ru) Пусть роботы байкшеддят на кодревью за вас",
        //             new Location (3, 53f, 1f,"Таганрог")),
        //     new FullEvent(4,
        //            "Театр Звука Efir | Ставрополь",
        //            new DateTime(2019, 7, 25, 19, 0, 0),
        //            "Выступление проекта экспериментальной этно-музыки Efir( https://vk.com/efir_ethno )Это вовсе не концерт в привычном понимании! Это калейдоскоп архаично-мелодичной музыки, пробуждающей первобытные инстинкты и дремлющую где-то глубоко изначальную Силу...",
        //             new Location (4, 0f, 1f))
        //};

        [Route("")]
        [HttpGet]
        public IHttpActionResult GetAllEvents()
        {
            // Получаем весь список событий
            List<FullEvent> Events = new List<FullEvent>();
            MyConnection.Open();
            SqlCommand command = new SqlCommand("SELECT Id, Title, Date, Description, LocationId FROM Events", MyConnection);
            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                FullEvent tempEvent = new FullEvent((int)reader.GetValue(0), (string)reader.GetValue(1), (DateTime)reader.GetValue(2), (string)reader.GetValue(3), new Location((int)reader.GetValue(4)));
                Events.Add(tempEvent);
            }
            reader.Close();

            // Получаем координаты для каждого события
            foreach (FullEvent tempEvent in Events)
            {
                command.CommandText = "SELECT Latitude, Longitude, Name FROM Locations WHERE Id=" + tempEvent.Location.Id;
                reader = command.ExecuteReader();
                if (reader.Read())
                {
                    tempEvent.Location.Latitude = (float)reader.GetValue(0);
                    tempEvent.Location.Longitude = (float)reader.GetValue(1);
                    tempEvent.Location.Name = (string)reader.GetValue(2);
                }
                reader.Close();
            }


            // Получаем список тегов для каждого события
            foreach (FullEvent tempEvent in Events)
            {
                command.CommandText = "SELECT Name FROM Tags WHERE Id= ANY(SELECT IdTag FROM EventsTags WHERE IdEvent=" + tempEvent.Id + ")";
                reader = command.ExecuteReader();
                List<string> tags = new List<string>();
                while (reader.Read())
                {
                    tags.Add((string)reader.GetValue(0));
                }
                tempEvent.Tags = tags.ToArray();
                reader.Close();
            }

            MyConnection.Close();
            return Ok(Events);
        }

        [Route("{id}")]
        [HttpGet]
        public IHttpActionResult GetEvent(int id)
        {
            // Получаем событие
            MyConnection.Open();
            SqlCommand command = new SqlCommand("SELECT Id, Title, Date, Description, LocationId FROM Events WHERE Id=" + id + "", MyConnection);
            SqlDataReader reader = command.ExecuteReader();

            if (!reader.Read()) // Если нет данных
            {
                MyConnection.Close();
                return NotFound();
            }
            FullEvent tempEvent = new FullEvent((int)reader.GetValue(0), (string)reader.GetValue(1), (DateTime)reader.GetValue(2), (string)reader.GetValue(3), new Location((int)reader.GetValue(4)));
            reader.Close();

            // Получаем координаты
            command.CommandText = "SELECT Latitude, Longitude, Name FROM Locations WHERE Id=" + tempEvent.Location.Id;
            reader = command.ExecuteReader();
            if (reader.Read())
            {
                tempEvent.Location.Latitude = (float)reader.GetValue(0);
                tempEvent.Location.Longitude = (float)reader.GetValue(1);
                tempEvent.Location.Name = (string)reader.GetValue(2);
            }
            reader.Close();

            // Получаем список тегов для каждого события
            command.CommandText = "SELECT Name FROM Tags WHERE Id= ANY(SELECT IdTag FROM EventsTags WHERE IdEvent=" + tempEvent.Id + ")";
            reader = command.ExecuteReader();

            List<string> tags = new List<string>();
            while (reader.Read())
            {
                tags.Add((string)reader.GetValue(0));
            }

            tempEvent.Tags = tags.ToArray();
            reader.Close();
            MyConnection.Close();
            return Ok(tempEvent);
        }

        [Route("")]
        [HttpPost]
        public IHttpActionResult PostEvent(FullEvent @event)
        {
            if (@event.Validate().Count > 0)
            {
                return BadRequest(ModelState);
            }

            // Заполняем координаты
            MyConnection.Open();
            SqlCommand command = new SqlCommand("INSERT into Locations VALUES(" + @event.Location.Latitude + ", " + @event.Location.Longitude + ",'" + @event.Location.Name + "') SELECT cast(  scope_identity() as int) ", MyConnection);
            command.ExecuteNonQuery();

            //command.CommandText = "SELECT scope_identity()";
            SqlDataReader reader = command.ExecuteReader();
            reader.Read();
            int LocationsId = (int)reader.GetValue(0);

            // Событие
            command.CommandText = "INSERT Events VALUES(" + @event.Title + ", " + @event.Date + ", " + @event.Description + ", " + LocationsId + ")";

            command.CommandText = "SELECT scope_identity()";
            reader = command.ExecuteReader();
            int EventId = (int)reader.GetValue(0);
            reader.Close();
            List<int> Tags = new List<int>();

            // Теги
            foreach (string tag in @event.Tags)
            {
                command.CommandText = "INSERT Tags VALUES('" + tag + "')";
                command.ExecuteNonQuery();
                command.CommandText = "SELECT scope_identity()";
                reader = command.ExecuteReader();
                Tags.Add((int)reader.GetValue(0));
                reader.Close();
            }

            // Теги и события
            foreach (int tag in Tags)
            {
                command.CommandText = "INSERT EventsTags VALUES('" + EventId + "', '" + tag + "')";
                command.ExecuteNonQuery();
            }

            MyConnection.Close();
            return Ok(@event);
        }

        [Route("{id}")]
        [HttpDelete]
        public HttpResponseMessage DeleteEvent(int id)
        {
            MyConnection.Open();
            SqlCommand command = new SqlCommand("DELETE FROM Events WHERE Id=" + id, MyConnection);
            command.ExecuteNonQuery();
            MyConnection.Close();
            return new HttpResponseMessage(HttpStatusCode.NoContent);
        }

    }
}
