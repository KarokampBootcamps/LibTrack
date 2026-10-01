using LibTrack.Core.Entities;

namespace LibTrack.Data.InMemory;

public static class ApplicationDbContext
{
    public static List<Book> Books { get; set; } = [];
    public static List<Member> Members { get; set; } = [];
    public static List<Loan> Loans { get; set; } = [];
    public static List<Author> Authors { get; set; } = [];
}