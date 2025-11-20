using Library.Domain.Model;
using MongoDB.Bson;

namespace Library.Domain.Data;

/// <summary>
/// Генератор тестовых данных с заполнением связей
/// </summary>
public class DataSeeder
{
    /// <summary>
    /// Набор типов изданий, используемых для классификации книг
    /// </summary>
    public List<EditionType> EditionTypes { get; }

    /// <summary>
    /// Коллекция издательств, доступных в библиотеке
    /// </summary>
    public List<Publisher> Publishers { get; }

    /// <summary>
    /// Список книг с заполненными связями на издательства и типы изданий
    /// </summary>
    public List<Book> Books { get; }

    /// <summary>
    /// Зарегистрированные читатели библиотеки с персональными данными
    /// </summary>
    public List<Reader> Readers { get; }

    /// <summary>
    /// История выдачи книг с указанием дат, сроков и возвратов
    /// </summary>
    public List<BookLoan> Loans { get; }


    public DataSeeder()
    {
        EditionTypes = GenerateEditionTypes();
        Publishers = GeneratePublishers();
        Readers = GenerateReaders();
        Books = GenerateBooks();
        Loans = GenerateLoans();
    }

    /// <summary>
    /// Создает набор типов изданий для последующего использования в книгах
    /// </summary>
    private List<EditionType> GenerateEditionTypes() =>
    [
        new EditionType { Name = "Роман" },
        new EditionType { Name = "Повесть" },
        new EditionType { Name = "Учебник" },
        new EditionType { Name = "Сборник рассказов" },
        new EditionType { Name = "Монография" },
        new EditionType { Name = "Энциклопедия" },
        new EditionType { Name = "Художественная литература" },
        new EditionType { Name = "Научная литература" },
        new EditionType { Name = "Документальная проза" },
        new EditionType { Name = "Журнал" }
    ];

    /// <summary>
    /// Формирует список издательств, которые могут быть связаны с книгами
    /// </summary>
    private List<Publisher> GeneratePublishers() =>
    [
        new Publisher { Name = "Эксмо" },
        new Publisher { Name = "АСТ" },
        new Publisher { Name = "Питер" },
        new Publisher { Name = "Манн Иванов Фербер" },
        new Publisher { Name = "Наука" },
        new Publisher { Name = "Просвещение" },
        new Publisher { Name = "Феникс" },
        new Publisher { Name = "Лабиринт" },
        new Publisher { Name = "Вита" },
        new Publisher { Name = "Мир" }
    ];

    /// <summary>
    /// Создает коллекцию читателей с заполненными персональными данными
    /// </summary>
    private List<Reader> GenerateReaders() =>
    [
        new Reader { FullName = "Иванов Иван Иванович", Address = "ул Ленина 1", Phone = "+79990000001", RegistrationDate = new DateTime(2023, 1, 10) },
        new Reader { FullName = "Петров Петр Петрович", Address = "ул Ленина 2", Phone = "+79990000002", RegistrationDate = new DateTime(2022, 11, 5) },
        new Reader { FullName = "Сидорова Анна Михайловна", Address = "ул Октябрьская 3", Phone = "+79990000003", RegistrationDate = new DateTime(2023, 3, 22) },
        new Reader { FullName = "Кузнецов Алексей Сергеевич", Address = "ул Молодежная 4", Phone = "+79990000004", RegistrationDate = new DateTime(2021, 9, 14) },
        new Reader { FullName = "Орлова Мария Андреевна", Address = "ул Чехова 5", Phone = "+79990000005", RegistrationDate = new DateTime(2024, 2, 1) },
        new Reader { FullName = "Соболев Кирилл Игоревич", Address = "ул Новая 6", Phone = "+79990000006", RegistrationDate = new DateTime(2023, 12, 10) },
        new Reader { FullName = "Волкова Екатерина Дмитриевна", Address = "ул Садовая 7", Phone = "+79990000007", RegistrationDate = new DateTime(2022, 7, 8) },
        new Reader { FullName = "Федоров Николай Павлович", Address = "ул Кирова 8", Phone = "+79990000008", RegistrationDate = new DateTime(2024, 1, 17) },
        new Reader { FullName = "Семенова Ольга Степановна", Address = "ул Полевая 9", Phone = "+79990000009", RegistrationDate = new DateTime(2021, 5, 30) },
        new Reader { FullName = "Громов Андрей Константинович", Address = "ул Центральная 10", Phone = "+79990000010", RegistrationDate = new DateTime(2022, 3, 19) }
    ];

    /// <summary>
    /// Генерирует список книг и связывает их с издательствами и типами изданий
    /// </summary>
    private List<Book> GenerateBooks() =>
    [
        new Book { InventoryNumber = 101, CatalogCode = "Б-001", Authors = "М А Булгаков", Title = "Мастер и Маргарита", EditionTypeId = EditionTypes[0].Id, PublisherId = Publishers[0].Id, Year = 1967 },
        new Book { InventoryNumber = 102, CatalogCode = "Д-002", Authors = "Ф М Достоевский", Title = "Преступление и наказание", EditionTypeId = EditionTypes[0].Id, PublisherId = Publishers[1].Id, Year = 1866 },
        new Book { InventoryNumber = 103, CatalogCode = "Т-003", Authors = "Л Н Толстой", Title = "Война и мир", EditionTypeId = EditionTypes[0].Id, PublisherId = Publishers[2].Id, Year = 1869 },
        new Book { InventoryNumber = 104, CatalogCode = "Р-004", Authors = "Э М Ремарк", Title = "Три товарища", EditionTypeId = EditionTypes[1].Id, PublisherId = Publishers[3].Id, Year = 1937 },
        new Book { InventoryNumber = 105, CatalogCode = "К-005", Authors = "С Кинг", Title = "Сияние", EditionTypeId = EditionTypes[6].Id, PublisherId = Publishers[4].Id, Year = 1977 },
        new Book { InventoryNumber = 106, CatalogCode = "Б-006", Authors = "М А Булгаков", Title = "Белая гвардия", EditionTypeId = EditionTypes[1].Id, PublisherId = Publishers[5].Id, Year = 1925 },
        new Book { InventoryNumber = 107, CatalogCode = "О-007", Authors = "Д Оруэлл", Title = "1984", EditionTypeId = EditionTypes[6].Id, PublisherId = Publishers[0].Id, Year = 1949 },
        new Book { InventoryNumber = 108, CatalogCode = "К-008", Authors = "С Кинг", Title = "Оно", EditionTypeId = EditionTypes[6].Id, PublisherId = Publishers[7].Id, Year = 1986 },
        new Book { InventoryNumber = 109, CatalogCode = "Г-009", Authors = "Д Роулинг", Title = "Гарри Поттер и философский камень", EditionTypeId = EditionTypes[6].Id, PublisherId = Publishers[8].Id, Year = 1997 },
        new Book { InventoryNumber = 110, CatalogCode = "Л-010", Authors = "Д Лондон", Title = "Мартин Иден", EditionTypeId = EditionTypes[0].Id, PublisherId = Publishers[9].Id, Year = 1909 }
    ];

    /// <summary>
    /// Создает историю выдачи книг с указанием сроков и дат возврата
    /// </summary>
    private List<BookLoan> GenerateLoans() =>
    [
        new BookLoan { BookId = Books[0].Id, ReaderId = Readers[0].Id, LoanDate = new DateTime(2024, 1, 5), LoanDays = 14, ReturnDate = new DateTime(2024, 1, 20) },
        new BookLoan { BookId = Books[1].Id, ReaderId = Readers[1].Id, LoanDate = new DateTime(2024, 2, 10), LoanDays = 30, ReturnDate = null },
        new BookLoan { BookId = Books[2].Id, ReaderId = Readers[2].Id, LoanDate = new DateTime(2023, 12, 1), LoanDays = 21, ReturnDate = new DateTime(2023, 12, 22) },
        new BookLoan { BookId = Books[3].Id, ReaderId = Readers[3].Id, LoanDate = new DateTime(2024, 2, 15), LoanDays = 10, ReturnDate = new DateTime(2024, 2, 25) },
        new BookLoan { BookId = Books[4].Id, ReaderId = Readers[4].Id, LoanDate = new DateTime(2024, 3, 1), LoanDays = 7, ReturnDate = null },
        new BookLoan { BookId = Books[5].Id, ReaderId = Readers[0].Id, LoanDate = new DateTime(2024, 1, 15), LoanDays = 14, ReturnDate = new DateTime(2024, 1, 29) },
        new BookLoan { BookId = Books[6].Id, ReaderId = Readers[0].Id, LoanDate = new DateTime(2024, 2, 15), LoanDays = 14, ReturnDate = null },
        new BookLoan { BookId = Books[7].Id, ReaderId = Readers[1].Id, LoanDate = new DateTime(2024, 3, 10), LoanDays = 60, ReturnDate = null },
        new BookLoan { BookId = Books[8].Id, ReaderId = Readers[2].Id, LoanDate = new DateTime(2024, 1, 12), LoanDays = 5, ReturnDate = new DateTime(2024, 1, 17) },
        new BookLoan { BookId = Books[9].Id, ReaderId = Readers[3].Id, LoanDate = new DateTime(2023, 11, 20), LoanDays = 90, ReturnDate = null }
    ];
}