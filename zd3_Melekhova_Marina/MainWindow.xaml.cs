using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace zd3_Melekhova_Marina
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Playlist myPlaylist;
        private bool isUpdating = false;

        public MainWindow()
        {
            InitializeComponent();
            myPlaylist = new Playlist();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Update();
        }
        // Обновление интерфейса
        private void Update()
        {
            // Защита от рекурсии
            if (isUpdating) return;
            isUpdating = true;

            try
            {
                if (lbSongs == null || lblCurrent == null) return;

                lbSongs.Items.Clear();
                var songs = myPlaylist.GetAllSongs();
                foreach (var song in songs)
                {
                    lbSongs.Items.Add(song);
                }

                if (myPlaylist.Count > 0)
                {
                    try
                    {
                        Song current = myPlaylist.CurrentSong();
                        int currentIndex = myPlaylist.GetCurrentIndex() + 1;
                        lblCurrent.Text = string.Format("{0} - {1} | Номер: {2} из {3}",
                            current.Author, current.Title, currentIndex, myPlaylist.Count);

                        // Отключаем обработчик события, чтобы избежать рекурсии
                        lbSongs.SelectionChanged -= LbSongs_SelectionChanged;
                        if (myPlaylist.GetCurrentIndex() >= 0 &&
                            myPlaylist.GetCurrentIndex() < lbSongs.Items.Count)
                            lbSongs.SelectedIndex = myPlaylist.GetCurrentIndex();
                        lbSongs.SelectionChanged += LbSongs_SelectionChanged;
                    }
                    catch (Exception)
                    {
                        lblCurrent.Text = "Ошибка при получении текущей песни";
                    }
                }
                else
                {
                    lblCurrent.Text = "Плейлист пуст. Заполните поля и нажмите 'Добавить песню'";
                }
            }
            finally
            {
                isUpdating = false;
            }
        }

        // Добавление песни
        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            string author = txtAuthor.Text.Trim();
            string title = txtTitle.Text.Trim();
            string filename = txtFilename.Text.Trim();

            if (string.IsNullOrEmpty(author))
            {
                MessageBox.Show("Введите автора", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (string.IsNullOrEmpty(title))
            {
                MessageBox.Show("Введите название", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (string.IsNullOrEmpty(filename))
            {
                MessageBox.Show("Введите путь к файлу", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (filename.IndexOfAny(System.IO.Path.GetInvalidPathChars()) >= 0)
            {
                MessageBox.Show("Путь содержит недопустимые символы", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            myPlaylist.AddSong(author, title, filename);

            // Очистка полей
            txtAuthor.Text = "";
            txtTitle.Text = "";
            txtFilename.Text = "";

            Update();
        }

        // Следующая песня
        private void NextMenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (myPlaylist.Count == 0)
            {
                MessageBox.Show("Плейлист пуст", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            myPlaylist.NextSong();
            Update();
        }

        // Предыдущая песня
        private void PrevMenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (myPlaylist.Count == 0)
            {
                MessageBox.Show("Плейлист пуст", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            myPlaylist.PrevSong();
            Update();
        }

        // В начало списка
        private void StartMenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (myPlaylist.Count == 0)
            {
                MessageBox.Show("Плейлист пуст", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            myPlaylist.GoToStart();
            Update();
        }

        // Переход по индексу
        private void GoToIndexMenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (myPlaylist.Count == 0)
            {
                MessageBox.Show("Плейлист пуст", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            Window dialog = new Window();
            dialog.Title = "Переход по индексу";
            dialog.Width = 500;
            dialog.Height = 200;
            dialog.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            dialog.Owner = this;
            dialog.ResizeMode = ResizeMode.NoResize;

            StackPanel panel = new StackPanel() { Margin = new Thickness(10) };
            TextBlock lblInfo = new TextBlock() { Text = string.Format("Введите номер песни (1-{0}):", myPlaylist.Count), Margin = new Thickness(0, 0, 0, 10) };
            TextBox txtIndex = new TextBox() { Height = 25 };
            Button btnOk = new Button() { Content = "Перейти", Height = 30, Margin = new Thickness(0, 10, 0, 0) };

            panel.Children.Add(lblInfo);
            panel.Children.Add(txtIndex);
            panel.Children.Add(btnOk);
            dialog.Content = panel;

            btnOk.Click += (s, args) =>
            {
                if (int.TryParse(txtIndex.Text, out int index) && index >= 1 && index <= myPlaylist.Count)
                {
                    myPlaylist.GoToIndex(index - 1);
                    Update();
                    dialog.Close();
                }
                else
                {
                    MessageBox.Show(string.Format("Введите число от 1 до {0}", myPlaylist.Count), "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            };

            dialog.ShowDialog();
        }

        // Удаление выбранной песни
        private void RemoveMenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (lbSongs.SelectedIndex == -1)
            {
                MessageBox.Show("Выберите песню в списке для удаления", "Удаление", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            MessageBoxResult result = MessageBox.Show("Удалить выбранную песню?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                myPlaylist.RemoveAt(lbSongs.SelectedIndex);
                Update();
            }
        }
        // Удаление по значению (первое совпадение)
        private void RemoveByValueMenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (myPlaylist.Count == 0)
            {
                MessageBox.Show("Плейлист пуст", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            Window dialog = new Window();
            dialog.Title = "Удаление песни по значению";
            dialog.Width = 450;
            dialog.Height = 350;
            dialog.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            dialog.Owner = this;
            dialog.ResizeMode = ResizeMode.NoResize;

            StackPanel panel = new StackPanel() { Margin = new Thickness(10) };

            TextBlock lblTitle = new TextBlock()
            {
                Text = "Введите данные песни для удаления (первое совпадение):",
                FontWeight = FontWeights.Bold,
                Margin = new Thickness(0, 0, 0, 15)
            };

            TextBlock lblAuthor = new TextBlock() { Text = "Автор:", Margin = new Thickness(0, 5, 0, 0) };
            TextBox txtAuthor = new TextBox() { Height = 25, Margin = new Thickness(0, 2, 0, 5) };
            TextBlock lblTitleSong = new TextBlock() { Text = "Название:", Margin = new Thickness(0, 5, 0, 0) };
            TextBox txtTitle = new TextBox() { Height = 25, Margin = new Thickness(0, 2, 0, 5) };
            TextBlock lblFilename = new TextBlock() { Text = "Путь к файлу:", Margin = new Thickness(0, 5, 0, 0) };
            TextBox txtFilename = new TextBox() { Height = 25, Margin = new Thickness(0, 2, 0, 5) };
            StackPanel buttonPanel = new StackPanel() { Orientation = Orientation.Horizontal, Margin = new Thickness(0, 15, 0, 0) };

            Button btnOk = new Button() { Content = "Удалить", Width = 100, Height = 30, Margin = new Thickness(0, 0, 10, 0) };
            btnOk.Background = new SolidColorBrush(Colors.LightCoral);

            Button btnCancel = new Button() { Content = "Отмена", Width = 100, Height = 30 };

            buttonPanel.Children.Add(btnOk);
            buttonPanel.Children.Add(btnCancel);

            panel.Children.Add(lblTitle);
            panel.Children.Add(lblAuthor);
            panel.Children.Add(txtAuthor);
            panel.Children.Add(lblTitleSong);
            panel.Children.Add(txtTitle);
            panel.Children.Add(lblFilename);
            panel.Children.Add(txtFilename);
            panel.Children.Add(buttonPanel);

            dialog.Content = panel;

            btnOk.Click += (s, args) =>
            {
                string author = txtAuthor.Text.Trim();
                string title = txtTitle.Text.Trim();
                string filename = txtFilename.Text.Trim();

                if (string.IsNullOrEmpty(author) &&
                    string.IsNullOrEmpty(title) &&
                    string.IsNullOrEmpty(filename))
                {
                    MessageBox.Show("Заполните хотя бы одно поле для поиска", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                Song songToRemove = new Song(author, title, filename);

                if (myPlaylist.RemoveSong(songToRemove))
                {
                    MessageBox.Show("Песня успешно удалена", "Успех",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                    dialog.Close();
                    Update();
                }
                else
                {
                    MessageBox.Show("Песня с указанными данными не найдена", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            };

            btnCancel.Click += (s, args) => dialog.Close();

            dialog.ShowDialog();
        }

        // Очистка всего плейлиста
        private void ClearMenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (myPlaylist.Count == 0) return;

            MessageBoxResult result = MessageBox.Show("Очистить весь плейлист?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                myPlaylist.Clear();
                Update();
            }
        }

        // Выбор песни из списка
        private void LbSongs_SelectionChanged(object sender, RoutedEventArgs e)
        {
            if (lbSongs.SelectedIndex != -1)
            {
                myPlaylist.GoToIndex(lbSongs.SelectedIndex);
                Update();
            }
        }

        // Выход
        private void ExitMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
        // Добавление тестовой песни 
        private void BtnAddTest_Click(object sender, RoutedEventArgs e)
        {
            Song testSong = new Song("Test Artist", "Test Song", "C:/test.mp3");
            myPlaylist.AddSong(testSong);
            Update();
        }
    }
}