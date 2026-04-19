using System;

class Program
{
    static void Main()
    {
        try {
            bool work = true;

            while (work)
            {
                Console.WriteLine(
                    "1 - Вычислить a^n, изменить позиции элементов у числа\n" +
                    "end - Закрыть программу"
                );

                Console.WriteLine("\nПросмотреть задачу");
                string task = Console.ReadLine();

                switch (task)
                {
                    case "1":
                        Console.Clear();
                        Console.WriteLine("Выбрано «Задание 1»\n\n");
                        Console.WriteLine("Задание: Вычислить a^n\n");
                        raisingToPower();

                        Console.WriteLine("\nЗадание: Изменить позиции элементов у числа\n");
                        changePositionOfElement();
                        break;
                    case "end":
                        work = false;
                        break;
                }

                if (work)
                {
                    Console.WriteLine("\nНажмите чтобы продолжить");
                    Console.ReadKey();
                    Console.Clear();
                }
            }
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