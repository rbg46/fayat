using System.Collections.Generic;

namespace Fred.Web.Shared.Models.Rapport.ExportPointagePersonnel
{
    public class ControleSaisiesTibcoModel
    {
        /// <summary>
        /// Code de l'établissement comptable
        /// </summary>
        public string EtablissementCode { get; set; }

        /// <summary>
        /// Nom du personnel
        /// </summary>
        public string PersonnelNom { get; set; }

        /// <summary>
        /// Prenom du personnel
        /// </summary>
        public string PersonnelPrenom { get; set; }

        /// <summary>
        /// Matricule du personnel
        /// </summary>
        public string PersonnelMatricule { get; set; }

        /// <summary>
        /// Libelle Noeud root
        /// </summary>
        public string LibelleNoeud
        {
            get
            {
                return string.Format("{0} - {1} {2} ({3})", EtablissementCode, PersonnelNom, PersonnelPrenom, PersonnelMatricule);
            }
        }

        /// <summary>
        /// Liste des erreurs detectées
        /// </summary>
        public ICollection<ControleSaisiesErreurTibcoModel> Erreurs { get; set; }
    }
}
