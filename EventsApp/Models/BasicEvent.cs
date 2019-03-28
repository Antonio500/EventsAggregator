using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EventsApp.Models
{
    public class BasicEvent
    {
        [Range(0, 1000000)]
        public int Id { get; set; }

        [Required(ErrorMessage = "Укажите название события")]
        [MinLength(4)]
        public string Title { get; set; }

        [Required(ErrorMessage = "Укажите дату и время проведения")]
        public DateTime Date { get; set; }

        public BasicEvent()
        {
        }
        public BasicEvent(int id, string title, DateTime date)
        {
            this.Id = id;
            this.Title = title;
            this.Date = date;
        }
    }
}