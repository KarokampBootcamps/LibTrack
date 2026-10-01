//
// // ---------- Composition root ----------
// var config = new ConfigurationBuilder()
//     .SetBasePath(AppContext.BaseDirectory)
//     .AddJsonFile("appsettings.json", optional: true)
//     .Build();
//
// string? connectionString = config.GetConnectionString("LibraTrack");

using LibTrack.Core.Entities;
using LibTrack.Core.Exceptions;
using LibTrack.Core.Interfaces;
using LibTrack.Core.Services;
using LibTrack.Data.InMemory.Repositories;
using Spectre.Console;

IBookRepository bookRepo = new BookRepository();
IMemberRepository memberRepo = new MemberRepository();
ILoanRepository loanRepo = new LoanRepository();

// Session 3+: swap these three lines for the SQL versions —
// nothing below this point changes.
// IBookRepository bookRepo = new SqlBookRepository(connectionString!);
// IMemberRepository memberRepo = new SqlMemberRepository(connectionString!);
// ILoanRepository loanRepo = new SqlLoanRepository(connectionString!);

var libraryService = new LibraryService(bookRepo, memberRepo, loanRepo);
// libraryService.LoanOverdue += loan =>
//     Console.WriteLine($"  [!] Loan #{loan.Id} is overdue (was due {loan.DueDate:d}).");

// ---------- Menu loop ----------
var running = true;
while (running)
{
    PrintMenu();
    var choice = Console.ReadLine();
    Console.WriteLine();

    try
    {
        switch (choice)
        {
            case "1": AddBook(bookRepo); break;
            case "2": ListBooks(bookRepo); break;
            case "3": AddMember(memberRepo); break;
            case "4": ListMembers(memberRepo); break;
            case "5": BorrowBook(libraryService); break;
            // case "6": ReturnBook(libraryService); break;
            case "7": libraryService.CheckOverdue(); break;
            case "0": running = false; break;
            default: AnsiConsole.MarkupLine("[yellow]Unknown option.[/]"); break;        }
    }
    catch (BookNotAvailableException ex)
    {
        Console.WriteLine($"  [Business rule] {ex.Message}");
    }
    catch (InvalidOperationException ex)
    {
        Console.WriteLine($"  [Not found] {ex.Message}");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"  [Unexpected error] {ex.Message}");
    }

    Console.WriteLine();
}


void PrintMenu()
{
    var menu = new Table()
        .Border(TableBorder.Rounded)
        .BorderColor(Color.Grey)
        .Title("[bold deepskyblue1]LibraTrack[/]")
        .HideHeaders()
        .AddColumn("")
        .AddColumn("");

    menu.AddRow("[bold green]1[/]  Add book",      "[bold green]2[/]  List books");
    menu.AddRow("[bold green]3[/]  Add member",    "[bold green]4[/]  List members");
    menu.AddRow("[bold green]5[/]  Borrow book",   "[bold green]6[/]  Return book");
    menu.AddRow("[bold green]7[/]  Check overdue", "[bold red]0[/]  [red]Exit[/]");

    AnsiConsole.Write(menu);
    AnsiConsole.Markup("[bold deepskyblue1]>[/] ");
}

// ---------- Menu actions ----------

void AddBook(IBookRepository bookRepo)
{
    var title = Ask("Title");
    var isbn = Ask("ISBN");
    var author = Ask("Author Id");
    var total = ReadInt("Total copies");

    bookRepo.Add(new Book
    {
        Title = title,
        Isbn = isbn,
        Author = author,
        TotalCopies = total,
        AvailableCopies = total
    });

    AnsiConsole.MarkupLine("[green]✔ Book added.[/]");
}

void ListBooks(IBookRepository bookRepo)
{
    var table = new Table().Border(TableBorder.Rounded).BorderColor(Color.Grey);
    table.AddColumn("[bold]#[/]");
    table.AddColumn("[bold]Title[/]");
    table.AddColumn(new TableColumn("[bold]Available[/]").RightAligned());

    foreach (var b in bookRepo.GetAll())
    {
        var color = b.AvailableCopies > 0 ? "green" : "red";
        table.AddRow(
            b.Id.ToString(),
            Markup.Escape(b.Title),
            $"[{color}]{b.AvailableCopies}/{b.TotalCopies}[/]");
    }

    AnsiConsole.Write(table);
}

void AddMember(IMemberRepository memberRepo)
{
    var name = Ask("Name");
    var mobile = Ask("Mobile");

    memberRepo.Add(new Member { Name = name, Mobile = mobile });
    AnsiConsole.MarkupLine("[green]✔ Member added.[/]");
}

void ListMembers(IMemberRepository memberRepo)
{
    var table = new Table().Border(TableBorder.Rounded).BorderColor(Color.Grey);
    table.AddColumn("[bold]#[/]");
    table.AddColumn("[bold]Name[/]");
    table.AddColumn("[bold]Mobile[/]");

    foreach (var m in memberRepo.GetAll())
        table.AddRow(m.Id.ToString(), Markup.Escape(m.Name), Markup.Escape(m.Mobile));

    AnsiConsole.Write(table);
}

void BorrowBook(LibraryService service)
{
    var bookId = ReadInt("Book Id: ");
    var memberId = ReadInt("Member Id: ");

    var loan = service.BorrowBook(bookId, memberId);
    Console.WriteLine($"Loan #{loan.Id} created. Due {loan.DueDate:d}.");
}

// void ReturnBook(LibraryService service
// {
//     var loanId = ReadInt("Loan Id: ");
//     service.ReturnBook(loanId);
//     Console.WriteLine("Book returned.");
// }

// ---------- Small input helper ----------
// Prevents a live demo from crashing on a mistyped number.
string Ask(string label)
{
    AnsiConsole.Markup($"[cyan]{label}:[/] ");
    return Console.ReadLine() ?? "";
}

int ReadInt(string label)
{
    while (true)
    {
        AnsiConsole.Markup($"[cyan]{label}:[/] ");
        if (int.TryParse(Console.ReadLine(), out var value))
            return value;

        AnsiConsole.MarkupLine("[red]  Please enter a valid number.[/]");
    }
}
