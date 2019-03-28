﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EventsApp.Models
{
    public class Location
    {
        public int Id { get; set; }
        public float Latitude { get; set; }   // Широта (-90 - 0 Северная, 0 - 90 Южная)
        public float Longitude { get; set; }  // Долгота (-180 - 0 Восточная, 0 - 180 Западная)
        public string Name { get; set; }
        public Location()
        {
            Id = 0;
            Latitude = 0;
            Longitude = 0;
            Name = "";
        }

        public Location(int id, float latitude = 0, float longitude = 0, string name = "")
        {
            this.Id = id;
            this.Latitude = latitude;
            this.Longitude = longitude;
            this.Name = name;
            if (Math.Abs(Latitude) > 90 || Math.Abs(Longitude) > 180)
            {
                throw new System.ArgumentException(" Широта должна быть от -90 до 90, долгота от -180 до 180!");
            }
        }
    }
}