using System.Collections.Generic;
using Fred.Entities;
using Fred.Entities.Moyen;
using Fred.Entities.Rapport;
using Fred.Web.Shared.Models.Moyen;

namespace Fred.Business.Moyen
{
    /// <summary>
    /// Fonctionnalité pour assurer l'interaction avec rapport de pointage : Rapport et RapportLigne
    /// </summary>
    public class RapportMoyenExtractionService : IRapportMoyenExtractionService
    {
        /// <summary>
        /// Rapport Moyen Extraction Service
        /// </summary>
        public RapportMoyenExtractionService()
        {

        }
        /// <summary>
        /// maping RapportLigne To RapportMoyenLigne
        /// </summary>
        /// <param name="listAffectationMoyen">list Affectation Moyen</param>
        /// <returns>List RapportMoyenLigneExcelModel</returns>
        public List<RapportMoyenLigneExcelModel> GetRapportMoyenLigneExcel(List<RapportLigneEnt> listAffectationMoyen)
        {
            return listAffectationMoyen?.ConvertAll(rapportLigne => ToRapportMoyenLigneExcelModel(rapportLigne));
        }

        /// <summary>
        /// rapportLigne To RapportMoyenLigneExcelModel
        /// </summary>
        /// <param name="ligne">ligne</param>
        /// <returns>RapportMoyenLigneExcelModel</returns>
        private RapportMoyenLigneExcelModel ToRapportMoyenLigneExcelModel(RapportLigneEnt ligne)
        {
            if (ligne == null)
            {
                return null;
            }

            return new RapportMoyenLigneExcelModel
            {
                Periode = ligne.DatePointage.ToShortDateString(),
                Societe = ligne.AffectationMoyen.Materiel?.Societe?.Libelle ?? string.Empty,
                Etablissement = ligne.AffectationMoyen.Materiel?.EtablissementComptable?.Libelle ?? string.Empty,
                ParcNum = ligne.AffectationMoyen.Materiel?.Code ?? string.Empty,
                Type = ligne.AffectationMoyen.Materiel?.Ressource?.SousChapitre?.Chapitre?.Libelle ?? string.Empty,
                Libelle = ligne.AffectationMoyen.Materiel?.Libelle ?? string.Empty,
                Modele = ligne.AffectationMoyen.Materiel?.Ressource?.Libelle ?? string.Empty,
                ImmatNum = ligne.AffectationMoyen.Materiel?.Immatriculation ?? string.Empty,
                SiteProrietaire = ligne.AffectationMoyen.Materiel?.SiteAppartenance?.Libelle ?? string.Empty,
                SiteActuel = ligne.AffectationMoyen.Site?.Libelle ?? string.Empty,
                AffectedTo = GetAffectedTo(ligne.AffectationMoyen),
                Ci = ligne.Ci?.Libelle ?? string.Empty,
                Heures = ligne.HeuresMachine.ToString(),
                Location = ligne.AffectationMoyen.Materiel != null && ligne.AffectationMoyen.Materiel.IsLocation ? Commun.Constantes.YES : Commun.Constantes.NO
            };
        }

        /// <summary>
        /// Get affected to libélle
        /// </summary>
        /// <param name="affectationEnt">Affectation moyen ent</param>
        /// <returns>Libélle Affected to</returns>
        private string GetAffectedTo(AffectationMoyenEnt affectationEnt)
        {
            if (affectationEnt.TypeAffectation == null)
            {
                return null;
            }

            if (affectationEnt.TypeAffectation.Code.Equals(AffectationMoyenTypeCode.Personnel.ToString()))
            {
                return affectationEnt.Personnel?.Nom + " " + affectationEnt.Personnel?.Prenom;
            }
            else if (affectationEnt.TypeAffectation.Code.Equals(AffectationMoyenTypeCode.CI.ToString()))
            {
                return affectationEnt.Ci?.Libelle;
            }
            else
            {
                return affectationEnt.TypeAffectation.Libelle;
            }
        }

    }
}
