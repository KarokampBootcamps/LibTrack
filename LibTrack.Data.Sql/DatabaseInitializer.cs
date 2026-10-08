using Dapper;

namespace LibTrack.Data.Sql;

public class DatabaseInitializer
{
    private readonly SqliteConnectionFactory _factory;

    public DatabaseInitializer(SqliteConnectionFactory factory)
    {
        _factory = factory;
    }

    public void Initialize()
    {
        using var connection = _factory.Create();

        connection.Execute("""
                           CREATE TABLE IF NOT EXISTS Books (
                               Id              INTEGER PRIMARY KEY AUTOINCREMENT,
                               Title           TEXT    NOT NULL,
                               Isbn            TEXT    NOT NULL,
                               Author          TEXT    NOT NULL,
                               AvailableCopies INTEGER NOT NULL,
                               TotalCopies     INTEGER NOT NULL,
                               CONSTRAINT CK_Books_Copies
                                   CHECK (AvailableCopies >= 0 AND AvailableCopies <= TotalCopies)
                           );
                           CREATE UNIQUE INDEX IF NOT EXISTS IX_Books_Isbn ON Books (Isbn);

                           CREATE TABLE IF NOT EXISTS Members (
                               Id       INTEGER PRIMARY KEY AUTOINCREMENT,
                               Name     TEXT NOT NULL,
                               Mobile   TEXT NOT NULL,
                               JoinDate TEXT NOT NULL
                           );
                           CREATE UNIQUE INDEX IF NOT EXISTS IX_Members_Mobile ON Members (Mobile);

                           CREATE TABLE IF NOT EXISTS Loans (
                               Id         INTEGER PRIMARY KEY AUTOINCREMENT,
                               BookId     INTEGER NOT NULL,
                               MemberId   INTEGER NOT NULL,
                               LoanDate   TEXT    NOT NULL,
                               DueDate    TEXT    NOT NULL,
                               ReturnDate TEXT    NULL,
                               FOREIGN KEY (BookId)   REFERENCES Books (Id)   ON DELETE RESTRICT,
                               FOREIGN KEY (MemberId) REFERENCES Members (Id) ON DELETE RESTRICT
                           );
                           CREATE INDEX IF NOT EXISTS IX_Loans_MemberId_ReturnDate
                               ON Loans (MemberId, ReturnDate);
                           """);
    }
}