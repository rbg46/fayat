using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Fred.Business.Groupe;
using Fred.Business.Societe;
using Fred.Entities;
using Fred.Entities.Groupe;
using Fred.Entities.Societe;
using Fred.ImportExport.Models.Groupe;

namespace Fred.ImportExport.Business.Groupe
{
    public class GroupeInterimaireManager : IGroupeInterimaireManager
    {
        private readonly ISocieteManager societeManager;
        private readonly IGroupeManager groupeManager;
        private readonly IMapper mapper;

        public GroupeInterimaireManager(ISocieteManager societeManager, IMapper mapper, IGroupeManager groupeManager)
        {
            this.societeManager = societeManager;
            this.groupeManager = groupeManager;
            this.mapper = mapper;
        }

        public List<GroupeInterimaireModel> GetGroupeInterimaire()
        {
            var groups = new List<GroupeEnt>();

            GroupeEnt rzbGroup = groupeManager.GetGroupeByCode(Constantes.CodeGroupeRZB);
            if (rzbGroup != null)
            {
                int rzbGroupId = rzbGroup.GroupeId;
                rzbGroup.Societes = new List<SocieteEnt>();

                SocieteEnt rzbCompany = societeManager.GetSocieteByCodeAndGroupeId(Constantes.CodeRazelBec, rzbGroupId);
                if (rzbCompany != null && !rzbGroup.Societes.Any(s => s.SocieteId == rzbCompany.SocieteId))
                {
                    rzbGroup.Societes.Add(rzbCompany);
                }

                SocieteEnt moulinCompany = societeManager.GetSocieteByCodeAndGroupeId(Constantes.CodeMoulinBTP, rzbGroupId);
                if (moulinCompany != null && !rzbGroup.Societes.Any(s => s.SocieteId == moulinCompany.SocieteId))
                {
                    rzbGroup.Societes.Add(moulinCompany);
                }

                groups.Add(rzbGroup);
            }

            GroupeEnt ftpGroup = groupeManager.GetGroupeByCodeIncludeSocietes(Constantes.CodeGroupeFTP);
            if (ftpGroup == null)
            {
                return mapper.Map<List<GroupeInterimaireModel>>(groups);
            }

            SocieteEnt interimaireCompany = ftpGroup.Societes.FirstOrDefault(s => s.IsInterimaire);
            if (interimaireCompany != null)
            {
                ftpGroup.Societes.Remove(interimaireCompany);
            }

            groups.Add(ftpGroup);

            return mapper.Map<List<GroupeInterimaireModel>>(groups);
        }
    }
}
