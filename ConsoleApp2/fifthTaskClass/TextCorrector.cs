using System;
using System.IO;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using ConsoleApp2.fourthTaskClass;

namespace ConsoleApp2.fifthTaskClass
{
    class TextCorrector
    {
        public static void Run()
        {
            Dictionary<string, string> wrongWords = getWrongWordsDictionary();

            Console.Write("Введите путь к файлу или директории (внутри workspace): ");
            string inputPath;

            try
            {
                inputPath = Workspace.ResolvePath(Console.ReadLine());
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return;
            }

            string[] files;

            if (File.Exists(inputPath))
            {
                if (!inputPath.EndsWith(".txt", StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine("Файл должен быть текстовым (.txt)");
                    return;
                }
                files = new[] { inputPath };
            }
            else if (Directory.Exists(inputPath))
            {
                files = Directory.GetFiles(inputPath, "*.txt");
            }
            else
            {
                Console.WriteLine("Путь не найден");
                return;
            }

            if (files.Length == 0)
            {
                Console.WriteLine("Нет текстовых файлов для обработки");
                return;
            }

            foreach (string file in files)
            {
                string text = File.ReadAllText(file);
                string result = fixWrongWords(text, wrongWords);

                result = replacePhoneNumbers(result);

                File.WriteAllText(file, result);

                Console.WriteLine("Файл обработан: " + Path.GetFileName(file));
            }
        }

        public static Dictionary<string, string> getWrongWordsDictionary()
        {
            return new Dictionary<string, string>
            {
                { "првиет", "привет" },
                { "пирвет", "привет" },
                { "пирвиет", "привет" },
                { "превет", "привет" },
                { "привте", "привет" },
                { "здарвствуйте", "здравствуйте" },
                { "здраствуйте", "здравствуйте" },
                { "здрвствуйте", "здравствуйте" },
                { "спсибо", "спасибо" },
                { "спосибо", "спасибо" },
                { "спасиба", "спасибо" },
                { "пожлуйста", "пожалуйста" },
                { "пажалуйста", "пожалуйста" },
                { "пожалуста", "пожалуйста" },
                { "пожайлуста", "пожалуйста" },
                { "досвидания", "до свидания" },
                { "дасвидания", "до свидания" },
                { "досвиданья", "до свидания" },
                { "сегдня", "сегодня" },
                { "севодня", "сегодня" },
                { "сиводня", "сегодня" },
                { "зафтра", "завтра" },
                { "завтро", "завтра" },
                { "фчера", "вчера" },
                { "вчира", "вчера" },
                { "типерь", "теперь" },
                { "канечна", "конечно" },
                { "канешно", "конечно" },
                { "ничево", "ничего" },
                { "ничиво", "ничего" },
                { "дириктория", "директория" },
                { "директроия", "директория" },
                { "колекцыя", "коллекция" },
                { "колекция", "коллекция" },
                { "калекция", "коллекция" },
                { "програма", "программа" },
                { "праграма", "программа" },
                { "кампютер", "компьютер" },
                { "кампутер", "компьютер" },
                { "ришение", "решение" },
                { "ришенье", "решение" },
                { "робота", "работа" },
                { "учиник", "ученик" },
                { "учитиль", "учитель" },
                { "учител", "учитель" },
                { "рускай", "русский" },
                { "руский", "русский" },
                { "англиский", "английский" },
                { "анлийский", "английский" },
                { "езык", "язык" },
                { "изык", "язык" },
                { "зодача", "задача" },
                { "задачя", "задача" },
                { "премер", "пример" },
                { "примар", "пример" },
                { "ошыбка", "ошибка" },
                { "ашыбка", "ошибка" },
                { "чиловек", "человек" },
                { "чилавек", "человек" },
                { "дивушка", "девушка" },
                { "дефушка", "девушка" },
                { "малчик", "мальчик" },
                { "мальчек", "мальчик" },
                { "болишой", "большой" },
                { "балшой", "большой" },
                { "маленкий", "маленький" },
                { "малинький", "маленький" },
                { "хароший", "хороший" },
                { "хараший", "хороший" },
                { "интеросный", "интересный" },
                { "интерестный", "интересный" },
                { "извесный", "известный" },
                { "чесный", "честный" },
                { "чювство", "чувство" },
                { "чуство", "чувство" },
                { "щастье", "счастье" },
                { "счасье", "счастье" },
                { "будещее", "будущее" },
                { "пасажир", "пассажир" },
                { "искуство", "искусство" },
                { "учитца", "учиться" },
                { "кароче", "короче" },
                { "вапще", "вообще" },
                { "каличество", "количество" }
            };
        }

        public static string fixWrongWords(string text, Dictionary<string, string> wrongWords)
        {
            foreach (var wrongWord in wrongWords)
            {
                string pattern = @"(?<!\p{L})" + Regex.Escape(wrongWord.Key) + @"(?!\p{L})";

                text = Regex.Replace
                (
                    text,
                    pattern,
                    match => fixWordCase(match.Value, wrongWord.Value),
                    RegexOptions.IgnoreCase
                );
            }

            return text;
        }

        public static string fixWordCase(string word, string correctWord)
        {
            if (word.Length > 0 && Char.IsUpper(word[0]))
            {
                return Char.ToUpper(correctWord[0]) + correctWord.Substring(1);
            }

            return correctWord;
        }

        public static string replacePhoneNumbers(string text)
        {
            string pattern = @"\((0\d{2})\)\s*(\d{3})-(\d{2})-(\d{2})";

            return Regex.Replace
            (
                text,
                pattern,
                match =>
                {
                    string operatorCode = match.Groups[1].Value.Substring(1);

                    return "+380 " +
                           operatorCode + " " +
                           match.Groups[2].Value + " " +
                           match.Groups[3].Value + " " +
                           match.Groups[4].Value;
                }
            );
        }
    }
}
