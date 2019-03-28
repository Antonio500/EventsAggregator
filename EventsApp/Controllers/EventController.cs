using EventsApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using static EventsApp.DataBase.SqlConnectoinEvents;
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
            List<BasicEvent> events= GetAllEventsFromDB();            
            return Ok(events);
        }

        [Route("{id}")]
        [HttpGet]
        public IHttpActionResult GetEvent(int id)
        {
            FullEvent @event = GetEventFromDB(id);
            return Ok(@event);
        }

        [Route("")]
        [HttpPost]
        public IHttpActionResult PostEvent(AddEvent @event)
        {
            PostEventToDB(@event);
            return Ok(@event);
        }

        [Route("{id}")]
        [HttpDelete]
        public HttpResponseMessage DeleteEvent(int id)
        {
            DeleteEventInDB(id);
            return new HttpResponseMessage(HttpStatusCode.NoContent);
        }

    }
}
