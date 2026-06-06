using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace zd1_Melekhova_Marina
{
    public class Cat
    {
        private string name;   //имя кота
        private double weight; //вес кота
        // Конструктор
        public Cat(string catName, double catWeight)
        {
            Name = catName;     //вызов свойства
            Weight = catWeight; //вызов свойства
        }
        // Свойство Name
        public string Name
        {
            get { return name; }
            set
            {
                // Проверка на пустоту
                if (string.IsNullOrEmpty(value))
                {
                    Console.WriteLine("ОШИБКА: Имя не может быть пустым");
                    return;
                }

                // Проверка: только буквы
                bool onlyLetters = true;
                foreach (char ch in value)
                {
                    if (!char.IsLetter(ch))
                    {
                        onlyLetters = false;
                        break;
                    }
                }

                if (onlyLetters)
                    name = value;
                else
                    Console.WriteLine($"ОШИБКА: '{value}' - имя может содержать только буквы");
            }
        }
        // Свойство Weight
        public double Weight
        {
            get { return weight; }
            set
            {
                if (value <= 0 || value > 22)
                    Console.WriteLine($"ОШИБКА: Вес {value} кг не может быть меньше или равен 0, а так же больше 22");
                else
                    weight = value;
            }
        }

        // Метод мяуканья
        public void Meow()
        {
            Console.WriteLine($"{Name} (вес {Weight} кг): МЯЯЯЯУ!!!!");
        }
    }
}