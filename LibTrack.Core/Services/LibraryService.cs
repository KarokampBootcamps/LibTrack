using LibTrack.Core.Entities;
using LibTrack.Core.Exceptions;
using LibTrack.Core.Interfaces;

namespace LibTrack.Core.Services;

// Core/Exceptions

// Core/Extensions
public static class LoanExtensions
{
    public static IEnumerable<Loan> Overdue(this IEnumerable<Loan> loans) =>
        loans.Where(l => l.ReturnDate == null && l.DueDate < DateTime.Now);
}

// Core/Services/LibraryService.cs
public class LibraryService
{
    private readonly IBookRepository _books;
    private readonly IMemberRepository _members;
    private readonly ILoanRepository _loans;

    // public event Action<Loan>? LoanOverdue;

    public LibraryService(IBookRepository books, IMemberRepository members, ILoanRepository loans)
    {
        _books = books;
        _members = members;
        _loans = loans;
    }

    public Loan BorrowBook(int bookId, int memberId, int loanDays = 14)
    {
        var book = _books.Get(bookId) ?? throw new InvalidOperationException("Book not found");
        if (book.AvailableCopies <= 0) throw new BookNotAvailableException(book.Title);
        _ = _members.Get(memberId) ?? throw new InvalidOperationException("Member not found");

        book.AvailableCopies--;
        _books.Update(book);

        var loan = new Loan
        {
            BookId = bookId,
            MemberId = memberId,
            LoanDate = DateTime.Now,
            DueDate = DateTime.Now.AddDays(loanDays)
        };
        _loans.Add(loan);
        return loan;
    }

    public void CheckOverdue()
    {
        // foreach (var loan in _loans.GetAll().Overdue())
        //     LoanOverdue?.Invoke(loan);
    }
}