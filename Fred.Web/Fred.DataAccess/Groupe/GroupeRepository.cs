using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fred.DataAccess.Common;
using Fred.DataAccess.Interfaces;
using Fred.Entities.Groupe;
using Fred.EntityFramework;
using Fred.Framework;
using Microsoft.EntityFrameworkCore;

namespace Fred.DataAccess.Groupe
{
    public class GroupeRepository : FredRepository<GroupeEnt>, IGroupeRepository
    {
        private readonly FredDbContext context;
        private readonly IOrganisationRepository userRepo;

        public GroupeRepository(FredDbContext context, ILogManager logMgr, IOrganisationRepository userRepo)
          : base(context)
        {
            this.context = context;
            this.userRepo = userRepo;
        }

        public override void CheckAccessToEntity(GroupeEnt entity, int userId)
        {
            if (entity.Organisation == null)
            {
                PerformEagerLoading(entity, x => x.Organisation);
            }
            if (entity.Organisation != null)
            {
                var orgaList = userRepo.GetOrganisationsAvailable(null, new List<int> { entity.Organisation.TypeOrganisationId }, userId);
                if (orgaList.All(o => o.OrganisationId != entity.Organisation.OrganisationId))
                {
                    throw new UnauthorizedAccessException();
                }
            }
        }

        public GroupeEnt GetGroupeByCode(string code)
        {
            return context.Groupes.FirstOrDefault(x => x.Code.Equals(code));
        }

        public async Task<int> GetGroupeIdByCodeAsync(string code)
        {
            return await context.Groupes.Where(x => x.Code == code).Select(x => x.GroupeId).SingleOrDefaultAsync();
        }

        public async Task<GroupeEnt> GetGroupebySocieteIdAsync(int societeId)
        {
            return await (from g in context.Groupes
                          join s in context.Societes on g.GroupeId equals s.GroupeId
                          where s.SocieteId == societeId
                          select g)
                       .FirstOrDefaultAsync();
        }

        public GroupeEnt GetGroupeByCodeIncludeSocietes(string code)
        {
            return Query()
                .Filter(g => g.Code == code)
                .Include(g => g.Societes)
                .Get()
                .FirstOrDefault();
        }

        public GroupeEnt GetGroupeByCodeSocieteComptableOfSociete(string codeSocieteComptable)
        {
            return (from s in context.Societes
                    where s.CodeSocieteComptable == codeSocieteComptable
                    from g in context.Groupes
                    where s.GroupeId == g.GroupeId
                    select g)
                   .AsNoTracking()
                   .SingleOrDefault();
        }

        public async Task<string> GetGroupCodeByCompanyIdAsync(int companyId)
        {
            return await context.Societes.Where(s => s.SocieteId == companyId).Select(s => s.Groupe.Code).SingleOrDefaultAsync();
        }

        public async Task<string> GetGroupCodeByReportIdAsync(int reportId)
        {
            return await context.Rapports.Where(r => r.RapportId == reportId).Select(r => r.CI.Societe.Groupe.Code).SingleOrDefaultAsync();
        }

        public async Task<string> GetGroupCodeByReportIdsAsync(IEnumerable<int> reportIds)
        {
            return await context.Rapports.Where(r => reportIds.Contains(r.RapportId)).Select(r => r.CI.Societe.Groupe.Code).Distinct().SingleOrDefaultAsync();
        }

        public async Task<string> GetGroupCodeByStaffIdsAsync(IEnumerable<int> staffIds)
        {
            return await context.Personnels.Where(p => staffIds.Contains(p.PersonnelId)).Select(p => p.Societe.Groupe.Code).Distinct().SingleOrDefaultAsync();
        }

        public async Task<string> GetGroupCodeByCiIdsAsync(IEnumerable<int> ciIds)
        {
            return await context.CIs.Where(c => ciIds.Contains(c.CiId)).Select(c => c.Societe.Groupe.Code).Distinct().SingleOrDefaultAsync();
        }

        public async Task<string> GetGroupCodeByAccountingCompanyCodeAsync(string accountingCompanyCode)
        {
            return await context.Societes.Where(s => s.CodeSocieteComptable == accountingCompanyCode).Select(s => s.Groupe.Code).SingleOrDefaultAsync();
        }

        public async Task<string> GetGroupCodeByPayrollCompanyCodeAsync(string payrollCompanyCode)
        {
            return await context.Societes.Where(s => s.CodeSocietePaye == payrollCompanyCode).Select(s => s.Groupe.Code).SingleOrDefaultAsync();
        }

        public async Task<string> GetGroupCodeByOrderIdAsync(int orderId)
        {
            return await context.Commandes.Where(s => s.CommandeId == orderId).Select(s => s.CI.Societe.Groupe.Code).SingleOrDefaultAsync();
        }
    }
}
