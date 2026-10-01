using LibTrack.Core.Common;
using LibTrack.Core.Entities;

namespace LibTrack.Core.Interfaces;

public interface ILoanRepository : IRepository<Loan>
{
    // IEnumerable<Loan> GetActiveLoans();
    // IEnumerable<Loan> GetOverdueLoans();
}