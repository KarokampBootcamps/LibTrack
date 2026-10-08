// LibTrack.Data.Sql/Repositories/SqlMemberRepository.cs
using LibTrack.Core.Entities;
using LibTrack.Core.Interfaces;

namespace LibTrack.Data.Sql.Repositories;

public class SqlMemberRepository : BaseRepository<Member>, IMemberRepository
{
    public SqlMemberRepository(SqliteConnectionFactory factory) : base(factory) { }

    protected override string TableName => "Members";

    protected override string InsertSql => """
                                           INSERT INTO Members (Name, Mobile, JoinDate)
                                           VALUES (@Name, @Mobile, @JoinDate)
                                           RETURNING Id;
                                           """;

    protected override string UpdateSql => """
                                           UPDATE Members
                                           SET Name = @Name, Mobile = @Mobile, JoinDate = @JoinDate
                                           WHERE Id = @Id
                                           """;
}