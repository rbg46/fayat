using Fred.Entities.Referential;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fred.Entities.Rapport
{
    /// <summary>
    ///   Représente ou défini les primes associées à une ligne de rapport
    /// </summary>
    public class RapportLignePrimeEnt : PointagePrimeBase
    {
        /// <summary>
        ///   Obtient ou définit l'identifiant unique de la ligne prime du rapport
        /// </summary>
        public override int PointagePrimeId { get; set; }

        /// <summary>
        ///   Obtient ou définit l'identifiant unique de la ligne prime du rapport
        /// </summary>
        public int RapportLignePrimeId
        {
            get { return PointagePrimeId; }

            set { PointagePrimeId = value; }
        }

        /// <summary>
        ///   Obtient ou définit l'id de pointage de la ligne de rapport de rattachement
        /// </summary>
        public override int PointageId
        {
            get { return RapportLigneId; }

            set { RapportLigneId = value; }
        }

        /// <summary>
        ///   Obtient ou définit l'id de la ligne de rapport de rattachement
        /// </summary>
        public int RapportLigneId { get; set; }

        /// <summary>
        ///   Obtient ou définit l'entité Rapport
        /// </summary>
        /// REMARQUE LORS DE LE MIGRATION CODE FIRST DANGER REFACTO POUR QUE MARCHE CODE FIRST  
        public RapportLigneEnt RapportLigne { get; set; }

        /// <summary>
        ///   Obtient ou définit l'id de la prime
        /// </summary>
        public override int PrimeId { get; set; }

        /// <summary>
        ///   Obtient ou définit l'entité Prime
        /// </summary>
        public override PrimeEnt Prime { get; set; }

        /// <summary>
        ///   Obtient ou définit une valeur indiquant si la prime est checkée
        /// </summary>
        public override bool IsChecked { get; set; }

        /// <summary>
        ///   Obtient ou définit l'heure de la prime uniquement si TypeHoraire est en heure
        /// </summary>
        public override double? HeurePrime { get; set; }

        /// <summary>
        ///   Obtient ou définit une valeur indiquant si la ligne est en création
        /// </summary>
        public override bool IsCreated => RapportLignePrimeId == 0;

        /// <summary>
        ///   Obtient ou définit une valeur indiquant si la ligne est en modification
        /// </summary>
        public override bool IsDeleted { get; set; } = false;

        /// <summary>
        ///   Vide une propriété
        /// </summary>
        public void CleanLinkedProperties()
        {
            RapportLigne = null;
            Prime = null;
        }

        /// <summary>
        ///   Duplication de l'entité (sans les identifiants du pointages)
        /// </summary>    
        /// <returns>Entité dupliquée</returns>
        public RapportLignePrimeEnt Duplicate()
        {
            return new RapportLignePrimeEnt
            {
                PointagePrimeId = 0,
                PointageId = 0,
                RapportLigne = null,
                PrimeId = PrimeId,
                Prime = Prime,
                HeurePrime = 0,
                IsChecked = IsChecked,
                IsDeleted = IsDeleted
            };
        }
    }
}
