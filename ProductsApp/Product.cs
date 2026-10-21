using System;

namespace ProductsApp
{
    public class Product
    {
        public string Name { get; }
        public decimal Price { get; }
        public string Country { get; }
        public DateTime PackDate { get; }
        public string Description { get; }

        public Product(string name, decimal price, string country,
                       DateTime packDate, string description)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Назва не може бути порожньою.");
            if (price <= 0)
                throw new ArgumentException("Ціна має бути більшою за нуль.");
            if (string.IsNullOrWhiteSpace(country))
                throw new ArgumentException("Вкажіть країну походження.");
            if (packDate.Date > DateTime.Today)
                throw new ArgumentException("Дата пакування не може бути в майбутньому.");

            Name = name.Trim();
            Price = price;
            Country = country.Trim();
            PackDate = packDate.Date;
            Description = description?.Trim() ?? string.Empty;
        }

        public virtual string TypeName => "Товар";
        public virtual string Details => Description;
    }
}