using LibTrack.Core.Entities;
using LibTrack.Core.Interfaces;

namespace LibTrack.Data.InMemory.Repositories;

public class LoanRepository : ILoanRepository
{
    private int _nextId = 1;
    
    public Loan? Get(int id)
    {
        return ApplicationDbContext.Loans.FirstOrDefault(b => b.Id == id);
    }

    public List<Loan> GetAll()
    {
        return ApplicationDbContext.Loans.ToList();
    }

    public void Add(Loan entity)
    {
        entity.Id = _nextId++;
        ApplicationDbContext.Loans.Add(entity);
    }

    public void Update(Loan entity)
    {
        var book = ApplicationDbContext.Loans.FirstOrDefault(b => b.Id == entity.Id);
        
        if (book is null)
            throw new ArgumentNullException(nameof(book));

        book = entity;
    }

    public void Remove(Loan entity)
    {
        ApplicationDbContext.Loans.Remove(entity);
    }
}