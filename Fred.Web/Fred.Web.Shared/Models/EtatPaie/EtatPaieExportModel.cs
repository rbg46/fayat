using System;
using System.Collections.Generic;
using Fred.Entities.Rapport;

namespace Fred.Web.Shared.Models.EtatPaie
{
    public class EtatPaieExportModel
  {
    /// <summary>
    ///   Obtient ou définit la date
    /// </summary>
    public DateTime? Date { get; set; }

        /// <summary>
        ///   Obtient ou définit l'année
        /// </summary>
        public int Annee { get; set; }

        /// <summary>
        ///   Obtient ou définit le mois
        /// </summary>
        public int Mois { get; set; }

        /// <summary>
        ///   Obtient ou définit le filtre d'état de paie
        /// </summary>
        public TypeFiltreEtatPaie Filtre { get; set; }

        /// <summary>
        ///   Obtient ou définit l'identifiant unique de l'organisation
        /// </summary>
        public int OrganisationId { get; set; }

        /// <summary>
        ///   Obtient ou définit le type de tri 
        /// </summary>
        public bool Tri { get; set; }

        /// <summary>
        ///   Obtient ou définit le type d'export
        /// </summary>
        public bool Pdf { get; set; }

        /// <summary>
        ///   Obtient ou définit l'identifiant unique d'un personnel
        /// </summary>
        public int? PersonnelId { get; set; }

        /// <summary>
        ///   Obtient ou définit le type de filtres des primes mensuelles
        /// </summary>
        public bool FiltresPrimesMensuelles { get; set; }

    /// <summary>
    ///   Obtient ou définit la liste des identifiants unique d'établissement de paie 
    /// </summary>
    public List<int?> EtablissementPaieIdList { get; set; } = new List<int?>();

    /// <summary>
    ///   Obtient ou définit de statut de personnel
    /// </summary>
    public IEnumerable<string> StatutPersonnelList { get; set; } = new List<string>();
  }
}
