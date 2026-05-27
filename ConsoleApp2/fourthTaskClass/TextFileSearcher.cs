using System;
using System.Collections.Generic;
using System.IO;

namespace ConsoleApp2.fourthTaskClass
{
    class TextFileSearcher
    {
        public List<string> SearchByKeyword(string directory, string keyword)
        {
            List<string> results = new List<string>();

            string resolvedDir = Workspace.ResolveDirectory(directory);

            foreach (string file in Directory.EnumerateFiles(resolvedDir, "*.txt", SearchOption.AllDirectories))
            {
                try
                {
                    string content = File.ReadAllText(file);
                    if (content.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        results.Add(file);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("Ошибка чтения файла {0}: {1}", file, e.Message);
                }
            }

            return results;
        }

        public void PrintResults(List<string> results)
        {
            if (results.Count == 0)
            {
                Console.WriteLine("Файлы не найдены.");
                return;
            }

            Console.WriteLine("Найдено файлов: {0}", results.Count);
            foreach (string file in results)
            {
                Console.WriteLine("  - {0}", file);
            }
        }
    }
}
