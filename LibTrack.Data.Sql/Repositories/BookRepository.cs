// LibTrack.Data.Sql/Repositories/SqlBookRepository.cs
using Dapper;
using LibTrack.Core.Entities;
using LibTrack.Core.Interfaces;

namespace LibTrack.Data.Sql.Repositories;

public class SqlBookRepository : BaseRepository<Book>, IBookRepository
{
    public SqlBookRepository(SqliteConnectionFactory factory) : base(factory) { }

    protected override string TableName => "Books";

    protected override string InsertSql => """
                                           INSERT INTO Books (Title, Isbn, Author, AvailableCopies, TotalCopies)
                                           VALUES (@Title, @Isbn, @Author, @AvailableCopies, @TotalCopies)
                                           RETURNING Id;
                                           """;

    protected override string UpdateSql => """
                                           UPDATE Books
                                           SET Title = @Title, Isbn = @Isbn, Author = @Author,
                                               AvailableCopies = @AvailableCopies, TotalCopies = @TotalCopies
                                           WHERE Id = @Id
                                           """;

    public IEnumerable<Book> Search(string keyword)
    {
        using var connection = Factory.Create();
        return connection.Query<Book>("""
                                      SELECT * FROM Books
                                      WHERE Title LIKE @pattern OR Author LIKE @pattern OR Isbn LIKE @pattern
                                      """,
            new { pattern = $"%{keyword}%" }).ToList();
    }
}