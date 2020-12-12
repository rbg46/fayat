using Fred.Entities.Referential;
using Fred.Entities.ReferentielFixe;
using System;
using System.Collections.Generic;

namespace Fred.Entities.Budget
{
    /// <summary>
    ///   Représente une ressource insérée dans une tache de niveau 4.
    ///   Peut-être "appelé" aussi T5
    /// </summary>
    public class RessourceTacheEnt : ICloneable
    {
        /// <summary>
        ///   Obtient ou définit l'identifiant unique d'un budget.
        /// </summary>
        public int RessourceTacheId { get; set; }

        /// <summary>
        ///   Obtient ou définit l'identifiant de la tâche (de niveau T4) à laquelle cette ressource appartient
        ///   La tâche est de niveau T4
        /// </summary>
        public int TacheId { get; set; }

        /// <summary>
        ///   Obtient ou définit la tâche (de niveau T4) à laquelle cette ressource appartient
        /// </summary>
        public TacheEnt Tache { get; set; }

        /// <summary>
        ///   Obtient ou définit l'identifiant de la ressource de référence attachée à cette ressource
        ///   La tâche est de niveau T4
        /// </summary>
        public int RessourceId { get; set; }

        /// <summary>
        ///   Obtient ou définit la ressource de référence attachée à cette ressource
        /// </summary>
        public RessourceEnt Ressource { get; set; }

        /// <summary>
        ///   Obtient ou définit la quantité de base
        /// </summary>
        public double? QuantiteBase { get; set; }

        /// <summary>
        ///   Obtient ou définit la quantité
        /// </summary>
        public double? Quantite { get; set; }

        /// <summary>
        /// Recopie du prix unitaire de la ressource si personnalisé
        /// </summary>
        public double? PrixUnitaire { get; set; }

        /// <summary>
        ///   Obtient ou définit la formule de calcul du montant
        /// </summary>
        public string Formule { get; set; }

        /// <summary>
        ///   Obtient ou définit la liste des ressourceTacheDevises
        /// </summary>
        public ICollection<RessourceTacheDeviseEnt> RessourceTacheDevises { get; set; }

        /// <summary>
        /// Clone
        /// </summary>
        /// <returns>Nouvelle référence</returns>
        public object Clone()
        {
            RessourceTacheEnt newRessTache = (RessourceTacheEnt)this.MemberwiseClone();

            if (this.Ressource != null)
            {
                newRessTache.Ressource = (RessourceEnt)this.Ressource.Clone();
            }

            return newRessTache;
        }

        /// <summary>
        /// Clean - retire toutes les dépendances pour insertion en base
        /// </summary>
        public void Clean()
        {
            this.RessourceTacheId = 0;
            this.TacheId = 0;
            this.Tache = null;
            this.RessourceId = 0;
            this.Ressource.Clean();
        }
    }
}