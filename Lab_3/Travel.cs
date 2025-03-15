using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab_3
{
    public class Travel
    {
        [BsonId]
        public int Id { get; set; }
        [DisplayName("Маршрт")]
        public int RouteId { get; set; }
        [DisplayName("Клієнт")]
        public int ClientId { get; set; }
        [DisplayName("Дата відправки")]
        public DateTime Date { get; set; }
        [DisplayName("К-сть людей")]
        public int Amount { get; set; }
        [DisplayName("Знижка")]
        public int Discount { get; set; }
        [DisplayName("Вартість")]
        public int Cost { get; set; }
        public Travel(int routeId, int clientId, DateTime date, int amount, int discount, int cost)
        {
            (RouteId, ClientId, Date,  Amount, Discount, Cost) = (routeId, clientId, date, amount, discount, cost);
        }
        public Travel(int id, int routeId, int clientId, DateTime date, int amount, int discount, int cost): 
            this(routeId, clientId, date, amount, discount, cost)
        {
            Id = id;
        }

    }
}
