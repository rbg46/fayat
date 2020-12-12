using System;
using System.Collections.Generic;
using System.Linq;
using Fred.Business.ExplorateurDepense;
using Fred.Entities;
using Fred.Entities.DatesClotureComptable;
using Fred.Entities.Personnel.Interimaire;
using Fred.Entities.Valorisation;
using Fred.Web.Shared.Models.Depense;

namespace Fred.Business.Depense.Converter
{
    /// <summary>
    /// Permet de convertir un ValorisationEnt en DepenseExhibition 
    /// </summary>
    public class DepenseValorisationConverter
    {
        /// <summary>
        /// Converti un ValorisationEnt en DepenseExhibition
        /// </summary>
        /// <param name="valorisations">Liste de ValorisationEnt</param>
        /// <param name="periodeDebut">Periode de début</param>
        /// <param name="periodeFin">Periode de fin</param>
        /// <param name="datesClotureComptables">Liste de dateClotureEnt</param>
        /// <returns>Liste de DepenseExhibition</returns>
        public IEnumerable<DepenseExhibition> Convert(List<ValorisationEnt> valorisations, DateTime? periodeDebut, DateTime? periodeFin, List<DatesClotureComptableEnt> datesClotureComptables = null)
        {
            List<DepenseExhibition> result = new List<DepenseExhibition>();
            foreach (ValorisationEnt valo in valorisations)
            {
                DepenseExhibition depenseExhibition = Convert(valo, periodeDebut, periodeFin, datesClotureComptables);
                if (depenseExhibition != null)
                {
                    result.Add(depenseExhibition);
                }
            }
            return result;
        }

        /// <summary>
        /// Converti un ValorisationEnt en DepenseExhibition
        /// </summary>
        /// <param name="valo"><see cref="ValorisationEnt"/></param>
        /// <param name="periodeDebut">Periode Debut</param>
        /// <param name="periodeFin">Periode de fin</param>
        /// <param name="datesClotureComptables">Liste de DatesClotureComptableEnt</param>
        /// <returns><see cref="DepenseExhibition"/></returns>
        private DepenseExhibition Convert(ValorisationEnt valo, DateTime? periodeDebut, DateTime? periodeFin, List<DatesClotureComptableEnt> datesClotureComptables = null)
        {
            DepenseExhibition depenseExhibition = new DepenseExhibition()
            {
                Ci = valo.CI,
                Code = valo.PersonnelId.HasValue ? valo.Personnel.Matricule : valo.Materiel.Code,
                RessourceId = valo.ReferentielEtendu?.RessourceId ?? 0,
                Ressource = valo.ReferentielEtendu.Ressource,
                TacheId = valo.TacheId,
                Tache = valo.Tache,
                NatureId = valo.ReferentielEtendu.NatureId ?? 0,
                Nature = valo.ReferentielEtendu.Nature,
                UniteId = valo.UniteId,
                Unite = valo.Unite,
                DeviseId = valo.DeviseId,
                Devise = valo.Devise,
                TypeDepense = Constantes.DepenseType.Valorisation,
                PUHT = valo.PUHT,
                Quantite = valo.Quantite,
                MontantHT = valo.Montant,
                Libelle1 = valo.PersonnelId.HasValue ? valo.Personnel.PrenomNom : valo.Materiel.Libelle,
                Libelle2 = valo.PersonnelId.HasValue ? valo.Personnel.Societe.Code : valo.Materiel.Societe.Code,
                DateDepense = valo.Date,
                Periode = valo.Date,
                DepenseId = valo.ValorisationId,
                GroupeRemplacementTacheId = valo.GroupeRemplacementTacheId ?? 0,
                RemplacementTaches = null,
                Personnel = valo.Personnel
            };

            depenseExhibition.Id = string.Concat(depenseExhibition.TypeDepense, valo.ValorisationId.ToString());

            DatesClotureComptableEnt period = datesClotureComptables.Find(x => depenseExhibition.Periode.Month == x.Mois && depenseExhibition.Periode.Year == x.Annee && (x.DateCloture.HasValue || x.DateTransfertFAR.HasValue));
            depenseExhibition.TacheRemplacable = period != null;
            depenseExhibition = Initialise(depenseExhibition, valo, periodeDebut, periodeFin);

            // Ajout numéro rapport sur le second libellé
            if (!string.IsNullOrEmpty(depenseExhibition.Libelle2))
            {
                depenseExhibition.Libelle2 = string.Concat(depenseExhibition.Libelle2, " - ", ExplorateurDepenseResources.Rapport, " #", valo.RapportId);
            }

            return depenseExhibition;
        }

        /// <summary>
        /// Initialise une DepenseExhibition
        /// </summary>
        /// <param name="depenseExhibition"><see cref="DepenseExhibition"/></param>
        /// <param name="valo"><see cref="ValorisationEnt"/></param>
        /// <param name="periodeDebut">Periode de début</param>
        /// <param name="periodeFin">Periode de fin</param>
        /// <returns>DepenseExhibition avec les informations des intérimaires</returns>
        private DepenseExhibition Initialise(DepenseExhibition depenseExhibition, ValorisationEnt valo, DateTime? periodeDebut, DateTime? periodeFin)
        {
            List<string> societeCode = new List<string>();
            List<string> fournisseurCode = new List<string>();
            if (valo.Personnel != null)
            {
                List<ContratInterimaireEnt> contrats = valo.Personnel.ContratInterimaires.ToList();
                societeCode = GetSocieteCodeFromContrats(contrats, periodeDebut, periodeFin, depenseExhibition.Ci.CiId);
                fournisseurCode = GetFournisseurCodeFromContrats(contrats, periodeDebut, periodeFin);
            }

            if (valo.Materiel != null)
            {
                depenseExhibition.Libelle2 = valo.Materiel.Societe?.Code;
            }
            else if (valo.Personnel?.IsInterimaire == false)
            {
                depenseExhibition.Libelle2 = valo.Personnel.Societe?.Code;
            }
            else
            {
                depenseExhibition.Libelle2 = valo.Personnel?.Societe?.Code;
                societeCode.ForEach(x => depenseExhibition.Libelle2 += (string.Format("-{0}", x)));
            }

            if (valo.Personnel?.IsInterimaire == true)
            {
                fournisseurCode.ForEach(x => depenseExhibition.Commentaire += fournisseurCode.Last() == x ? string.Format("{0}", x) : string.Format("{0}-", x));
            }

            return depenseExhibition;
        }

        /// <summary>
        /// Récupere la liste des Codes Societe pour une Liste de contrat ContratInterimaire pour un CI
        /// </summary>
        /// <param name="contratsList">Liste de contrat ContratInterimaire</param>
        /// <param name="periodeDebut">Periode de début</param>
        /// <param name="periodeFin">Periode de fin</param>
        /// <param name="ciId">Identifiant du CI</param>
        /// <returns>Liste des codes societe</returns>
        private List<string> GetSocieteCodeFromContrats(List<ContratInterimaireEnt> contratsList, DateTime? periodeDebut, DateTime? periodeFin, int ciId)
        {
            List<ContratInterimaireEnt> contratWithSameCi = contratsList.Where(x => x.CiId == ciId).ToList();
            return contratWithSameCi.Where(x => x.DateFin.AddDays(x.Souplesse) <= periodeFin.Value && periodeDebut != null ? x.DateDebut >= periodeDebut.Value : x.DateDebut != null).Select(m => m.Societe?.Code).Distinct().ToList();
        }

        /// <summary>
        /// Récupere la liste des Codes Fournisseurs pour une Liste de contrat ContratInterimaire
        /// </summary>
        /// <param name="contratsList">Liste de contrat ContratInterimaire</param>
        /// <param name="periodeDebut">Periode de début</param>
        /// <param name="periodeFin">Periode de fin</param>
        /// <returns>Liste des codes fournisseurs</returns>
        private List<string> GetFournisseurCodeFromContrats(List<ContratInterimaireEnt> contratsList, DateTime? periodeDebut, DateTime? periodeFin)
        {
            return contratsList.Where(x => x.DateFin.AddDays(x.Souplesse) >= periodeFin.Value.AddDays(-14) && periodeDebut != null ? x.DateDebut >= periodeDebut : x.DateDebut != null).Select(m => m.Fournisseur?.Code).Distinct().ToList();
        }

        /// <summary>
        /// Converti un ValorisationEnt en DepenseExhibition pour l'export
        /// </summary>
        /// <param name="valorisations">Liste de ValorisationEnt</param>
        /// <param name="periodeDebut">Periode de début</param>
        /// <param name="periodeFin">Periode de fin</param>
        /// <param name="datesClotureComptables">Liste de dateClotureEnt</param>
        /// <returns>Liste de DepenseExhibition</returns>
        public IEnumerable<DepenseExhibition> ConvertForExport(List<ValorisationEnt> valorisations, DateTime? periodeDebut, DateTime? periodeFin, List<DatesClotureComptableEnt> datesClotureComptables = null)
        {
            List<DepenseExhibition> result = new List<DepenseExhibition>();
            foreach (ValorisationEnt valo in valorisations)
            {
                DepenseExhibition expDep = ConvertForExport(valo, periodeDebut, periodeFin, datesClotureComptables);
                if (expDep != null)
                {
                    result.Add(expDep);
                }
            }
            return result;
        }

        /// <summary>
        /// Converti un ValorisationEnt en DepenseExhibition pour l'export
        /// </summary>
        /// <param name="valo"><see cref="ValorisationEnt"/></param>
        /// <param name="periodeDebut">Periode Debut</param>
        /// <param name="periodeFin">Periode de fin</param>
        /// <param name="datesClotureComptables">Liste de DatesClotureComptableEnt</param>
        /// <returns><see cref="DepenseExhibition"/></returns>
        private DepenseExhibition ConvertForExport(ValorisationEnt valo, DateTime? periodeDebut, DateTime? periodeFin, List<DatesClotureComptableEnt> datesClotureComptables = null)
        {
            DepenseExhibition depenseExhibition = Convert(valo, periodeDebut, periodeFin, datesClotureComptables);
            depenseExhibition.RemplacementTaches = valo.RemplacementTaches;
            return depenseExhibition;
        }
    }
}
