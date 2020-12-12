using Fred.Entities.Budget;
using Fred.Entities.Budget.Recette;
using Fred.Entities.CI;
using Fred.Entities.Commande;
using Fred.Entities.Depense;
using Fred.Entities.Rapport;
using Fred.Entities.Utilisateur;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using System.Linq;

namespace Fred.Entities.Referential
{
    /// <summary>
    ///   Représente une tâche.
    /// </summary>
    [DebuggerDisplay("({TacheId}) {Code} {Libelle} (niveau {Niveau})")]
    public class TacheEnt : ICloneable
    {
        private DateTime? dateSuppression;
        private DateTime dateCreation;
        private DateTime? dateModification;

        /// <summary>
        ///   Obtient ou définit l'identifiant unique d'une tâche.
        /// </summary>
        public int TacheId { get; set; }

        /// <summary>
        ///   Obtient ou définit le code d'une tâche.
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        ///   Obtient ou définit le libellé d'une tâche.
        /// </summary>
        public string Libelle { get; set; }

        /// <summary>
        ///   Obtient ou définit une valeur indiquant si le champs indiquant si une tâche est la tâche par défaut.
        /// </summary>
        public bool TacheParDefaut { get; set; }

        /// <summary>
        ///   Obtient ou définit une valeur de l'identifiant CI
        /// </summary>
        public int CiId { get; set; }

        /// <summary>
        ///   Obtient ou définit une valeur du CI.
        /// </summary>
        public CIEnt CI { get; set; }

        /// <summary>
        ///   Obtient une concaténation du code et du libellé.
        /// </summary>
        public string CodeLibelle => Code + " - " + Libelle;

        /// <summary>
        ///   Obtient ou définit une valeur .
        /// </summary>
        public int? ParentId { get; set; }

        /// <summary>
        ///   Obtient ou définit une valeur .
        /// </summary>
        public TacheEnt Parent { get; set; }

        /// <summary>
        ///   Obtient ou définit la liste des taches enfants
        /// </summary>
        public ICollection<TacheEnt> TachesEnfants { get; set; }

        /// <summary>
        ///   Obtient ou définit le niveau de la tache.
        /// </summary>
        public int? Niveau { get; set; }

        /// <summary>
        ///   Obtient ou définit si la tache est active.
        /// </summary>
        public bool Active { get; set; } = true;

        /// <summary>
        ///   Obtient ou définit la liste des ressources assiciées à une tâche T4 dans un budget
        ///   Ne devrait contenir des éléments que dans le cas d'une tâche T4
        /// </summary>
        public ICollection<RessourceTacheEnt> RessourceTaches { get; set; }

        /// <summary>
        ///   Obtient ou définit la liste des recette par devise
        /// </summary>
        public ICollection<TacheRecetteEnt> TacheRecettes { get; set; }

        /// <summary>
        /// QuantiteBase
        /// </summary>
        public double? QuantiteBase { get; set; }

        /// <summary>
        /// correspond prix total QB
        /// </summary>
        public double? PrixTotalQB { get; set; }

        /// <summary>
        /// correspond prix unitaire QB
        /// </summary>
        public double? PrixUnitaireQB { get; set; }

        /// <summary>
        /// TotalHeureMO
        /// </summary>
        public double? TotalHeureMO { get; set; }

        /// <summary>
        /// HeureMOUnite
        /// </summary>
        public double? HeureMOUnite { get; set; }

        /// <summary>
        /// QuantiteARealise
        /// </summary>
        public double? QuantiteARealise { get; set; }

        /// <summary>
        /// NbrRessourcesToParam
        /// </summary>
        public int? NbrRessourcesToParam { get; set; }

        /// <summary>
        /// Obtient ou définit le type de la tâche (<see cref="Fred.Entities.TacheType"/>).
        /// </summary>
        public int TacheType { get; set; }

        /// <summary>
        ///   Obtient ou définit une valeur .
        /// </summary>
        public BudgetEnt Budget { get; set; }

        /// <summary>
        ///   Obtient ou définit une valeur .
        /// </summary>
        public int? BudgetId { get; set; }

        /// <summary>
        /// Child CommandeLignes where [FRED_COMMANDE_LIGNE].[TacheId] point to this entity (FK_COMMANDE_LIGNE_TACHE)
        /// </summary>
        public virtual System.Collections.Generic.ICollection<CommandeLigneEnt> CommandeLignes { get; set; } // FRED_COMMANDE_LIGNE.FK_COMMANDE_LIGNE_TACHE

        /// <summary>
        /// Child Depenses where [FRED_DEPENSE].[TacheId] point to this entity (FK_DEPENSE_TACHE)
        /// </summary>
        public virtual System.Collections.Generic.ICollection<DepenseAchatEnt> Depenses { get; set; } // FRED_DEPENSE.FK_DEPENSE_TACHE

        /// <summary>
        /// Child DepenseTemporaires where [FRED_DEPENSE_TEMPORAIRE].[TacheId] point to this entity (FK_DEPENSE_TEMPORAIRE_TACHE)
        /// </summary>
        public virtual System.Collections.Generic.ICollection<DepenseTemporaireEnt> DepenseTemporaires { get; set; } // FRED_DEPENSE_TEMPORAIRE.FK_DEPENSE_TEMPORAIRE_TACHE



        /// <summary>
        /// Child RapportLigneTaches where [FRED_RAPPORT_LIGNE_TACHE].[TacheId] point to this entity (FK_RAPPORT_LIGNE_TACHE_TACHE)
        /// </summary>
        public virtual System.Collections.Generic.ICollection<RapportLigneTacheEnt> RapportLigneTaches { get; set; } // FRED_RAPPORT_LIGNE_TACHE.FK_RAPPORT_LIGNE_TACHE_TACHE

        #region Ajout, Mise à jour et suppression avec l'auteur


        /// <summary>
        ///   Obtient ou définit la date de création
        /// </summary>
        public DateTime DateCreation
        {
            get
            {
                return DateTime.SpecifyKind(dateCreation, DateTimeKind.Utc);
            }
            set
            {
                dateCreation = DateTime.SpecifyKind(value, DateTimeKind.Utc);
            }
        }


        /// <summary>
        ///   Obtient ou définit la date de modification
        /// </summary>
        public DateTime? DateModification
        {
            get
            {
                return (dateModification.HasValue) ? DateTime.SpecifyKind(dateModification.Value, DateTimeKind.Utc) : default(DateTime?);
            }
            set
            {
                dateModification = (value.HasValue) ? DateTime.SpecifyKind(value.Value, DateTimeKind.Utc) : default(DateTime?);
            }
        }


        /// <summary>
        ///   Obtient ou définit la date de suppression
        /// </summary>
        public DateTime? DateSuppression
        {
            get
            {
                return (dateSuppression.HasValue) ? DateTime.SpecifyKind(dateSuppression.Value, DateTimeKind.Utc) : default(DateTime?);
            }
            set
            {
                dateSuppression = (value.HasValue) ? DateTime.SpecifyKind(value.Value, DateTimeKind.Utc) : default(DateTime?);
            }
        }

        /// <summary>
        ///   Obtient ou définit l'identifiant de l'auteur de la création
        /// </summary>
        public int? AuteurCreationId { get; set; }

        /// <summary>
        /// Parent Utilisateur pointed by [FRED_TACHE].([AuteurCreationId]) (FK_FRED_TACHE_AUTEUR_CREATION)
        /// </summary>
        public virtual UtilisateurEnt AuteurCreation { get; set; } // FK_FRED_TACHE_AUTEUR_CREATION

        /// <summary>
        ///   Obtient ou définit l'identifiant de l'auteur de la modification
        /// </summary>
        public int? AuteurModificationId { get; set; }

        /// <summary>
        /// Parent Utilisateur pointed by [FRED_TACHE].([AuteurModificationId]) (FK_FRED_TACHE_AUTEUR_MODIFICATION)
        /// </summary>
        public virtual UtilisateurEnt AuteurModification { get; set; } // FK_FRED_TACHE_AUTEUR_MODIFICATION

        /// <summary>
        ///   Obtient ou définit l'identifiant de l'auteur de la suppression
        /// </summary>
        public int? AuteurSuppressionId { get; set; }

        /// <summary>
        /// Parent Utilisateur pointed by [FRED_TACHE].([AuteurSuppressionId]) (FK_FRED_TACHE_AUTEUR_SUPPRESSION)
        /// </summary>
        public virtual UtilisateurEnt AuteurSuppression { get; set; } // FK_FRED_TACHE_AUTEUR_SUPPRESSION

        #endregion Ajout, Mise à jour et suppression avec l'auteur

        /// <summary>
        /// Clone
        /// </summary>
        /// <returns>Nouvelle référence</returns>
        public object Clone()
        {
            TacheEnt newTache = (TacheEnt)this.MemberwiseClone();

            if (this.RessourceTaches.Any())
            {
                newTache.RessourceTaches = new List<RessourceTacheEnt>();

                foreach (var resTache in this.RessourceTaches)
                {
                    newTache.RessourceTaches.Add((RessourceTacheEnt)resTache.Clone());
                }
            }

            return newTache;
        }

        // A faire : REMARQUE LORS DE LE MIGRATION CODE FIRST 
        // CECI EST DU CODE METIER CELA N A RIEN A FAIRE ICI : FAIRE UNE METHODE D EXTENSION OU CLASSE PARTIELLE
        /// <summary>
        /// Clean - retire toutes les dépendances pour insertion en base
        /// </summary>
        public void Clean()
        {
            this.TachesEnfants = null;
            this.Parent = null;

            if (this.RessourceTaches.Any())
            {
                foreach (var resTache in this.RessourceTaches)
                {
                    resTache.Clean();
                }
            }
        }
    }
}
