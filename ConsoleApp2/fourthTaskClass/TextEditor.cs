using System;
using System.Collections.Generic;
using System.IO;

namespace ConsoleApp2.fourthTaskClass
{
    class TextEditor
    {
        private TextFile _textFile;
        private List<string> _lines;
        private string _filePath;
        private History _history;

        public TextEditor()
        {
            _lines = new List<string>();
            _history = new History();
        }

        public void Load(string path)
        {
            _filePath = Workspace.ResolvePath(path);

            if (File.Exists(_filePath))
            {
                string content = File.ReadAllText(_filePath);
                _textFile = new TextFile(Path.GetFileName(_filePath), content);
                _lines = new List<string>(content.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None));
            }
            else
            {
                _textFile = new TextFile(Path.GetFileName(_filePath), "");
                _lines = new List<string>();
            }

            _history.Clear();
            saveSnapshot();
        }

        public void Show()
        {
            Console.WriteLine("\n--- {0} ---", _filePath ?? "[новый файл]");
            if (_lines.Count == 0)
            {
                Console.WriteLine("[пусто]");
            }
            else
            {
                for (int i = 0; i < _lines.Count; ++i)
                {
                    Console.WriteLine("{0,4}: {1}", i + 1, _lines[i]);
                }
            }
            Console.WriteLine("----------------------");
        }

        public void AddLine(string line)
        {
            saveSnapshot();
            _lines.Add(line);
        }

        public void InsertLine(int index, string line)
        {
            if (index >= 0 && index <= _lines.Count)
            {
                saveSnapshot();
                _lines.Insert(index, line);
            }
        }

        public void DeleteLine(int index)
        {
            if (index >= 0 && index < _lines.Count)
            {
                saveSnapshot();
                _lines.RemoveAt(index);
            }
        }

        public void EditLine(int index, string newContent)
        {
            if (index >= 0 && index < _lines.Count)
            {
                saveSnapshot();
                _lines[index] = newContent;
            }
        }

        public void Save()
        {
            if (_filePath != null)
            {
                File.WriteAllLines(_filePath, _lines);
                Console.WriteLine("Файл сохранён: {0}", _filePath);
            }
        }

        public void SaveAs(string format)
        {
            if (_textFile == null) return;

            _textFile.Content = string.Join(Environment.NewLine, _lines);
            _textFile.ModifiedAt = DateTime.Now;

            string savePath = _filePath;

            if (format == "xml")
            {
                savePath = Path.ChangeExtension(_filePath, ".xml");
                _textFile.SaveXml(savePath);
            }
            else
            {
                savePath = Path.ChangeExtension(_filePath, ".dat");
                _textFile.SaveBinary(savePath);
            }

            Console.WriteLine("Файл сохранён: {0}", savePath);
        }

        public bool Undo()
        {
            Memento memento = _history.Pop();
            if (memento != null)
            {
                _lines = new List<string>(memento.Lines);
                return true;
            }
            return false;
        }

        private void saveSnapshot()
        {
            _history.Push(new Memento(new List<string>(_lines)));
        }

        private class Memento
        {
            public List<string> Lines { get; }

            public Memento(List<string> lines)
            {
                Lines = lines;
            }
        }

        private class History
        {
            private Stack<Memento> _states = new Stack<Memento>();

            public void Push(Memento memento)
            {
                _states.Push(memento);
            }

            public Memento Pop()
            {
                return _states.Count > 0 ? _states.Pop() : null;
            }

            public void Clear()
            {
                _states.Clear();
            }
        }

        public static void RunEditor()
        {
            TextEditor editor = new TextEditor();

            Console.Write("Введите путь к файлу (внутри workspace): ");
            string path = Console.ReadLine();
            editor.Load(path);
            Console.Clear();

            bool working = true;
            while (working)
            {
                editor.Show();

                Console.WriteLine(
                    "1 - Добавить строку\n" +
                    "2 - Вставить строку\n" +
                    "3 - Удалить строку\n" +
                    "4 - Редактировать строку\n" +
                    "5 - Отменить (undo)\n" +
                    "6 - Сохранить (txt)\n" +
                    "7 - Сохранить как (xml/binary)\n" +
                    "0 - Выйти"
                );
                Console.Write("\nВыберите действие: ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        Console.Write("Текст: ");
                        editor.AddLine(Console.ReadLine());
                        break;
                    case "2":
                        Console.Write("Номер строки: ");
                        if (int.TryParse(Console.ReadLine(), out int insertIdx))
                        {
                            Console.Write("Текст: ");
                            editor.InsertLine(insertIdx - 1, Console.ReadLine());
                        }
                        break;
                    case "3":
                        Console.Write("Номер строки: ");
                        if (int.TryParse(Console.ReadLine(), out int deleteIdx))
                        {
                            editor.DeleteLine(deleteIdx - 1);
                        }
                        break;
                    case "4":
                        Console.Write("Номер строки: ");
                        if (int.TryParse(Console.ReadLine(), out int editIdx))
                        {
                            Console.Write("Новый текст: ");
                            editor.EditLine(editIdx - 1, Console.ReadLine());
                        }
                        break;
                    case "5":
                        if (!editor.Undo())
                        {
                            Console.WriteLine("Нечего отменять.");
                        }
                        else
                        {
                            Console.WriteLine("Отмена выполнена.");
                        }
                        break;
                    case "6":
                        editor.Save();
                        break;
                    case "7":
                        Console.Write("Формат (xml/binary): ");
                        editor.SaveAs(Console.ReadLine().ToLower());
                        break;
                    case "0":
                        working = false;
                        break;
                }

                if (working)
                {
                    Console.WriteLine("\nНажмите любую клавишу...");
                    Console.ReadKey();
                    Console.Clear();
                }
            }
        }
    }
}
