using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace EventsApp
{
    public class ErrorTextGeneration
    {
        public enum Errors
        {
            IdError,
            TitleError,
            DescriptionError,
            LocationeError,
            LocationIdError,
            DateError
        }

        static public string ErrorWrite(List<Errors> errors)
        {
            if (errors.Count == 0)
            {
                throw new Exception("");
            }
            
                var stringBuilder = new StringBuilder();
                stringBuilder.Append("При валидации возникли следующие ошибки:");
                stringBuilder.Append(Environment.NewLine);
            
            foreach (Errors error in errors)
            {
                if (error == Errors.IdError)
                {
                    Console.Write("Id должен быть от 0 до 1 000 000!");
                }

                if (error == Errors.TitleError)
                {
                    Console.Write("Заголовок должен быть от 4 до 200 символов!");
                }

                if (error == Errors.DescriptionError)
                {
                    Console.Write("Описание должно быть от 10 символов!");
                }

                if (error == Errors.LocationeError)
                {
                    Console.Write("Ошибка задания координат! Широта должна быть от -90 до 90, долгота от -180 до 180!");
                }

                if (error == Errors.LocationIdError)
                {
                    Console.Write("Ошибка задания Id локации!");
                }

                if (error == Errors.DateError)
                {
                    Console.Write("Дата должна быть не меньше текущей!");
                }

            }
            return stringBuilder.ToString();
        }
    }
}