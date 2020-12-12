using Fred.Entities.Referential;
using Fred.Entities.ReferentielEtendu;
using Fred.Entities.ReferentielFixe;
using Fred.Entities.Societe;

namespace Fred.Business.CommandeEnergies
{
    /// <summary>
    /// Pointage Materiel Commande Energie
    /// </summary>
    public class PointageMaterielCommandeEnergie
    {
        /// <summary>
        /// Materiel
        /// </summary>
        public MaterielEnt Materiel { get; set; }

        /// <summary>
        /// Ressource
        /// </summary>
        public RessourceEnt Ressource { get; set; }

        /// <summary>
        /// Societe
        /// </summary>
        public SocieteEnt SocieteMatriel { get; set; }

        /// <summary>
        /// Référentiel étendu de la ressource liée au matériel
        /// </summary>
        public ReferentielEtenduEnt ReferentielEtendu { get; set; }

        /// <summary>
        /// Quantité d'heure pointée sur la période
        /// </summary>
        public double QuantitePointee { get; set; }
    }
}
