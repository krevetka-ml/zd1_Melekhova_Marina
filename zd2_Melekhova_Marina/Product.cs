using System;

namespace zd2_3_Melekhova_Marina
{
    public class Product
    {
        // Цена товара
        public decimal Price { get; set; }

        // Наименование товара
        public string Name { get; set; }

        // Конструктор товара
        public Product(string Name, decimal Price)
        {
            this.Name = Name;
            this.Price = Price;
        }

        // Получение информации о товаре (в точности как в методичке)
        public string GetInfo()
        {
            return $"Наименование: {Name}; Цена: {Price}";
        }

        // Переопределение Equals для корректной работы Dictionary
        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;
            Product other = (Product)obj;
            return Name == other.Name && Price == other.Price;
        }

        // Переопределение GetHashCode
        public override int GetHashCode()
        {
            return HashCode.Combine(Name, Price);
        }
    }
}