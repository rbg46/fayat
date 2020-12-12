using Fred.Entities.CI;
using Fred.Entities.Personnel;
using Fred.Entities.Referential;
using Fred.Entities.Utilisateur;
using System;

namespace Fred.Entities.IndemniteDeplacement
{
    /// <summary>
    ///   Représente une indemnité de déplacement
    /// </summary>
    public class IndemniteDeplacementEnt
    {
        private DateTime? dateDernierCalcul;
        private DateTime? dateCreation;
        private DateTime? dateModification;
        private DateTime? dateSuppression;

        /// <summary>
        ///   Obtient ou définit l'identifiant unique d'une indemnité de déplacement.
        /// </summary>
        public int IndemniteDeplacementId { get; set; }

        /// <summary>
        ///   Obtient ou définit l'identifiant du personnel auquel est rattachée l'indemnité
        /// </summary>
        public int PersonnelId { get; set; }

        /// <summary>
        ///   Obtient ou définit l'identifiant du CI auquel est rattachée l'indemnité
        /// </summary>
        public int CiId { get; set; }

        /// <summary>
        ///   Obtient ou définit une distance en kilometre
        /// </summary>
        public double NombreKilometres { get; set; }

        /// <summary>
        ///   Obtient ou définit une distance à vol d'oiseau en kilometre entre le domicile du personnel et l'etablissement de
        ///   rattachement
        /// </summary>
        public double? NombreKilometreVODomicileRattachement { get; set; }

        /// <summary>
        ///   Obtient ou définit une distance à vol d'oiseau en kilometre entre le domicile du personnel et le chantier
        /// </summary>
        public double? NombreKilometreVODomicileChantier { get; set; }

        /// <summary>
        ///   Obtient ou définit une distance à vol d'oiseau en kilometre entre le chantier et l'etablissement de rattachement
        /// </summary>
        public double? NombreKilometreVOChantierRattachement { get; set; }

        /// <summary>
        ///   Obtient ou définit la date de dernier calcul de l'indemnité
        /// </summary>
        public DateTime? DateDernierCalcul
        {
            get
            {
                return (dateDernierCalcul.HasValue) ? DateTime.SpecifyKind(dateDernierCalcul.Value, DateTimeKind.Utc) : default(DateTime?);
            }
            set
            {
                dateDernierCalcul = (value.HasValue) ? DateTime.SpecifyKind(value.Value, DateTimeKind.Utc) : default(DateTime?);
            }
        }

        /// <summary>
        ///   Obtient ou définit une valeur indiquant si il s'agit d'une indemnité voyage détente
        /// </summary>
        public bool IVD { get; set; }

        /// <summary>
        ///   Obtient ou définit le code déplacement de l'indemnité
        /// </summary>
        public int? CodeDeplacementId { get; set; }

        /// <summary>
        ///   Obtient ou définit le code zone déplacement de l'indemnité
        /// </summary>
        public int? CodeZoneDeplacementId { get; set; }

        /// <summary>
        ///   Obtient ou définit une valeur indiquant si il s'agit d'une saisie manuelle
        /// </summary>
        public bool SaisieManuelle { get; set; }

        /// <summary>
        ///   Obtient ou définit la date de création de l'indemnité
        /// </summary>
        public DateTime? DateCreation
        {
            get
            {
                return (dateCreation.HasValue) ? DateTime.SpecifyKind(dateCreation.Value, DateTimeKind.Utc) : default(DateTime?);
            }
            set
            {
                dateCreation = (value.HasValue) ? DateTime.SpecifyKind(value.Value, DateTimeKind.Utc) : default(DateTime?);
            }
        }

        /// <summary>
        ///   Obtient ou définit l'id de l'auteur de la création de l'indemnité
        /// </summary>
        public int? AuteurCreation { get; set; }

        /// <summary>
        ///   Obtient ou définit la date de modification de l'indemnité
        /// </summary>
        public DateTime? DateModification
        {
            get
            {
                return (dateModification.HasValue) ? DateTime.SpecifyKind(dateModification.Value, DateTimeKind.Utc) : default(DateTime?);
            }
            set
            {
                dateModification = (value.HasValue) ? DateTime.SpecifyKind(value.Value, DateTimeKind.Utc) : default(DateTime?);
            }
        }

        /// <summary>
        ///   Obtient ou définit l'id de l'auteur de la modification de l'indemnité
        /// </summary>
        public int? AuteurModification { get; set; }

        /// <summary>
        ///   Obtient ou définit la date de suppression de l'indemnité
        /// </summary>
        public DateTime? DateSuppression
        {
            get
            {
                return (dateSuppression.HasValue) ? DateTime.SpecifyKind(dateSuppression.Value, DateTimeKind.Utc) : default(DateTime?);
            }
            set
            {
                dateSuppression = (value.HasValue) ? DateTime.SpecifyKind(value.Value, DateTimeKind.Utc) : default(DateTime?);
            }
        }

        /// <summary>
        ///   Obtient ou définit l'id de l'auteur de la suppression de l'indemnité
        /// </summary>
        public int? AuteurSuppression { get; set; }

        // REMARQUE LORS DE LE MIGRATION CODE FIRST 
        // AVANT LA MIGRATION LA COLUMN N'ETAIT PAS DEFINI COMME UNE FOREIGNKEY
        /// <summary>
        ///   Obtient ou définit le CI liée
        /// </summary>
        public CIEnt CI { get; set; }

        /// <summary>
        ///   Obtient ou définit le code zone déplacement concernant l'indeminité de déplacement
        /// </summary>
        public CodeZoneDeplacementEnt CodeZoneDeplacement { get; set; }

        /// <summary>
        ///   Obtient ou définit le code déplacement concernant l'indeminité de déplacement
        /// </summary>
        public CodeDeplacementEnt CodeDeplacement { get; set; }

        /// <summary>
        ///   Obtient ou définit le Personnel lié
        /// </summary>
        public PersonnelEnt Personnel { get; set; }

        // RENOMMER LE CHAMP
        /// <summary>
        /// Parent Utilisateur pointed by [FRED_INDEMNITE_DEPLACEMENT].([AuteurCreation]) (fk_indemniteAjout)
        /// </summary>
        public virtual UtilisateurEnt UtilisateurAuteurCreation { get; set; }

        // RENOMMER LE CHAMP
        /// <summary>
        /// Parent Utilisateur pointed by [FRED_INDEMNITE_DEPLACEMENT].([AuteurModification]) (fk_indemniteModification)
        /// </summary>
        public virtual UtilisateurEnt UtilisateurAuteurModification { get; set; }

        // RENOMMER LE CHAMP
        /// <summary>
        /// Parent Utilisateur pointed by [FRED_INDEMNITE_DEPLACEMENT].([AuteurSuppression]) (fk_indemniteSuppression)
        /// </summary>
        public virtual UtilisateurEnt UtilisateurAuteurSuppression { get; set; }
    }
}