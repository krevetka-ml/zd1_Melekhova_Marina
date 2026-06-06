using zd1_Melekhova_Marina;

Console.Write("Введите количество котов: ");
int count;
while (!int.TryParse(Console.ReadLine(), out count) || count <= 0)
{
    Console.Write("ОШИБКА! Введите положительное целое число: ");
}

//Создаем массив котов
Cat[] cats = new Cat[count];

//Ввод данных для каждого кота
for (int i = 0; i < count; i++)
{
    Console.WriteLine($"\nКот №{i + 1}");

    string name = "";
    bool nameValid = false;

    while (!nameValid)
    {
        Console.Write("Введите имя кота (только буквы): ");
        name = Console.ReadLine();

        //Проверка на пустоту
        if (string.IsNullOrEmpty(name))
        {
            Console.WriteLine("ОШИБКА! Имя не может быть пустым\n");
            continue;
        }

        //Проверка: только буквы
        bool onlyLetters = true;
        foreach (char ch in name)
        {
            if (!char.IsLetter(ch))
            {
                onlyLetters = false;
                break;
            }
        }

        if (onlyLetters)
        {
            nameValid = true;  //Имя корректно, выходим из цикла
        }
        else
        {
            Console.WriteLine($"ОШИБКА: '{name}' - имя может содержать только буквы\n");
        }
    }

    double weight = 0;
    bool weightValid = false;

    while (!weightValid)
    {
        Console.Write("Введите вес кота (кг): ");
        string weightInput = Console.ReadLine();

        if (double.TryParse(weightInput, out weight))
        {
            if (weight > 0 && weight <= 22) //минимум 1, максимум 22 кг
            {
                weightValid = true;  //Вес корректен, выходим из цикла
            }
            else
            {
                Console.WriteLine($"ОШИБКА: Вес {weight} кг не может быть меньше или равен 0, а так же больше 22 кг\n");
            }
        }
        else
        {
            Console.WriteLine($"ОШИБКА: '{weightInput}' - это не число\n");
        }
    }

    //Создаем кота
    cats[i] = new Cat(name, weight);
}

//Вывод результатов
Console.WriteLine("\nРЕЗУЛЬТАТ\n");
foreach (var cat in cats)
{
    cat.Meow();
}