using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using static EventsApp.ErrorTextGeneration;

namespace EventsApp.Models
{
    public class FullEvent : BasicEvent
    {
        [Required(ErrorMessage = "Заполните описание события")]
        [MinLength(10)]
        public string Description { get; set; }

        [Required(ErrorMessage = "Укажите место проведения")]
        public Location Location { get; set; }

        public string[] Tags { get; set; }

        public FullEvent()
        {
        }

        public FullEvent(int id, string title, DateTime date, string description, Location location = null, string[] tags = null)
        {
            this.Id = id;
            this.Title = title;
            this.Date = date;
            this.Description = description;
            this.Location = location;
            this.Tags = tags;

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
            if (Id < 0 || Id > 1000000)
            {
                Errors.Add(ErrorTextGeneration.Errors.IdError);
            }

            if (Title.Length < 4 || Title.Length > 200)
            {
                Errors.Add(ErrorTextGeneration.Errors.TitleError);
            }

            if (Description.Length < 10)
            {
                Errors.Add(ErrorTextGeneration.Errors.DescriptionError);
            }

            if (Math.Abs(Location.Latitude) > 90 || Math.Abs(Location.Longitude) > 180)
            {
                Errors.Add(ErrorTextGeneration.Errors.LocationeError);
            }

            if (Date < DateTime.Now)
            {
                Errors.Add(ErrorTextGeneration.Errors.DateError);
            }
            return Errors;
        }
    }
}