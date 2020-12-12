using Fred.Entities.Referential;
using Fred.Entities.Utilisateur;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Fred.Entities.Budget
{
    /// <summary>
    ///   Represente une révision d'un budget
    /// </summary>
    public class BudgetRevisionEnt : ICloneable
    {
        /// <summary>
        ///   Obtient ou définit l'identifiant unique d'une revision d'un budget.
        /// </summary>
        public int BudgetRevisionId { get; set; }

        /// <summary>
        ///   Obtient ou définit une valeur indiquant le statut du budget,
        ///   relatif à l'enum eStatutBudget
        /// </summary>
        public int Statut { get; set; }

        /// <summary>
        ///   Obtient ou définit le numéro de révision du budget
        /// </summary>
        public int RevisionNumber { get; set; }

        /// <summary>
        ///   Obtient ou définit la liste des taches du bugdet
        /// </summary>
        public ICollection<TacheEnt> Taches { get; set; }

        /// <summary>
        ///   Obtient ou définit l'identifiant du budget auquelle cette révision appartient
        /// </summary>
        public int BudgetId { get; set; }

        /// <summary>
        ///   Obtient ou définit le budget auquelle cette révision appartient
        /// </summary>
        public BudgetEnt Budget { get; set; }

        /// <summary>
        ///   Obtient ou définit la date de changement du statut vers "Validé"
        /// </summary>
        public DateTime? DateValidation { get; set; }

        /// <summary>
        ///   Obtient ou définit la date de changement de statut vers "A Valider"
        /// </summary>
        public DateTime? DateaValider { get; set; }

        /// <summary>
        ///   Obtient ou définit l'identifiant de l'auteur de la création
        /// </summary>
        public int? AuteurCreationId { get; set; }

        /// <summary>
        ///   Obtient ou définit l'identifiant de l'auteur de la création
        /// </summary>
        public UtilisateurEnt AuteurCreation { get; set; }

        /// <summary>
        ///   Obtient ou définit l'identifiant de l'auteur de la modification
        /// </summary>
        public int? AuteurModificationId { get; set; }

        /// <summary>
        ///   Obtient ou définit l'identifiant de l'auteur de la création
        /// </summary>
        public UtilisateurEnt AuteurModification { get; set; }

        /// <summary>
        ///   Obtient ou définit l'identifiant de l'auteur de la validation
        /// </summary>
        public int? AuteurValidationId { get; set; }

        /// <summary>
        ///   Obtient ou définit l'identifiant de l'auteur de la validation
        /// </summary>
        public UtilisateurEnt AuteurValidation { get; set; }

        /// <summary>
        ///   Obtient ou définit la date de creation
        /// </summary>
        public DateTime? DateCreation { get; set; }

        /// <summary>
        ///   Obtient ou définit la date de modification
        /// </summary>
        public DateTime? DateModification { get; set; }

        /// <summary>
        ///   Obtient ou définit la somme des recettes de toutes les tâches dans la devise de référence.
        /// </summary>
        public double Recettes { get; set; }

        /// <summary>
        ///   Obtient ou définit la somme des dépenses
        /// </summary>
        public double Depenses { get; set; }

        /// <summary>
        ///   Obtient ou définit la marge brute
        /// </summary>
        public double MargeBrute { get; set; }

        /// <summary>
        ///   Obtient ou définit le pourcentage de marge brute
        /// </summary>
        public double MargeBrutePercent { get; set; }

        /// <summary>
        ///   Obtient ou définit la marge brute
        /// </summary>
        public double MargeNette { get; set; }

        /// <summary>
        ///   Obtient ou définit le pourcentage de marge brute
        /// </summary>
        public double MargeNettePercent { get; set; }

        /// <summary>
        /// Clone
        /// </summary>
        /// <returns>Nouvelle référence</returns>
        public object Clone()
        {
            BudgetRevisionEnt newRev = (BudgetRevisionEnt)this.MemberwiseClone();

            if (this.Taches.Any())
            {
                newRev.Taches = new List<TacheEnt>();

                foreach (var tache in this.Taches)
                {
                    newRev.Taches.Add((TacheEnt)tache.Clone());
                }
            }

            return newRev;
        }

        /// <summary>
        /// Clean - retire toutes les dépendances pour insertion en base
        /// </summary>
        public void Clean()
        {
            this.BudgetRevisionId = 0;
            this.Budget = null;

            if (this.Taches.Any())
            {
                foreach (var tache in this.Taches)
                {
                    tache.Clean();
                }
            }
        }
    }
}