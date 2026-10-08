// LibTrack.Data.Sql/Repositories/SqlLoanRepository.cs
using Dapper;
using LibTrack.Core.Entities;
using LibTrack.Core.Interfaces;

namespace LibTrack.Data.Sql.Repositories;

public class SqlLoanRepository : BaseRepository<Loan>, ILoanRepository
{
    public SqlLoanRepository(SqliteConnectionFactory factory) : base(factory) { }

    protected override string TableName => "Loans";

    protected override string InsertSql => """
                                           INSERT INTO Loans (BookId, MemberId, LoanDate, DueDate, ReturnDate)
                                           VALUES (@BookId, @MemberId, @LoanDate, @DueDate, @ReturnDate)
                                           RETURNING Id;
                                           """;

    protected override string UpdateSql => """
                                           UPDATE Loans
                                           SET BookId = @BookId, MemberId = @MemberId, LoanDate = @LoanDate,
                                               DueDate = @DueDate, ReturnDate = @ReturnDate
                                           WHERE Id = @Id
                                           """;

    public List<Loan> GetActiveByMember(int memberId)
    {
        using var connection = Factory.Create();
        return connection.Query<Loan>(
            "SELECT * FROM Loans WHERE MemberId = @memberId AND ReturnDate IS NULL",
            new { memberId }).ToList();
    }

    public List<Loan> GetOverdue()
    {
        using var connection = Factory.Create();
        return connection.Query<Loan>(
            "SELECT * FROM Loans WHERE ReturnDate IS NULL AND DueDate < @now",
            new { now = DateTime.UtcNow }).ToList();
    }
}