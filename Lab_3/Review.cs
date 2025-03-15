using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab_3
{
    public class Review
    {
        [BsonId]
        public int Id { get; set; }
        [DisplayName("Клієнт")]
        public int ClientId { get; set; }
        [DisplayName("Маршрут")]
        public int RouteId { get; set; }
        [DisplayName("Оцінка")]
        public int Rating { get; set; }
        [DisplayName("Текст відгуку")]
        public string ReviewText { get; set; }
        [DisplayName("Дата написання")]
        public DateTime Date { get; set; }

        public Review(int clientId, int routeId, int rating, string text, DateTime date)
        {
            (ClientId, RouteId, Rating, ReviewText, Date) = (clientId, routeId, rating, text, date);
        }

        public Review(int id, int clientId, int routeId, int rating, string text, DateTime date) : 
            this(clientId, routeId, rating, text, date)
        {
            Id = id;
        }


    }
}
