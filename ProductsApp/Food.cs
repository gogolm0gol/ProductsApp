using System;

namespace ProductsApp
{
    public class Food : Product
    {
        public DateTime ExpiryDate { get; }   
        public decimal Quantity { get; }      
        public string Unit { get; }           

        public Food(string name, decimal price, string country, DateTime packDate,
                    string description, DateTime expiryDate, decimal quantity, string unit)
            : base(name, price, country, packDate, description)
        {
            if (expiryDate.Date < packDate.Date)
                throw new ArgumentException("Термін придатності не може бути раніше дати пакування.");
            if (quantity <= 0)
                throw new ArgumentException("Кількість має бути більшою за нуль.");
            if (string.IsNullOrWhiteSpace(unit))
                throw new ArgumentException("Вкажіть одиницю виміру.");

            ExpiryDate = expiryDate.Date;
            Quantity = quantity;
            Unit = unit.Trim();
        }

        public override string TypeName => "Продукт";

        public override string Details =>
            $"{Description} Придатний до {ExpiryDate:dd.MM.yyyy}. Кількість: {Quantity} {Unit}";
    }
}