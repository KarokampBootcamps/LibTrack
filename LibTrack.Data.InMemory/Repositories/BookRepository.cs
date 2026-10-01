using LibTrack.Core.Entities;
using LibTrack.Core.Interfaces;

namespace LibTrack.Data.InMemory.Repositories;

public class BookRepository : IBookRepository
{
    private int _nextId = 1;
    
    public Book? Get(int id)
    {
        return ApplicationDbContext.Books.FirstOrDefault(b => b.Id == id);
    }

    public List<Book> GetAll()
    {
        return ApplicationDbContext.Books.ToList();
    }

    public void Add(Book entity)
    {
        entity.Id = _nextId++;
        ApplicationDbContext.Books.Add(entity);
    }

    public void Update(Book entity)
    {
        var book = ApplicationDbContext.Books.FirstOrDefault(b => b.Id == entity.Id);
        
        if (book is null)
            throw new ArgumentNullException(nameof(book));

        book = entity;
    }

    public void Remove(Book entity)
    {
        ApplicationDbContext.Books.Remove(entity);
    }

    public IEnumerable<Book> Search(string keyword)
    {
        throw new NotImplementedException();
    }
}