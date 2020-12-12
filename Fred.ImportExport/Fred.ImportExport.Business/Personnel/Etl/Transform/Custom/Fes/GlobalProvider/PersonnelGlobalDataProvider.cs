using System.Collections.Generic;
using System.Linq;
using Fred.Business.Personnel;
using Fred.Entities.Affectation;
using Fred.Entities.CI;
using Fred.Entities.Personnel;
using Fred.Entities.Referential;
using Fred.Entities.ReferentielFixe;
using Fred.Entities.Societe;
using Fred.Entities.Utilisateur;
using Fred.Framework.Exceptions;
using Fred.ImportExport.Business.Personnel.Etl.Process;

namespace Fred.ImportExport.Business.Personnel.Etl.Transform.Custom.Fes
{
    /// <summary>
    /// Classe qui recupere toutes les information necessaires pour effectué l'import du personnel
    /// </summary>
    public class PersonnelGlobalDataProvider
    {
        private readonly IImportPersonnelManager importPersonnelFesManager;
        private readonly PersonnelEtlParameter parameter;

        public PersonnelGlobalDataProvider(PersonnelEtlParameter parameter, IImportPersonnelManager importPersonnelFesManager)
        {
            this.importPersonnelFesManager = importPersonnelFesManager;
            this.parameter = parameter;
        }

        public ImportPersonnelsGlobalData GetGlobalData(List<string> codeSocietePaies, IEnumerable<string> ciCodes)
        {
            parameter.Logger.LogStartRequestAllData();

            var societes = importPersonnelFesManager.GetSocieteList().ToList();

            IEnumerable<SocieteEnt> societesWithCodeSocietePaies = societes.Where(s => codeSocietePaies.Contains(s.CodeSocietePaye));
            if (!societesWithCodeSocietePaies.Any())
            {
                string error = parameter.Logger.ErrorNoSocieteWithCodeSocietePaie(codeSocietePaies);
                throw new FredBusinessException(error);
            }

            IEnumerable<int> groupesIds = societesWithCodeSocietePaies.Select(s => s.GroupeId).Distinct();
            if (groupesIds.Count() > 1)
            {
                string error = parameter.Logger.ErrorDifferentGroupe(codeSocietePaies);
                throw new FredBusinessException(error);
            }
            if (societesWithCodeSocietePaies.Count() != codeSocietePaies.Count)
            {
                string error = parameter.Logger.ErrorSocietesMissing(codeSocietePaies);
                throw new FredBusinessException(error);
            }

            SocieteEnt firstSociete = societesWithCodeSocietePaies.FirstOrDefault();
            List<RessourceEnt> ressourcesForGroupe = this.importPersonnelFesManager.GetRessourceListByGroupeId(firstSociete.GroupeId).ToList();

            var societeIds = societesWithCodeSocietePaies.Select(s => s.SocieteId).ToList();

            List<EtablissementPaieEnt> etablissementPaiesForAllSocietes = this.importPersonnelFesManager.GetEtablissementPaiesBySocieteIds(societeIds).ToList();

            List<PersonnelEnt> fredPersonnelsForAllSocietes = this.importPersonnelFesManager.GetPersonnelListBySocieteIds(societeIds).ToList();

            List<AffectationEnt> affectations = this.importPersonnelFesManager.GetDefaultAffectations().ToList();

            List<CIEnt> cis = this.importPersonnelFesManager.GetCIsByCodes(ciCodes).ToList();//filtre sur la societe est fait plus bas

            List<EtablissementComptableEnt> etablissementComptablessForAllSocietes = this.importPersonnelFesManager.GetEtablissementComptablesBySocieteIds(societeIds).ToList();

            UtilisateurEnt fredIe = importPersonnelFesManager.GetFredIe();

            ImportPersonnelsGlobalData result = new ImportPersonnelsGlobalData
            {
                FredIe = fredIe,
                RessourcesForGroupe = ressourcesForGroupe,
                Affectations = affectations,
                SocietesData = new List<ImportPersonnelsSocieteData>(),
            };

            foreach (var societeId in societeIds)
            {
                ImportPersonnelsSocieteData societesData = new ImportPersonnelsSocieteData
                {
                    Societe = societesWithCodeSocietePaies.FirstOrDefault(s => s.SocieteId == societeId),
                    EtablissementPaiesForSociete = etablissementPaiesForAllSocietes.Where(e => e.SocieteId.HasValue && e.SocieteId.Value == societeId).ToList(),
                    FredPersonnelsForSociete = fredPersonnelsForAllSocietes.Where(p => p.SocieteId.HasValue && p.SocieteId.Value == societeId).ToList(),
                    Cis = cis.Where(c => c.SocieteId.HasValue && c.SocieteId.Value == societeId).ToList(),
                    EtablissementComptablesForSociete = etablissementComptablessForAllSocietes.Where(e => e.SocieteId.HasValue && e.SocieteId.Value == societeId).ToList()
                };
                result.SocietesData.Add(societesData);
            }

            return result;
        }
    }
}
