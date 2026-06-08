using System;
using System.Collections.Generic;
using System.Text;

namespace zd3_Melekhova_Marina
{
    // Класс управления плейлистом
    public class Playlist
    {
        private List<Song> list; // Список композиций
        private int currentIndex; // Индекс текущей песни

        // Конструктор
        public Playlist()
        {
            list = new List<Song>();
            currentIndex = 0;
        }

        // Получение текущей композиции
        public Song CurrentSong()
        {
            if (list.Count > 0)
                return list[currentIndex];
            else
                throw new IndexOutOfRangeException("Плейлист пуст!");
        }

        // Перегрузка: по полям
        public void AddSong(string author, string title, string filename)
        {
            Song newSong = new Song(author, title, filename);
            list.Add(newSong);
        }

        // Перегрузка: объектом Song
        public void AddSong(Song song)
        {
            list.Add(song);
        }

        // Переход к следующей
        public void NextSong()
        {
            if (list.Count == 0) return;
            currentIndex = (currentIndex + 1) % list.Count;
        }

        // Переход к предыдущей
        public void PrevSong()
        {
            if (list.Count == 0) return;
            currentIndex = (currentIndex - 1 + list.Count) % list.Count;
        }

        // Переход по индексу
        public bool GoToIndex(int index)
        {
            if (index >= 0 && index < list.Count)
            {
                currentIndex = index;
                return true;
            }
            return false;
        }

        // Переход к началу списка
        public void GoToStart()
        {
            if (list.Count > 0)
                currentIndex = 0;
        }

        // Удаление по индексу
        public bool RemoveAt(int index)
        {
            if (index >= 0 && index < list.Count)
            {
                list.RemoveAt(index);
                // Корректировка текущего индекса
                if (list.Count == 0)
                    currentIndex = 0;
                else if (currentIndex >= list.Count)
                    currentIndex = list.Count - 1;
                return true;
            }
            return false;
        }

        // Удаление по значению (первое совпадение)
        public bool RemoveSong(Song song)
        {
            int index = list.FindIndex(s => s.Author == song.Author &&
                                           s.Title == song.Title &&
                                           s.Filename == song.Filename);
            if (index != -1)
            {
                return RemoveAt(index);
            }
            return false;
        }

        // Очистка плейлиста
        public void Clear()
        {
            list.Clear();
            currentIndex = 0;
        }

        // Получение всех песен
        public List<Song> GetAllSongs()
        {
            return new List<Song>(list);
        }

        // Получение текущего индекса
        public int GetCurrentIndex()
        {
            return currentIndex;
        }

        // Количество песен
        public int Count
        {
            get { return list.Count; }
        }
    }
}
