using System;
using System.Collections.Generic;
using Fred.Entities.Rapport;
using Fred.Web.Models.CI;
using Fred.Web.Models.Rapport;
using Fred.Web.Models.Referential;
using Fred.Web.Models.Utilisateur;

namespace Fred.Web.Dtos.Mobile.Rapport
{
    /// <summary>
    /// Dto Rapport
    /// </summary>
    public class RapportDto : DtoBase
    {
        /// <summary>
        /// Obtient ou définit l'identifiant du rapport
        /// </summary>
        public int RapportId { get; set; }

        /// <summary>
        /// Obtient ou définit l'ouvrage
        /// </summary>
        public string Ouvrage { get; set; }

        /// <summary>
        /// Obtient ou définit la date chantier
        /// </summary>
        public DateTime DateChantier { get; set; }

        /// <summary>
        /// Obtient ou définit la date du rapport
        /// </summary>
        public DateTime DateRapport { get; set; }

        /// <summary>
        /// Obtient ou définit horaire début le matin
        /// </summary>
        public DateTime HoraireDebutM { get; set; }

        /// <summary>
        /// Obtient ou définit l'horaire début le soir
        /// </summary>
        public DateTime HoraireDebutS { get; set; }

        /// <summary>
        /// Obtient ou définit l'horaire fin matin
        /// </summary>
        public DateTime HoraireFinM { get; set; }

        /// <summary>
        /// Obtient ou définit l'horaire fin soir
        /// </summary>
        public DateTime HoraireFinS { get; set; }

        /// <summary>
        /// Obtient ou définit la météo
        /// </summary>
        public string Meteo { get; set; }

        /// <summary>
        /// Obtient ou définit l'information
        /// </summary>
        public string Information { get; set; }

        /// <summary>
        /// Obtient ou définit l'identifiant
        /// </summary>
        public int CiId { get; set; }

        /// <summary>
        /// Obtient ou définit m'id de l'utilisateur
        /// </summary>
        public int UtilisateurFredId { get; set; }

        /// <summary>
        /// Obtient ou définit la liste des lignes du rapport
        /// </summary>
        public ICollection<RapportLigneDto> ListLignes { get; set; }

        /// <summary>
        /// Obtient ou définit l'identifiant unique d'un statut de rapport.
        /// </summary>
        public int RapportStatutId { get; set; }

        /// <summary>
        /// Obtient ou définit le type de rapport.
        /// </summary>
        public TypeRapportEnum TypeRapport { get; set; }

        /// <summary>
        ///  Obtient ou définit l'identifiant de l'utilisateur valideur CDC - Chef de Chantier
        /// </summary>
        public int? ValideurCdcId { get; set; }

        /// <summary>
        ///  Obtient ou définit l'identifiant de l'utilisateur valideur CDT - Conducteur de Travaux
        /// </summary>
        public int? ValideurCdtId { get; set; }

        /// <summary>
        ///  Obtient ou définit l'identifiant de l'utilisateur valideur DRC - Directeur de Chantier
        /// </summary>
        public int? ValideurDrcId { get; set; }

        /// <summary>
        ///  Obtient ou définit l'identifiant de l'utilisateur valideur GSP - Correspondant Paie
        /// </summary>
        public int? ValideurGspId { get; set; }

        /// <summary>
        ///  Obtient ou définit les date et heure de la validation CDC - Chef de Chantier
        /// </summary>
        public DateTime? DateValidationCdc { get; set; }

        /// <summary>
        ///  Obtient ou définit les date et heure de la validation CDT - Conducteur de Travaux
        /// </summary>
        public DateTime? DateValidationCdt { get; set; }

        /// <summary>
        ///  Obtient ou définit les date et heure de la validation DRC - Directeur de Chantier
        /// </summary>
        public DateTime? DateValidationDrc { get; set; }

        /// <summary>
        ///  Obtient ou définit les date et heure de la validation GSP - Correspondant Paie
        /// </summary>
        public DateTime? DateValidationGsp { get; set; }
    }
}