using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab_3
{
    public class Route
    {
        [BsonId]
        public int Id { get; set; }
        [DisplayName("Країна")]
        public string Country { get; set; }
        [DisplayName("Клімат")]
        public string Climate { get; set; }
        [DisplayName("Тривалість")]
        public int Duration { get; set; }
        [DisplayName("Готель")]
        public string Hotel { get; set; }
        [DisplayName("Ціна")]
        public int Money { get; set; }
        public Route(string country, string climate, int duration, string hotel, int money)
        {
            (Country, Climate, Duration, Hotel, Money) = (country, climate, duration, hotel, money);
        }
        public Route(int id, string country, string climate, int duration, string hotel, int money) : 
            this(country, climate, duration, hotel, money)
        {
            Id = id;
        }

    }
}
