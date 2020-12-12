using System.Collections.Generic;

namespace Fred.Entities.VerificationPointage
{
    /// <summary>
    /// Model Vérification pointage
    /// </summary>
    public class ChekingPointingMonth
    {
        /// <summary>
        /// info Labelle sur le personnel/Materiel
        /// </summary>
        public string InfoLabelle { get; set; }

        /// <summary>
        /// info sur CI
        /// </summary>
        public string InfoCI { get; set; }

        /// <summary>
        /// Etat Machine (Marche,Panne,Arret,intemperie)
        /// </summary>
        public string EtatMachine { get; set; }

        /// <summary>
        /// list des jours de travail du 1 au 31
        /// </summary>
        public Dictionary<int, double> DayWorks { get; set; }
    }
}
