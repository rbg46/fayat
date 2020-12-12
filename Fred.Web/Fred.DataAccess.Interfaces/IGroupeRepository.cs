using System.Collections.Generic;
using System.Threading.Tasks;
using Fred.Entities.Groupe;

namespace Fred.DataAccess.Interfaces
{
    public interface IGroupeRepository : IFredRepository<GroupeEnt>
    {
        GroupeEnt GetGroupeByCode(string code);
        Task<int> GetGroupeIdByCodeAsync(string code);
        Task<GroupeEnt> GetGroupebySocieteIdAsync(int societeId);
        GroupeEnt GetGroupeByCodeIncludeSocietes(string code);
        GroupeEnt GetGroupeByCodeSocieteComptableOfSociete(string codeSocieteComptable);
        Task<string> GetGroupCodeByCompanyIdAsync(int companyId);
        Task<string> GetGroupCodeByReportIdAsync(int reportId);
        Task<string> GetGroupCodeByReportIdsAsync(IEnumerable<int> reportIds);
        Task<string> GetGroupCodeByStaffIdsAsync(IEnumerable<int> staffIds);
        Task<string> GetGroupCodeByCiIdsAsync(IEnumerable<int> ciIds);
        Task<string> GetGroupCodeByAccountingCompanyCodeAsync(string accountingCompanyCode);
        Task<string> GetGroupCodeByPayrollCompanyCodeAsync(string payrollCompanyCode);
        Task<string> GetGroupCodeByOrderIdAsync(int orderId);
    }
}
