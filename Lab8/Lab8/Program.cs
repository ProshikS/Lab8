using System;
using System.Collections.Generic;

class Program
{
    static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        string filePath = "shop_database.bin";

        List<Product> products = DatabaseManager.LoadFromFile(filePath);
        DatabaseManager.SeedIfEmpty(products);

        while (true)
        {
            Console.WriteLine();
            Console.WriteLine("|||||Магазин: управление базой данных||||");
            Console.WriteLine("1 - Просмотр базы данных");
            Console.WriteLine("2 - Добавить товар");
            Console.WriteLine("3 - Удалить товар по ID");
            Console.WriteLine("4 - Запрос: товары заданной категории");
            Console.WriteLine("5 - Запрос: товары дешевле заданной цены");
            Console.WriteLine("6 - Запрос: общая стоимость товаров на складе");
            Console.WriteLine("7 - Запрос: средняя цена товаров");
            Console.WriteLine("8 - Сохранить и выйти");
            Console.WriteLine("0 - Выйти без сохранения");
            Console.Write("Ваш выбор: ");

            string choice = Console.ReadLine();

            try
            {
                switch (choice)
                {
                    case "1":
                        DatabaseManager.DisplayAll(products);
                        break;
                    case "2":
                        DatabaseManager.AddProduct(products);
                        break;
                    case "3":
                        DatabaseManager.DeleteById(products);
                        break;
                    case "4":
                        DatabaseManager.QueryByCategory(products);
                        break;
                    case "5":
                        DatabaseManager.QueryByMaxPrice(products);
                        break;
                    case "6":
                        DatabaseManager.ShowTotalInventoryValue(products);
                        break;
                    case "7":
                        DatabaseManager.ShowAveragePrice(products);
                        break;
                    case "8":
                        DatabaseManager.SaveToFile(filePath, products);
                        Console.WriteLine("База данных сохранена. Выход.");
                        return;
                    case "0":
                        Console.WriteLine("Выход без сохранения.");
                        return;
                    default:
                        Console.WriteLine("Неверный выбор. Повторите ввод.");
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
        }
    }
}