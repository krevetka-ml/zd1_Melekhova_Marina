using System;
using System.Collections.Generic;
using System.Text;

namespace zd3_Melekhova_Marina
{  
    // Структура музыкальной композиции
    public struct Song
    {
        public string Author; // Автор
        public string Title; // Название
        public string Filename; // Путь к файлу

        // Конструктор
        public Song(string author, string title, string filename)
        {
            Author = author;
            Title = title;
            Filename = filename;
        }

        // Переопределение для отображения
        public override string ToString()
        {
            return $"Автор: {Author} - {Title};\nФайл: {Filename}";
        }
    }
}
