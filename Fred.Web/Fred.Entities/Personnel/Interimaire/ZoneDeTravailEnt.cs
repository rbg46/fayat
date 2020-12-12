using Fred.Entities.Referential;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fred.Entities.Personnel.Interimaire
{
    /// <summary>
    /// Entité de liaison entre un intrimaire et un établissement de paie
    /// </summary>
    public class ZoneDeTravailEnt
    {
        /// <summary>
        ///   Obtient ou définit l'identifiant de le contrat intérimaire
        /// </summary>
        public int ContratInterimaireId { get; set; }

        /// <summary>
        ///   Obtient ou définit le contrat intérimaire
        /// </summary>
        public ContratInterimaireEnt Contrat { get; set; }

        /// <summary>
        ///   Obtient ou définit l'identifiant de l'établissement comptable
        /// </summary>
        public int EtablissementComptableId { get; set; }

        /// <summary>
        ///   Obtient ou définit l'établissement comptable
        /// </summary>
        public EtablissementComptableEnt EtablissementComptable { get; set; }
    }
}

