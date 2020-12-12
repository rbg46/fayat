using System;
using System.Collections.Generic;
using System.Linq;
using Fred.Entities.Rapport;
using Fred.Entities.Societe;
using Fred.ImportExport.Business.Pointage.PointageEtl.Parameter;
using Fred.ImportExport.Business.Pointage.PointageEtl.Process;
using Fred.ImportExport.Business.Pointage.PointageEtl.Settings;

namespace Fred.ImportExport.Business.Pointage.PointageEtl
{
    /// <summary>
    /// Factory d'etl. Un etl est créer si le fichier de mapping a une entree avec un CodeSocieteStorm correspondant 
    /// à la societe du rapport.
    /// </summary>
    public class PointageEtlFactory
    {
        private readonly PointageEtlDependenciesWrapper dependenciesWrapper;

        public PointageEtlFactory(PointageEtlDependenciesWrapper dependenciesWrapper)
        {
            this.dependenciesWrapper = dependenciesWrapper;

        }


        public IPointageProcess GetEtl(int rapportId, string backgroundJobId)
        {
            IPointageProcess etl = null;

            // je recupere le rapport de la base
            var dbRapport = dependenciesWrapper.FredIePointageFluxService.FindByRapportId(rapportId);
            if (dbRapport != null)
            {
                // je recupere le l'organisationId du ci
                var dbCiOrguanisationId = dependenciesWrapper.CIManager.GetOrganisationIdByCiId(dbRapport.CiId);
                if (dbCiOrguanisationId.HasValue)
                {
                    //je recupere la societe du ci
                    var societe = dependenciesWrapper.SocieteManager.GetSocieteParentByOrgaId(dbCiOrguanisationId.Value);
                    if (societe != null)
                    {
                        etl = new BasePointageProcess();

                        var parameter = new EtlPointageParameter()
                        {
                            RapportId = rapportId,
                            CodeSocieteStorm = societe.CodeSocieteStorm,
                            SocieteId = societe.SocieteId,
                            SocieteLibelle = societe.Libelle,
                            LogPrefix = "[POINTAGE][FRED TO BRIDGE]",
                            AuteurId = dbRapport.AuteurVerrouId ?? dbRapport.AuteurModificationId ?? dbRapport.AuteurCreationId.Value,
                            EtlDependencies = dependenciesWrapper,
                            BackgroundJobId = backgroundJobId
                        };
                        etl.Init(parameter);
                    }
                }
            }
            return etl;
        }

        public List<IPointageProcess> GetEtl(List<int> rapportIds, string backgroundJobId)
        {
            List<IPointageProcess> etls = new List<IPointageProcess>();
            try
            {
                var firstRapport = dependenciesWrapper.FredIePointageFluxService.FindByRapportId(rapportIds.First());
                var auteurId = firstRapport.AuteurVerrouId ?? firstRapport.AuteurModificationId ?? firstRapport.AuteurCreationId.Value;
                Dictionary<int, SocieteEnt> societes = GetSocietes(rapportIds);

                foreach (int rapportId in rapportIds)
                {
                    SocieteEnt societe = societes[rapportId];
                    if (societe != null)
                    {
                        IPointageProcess etl = new BasePointageProcess();

                        var parameter = new EtlPointageParameter()
                        {
                            RapportId = rapportId,
                            CodeSocieteStorm = societe.CodeSocieteStorm,
                            SocieteId = societe.SocieteId,
                            SocieteLibelle = societe.Libelle,
                            LogPrefix = "[POINTAGE][FRED TO BRIDGE]",
                            AuteurId = auteurId,
                            EtlDependencies = dependenciesWrapper,
                            BackgroundJobId = backgroundJobId
                        };
                        etl.Init(parameter);
                        etls.Add(etl);
                    }
                }
            }
            catch (Exception e)
            {
                NLog.LogManager.GetCurrentClassLogger().Error(e, e.Message);
            }
            return etls;
        }

        /// <summary>
        /// Récupération des sociétés des rapports 
        /// /!\ TSA: cette fonction peut poser des problèmes de perf, il faudra la revoir... /!\
        /// /!\ même méthode utilisée dans ExportMaterielToSapManager.cs
        /// </summary>
        /// <param name="rapportIds">Liste des identifiants rapports</param>
        /// <returns>Dictionnaire RapportId, SocieteEnt</returns>
        private Dictionary<int, SocieteEnt> GetSocietes(List<int> rapportIds)
        {
            Dictionary<int, SocieteEnt> dico = new Dictionary<int, SocieteEnt>();
            int? orgaId = null;

            // Je récupère et boucle sur tous les Rapports
            foreach (RapportEnt r in dependenciesWrapper.FredIePointageFluxService.GetRapportList(rapportIds).ToList())
            {
                // Je récupère une à une l'organisation du CI du rapport (peut mieux faire...)
                orgaId = dependenciesWrapper.CIManager.GetOrganisationIdByCiId(r.CiId);

                if (orgaId.HasValue)
                {
                    // Je récupère une à une la société du CI du rapport (via organisationId du CI) (peut mieux faire...)
                    SocieteEnt societe = dependenciesWrapper.SocieteManager.GetSocieteParentByOrgaId(orgaId.Value);
                    dico.Add(r.RapportId, societe);
                }
            }

            return dico;
        }
    }
}
