using System;
using System.Collections.Generic;
using System.IO;

namespace ConsoleApp2.fourthTaskClass
{
    class FileIndexer
    {
        private Dictionary<string, List<string>> _index;

        public FileIndexer()
        {
            _index = new Dictionary<string, List<string>>();
        }

        public void IndexDirectory(string directory, params string[] keywords)
        {
            string resolvedDir = Workspace.ResolveDirectory(directory);

            foreach (string keyword in keywords)
            {
                string lowerKeyword = keyword.ToLower();
                if (!_index.ContainsKey(lowerKeyword))
                {
                    _index[lowerKeyword] = new List<string>();
                }
            }

            foreach (string file in Directory.EnumerateFiles(resolvedDir, "*.txt", SearchOption.AllDirectories))
            {
                try
                {
                    string content = File.ReadAllText(file).ToLower();

                    foreach (string keyword in keywords)
                    {
                        string lowerKeyword = keyword.ToLower();
                        if (content.Contains(lowerKeyword))
                        {
                            if (!_index[lowerKeyword].Contains(file))
                            {
                                _index[lowerKeyword].Add(file);
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("Ошибка чтения файла {0}: {1}", file, e.Message);
                }
            }
        }

        public List<string> Search(string keyword)
        {
            string lowerKeyword = keyword.ToLower();
            if (_index.ContainsKey(lowerKeyword))
            {
                return _index[lowerKeyword];
            }
            return new List<string>();
        }

        public void PrintIndex()
        {
            Console.WriteLine("\n--- Индекс файлов ---");
            if (_index.Count == 0)
            {
                Console.WriteLine("Индекс пуст. Запустите индексацию.");
                return;
            }

            foreach (var entry in _index)
            {
                Console.WriteLine("Ключевое слово \"{0}\": {1} файлов", entry.Key, entry.Value.Count);
                foreach (string file in entry.Value)
                {
                    Console.WriteLine("  - {0}", file);
                }
            }
        }

        public static void RunIndexer()
        {
            FileIndexer indexer = new FileIndexer();
            bool working = true;

            while (working)
            {
                Console.WriteLine(
                    "\n--- Индексатор файлов ---\n" +
                    "1 - Индексировать директорию\n" +
                    "2 - Поиск по ключевому слову\n" +
                    "3 - Показать индекс\n" +
                    "0 - Назад"
                );
                Console.Write("Выберите действие: ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        Console.Write("Путь к директории: ");
                        string dir = Console.ReadLine();
                        Console.Write("Ключевые слова (через запятую): ");
                        string[] keywords = Console.ReadLine().Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                        for (int i = 0; i < keywords.Length; ++i)
                        {
                            keywords[i] = keywords[i].Trim();
                        }
                        indexer.IndexDirectory(dir, keywords);
                        Console.WriteLine("Индексация завершена.");
                        break;
                    case "2":
                        Console.Write("Ключевое слово: ");
                        string keyword = Console.ReadLine();
                        List<string> results = indexer.Search(keyword);
                        if (results.Count == 0)
                        {
                            Console.WriteLine("Файлы не найдены.");
                        }
                        else
                        {
                            Console.WriteLine("Найдено файлов: {0}", results.Count);
                            foreach (string file in results)
                            {
                                Console.WriteLine("  - {0}", file);
                            }
                        }
                        break;
                    case "3":
                        indexer.PrintIndex();
                        break;
                    case "0":
                        working = false;
                        break;
                }

                if (working)
                {
                    Console.WriteLine("\nНажмите любую клавишу...");
                    Console.ReadKey();
                }
            }
        }
    }
}
