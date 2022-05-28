using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace book2
{
    class Program : Menu
    {
        static void Main(string[] args)
        {
            Library library = new Library("'Казанская Национальная'");
            Menu.Start(library);
        }
    }
    class Book : Menu
    {
        public string Author;
        public string Title;
        public bool IsAvailable;

        public Book(string author, string title)
        {
            Author = author;
            Title = title;
            IsAvailable = true; // этот параметр будет изменяться когда книгу берут из библиотеки, изначально все true(криво оно меняется)
        }
    }
    class Library : Menu
    {
        public string LibraryName;
        public List<Book> ListOfBooks = new List<Book>()
        {
                {new Book("Достоевский Ф.М.","Идиот") },
                {new Book("Пушкин С.А.", " Метель") },
                {new Book("Толстой Л.Н.", "Акула") },
                {new Book("Толстой Л.Н", "Воскресение") },
                {new Book("Грин А.С.", "Рай") },

        };
        public List<Reader> Ledger = new List<Reader>();

        public Library(string libraryName)
        {
            LibraryName = libraryName;
        }

        public List<Book> SearchBook(string name)
        {
            // создаем новый лист и записываем в него книги если они содержат введеную для поиска подстроку
            List<Book> newList = new List<Book>();

            foreach (var book in ListOfBooks)
            {
                if (book.Title.ToLower().Contains(name.ToLower()))
                {
                    newList.Add(book);
                }
            }
            return newList;
        }

        public void RemoveReader(int IndexOfReaderToRemove)
        {
            Ledger.RemoveAt(IndexOfReaderToRemove);
        }

        public List<Book> SortByAuthor()
        {
            // возвращает BookLedger отсортированный по возрастанию в имени автора
            return ListOfBooks.OrderBy(book => book.Author).ToList();
        }
        public void AddBook(string author, string bookTitle)
        {
            // добавляет книгу в лист
            ListOfBooks.Add(new Book(author, bookTitle));
        }

        public void RemoveBook(string author, string bookTitle)
        {
            // тут мы перебираем все книги пока не совпадёт и автор и название. Проверяем что в результате перебора совпало чтото {toRemove} = true и только тогда удаляем из листа
            int i = 0;
            int elementIdToRemoveFromLedger = 0;
            bool toRemove = false;
            foreach (var book in ListOfBooks)
            {
                if (book.Author == author && book.Title == bookTitle)
                {
                    elementIdToRemoveFromLedger = i;
                    toRemove = true;
                    break;
                }
                i++;
            }
            if (toRemove == true)
            {
                if (ListOfBooks[elementIdToRemoveFromLedger].IsAvailable)
                {
                    ListOfBooks.RemoveAt(elementIdToRemoveFromLedger);
                    Console.WriteLine("Книга удалена.");
                }
                else
                {
                    Console.WriteLine("Кто-то читает эту книгу. Подождите, пока он вернется в библиотеку!");
                }
            }
            else
            {
                Console.WriteLine("Ничего не найдено, ничего не удалено.");
            }
        }
    }
    class Menu
    {
        static public void Start(Library library)
        {
            int day = 1;
            while (true)
            {
                Console.Clear();
                Console.WriteLine($"Добро пожаловать в {library.LibraryName} Библиотека! \n");
                switch (AskMainMenuPoint())
                {
                    case 1:
                        TheListOfBooks(library);
                        break;
                    case 2:
                        SearchBook(library);
                        break;
                    case 3:
                        SortByAuthor(library);
                        break;
                    case 4:
                        AddBook(library);
                        break;
                    case 5:
                        RemoveBook(library);
                        break;
                    case 6:
                        Environment.Exit(0);
                        break;
                }

                day++;
            }
        }

        static public int TheListOfAvaliableBooks(Library library)
        {
            int indx = 1;
            Console.WriteLine("\n Все доступные книги в библиотеке:\n");
            foreach (var book in library.ListOfBooks)
            {
                if (book.IsAvailable == true)
                {
                    Console.WriteLine(indx + "." + book.Author + "-" + book.Title);
                    indx++;
                }
            }
            return indx;
        }

        static public string ChooseName(Library library)
        {
            bool check = false;
            Console.WriteLine("\nВведите имя [длина 2+, только латинские символы] и нажмите \"Enter\":");
            string name = Console.ReadLine();
            name = name.Trim(new char[] { ' ' });
            for (int i = 0; i < name.Length; i++)
            {
                if (name.ToLower()[i] >= 'a' && name[i] <= 'z' && name[i] != ' ')
                {
                    check = true;
                }
                else
                {
                    check = false;
                    break;
                }
            }
            while (name.Length < 2 || !check)
            {
                Console.WriteLine("\n Неверный ввод! Введите еще раз и нажмите \"Enter\":");
                Console.Beep(330, 500);
                name = Console.ReadLine();
                name = name.Trim(new char[] { ' ' });
                for (int i = 0; i < name.Length; i++)
                {
                    if (name.ToLower()[i] >= 'a' && name[i] <= 'z' && name[i] != ' ')
                    {
                        check = true;
                    }
                    else
                    {
                        check = false;
                        break;
                    }
                }
            }
            return name;
        }


        static public int SearchBookIndexByNameAndAuthor(Library library, string name, string author)
        {
            int indx = 0;
            foreach (var book in library.ListOfBooks)
            {
                if (book.Title == name && book.Author == author)
                {
                    return indx + 1;
                }
                indx++;
            }
            return 0;
        }

        static public int AskMainMenuPoint()
        {
            Console.WriteLine($"\n Пожалуйста, выберите опцию по номеру от 1 до 6 и нажмите \"Enter\": \n"
                + "1 - Список книг \n"
                + "2 - Поиск книги \n"
                + "3 - Сортировать книги по авторам \n"
                + "4 - Добавить книгу \n"
                + "5 - Удалить книгу \n"
                + "6 - Выход");

            int menuPoint;
            bool isNum = int.TryParse(Console.ReadLine(), out menuPoint);

            while (!isNum || (menuPoint < 1 || menuPoint > 6))
            {
                Console.WriteLine("\n Неверный вход! Попробуйте еще раз. Выберите опции по номеру от 1 до 7 и нажмите \"Enter\":");
                isNum = int.TryParse(Console.ReadLine(), out menuPoint);
            }
            return menuPoint;
        }

        static public void TheListOfBooks(Library library)
        {
            Console.WriteLine("\n Все книги в библиотеке:\n");
            foreach (var book in library.ListOfBooks)
            {
                Console.WriteLine(book.Author + " - " + book.Title + " [" + book.IsAvailable + "]");
            }
            Console.WriteLine("\n нажмите любую клавишу для продолжения...");
            Console.ReadKey(true);
        }

        static public void SearchBook(Library library)
        {
            string name = " ";
            while (name == " ")
            {
                Console.Write("\n Введите название книги для поиска и нажмите \"Enter\":");
                name = Console.ReadLine().Trim();
                foreach (char x in name)
                {
                    if (!char.IsLetter(x))
                    {
                        name = " ";
                        Console.WriteLine("\n Неверный заголовок. Попробуйте еще раз.");
                        break;
                    }
                }
                if (name.Length == 0)
                {
                    name = " ";
                    Console.WriteLine("\n Название пусто.");
                    continue;
                }
            }


            List<Book> newList = library.SearchBook(name);
            if (newList.Count > 0)
            {
                Console.WriteLine("\n Нашел такие книги:");
                foreach (var book in newList)
                {
                    Console.WriteLine(book.Author + "  " + book.Title + " [" + book.IsAvailable + "]");
                }
            }
            else
            {
                Console.WriteLine("\n Ничего не найдено!");
            }
            Console.WriteLine("\n нажмите любую клавишу для продолжения...");
            Console.ReadKey(true);
        }

        static public void SortByAuthor(Library library)
        {
            Console.WriteLine("\n Все книги в библиотеке отсортированы по автору:\n");
            foreach (var book in library.SortByAuthor())
            {
                Console.WriteLine(book.Author + " - " + book.Title + " [" + book.IsAvailable + "]");
            }
            Console.WriteLine("\n нажмите любую клавишу для продолжения...");
            Console.ReadKey(true);
        }
        static public void AddBook(Library library)
        {
            string author = " ";
            while (author == " ")
            {
                Console.Write("\n Введите автора книги для добавления и нажмите\"Enter\":");
                author = Console.ReadLine().Trim();
                foreach (char x in author)
                {
                    if (!char.IsLetter(x))
                    {
                        author = " ";
                        Console.WriteLine("\n Неправильный автор. Попробуйте еще раз.");
                        break;
                    }
                }
                if (author.Length == 0)
                {
                    author = " ";
                    Console.WriteLine("\n Автор пуст.");
                    continue;
                }
            }
            string title = " ";
            while (title == " ")
            {
                Console.Write("\n Введите название книги для добавления и нажмите \"Enter\":");
                title = Console.ReadLine().Trim();
                foreach (char x in title)
                {
                    if (!char.IsLetter(x))
                    {
                        title = " ";
                        Console.WriteLine("\n Неверный заголовок. Попробуйте еще раз.");
                        break;
                    }
                }
                if (title.Length == 0)
                {
                    title = " ";
                    Console.WriteLine("\n Название пусто.");
                    continue;
                }
            }
            library.AddBook(author, title);
            TheListOfBooks(library);
        }

        static public void RemoveBook(Library library)
        {
            string author = " ";
            while (author == " ")
            {
                Console.Write("\n Введите автора книги, которую нужно удалить \"Enter\":");
                author = Console.ReadLine().Trim();
                foreach (char x in author)
                {
                    if (!char.IsLetter(x))
                    {
                        author = " ";
                        Console.WriteLine("\n Неправильный автор. Попробуйте еще раз.");
                        break;
                    }
                }
                if (author.Length == 0)
                {
                    author = " ";
                    Console.WriteLine("\n Название пусто.");
                    continue;
                }
            }
            string title = " ";
            while (title == " ")
            {
                Console.Write("\n Введите название книги, которую нужно удалить, и нажмите \"Enter\":");
                title = Console.ReadLine().Trim();
                foreach (char x in title)
                {
                    if (!char.IsLetter(x))
                    {
                        title = " ";
                        Console.WriteLine("\n Неверный заголовок. Попробуйте еще раз.");
                        break;
                    }
                }
                if (title.Length == 0)
                {
                    title = " ";
                    Console.WriteLine("\n Название пусто.");
                    continue;
                }
            }
            library.RemoveBook(author, title);
            TheListOfBooks(library);
        }
    }
    class Reader : Menu
    {
        public string Name;
        public Book Book;
        public int Term; // на какой срок
        public int LeftTerm;

        public Reader(string name, Book book, int term)
        {
            Name = name;
            Book = book;
            Term = term;
            LeftTerm = term;
        }
    }
}