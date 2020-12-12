using Fred.Entities.Referential;
using Fred.Entities.ReferentielFixe;

namespace Fred.Entities.Budget
{
    /// <summary>
    /// Rperésente les valeurs saisies lors du controle budgétaire.
    /// Une valeur est toujours liée par une tache et une ressource
    /// </summary>
    public class ControleBudgetaireValeursEnt
    {
        /// <summary>
        /// Un controle budgétaire estl ié à un budget
        /// </summary>
        public ControleBudgetaireEnt ControleBudgetaire { get; set; }

        /// <summary>
        /// Id du budget auquel on est rattaché
        /// </summary>
        public int ControleBudgetaireId { get; set; }

        /// <summary>
        /// Periode d'application, fait partie de la foreign key composée
        /// </summary>
        public int Periode { get; set; }

        /// <summary>
        /// Une valeur du controle budgétaire est nécessairement liée à une tache
        /// </summary> 
        public TacheEnt Tache { get; set; }

        /// <summary>
        /// Id de la tache a laquelle on est rattachée
        /// </summary>
        public int TacheId { get; set; }

        /// <summary>
        /// Ressource associée
        /// </summary>
        public RessourceEnt Ressource { get; set; }

        /// <summary>
        /// Id de la ressource associée
        /// </summary>
        public int RessourceId { get; set; }

        /// <summary>
        /// Valeur de l'ajustement e.g Tel tache coutera finalement X€ plus cher 
        /// </summary>
        public decimal Ajustement { get; set; }

        /// <summary>
        /// Le commantaire liée à la valeur du montant de l'ajustement
        /// </summary>
        public string CommentaireAjustement { get; set; }

        /// <summary>
        /// Le montant de la prévision fin affaire calculée pour le mois précédent (YYYYMM -1 mois)
        /// </summary>
        public decimal Pfa { get; set; }
    }
}
