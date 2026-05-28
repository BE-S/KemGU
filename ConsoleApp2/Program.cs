using System;
using System.IO;
using System.Collections.Generic;
using ConsoleApp2.thirdTaskClass;
using ConsoleApp2.secondTaskClass;
using ConsoleApp2.secondTaskClass.animalTypes;
using ConsoleApp2.fourthTaskClass;
using ConsoleApp2.fifthTaskClass;

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
                        "3 - Работа с матрицами\n" +
                        "4 - Текстовый редактор / Индексация\n" +
                        "5 - Строки и коллекции\n" +
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
                case "3":
                case "4":
                case "5":
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
                    case "3":
                        matrixCalculator();
                        break;
                    case "4":
                        fourthTaskMenu();
                        break;
                    case "5":
                        TextCorrector.Run();
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

        static void matrixCalculator()
        {
            SquareMatrix matrix1 = null;
            SquareMatrix matrix2 = null;
            SquareMatrix result = null;

            doMenu(matrixCalculatorMenu, new SquareMatrix[] { matrix1, matrix2, result });
        }

        static bool matrixCalculatorMenu(SquareMatrix[] matrixValues)
        {
            bool work = true;

            Console.WriteLine(
                "1 - Создать первую матрицу\n" +
                "2 - Создать вторую матрицу\n" +
                "3 - Сложить матрицы\n" +
                "4 - Умножить матрицы\n" +
                "5 - Найти определитель первой матрицы\n" +
                "6 - Найти обратную матрицу для первой матрицы\n" +
                "7 - Клонировать первую матрицу\n" +
                "8 - Показать матрицы\n" +
                "end - Вернуться в главное меню"
            );

            Console.Write("\nВыбирите действие: ");
            string action = Console.ReadLine();
            Console.WriteLine("\n");

            try
            {
                switch (action)
                {
                    case "1":
                        matrixValues[0] = createMatrix();
                        Console.WriteLine("Первая матрица создана:\n" + matrixValues[0]);
                        break;
                    case "2":
                        matrixValues[1] = createMatrix();
                        Console.WriteLine("Вторая матрица создана:\n" + matrixValues[1]);
                        break;
                    case "3":
                        matrixValues[2] = matrixValues[0] + matrixValues[1];
                        Console.WriteLine("Результат сложения:\n" + matrixValues[2]);
                        break;
                    case "4":
                        matrixValues[2] = matrixValues[0] * matrixValues[1];
                        Console.WriteLine("Результат умножения:\n" + matrixValues[2]);
                        break;
                    case "5":
                        checkMatrix(matrixValues[0], "первая матрица");
                        Console.WriteLine("Определитель первой матрицы: " + matrixValues[0].Determinant());
                        break;
                    case "6":
                        checkMatrix(matrixValues[0], "первая матрица");
                        matrixValues[2] = matrixValues[0].InverseMatrix();
                        Console.WriteLine("Обратная матрица:\n" + matrixValues[2]);
                        break;
                    case "7":
                        checkMatrix(matrixValues[0], "первая матрица");
                        matrixValues[2] = matrixValues[0].Clone();
                        Console.WriteLine("Клон первой матрицы:\n" + matrixValues[2]);
                        Console.WriteLine("Клон равен оригиналу: " + matrixValues[2].Equals(matrixValues[0]));
                        Console.WriteLine("Клон и оригинал один объект: " + Object.ReferenceEquals(matrixValues[2], matrixValues[0]));
                        break;
                    case "8":
                        showMatrix("Первая матрица", matrixValues[0]);
                        showMatrix("Вторая матрица", matrixValues[1]);
                        showMatrix("Последний результат", matrixValues[2]);
                        break;
                    case "end":
                        work = false;
                        break;
                    default:
                        Console.WriteLine("Команда не распознана");
                        break;
                }
            }
            catch (MatrixException e)
            {
                Console.WriteLine("Ошибка матрицы: " + e.Message);
            }

            if (work)
            {
                Console.WriteLine("\nНажмите чтобы продолжить");
                Console.ReadKey();
                Console.Clear();
            }

            return work;
        }

        static SquareMatrix createMatrix()
        {
            Console.Write("Введите размер матрицы: ");
            int size = readInt();

            if (size <= 1)
            {
                throw new MatrixSizeException("Размер матрицы должен быть больше 1.");
            }

            Console.Write("Заполнить случайными значениями? y/n: ");
            string random = Console.ReadLine();

            if (random == "y")
            {
                return new SquareMatrix(size);
            }

            double[,] matrix = new double[size, size];

            for (int i = 0; i < size; ++i)
            {
                for (int j = 0; j < size; ++j)
                {
                    Console.Write("Элемент [" + i + ", " + j + "]: ");
                    matrix[i, j] = readDouble();
                }
            }

            return new SquareMatrix(matrix);
        }

        static int readInt()
        {
            int number;

            while (!int.TryParse(Console.ReadLine(), out number))
            {
                Console.WriteLine("Ошибка! Введите целое число:");
            }

            return number;
        }

        static double readDouble()
        {
            double number;

            while (!double.TryParse(Console.ReadLine(), out number))
            {
                Console.WriteLine("Ошибка! Введите число:");
            }

            return number;
        }

        static void checkMatrix(SquareMatrix matrix, string matrixName)
        {
            if (matrix == null)
            {
                throw new MatrixNullException("Не создана " + matrixName + ".");
            }
        }

        static void showMatrix(string name, SquareMatrix matrix)
        {
            Console.WriteLine(name + ":");

            if (matrix == null)
            {
                Console.WriteLine("Матрица не создана\n");
                return;
            }

            Console.WriteLine(matrix);
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
                result *= a;
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

        static void fourthTaskMenu()
        {
            bool working = true;

            while (working)
            {
                Console.Clear();
                Console.WriteLine(
                    "1 - Текстовый редактор\n" +
                    "2 - Индексатор файлов\n" +
                    "3 - Поиск файлов по ключевому слову\n" +
                    "end - Назад"
                );
                Console.Write("\nВыберите действие: ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        Console.Clear();
                        TextEditor.RunEditor();
                        break;
                    case "2":
                        Console.Clear();
                        FileIndexer.RunIndexer();
                        break;
                    case "3":
                        Console.Clear();
                        searchByKeywordMenu();
                        break;
                    case "end":
                        working = false;
                        break;
                    default:
                        Console.WriteLine("Команда не распознана");
                        break;
                }
            }
        }

        static void searchByKeywordMenu()
        {
            Console.Write("Введите путь к директории (внутри workspace): ");
            string dir = Console.ReadLine();

            Console.Write("Введите ключевое слово: ");
            string keyword = Console.ReadLine();

            TextFileSearcher searcher = new TextFileSearcher();
            List<string> results = searcher.SearchByKeyword(dir, keyword);

            searcher.PrintResults(results);

            Console.WriteLine("\nНажмите любую клавишу...");
            Console.ReadKey();
        }
    }
}
