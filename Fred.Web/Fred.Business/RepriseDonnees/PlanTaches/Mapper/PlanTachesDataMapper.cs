using System;
using System.Collections.Generic;
using System.Linq;
using Fred.Business.Organisation.Tree;
using Fred.Business.RepriseDonnees.Common.Selector;
using Fred.Business.RepriseDonnees.PlanTaches.Models;
using Fred.Business.RepriseDonnees.PlanTaches.Selector;
using Fred.Entities.Organisation.Tree;
using Fred.Entities.Referential;
using Fred.Entities.RepriseDonnees.Excel;

namespace Fred.Business.RepriseDonnees.PlanTaches.Mapper
{
    /// <summary>
    /// Transform les données du fichier excel en plan de taches
    /// </summary>
    public class PlanTachesDataMapper : IPlanTachesDataMapper
    {
        /// <summary>
        /// Creer un Plan de tâches d'une liste de RepriseExcelPlanTaches
        /// </summary>
        /// <param name="context">context contenant les data necessaire a l'import</param>
        /// <param name="listRepriseExcelPlanTaches">le Plan de tâches sous la forme excel</param>
        /// <returns>Liste de Taches</returns>
        public List<TacheEnt> Transform(ContextForImportPlanTaches context, List<RepriseExcelPlanTaches> listRepriseExcelPlanTaches)
        {
            List<TacheExcelMapResult> mappage = new List<TacheExcelMapResult>();

            // 1- Je créer les taches et l'associe au RepriseExcelPlanTaches dans une classe de mapping
            foreach (RepriseExcelPlanTaches repriseExcelPlanTache in listRepriseExcelPlanTaches)
            {
                TacheEnt tache = MapTache(context, repriseExcelPlanTache, listRepriseExcelPlanTaches);

                mappage.Add(new TacheExcelMapResult()
                {
                    Tache = tache,
                    RepriseExcelPlanTache = repriseExcelPlanTache
                });
            }

            // 2- J'affecte les enfants au parent
            foreach (TacheExcelMapResult map in mappage)
            {
                List<TacheExcelMapResult> children = GetChildren(map, mappage);
                if (children.Count > 0)
                {
                    TacheEnt tache = map.Tache;
                    tache.TachesEnfants = children.Select(x => x.Tache).ToList();
                }
            }

            return mappage.Select(x => x.Tache).ToList();
        }

        /// <summary>
        /// Transforme une repriseExcelPlanTache en TacheEnt
        /// </summary>
        /// <param name="context">ContextForImportPlanTaches</param>
        /// <param name="repriseExcelPlanTaches">RepriseExcelPlanTaches</param>
        /// <param name="listRepriseExcelPlanTaches">Le plan de tâche sous forme Excel</param>
        /// <returns>Un tacheEnt</returns>
        private TacheEnt MapTache(ContextForImportPlanTaches context, RepriseExcelPlanTaches repriseExcelPlanTaches, List<RepriseExcelPlanTaches> listRepriseExcelPlanTaches)
        {
            TacheLevelSelector niveauSelector = new TacheLevelSelector();
            CommonFieldSelector commonSelector = new CommonFieldSelector();
            TacheEnt tache = new TacheEnt();
            tache.Code = repriseExcelPlanTaches.CodeTache;
            tache.Libelle = repriseExcelPlanTaches.LibelleTache;
            // La validation doit eviter les nullreferenceexception ci dessous
            tache.TacheParDefaut = commonSelector.GetBoolean(repriseExcelPlanTaches.T3ParDefaut);
            tache.Niveau = niveauSelector.GetTacheNiveau(repriseExcelPlanTaches.NiveauTache).Value;
            tache.Active = true;
            tache.AuteurCreationId = context.FredIeUser.UtilisateurId;
            tache.AuteurModification = null;
            tache.AuteurSuppressionId = null;
            tache.DateCreation = DateTime.UtcNow;
            tache.DateModification = null;
            tache.DateSuppression = null;
            // La validation doit eviter les nullreferenceexception ci dessous
            tache.CiId = commonSelector.GetCiOfDatabase(context.OrganisationTree, context.GroupeId, repriseExcelPlanTaches.CodeSociete, repriseExcelPlanTaches.CodeCi, context.CisUsedInExcel).CiId;
            tache.QuantiteBase = null;
            tache.PrixTotalQB = null;
            tache.PrixUnitaireQB = null;
            tache.TotalHeureMO = null;
            tache.HeureMOUnite = null;
            tache.QuantiteARealise = null;
            tache.NbrRessourcesToParam = null;
            tache.TacheType = 0;
            tache.BudgetId = null;
            // Pour les Taches ayant des Parents (hors niveau T1 donc), dans le cas ou le Parent est en BDD (mais pas dans le Excel) alors on le renseigne :
            if (!string.Equals(repriseExcelPlanTaches.NiveauTache.Trim(), "T1", StringComparison.OrdinalIgnoreCase)
                && !listRepriseExcelPlanTaches.Any(x => x.CodeCi == repriseExcelPlanTaches.CodeCi
                                                            && x.CodeSociete == repriseExcelPlanTaches.CodeSociete
                                                            && x.CodeTache == repriseExcelPlanTaches.CodeTacheParent))
            {
                int ciId = GetCiIdByCodeCi(context.GroupeId, repriseExcelPlanTaches.CodeSociete, repriseExcelPlanTaches.CodeCi, context.OrganisationTree);

                TacheEnt tacheParentFromBDD = context.TachesParentsUsedInExcel.Find(x => x.Code == repriseExcelPlanTaches.CodeTacheParent
                                                && x.CiId == ciId);

                tache.ParentId = tacheParentFromBDD?.TacheId;
            }
            return tache;
        }

        /// <summary>
        /// Recherche les enfants.
        /// Il est necessaire d'avoir la tache et la RepriseExcelPlanTaches associées pour pouvoir effectuer la recheche des enfants.
        /// </summary>
        /// <param name="parent">parent TacheExcelMapResult</param>
        /// <param name="allTaches">Tous les mappping pour faire la recheche des enfants</param>
        /// <returns>La liste des enfants du parent passé en parametres</returns>
        private List<TacheExcelMapResult> GetChildren(TacheExcelMapResult parent, List<TacheExcelMapResult> allTaches)
        {
            TacheLevelSelector niveauSelector = new TacheLevelSelector();

            return allTaches.Where(child => child.RepriseExcelPlanTache.CodeTacheParent == parent.RepriseExcelPlanTache.CodeTache
                && parent.Tache.CiId == child.Tache.CiId
                && niveauSelector.GetTacheNiveau(child.RepriseExcelPlanTache.NiveauTache) == niveauSelector.GetTacheNiveau(parent.RepriseExcelPlanTache.NiveauTache) + 1)
                .ToList();
        }

        private int GetCiIdByCodeCi(int groupeId, string codeSociete, string codeCi, OrganisationTree organisationTree)
        {
            int result = 0;

            List<OrganisationBase> societesOfGroupe = organisationTree.GetAllSocietesForGroupe(groupeId);

            OrganisationBase societe = societesOfGroupe.Find(s => s.Code == codeSociete);

            if (societe != null)
            {
                List<OrganisationBase> cisOfSociete = organisationTree.GetAllCisOfSociete(societe.Id);

                result = cisOfSociete.Where(x => x.Code == codeCi).Select(x => x.Id).FirstOrDefault();
            }

            return result;
        }
    }
}
