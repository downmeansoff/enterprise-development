using AutoMapper;
using Library.Application.Contracts.BookLoans;
using Library.Application.Contracts.Books;
using Library.Application.Contracts.EditionTypes;
using Library.Application.Contracts.Publishers;
using Library.Application.Contracts.Readers;
using Library.Domain.Model;

namespace Library.Application;

/// <summary>
/// AutoMapper профиль для доменной области библиотеки
/// </summary>
public class LibraryProfile : Profile
{
    /// <summary>
    /// Конструктор профиля, инициализирующий связи между Entity и Dto сущностями
    /// </summary>
    public LibraryProfile()
    {
        CreateMap<BookLoan, BookLoanDto>();
        CreateMap<BookLoanCreateUpdateDto, BookLoan>();

        CreateMap<Book, BookDto>();
        CreateMap<BookCreateUpdateDto, Book>();

        CreateMap<EditionType, EditionTypeDto>();
        CreateMap<EditionTypeCreateUpdateDto, EditionType>();

        CreateMap<Publisher, PublisherDto>();
        CreateMap<PublisherCreateUpdateDto, Publisher>();

        CreateMap<Reader, ReaderDto>();
        CreateMap<ReaderCreateUpdateDto, Reader>();
    }
}