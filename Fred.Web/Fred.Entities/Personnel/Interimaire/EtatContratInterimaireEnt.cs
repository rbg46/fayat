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
    public class EtatContratInterimaireEnt
    {
        /// <summary>
        ///   Obtient ou définit l'identifiant unique d'un état du contrat interimaire.
        /// </summary>
        public int EtatContratInterimaireId { get; set; }

        /// <summary>
        ///   Obtient ou définit le code l'état du contrat intérimaire.
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        ///   Obtient ou définit le libellé de l'état du contrat intérimaire.
        /// </summary>
        public string Libelle { get; set; }
        
        /// <summary>
        /// Obtient ou définit les zones de travail
        /// </summary>
        public virtual ICollection<ContratInterimaireEnt> ContratInterimaires { get; set; }
    }
}
