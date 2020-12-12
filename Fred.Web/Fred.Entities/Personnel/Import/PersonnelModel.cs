using System;
using System.Diagnostics;

namespace Fred.Entities.Personnel.Imports
{
    /// <summary>
    /// Model representant un personel du systeme distant
    /// </summary>
    [DebuggerDisplay("Matricule = {Matricule} Nom = {Nom} CodeSocietePaye = {CodeSocietePaye} CodeEtablissement = {CodeEtablissement} CodeEmploi = {CodeEmploi}")]
    public class PersonnelModel
    {
        /// <summary>
        /// Obtient ou définit le code de la société Paye
        /// </summary>
        public string CodeSocietePaye { get; set; }

        /// <summary>
        /// Obtient ou définit le code de l'établissement.
        /// </summary>
        public string CodeEtablissement { get; set; }

        /// <summary>
        /// Obtient ou définit le matricule.
        /// </summary>
        public string Matricule { get; set; }

        /// <summary>
        /// Obtient ou définit le nom.
        /// </summary>
        public string Nom { get; set; }

        /// <summary>
        /// Obtient ou définit le prénom.
        /// </summary>
        public string Prenom { get; set; }

        /// <summary>
        /// Obtient ou définit la catégorie interne.
        /// </summary>    
        public string CategoriePerso { get; set; }

        /// <summary>
        /// Obtient ou définit le statut.
        /// </summary>    
        public string Statut { get; set; }

        /// <summary>
        /// Obtient ou définit le code emploi.
        /// </summary>    
        public string CodeEmploi { get; set; }

        /// <summary>
        /// Obtient ou définit la date d'entrée du personnel.
        /// </summary>
        public DateTime? DateEntree { get; set; }

        /// <summary>
        /// Obtient ou définit la date de sortie du personnel.
        /// </summary>
        public DateTime? DateSortie { get; set; }

        /// <summary>
        ///   Obtient ou définit la date de modification.
        /// </summary>
        public DateTime? DateModification { get; set; }

        /// <summary>
        ///   Obtient ou définit la Section.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        ///   Obtient ou définit la Section.
        /// </summary>
        public string Section { get; set; }

        /// <summary>
        ///   Obtient ou définit l'Entreprise.
        /// </summary>
        public string Entreprise { get; set; }
        /// <summary>
        ///   Obtient ou définit le matricule du  manager.
        /// </summary>
        public string MatriculeManager { get; set; }

        /// <summary>
        ///   Obtient ou définit la societe du  manager.
        /// </summary>
        public string SocieteManager { get; set; }

        /// <summary>
        ///   Obtient ou définit le societe comptable.
        /// </summary>
        public string SocieteComptable { get; set; }

        /// <summary>
        ///   Obtient ou définit l'établissement comptable.
        /// </summary>
        public string EtablissementComptable { get; set; }
    }
}
