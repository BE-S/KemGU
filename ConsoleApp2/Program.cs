using System;
using System.Collections.Generic;
using ConsoleApp2.secondTaskClass;
using ConsoleApp2.secondTaskClass.animalTypes;

namespace app
{
    class Program
    {
        static void Main()
        {
            try
            {
                doMenu(mainMenu);
            }
            catch (OverflowException e)
            {
                Console.WriteLine("Ошибка: " + e.Message);
            }
            catch (FormatException e)
            {
                Console.WriteLine("Ошибка: " + e.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine("Ошибка: " + e.Message);
            }
        }

        static bool mainMenu()
        {
            bool work = true;

            Console.WriteLine(
                        "1 - Вычислить a^n, изменить позиции элементов у числа\n" +
                        "2 - Зоомагазин\n" +
                        "end - Закрыть программу"
                    );

            Console.Write("\nПросмотреть задачу: ");
            string task = Console.ReadLine();
            Console.WriteLine("\n");


            bool selected = false;

            switch (task)
            {
                case "1":
                case "2":
                    selected = true;
                    break;
                case "end":
                    work = false;
                    break;
                default:
                    Console.WriteLine("Команда не распознана");
                    break;
            }

            if (selected)
            {
                Console.Clear();
                Console.WriteLine("Выбрано «Задание {0}»\n\n", task);

                switch (task)
                {
                    case "1":
                        Console.WriteLine("Задание: Вычислить a^n\n");
                        raisingToPower();

                        Console.WriteLine("\nЗадание: Изменить позиции элементов у числа\n");
                        changePositionOfElement();
                        break;
                    case "2":
                        Console.WriteLine("Приветствую в моём зоомагазинчике! Что вы хотели бы?");
                        zooManager();
                        break;
                }
            }
            

            if (work)
            {
                Console.WriteLine("\nНажмите чтобы продолжить");
                Console.ReadKey();
                Console.Clear();
            }

            return work;
        }

        static void zooManager()
        {
            //
            // Получаем экземляр менеджера
            //
            AnimalManager animalManager = AnimalManager.getInstance();



            //
            // Иметируем поставку продукции в магазин. По идеи товар должен поступать на склад магазина и продукция должна автоматом заноситься в БД по «обмену» между системами, например 1С
            //
            animalManager.setProducts
            (
                new Amphibian("Гриша", 1, "Аквариум", "Ну чё дадут, то и схаваю", 40),
                new Amphibian("ХУИША", 2, "Аквариум", "Ну чё дадут, то и схаваю", 10)
            );

            doMenu(zooManagerMenu, animalManager);
        }

        static bool zooManagerMenu(AnimalManager animalManager)
        {
            bool work = true;

            Console.WriteLine(
                "1 - Посмотреть ассортимент\n" +
                "2 - Найти товар по имени\n" +
                "3 - Найти товар по индексу\n" +
                "end - Закрыть программу"
            );

            Console.Write("\nВыбирите действие: ");
            string action = Console.ReadLine();
            Console.WriteLine("\n");


            switch (action)
            {
                case "1":
                    animalManager.getProducts();
                    break;
                case "2":
                    Console.Write("\nВвидите имя: ");
                    string name = Console.ReadLine();

                    animalManager.findProductByName(name);
                    break;
                case "3":
                    Console.Write("Ввидите позицию товара: ");

                    string indexInString = Console.ReadLine();

                    if (int.TryParse(indexInString, out int index))
                    {
                        animalManager.findProductsByIndex(index);
                    }
                    else
                    {
                        Console.WriteLine("\nВведите число\n");
                    }
                    break;
                case "end":
                    work = false;
                    break;
                default:
                    Console.WriteLine("Команда не распознана");
                    break;
            }

            Console.WriteLine("\n");

            return work;
        }

        static void doMenu(Func<bool> func)
        {
            bool work = true;

            while (work)
            {
                work = func();
            }
        }

        static void doMenu<T>(Func<T, bool> func, T values)
        {
            bool work = true;

            while (work)
            {
                work = func(values);
            }
        }

        static void raisingToPower()
        {
            Console.WriteLine("Введите значение основания степени: ");

            int a, n;

            while (!int.TryParse(Console.ReadLine(), out a))
            {
                Console.WriteLine("Ошибка! Введите целое число:");
            }

            Console.WriteLine("Введите значение степени: ");

            while (!int.TryParse(Console.ReadLine(), out n))
            {
                Console.WriteLine("Ошибка! Введите целое число:");
            }

            int result = a;

            for (int i = 1; i < n; ++i)
            {
                result *= n;
            }

            Console.WriteLine("Результат: " + result + "\n");
        }

        static void changePositionOfElement()
        {
            Console.WriteLine("Введите число: ");
            string numberInString = Console.ReadLine();

            long number = long.Parse(numberInString);

            if (number < 100)
            {
                throw new Exception("Ошибка: Значение меньше 100!");
            }

            int elementPositionInteger = 1;

            int newElementPositionInteger = numberInString.Length - 1;

            char[] tempNumberInString = numberInString.ToCharArray();

            char temp = tempNumberInString[elementPositionInteger];
            tempNumberInString[elementPositionInteger] = tempNumberInString[newElementPositionInteger];
            tempNumberInString[newElementPositionInteger] = temp;

            Console.Write("\nРезультат: ");
            Console.Write(tempNumberInString);
        }
    }
}