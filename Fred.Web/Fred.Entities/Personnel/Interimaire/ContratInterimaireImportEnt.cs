using Fred.Entities.CI;
using Fred.Entities.Commande;
using Fred.Entities.Referential;
using Fred.Entities.ReferentielFixe;
using Fred.Entities.Societe;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fred.Entities.Personnel.Interimaire
{
    /// <summary>
    ///   Représente un état du contrat intérimaire.
    /// </summary>
    public class ContratInterimaireImportEnt
    {
        /// <summary>
        ///   Obtient ou définit l'identifiant unique de l'import du contrat interimaire.
        /// </summary>
        public int ImportId { get; set; }

        /// <summary>
        ///   Obtient ou définit l'identifiant du contrat interimaire.
        /// </summary>
        public int ContratInterimaireId { get; set; }

        /// <summary>
        ///   Obtient ou définit le contrat interimaire.
        /// </summary>
        public ContratInterimaireEnt ContratInterimaire { get; set; }

        /// <summary>
        ///   Obtient ou définit le timestamp de l'import.
        /// </summary>
        public ulong? TimestampImport { get; set; }

        /// <summary>
        ///   Obtient ou définit l'historique de l'import.
        /// </summary>
        public string HistoriqueImport { get; set; }
    }
}
