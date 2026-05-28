using System;
using System.IO;

namespace ConsoleApp2.fourthTaskClass
{
    static class Workspace
    {
        public static string Root { get; }

        static Workspace()
        {
            Root = Path.GetFullPath(
                Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "workspace")
            );

            if (!Directory.Exists(Root))
            {
                Directory.CreateDirectory(Root);
            }
        }

        public static string ResolvePath(string userPath)
        {
            if (string.IsNullOrWhiteSpace(userPath))
            {
                throw new Exception("Путь не может быть пустым.");
            }

            string fullPath = Path.GetFullPath(
                Path.Combine(Root, userPath)
            );

            if (!fullPath.StartsWith(Root, StringComparison.OrdinalIgnoreCase))
            {
                throw new Exception("Ошибка: доступ за пределы рабочей директории запрещён.");
            }

            return fullPath;
        }

        public static string ResolveDirectory(string userPath)
        {
            string fullPath = ResolvePath(userPath);

            if (!Directory.Exists(fullPath))
            {
                throw new Exception("Директория не найдена: " + fullPath);
            }

            return fullPath;
        }
    }
}
