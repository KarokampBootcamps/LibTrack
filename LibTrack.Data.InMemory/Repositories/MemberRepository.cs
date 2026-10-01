using LibTrack.Core.Entities;
using LibTrack.Core.Interfaces;

namespace LibTrack.Data.InMemory.Repositories;

public class MemberRepository : IMemberRepository
{
    private int _nextId = 1;
    
    public Member? Get(int id)
    {
        return ApplicationDbContext.Members.FirstOrDefault(b => b.Id == id);
    }

    public List<Member> GetAll()
    {
        return ApplicationDbContext.Members.ToList();
    }

    public void Add(Member entity)
    {
        entity.Id = _nextId++;
        ApplicationDbContext.Members.Add(entity);
    }

    public void Update(Member entity)
    {
        var member = ApplicationDbContext.Members.FirstOrDefault(b => b.Id == entity.Id);
        
        if (member is null)
            throw new ArgumentNullException(nameof(member));

        member = entity;
    }

    public void Remove(Member entity)
    {
        ApplicationDbContext.Members.Remove(entity);
    }

    public IEnumerable<Member> Search(string keyword)
    {
        throw new NotImplementedException();
    }
}