using LibTrack.Core.Common;

namespace LibTrack.Core.Entities;

public class Book : Entity
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Isbn { get; set; }
    public string Author { get; set; }
    
    public int AvailableCopies { get; set; }
    public int TotalCopies { get; set; }
}