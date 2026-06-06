using System;
using System.Windows;

namespace zd2_3_Melekhova_Marina
{
    public partial class MainWindow : Window
    {
        // Объект магазина
        private Shop shop;

        // Конструктор окна
        public MainWindow()
        {
            InitializeComponent();
            shop = new Shop();
            TitleBlock.Text = "МАГАЗИН";
            ResultTextBlock.Text = "Магазин готов к работе!";
        }

        // Пункт меню "Магазин"
        private void Shop_Click(object sender, RoutedEventArgs e)
        {
            TitleBlock.Text = "МАГАЗИН";
            ResultTextBlock.Text = "Магазин готов к работе!";
        }

        // Пункт меню "Выход"
        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        // Создание товара (кнопка "Добавить товар")
        private void CreateProduct_Click(object sender, RoutedEventArgs e)
        {
            // Проверка на пустое название
            if (string.IsNullOrEmpty(CreateName.Text))
            {
                ResultTextBlock.Text = "Ошибка: Введите название товара!";
                return;
            }

            // Проверка на пустую цену
            if (string.IsNullOrEmpty(CreatePrice.Text))
            {
                ResultTextBlock.Text = "Ошибка: Введите цену!";
                return;
            }

            // Проверка на пустое количество
            if (string.IsNullOrEmpty(CreateCount.Text))
            {
                ResultTextBlock.Text = "Ошибка: Введите количество!";
                return;
            }

            // Парсинг цены
            if (!decimal.TryParse(CreatePrice.Text, out decimal price))
            {
                ResultTextBlock.Text = "Ошибка: Цена должна быть числом!";
                return;
            }

            // Парсинг количества
            if (!int.TryParse(CreateCount.Text, out int count))
            {
                ResultTextBlock.Text = "Ошибка: Количество должно быть целым числом!";
                return;
            }

            // Проверка: количество больше 0
            if (count <= 0)
            {
                ResultTextBlock.Text = "Ошибка: Количество должно быть больше 0!";
                return;
            }

            // Проверка: цена больше 0
            if (price <= 0)
            {
                ResultTextBlock.Text = "Ошибка: Цена должна быть больше 0!";
                return;
            }

            // Добавление товара в магазин
            string name = CreateName.Text;
            shop.CreateProduct(name, price, count);

            ResultTextBlock.Text = $"Товар '{name}' добавлен!\nЦена: {price} руб.\nКоличество: {count} шт.";

            // Очистка полей ввода
            CreateName.Text = "";
            CreatePrice.Text = "";
            CreateCount.Text = "";
        }

        // Продажа товара (кнопка "Продать")
        private void SellProduct_Click(object sender, RoutedEventArgs e)
        {
            // Проверка на пустое название
            if (string.IsNullOrEmpty(SellName.Text))
            {
                ResultTextBlock.Text = "Ошибка: Введите название товара!";
                return;
            }

            string name = SellName.Text;
            int count = 1; // по умолчанию продаем 1 штуку

            // Проверка количества (если поле не пустое)
            if (!string.IsNullOrEmpty(SellCount.Text))
            {
                if (!int.TryParse(SellCount.Text, out count))
                {
                    ResultTextBlock.Text = "Ошибка: Количество должно быть целым числом!";
                    return;
                }
            }

            // Проверка: количество больше 0
            if (count <= 0)
            {
                ResultTextBlock.Text = "Ошибка: Количество должно быть больше 0!";
                return;
            }

            // Продажа (1 штука или несколько)
            string result;
            if (count == 1)
                result = shop.Sell(name);
            else
                result = shop.SellMultiple(name, count);

            ResultTextBlock.Text = result;

            // Очистка полей ввода
            SellName.Text = "";
            SellCount.Text = "1";
        }

        // Поиск товара (кнопка "Найти")
        private void FindProduct_Click(object sender, RoutedEventArgs e)
        {
            // Проверка на пустое название
            if (string.IsNullOrEmpty(FindName.Text))
            {
                ResultTextBlock.Text = "Ошибка: Введите название товара для поиска!";
                return;
            }

            string name = FindName.Text;
            Product product = shop.FindByName(name);

            if (product != null)
            {
                int count = shop.GetProductCount(product);
                ResultTextBlock.Text = $"Товар найден!\n\n{product.GetInfo()}\nКоличество на складе: {count} шт.";
            }
            else
            {
                ResultTextBlock.Text = $"Товар '{name}' не найден!";
            }

            // Очистка поля ввода
            FindName.Text = "";
        }

        // Показать все товары (кнопка "Все товары")
        private void AllProducts_Click(object sender, RoutedEventArgs e)
        {
            ResultTextBlock.Text = shop.GetAllProductsInfo();
        }

        // Показать прибыль (кнопка "Прибыль")
        private void Profit_Click(object sender, RoutedEventArgs e)
        {
            ResultTextBlock.Text = $"Прибыль магазина: {shop.GetProfit():F2} руб.";
        }

        // Очистить магазин (кнопка "Очистить")
        private void ClearShop_Click(object sender, RoutedEventArgs e)
        {
            shop.Clear();
            ResultTextBlock.Text = "Магазин очищен! Все товары удалены.";
        }
    }
}