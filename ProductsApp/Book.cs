using System;
using System.Collections.Generic;

namespace ProductsApp
{
    public class Book : Product
    {
        public int Pages { get; }
        public string Publisher { get; }
        public List<string> Authors { get; }

        public Book(string name, decimal price, string country, DateTime packDate,
                    string description, int pages, string publisher, List<string> authors)
            : base(name, price, country, packDate, description)
        {
            if (pages <= 0)
                throw new ArgumentException("Кількість сторінок має бути більшою за нуль.");
            if (string.IsNullOrWhiteSpace(publisher))
                throw new ArgumentException("Вкажіть видавництво.");
            if (authors == null || authors.Count == 0)
                throw new ArgumentException("Вкажіть хоча б одного автора.");

            Pages = pages;
            Publisher = publisher.Trim();
            Authors = authors;
        }

        public override string TypeName => "Книга";

        public override string Details =>
            $"{Description} Сторінок: {Pages}. Видавництво: {Publisher}. Автори: {string.Join(", ", Authors)}";
    }
}