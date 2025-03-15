using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab_3
{
    public class Client
    {
        [BsonId]
        public int Id { get; set; }
        [DisplayName("Ім'я")]
        public string Name { get; set; }
        [DisplayName("Прізвище")]
        public string Surname { get; set; }
        [DisplayName("По-батькові")]
        public string Patronymic {  get; set; }
        [DisplayName("Адреса")]
        public string Address { get; set; }
        [DisplayName("Телефон")]
        public string Phone { get; set; }
        public Client(string name, string surname, string patro, string address, string phone)
        {
            (Name, Surname, Patronymic, Address, Phone) = (name, surname, patro, address, phone);
        }
        public Client(int id, string name, string surname, string patro, string address, string phone) : 
            this(name, surname, patro, address, phone)
        {
            Id = id;
        }

    }
}
