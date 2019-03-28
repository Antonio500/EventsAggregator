using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using static EventsApp.ErrorTextGeneration;

namespace EventsApp.Models
{
    public class AddEvent
    {
        [Required(ErrorMessage = "Укажите название события")]
        [MinLength(4)]
        public string Title { get; set; }

        [Required(ErrorMessage = "Укажите дату и время проведения")]
        public DateTime Date { get; set; }

        [Required(ErrorMessage = "Заполните описание события")]
        [MinLength(10)]
        public string Description { get; set; }

       // [Required(ErrorMessage = "Укажите место проведения")]
        public int LocationId { get; set; }

        AddEvent()
        {
        }

        AddEvent(string title, DateTime date, string description, int locationId)
        {
            this.Title = title;
            this.Date = date;
            this.Description = description;
            this.LocationId = locationId;

            List<Errors> Errors = this.Validate();
            if (Errors.Count != 0)
            {
                ErrorWrite(Errors);
                throw new System.ArgumentException();
            }
        }

        public List<Errors> Validate()
        {
            List<Errors> Errors = new List<Errors>();
            if (Title.Length < 4 || Title.Length > 200)
            {
                Errors.Add(ErrorTextGeneration.Errors.TitleError);
            }

            if (Description.Length < 10)
            {
                Errors.Add(ErrorTextGeneration.Errors.DescriptionError);
            }

            if (LocationId < 0 || LocationId > 1000000)
            {
                Errors.Add(ErrorTextGeneration.Errors.LocationIdError);
            }

            if (Date < DateTime.Now)
            {
                Errors.Add(ErrorTextGeneration.Errors.DateError);
            }
            return Errors;
        }
    }
}