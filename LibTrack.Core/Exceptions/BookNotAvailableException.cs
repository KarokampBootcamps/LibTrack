namespace LibTrack.Core.Exceptions;

public class BookNotAvailableException : Exception
{
    public BookNotAvailableException(string title) : base($"'{title}' has no available copies.") { }
}