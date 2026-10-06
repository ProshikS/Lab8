using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public static class DatabaseManager
{
    // чтение базы из бинарного файла
    public static List<Product> LoadFromFile(string path)
    {
        List<Product> products = new List<Product>();

        if (!File.Exists(path))
        {
            Console.WriteLine("Файл базы данных не найден. Будет создана новая база.");
            return products;
        }

        try
        {
            using (BinaryReader reader = new BinaryReader(File.Open(path, FileMode.Open)))
            {
                int count = reader.ReadInt32();
                for (int i = 0; i < count; i++)
                {
                    Product p = new Product();
                    p.Id = reader.ReadInt32();
                    p.Name = reader.ReadString();
                    p.Category = reader.ReadString();
                    p.Price = reader.ReadDecimal();
                    p.Quantity = reader.ReadInt32();
                    p.Manufacturer = reader.ReadString();
                    products.Add(p);
                }
            }
            Console.WriteLine($"Загружено записей: {products.Count}.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка чтения базы данных: {ex.Message}");
        }

        return products;
    }

    // сохранение в бинарный файл
    public static void SaveToFile(string path, List<Product> products)
    {
        try
        {
            using (BinaryWriter writer = new BinaryWriter(File.Open(path, FileMode.Create)))
            {
                writer.Write(products.Count);
                foreach (Product p in products)
                {
                    writer.Write(p.Id);
                    writer.Write(p.Name);
                    writer.Write(p.Category);
                    writer.Write(p.Price);
                    writer.Write(p.Quantity);
                    writer.Write(p.Manufacturer);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка сохранения: {ex.Message}");
        }
    }

    // заполнение примерами
    public static void SeedIfEmpty(List<Product> products)
    {
        if (products.Count > 0)
        {
            return;
        }

        products.Add(new Product(1, "Молоко", "Молочные продукты", 89.90m, 100, "Простоквашино"));
        products.Add(new Product(2, "Хлеб", "Хлебобулочные изделия", 45.50m, 200, "Хлебозавод №1"));
        products.Add(new Product(3, "Сыр Российский", "Молочные продукты", 450.00m, 30, "Вимм-Билль-Данн"));
        products.Add(new Product(4, "Яблоки", "Фрукты", 120.00m, 50, "Сады Придонья"));
        products.Add(new Product(5, "Куриное филе", "Мясо", 350.00m, 40, "Мираторг"));
        products.Add(new Product(6, "Кола", "Напитки", 99.00m, 80, "Coca-Cola"));

        Console.WriteLine("База данных была пуста. Загружены демонстрационные записи.");
    }

    // просмотр баз данных
    public static void DisplayAll(List<Product> products)
    {
        if (products.Count == 0)
        {
            Console.WriteLine("База данных пуста.");
            return;
        }

        var sorted = from p in products
                     orderby p.Id
                     select p;

        foreach (Product p in sorted)
        {
            Console.WriteLine(p);
        }
    }

    // добавление эл-тов
    public static void AddProduct(List<Product> products)
    {
        int id = ReadUniqueId(products);
        string name = ReadNonEmptyString("Введите название товара: ");
        string category = ReadNonEmptyString("Введите категорию: ");
        decimal price = ReadPositiveDecimal("Введите цену: ");
        int quantity = ReadNonNegativeInt("Введите количество: ");
        string manufacturer = ReadNonEmptyString("Введите производителя: ");

        Product p = new Product(id, name, category, price, quantity, manufacturer);
        products.Add(p);
        Console.WriteLine("Товар успешно добавлен.");
    }

    // удаление эл-тов по ключу
    public static void DeleteById(List<Product> products)
    {
        if (products.Count == 0)
        {
            Console.WriteLine("База данных пуста.");
            return;
        }

        int id = ReadInt("Введите ID товара для удаления: ");

        Product toRemove = (from p in products
                            where p.Id == id
                            select p).FirstOrDefault();

        if (toRemove == null)
        {
            Console.WriteLine($"Товар с ID {id} не найден.");
            return;
        }

        products.Remove(toRemove);
        Console.WriteLine($"Товар '{toRemove.Name}' удалён.");
    }

    // Товары заданной категории
    public static void QueryByCategory(List<Product> products)
    {
        if (products.Count == 0)
        {
            Console.WriteLine("База данных пуста.");
            return;
        }

        string category = ReadNonEmptyString("Введите категорию: ");

        var result = from p in products
                     where string.Equals(p.Category, category, StringComparison.OrdinalIgnoreCase)
                     orderby p.Price
                     select p;

        List<Product> list = result.ToList();

        if (list.Count == 0)
        {
            Console.WriteLine("Товары не найдены.");
            return;
        }

        Console.WriteLine($"Найдено товаров: {list.Count}");
        foreach (Product p in list)
        {
            Console.WriteLine(p);
        }
    }

    // возвращение перечня
    // Товары дешевле заданной цены
    public static void QueryByMaxPrice(List<Product> products)
    {
        if (products.Count == 0)
        {
            Console.WriteLine("База данных пуста.");
            return;
        }

        decimal maxPrice = ReadPositiveDecimal("Введите максимальную цену: ");

        var result = from p in products
                     where p.Price <= maxPrice
                     orderby p.Price
                     select p;

        List<Product> list = result.ToList();

        if (list.Count == 0)
        {
            Console.WriteLine("Товары не найдены.");
            return;
        }

        Console.WriteLine($"Найдено товаров: {list.Count}");
        foreach (Product p in list)
        {
            Console.WriteLine(p);
        }
    }

    // возвращение одного значения
    // Общая стоимость товаров на складе (цена * количество)
    public static void ShowTotalInventoryValue(List<Product> products)
    {
        if (products.Count == 0)
        {
            Console.WriteLine("База данных пуста.");
            return;
        }

        decimal total = (from p in products
                         select p.Price * p.Quantity).Sum();

        Console.WriteLine($"Общая стоимость товаров на складе: {total:F2}");
    }

    // возвращение одного перечня
    // Средняя цена товаров
    public static void ShowAveragePrice(List<Product> products)
    {
        if (products.Count == 0)
        {
            Console.WriteLine("База данных пуста.");
            return;
        }

        decimal average = (from p in products
                           select p.Price).Average();

        Console.WriteLine($"Средняя цена товаров: {average:F2}");
    }

    // проверки ввода
    private static int ReadInt(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            string input = Console.ReadLine();
            if (int.TryParse(input, out int result))
            {
                return result;
            }
            Console.WriteLine("Ошибка! Введите целое число.");
        }
    }

    private static int ReadUniqueId(List<Product> products)
    {
        while (true)
        {
            int id = ReadInt("Введите ID товара: ");
            if (id <= 0)
            {
                Console.WriteLine("ID должен быть положительным числом.");
                continue;
            }

            bool exists = (from p in products
                           where p.Id == id
                           select p).Any();

            if (exists)
            {
                Console.WriteLine("Товар с таким ID уже существует. Введите другой.");
                continue;
            }

            return id;
        }
    }

    private static int ReadNonNegativeInt(string prompt)
    {
        while (true)
        {
            int val = ReadInt(prompt);
            if (val >= 0)
            {
                return val;
            }
            Console.WriteLine("Ошибка! Значение должно быть неотрицательным.");
        }
    }

    private static decimal ReadPositiveDecimal(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            string input = Console.ReadLine();
            if (decimal.TryParse(input, out decimal result) && result > 0)
            {
                return result;
            }
            Console.WriteLine("Ошибка! Введите положительное число.");
        }
    }

    private static string ReadNonEmptyString(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            string input = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(input))
            {
                return input.Trim();
            }
            Console.WriteLine("Ошибка! Строка не может быть пустой.");
        }
    }
}