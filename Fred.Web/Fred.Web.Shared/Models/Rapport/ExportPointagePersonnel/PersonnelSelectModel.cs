using System;
namespace Fred.Web.Shared.Models.Rapport.ExportPointagePersonnel
{
    public class PersonnelSelectModel
    {
        /// <summary>
        ///   Obtient ou définit le nom du personnel.
        /// </summary>
        public string Nom { get; set; }

        /// <summary>
        ///   Obtient ou définit le prénom du personnel.
        /// </summary>
        public string Prenom { get; set; }

        /// <summary>
        ///   Obtient ou définit le matricule du personnel.
        /// </summary>
        public string Matricule { get; set; }

        /// <summary>
        /// Code de la société
        /// </summary>
        public string SocieteCode { get; set; }

        /// <summary>
        /// Code de l’établissement de paie 
        /// </summary>
        public string EtablissementPaieCode { get; set; }

        /// <summary>
        /// Identifiant de l’établissement comptable 
        /// </summary>
        public int? EtablissementPaieEtablissementComptableId { get; set; }

        /// <summary>
        /// Code de l’établissement comptable 
        /// </summary>
        public string EtablissementPaieEtablissementComptableCode { get; set; }

        /// <summary>
        /// Identifiant du personnel
        /// </summary>
        public int PersonnelId { get; set; }
        /// <summary>
        ///   Obtient ou définit la date de sortie du personnel.
        /// </summary>
        public DateTime? DateSortie { get; set; }

        public override bool Equals(object obj)
        {
            var person = obj as PersonnelSelectModel;
            if (person == null)
            {
                return false;
            }
            return person.Matricule == Matricule;
        }

        public override int GetHashCode() => Matricule.GetHashCode();
    }
}
