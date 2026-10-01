using LibTrack.Core.Common;
using LibTrack.Core.Entities;

namespace LibTrack.Core.Interfaces;

public interface IBookRepository : IRepository<Book>
{
    IEnumerable<Book> Search(string keyword);
}