using System;
using System.Collections.Generic;
using System.Text;

namespace zd2_3_Melekhova_Marina
{
    public class Shop
    {
        // Словарь товаров: ключ - товар, значение - количество на складе
        private Dictionary<Product, int> products;

        // Прибыль магазина
        private decimal profit;

        // Конструктор магазина
        public Shop()
        {
            products = new Dictionary<Product, int>();
            profit = 0;
        }

        // Добавление товара в магазин с указанием количества
        public void AddProduct(Product product, int count)
        {
            products.Add(product, count);
        }

        // Создание и добавление нового товара
        public void CreateProduct(string name, decimal price, int count)
        {
            products.Add(new Product(name, price), count);
        }

        // Вывод всех товаров
        public void WriteAllProducts()
        {
            Console.WriteLine("Список продуктов: ");
            foreach (var product in products)
            {
                Console.WriteLine(product.Key.GetInfo() + "; Количество: " + product.Value);
            }
        }

        // Продажа товара по объекту
        public void Sell(Product product)
        {
            if (products.ContainsKey(product))
            {
                if (products[product] == 0)
                {
                    Console.WriteLine("Нет в наличии!");
                }
                else
                {
                    products[product]--;
                    profit += product.Price;
                }
            }
            else
            {
                Console.WriteLine("Товар не найден!");
            }
        }

        // Продажа товара по имени 
        public string Sell(string ProductName)
        {
            Product ToSell = FindByName(ProductName);
            if (ToSell != null)
            {
                if (products.ContainsKey(ToSell))
                {
                    if (products[ToSell] == 0)
                    {
                        return "Нет в наличии!";
                    }
                    else
                    {
                        products[ToSell]--;
                        profit += ToSell.Price;
                        return "Товар '" + ProductName + "' продан за " + ToSell.Price.ToString("F2") + " руб.";
                    }
                }
                else
                {
                    return "Товар не найден!";
                }
            }
            else
            {
                return "Товар '" + ProductName + "' не найден!";
            }
        }

        // Поиск товара по имени
        public Product FindByName(string name)
        {
            foreach (var product in products.Keys)
            {
                if (product.Name == name)
                {
                    return product;
                }
            }
            return null;
        }

        // Продажа нескольких товаров
        public string SellMultiple(string productName, int count)
        {
            if (count <= 0)
            {
                return "Ошибка: Количество должно быть больше 0!";
            }

            Product product = FindByName(productName);
            if (product == null)
            {
                return "Ошибка: Товар '" + productName + "' не найден!";
            }

            if (!products.ContainsKey(product))
            {
                return "Ошибка: Товар '" + productName + "' не найден в наличии!";
            }

            int available = products[product];
            if (available < count)
            {
                return "Ошибка: Недостаточно товара! Доступно: " + available + ", запрошено: " + count;
            }

            products[product] -= count;
            decimal totalProfit = product.Price * count;
            profit += totalProfit;

            if (products[product] == 0)
            {
                products.Remove(product);
            }

            return "Продано " + count + " шт. '" + productName + "' на сумму " + totalProfit.ToString("F2") + " руб.";
        }

        // Получить количество товара по названию
        public int GetProductCount(string productName)
        {
            Product product = FindByName(productName);
            if (product != null && products.ContainsKey(product))
            {
                return products[product];
            }
            return 0;
        }

        // Получить количество товара по объекту
        public int GetProductCount(Product product)
        {
            Product existing = FindByName(product.Name);
            if (existing != null && products.ContainsKey(existing))
            {
                return products[existing];
            }
            return 0;
        }

        // Получить прибыль
        public decimal GetProfit()
        {
            return profit;
        }

        // Получить прибыль с валютой
        public string GetProfit(string currency)
        {
            return profit.ToString("F2") + " " + currency;
        }

        // Получить прибыль с округлением
        public decimal GetProfit(int roundDigits)
        {
            return Math.Round(profit, roundDigits);
        }

        // Получить информацию о всех товарах
        public string GetAllProductsInfo()
        {
            if (products.Count == 0)
            {
                return "В магазине нет товаров.";
            }

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("СПИСОК ПРОДУКТОВ:");
            foreach (var item in products)
            {
                sb.AppendLine(item.Key.GetInfo() + "; Количество: " + item.Value);
            }
            sb.AppendLine("ИТОГО ПРИБЫЛЬ: " + profit.ToString("F2") + " руб.");

            return sb.ToString();
        }

        // Очистить магазин
        public void Clear()
        {
            products.Clear();
            profit = 0;
        }
    }
}