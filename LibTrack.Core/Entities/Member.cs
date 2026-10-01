using LibTrack.Core.Common;

namespace LibTrack.Core.Entities;

public class Member : Entity
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Mobile { get; set; }
    public DateTime JoinDate { get; set; }
}