using EventsApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace EventsApp.Controllers
{
    [RoutePrefix("api/events")]
    public class EventController : ApiController
    {
        static List<EventDescription> Events = new List<EventDescription>() {
             new EventDescription(1,
                    "Премьера фильма Стекло",
                    "Стекло» — американский супергеройский фильм в жанре фантастического триллера режиссёра М. Найта Шьямалана. Третий фильм серии «Неуязвимый» и общий сиквел фильмов «Неуязвимый» и «Сплит».",
                    new Location (12f, 61f, "16-й пер., 7, Таганрог"),
                    new DateTime(2019, 1, 17, 18, 0, 0)),
              new EventDescription(2,
                    "Клуб гуманитарного самообразования",
                    "Мы обсуждаем книги и фильмы. Читаем вслух. Делимся мнением о посещенных спектаклях, выставках и концертах.Рассматриваем со всех сторон: мастерство автора или режиссера, его замысел, исторический контекст, взгляды критиков, влияние произведения на общество и т.д.Также, можно обсудить связь искусства, культуры, истории и реальности.А ещё мы немного пишем по поводу прочитанного, просмотренного, подмеченного в реальной жизни.Приходите — будет интересно.",
                     new Location (52f, 11f,"ул. Фрунзе, 82, Таганрог"),
                    new DateTime(2019, 4, 15, 18, 30, 0)),
             new EventDescription(3,
                    "Taganrog Mobile Talks #1: Мобильный фарш",
                    "Первый в 2019 митап, посвященный исключительно тематике мобильной разработки.Нас ждут доклады разделов дизайн, IOS, архитектура и профайлинг:• Михаил Никипелов (арт-директор, Distillery) «Ещё никто мне так приятно юиксом не делал»• Артём Ковалёв (ведущий мобильный разработчик, Mentalstack) «IOS и темплейтинг»• Денис Александров (старший Android разработчик, Arcadia) «LiveData — больше, чем шаблон проектирования»• Михаил Левченко (Android разработчик, Auto.ru) Пусть роботы байкшеддят на кодревью за вас",
                     new Location (53f, 1f,"Таганрог"),
                    new DateTime(2019, 5, 16, 14, 0, 0)),
             new EventDescription(4,
                    "Театр Звука Efir | Ставрополь",
                    "Выступление проекта экспериментальной этно-музыки Efir( https://vk.com/efir_ethno )Это вовсе не концерт в привычном понимании! Это калейдоскоп архаично-мелодичной музыки, пробуждающей первобытные инстинкты и дремлющую где-то глубоко изначальную Силу...",
                     new Location (0f, 1f),
                    new DateTime(2019, 7, 25, 19, 0, 0))
        };

        [Route("")]
        [HttpGet]
        public IHttpActionResult GetAllEvents()
        {
            return Ok(Events);
        }

        [Route("{id}")]
        [HttpGet]
        public IHttpActionResult GetEvent(int id)
        {
            var @event = Events.FirstOrDefault((p) => p.Id == id);
            if (@event == null)
            {
                return NotFound();
            }
            return Ok(@event);
        }

        [Route("")]
        [HttpPost]
        public IHttpActionResult PostEvent(EventDescription @event)
        {
            if (@event.Validate()!=null)
            {
                return BadRequest(ModelState);
            }
            Events.Add(@event);
            return Ok(@event);
        }
        
        [Route("{id}")]
        [HttpDelete]
        public HttpResponseMessage DeleteEvent(int id)
        {
            var @event = Events.FirstOrDefault((p) => p.Id == id);
            if (@event == null)
            {
                return new HttpResponseMessage(HttpStatusCode.NotFound); ;
            }
            Events.RemoveAt(id);
            return new HttpResponseMessage(HttpStatusCode.NoContent);
        }

        [Route("")]
        [HttpDelete]
        public HttpResponseMessage DeleteAllEvents()
        {
            Events.Clear();
            return new HttpResponseMessage(HttpStatusCode.NoContent);
        }

    }
}
