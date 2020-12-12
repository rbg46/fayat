using Fred.Entities.Personnel;
using Fred.Entities.ReferentielEtendu;
using Fred.Entities.ReferentielFixe;
using Fred.Entities.Societe;

namespace Fred.Business.CommandeEnergies
{
    /// <summary>
    /// Pointage Commande Energie
    /// </summary>
    public class PointagePersonnelCommandeEnergie
    {
        /// <summary>
        /// Personnel
        /// </summary>
        public PersonnelEnt Personnel { get; set; }

        /// <summary>
        /// Ressource
        /// </summary>
        public RessourceEnt Ressource { get; set; }

        /// <summary>
        /// Societe
        /// </summary>
        public SocieteEnt SocietePerso { get; set; }

        /// <summary>
        /// Référentiel étendu de la ressource liée au personnel
        /// </summary>
        public ReferentielEtenduEnt ReferentielEtendu { get; set; }

        /// <summary>
        /// Quantité d'heure pointée sur la période
        /// </summary>
        public double QuantitePointee { get; set; }
    }
}
