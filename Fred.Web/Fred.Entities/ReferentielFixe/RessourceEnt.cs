using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using System.Linq;
using Fred.Entities.Budget;
using Fred.Entities.Carburant;
using Fred.Entities.CI;
using Fred.Entities.Commande;
using Fred.Entities.Depense;
using Fred.Entities.Personnel;
using Fred.Entities.Referential;
using Fred.Entities.ReferentielEtendu;
using Fred.Entities.Utilisateur;

namespace Fred.Entities.ReferentielFixe
{
    /// <summary>
    ///   Représente une ressource.
    /// </summary>
    [DebuggerDisplay("[{Code} - {Libelle}")]
    public class RessourceEnt : ICloneable
    {
        private DateTime? dateSuppression;
        private DateTime? dateModification;
        private DateTime? dateCreation;

        /// <summary>
        ///   Obtient ou définit l'identifiant unique d'une ressource.
        /// </summary>
        public int RessourceId { get; set; }

        /// <summary>
        ///   Obtient ou définit l'identifiant unique d'un sous-chapitre.
        /// </summary>
        public int SousChapitreId { get; set; }

        /// <summary>
        ///   Obtient ou définit l'objet groupe attaché à un sous-chapitre
        /// </summary>
        public SousChapitreEnt SousChapitre { get; set; }

        /// <summary>
        ///   Obtient ou définit le code d'une ressource.
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        ///   Obtient ou définit le libellé d'une ressource.
        /// </summary>
        public string Libelle { get; set; }

        /// <summary>
        ///   Obtient une concaténation du code et du libellé.
        /// </summary>
        public string CodeLibelle => Code + " - " + Libelle;

        /// <summary>
        ///   Obtient ou définit le libellé d'une ressource.
        /// </summary>
        public bool Active { get; set; } = true;

        /// <summary>
        ///   Obtient ou définit l
        /// </summary>
        public int? TypeRessourceId { get; set; }

        /// <summary>
        ///   Obtient ou définit le libellé d'une ressource.
        /// </summary>
        public TypeRessourceEnt TypeRessource { get; set; }

        /// <summary>
        ///   Obtient ou définit la date de création
        /// </summary>
        public DateTime? DateCreation
        {
            get
            {
                return (dateCreation.HasValue) ? DateTime.SpecifyKind(dateCreation.Value, DateTimeKind.Utc) : default(DateTime?);
            }
            set
            {
                dateCreation = (value.HasValue) ? DateTime.SpecifyKind(value.Value, DateTimeKind.Utc) : default(DateTime?);
            }
        }

        /// <summary>
        ///   Obtient ou définit l'id de l'auteur de la création
        /// </summary>
        public int? AuteurCreationId { get; set; }

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
        ///   Obtient ou définit l'id de l'auteur de la modification
        /// </summary>
        public int? AuteurModificationId { get; set; }

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
        ///   Obtient ou définit l'id de l'auteur de la suppression
        /// </summary>
        public int? AuteurSuppressionId { get; set; }

        /// <summary>
        ///   Obtient ou définit l'identifiant du carburant
        /// </summary>
        public int? CarburantId { get; set; }

        /// <summary>
        ///   Obtient ou définit le carburant
        /// </summary>
        public CarburantEnt Carburant { get; set; }

        /// <summary>
        ///   Obtient ou définit la consommation d'une ressource matérielle
        /// </summary>
        public decimal? Consommation { get; set; }

        /// <summary>
        ///   Obtient ou définit l'ID de la ressource Parent.
        /// </summary>
        public int? ParentId { get; set; }

        /// <summary>
        ///   Obtient ou définit la ressource Parent .
        /// </summary>
        public RessourceEnt Parent { get; set; }

        /// <summary>
        /// Permet de savoir si la ressource est spécifique ci
        /// </summary>
        public bool IsRessourceSpecifiqueCi { get; set; } = false;

        /// <summary>
        /// Obtient ou définit l'identifiant ressource rattachée pour les ressources spécifiques ci
        /// </summary>
        public int? RessourceRattachementId { get; set; }

        /// <summary>
        /// Obtient ou définit la ressource rattachée pour les ressources spécifiques ci
        /// </summary>
        public RessourceEnt RessourceRattachement { get; set; }

        /// <summary>
        /// Obtient ou définit une valeur indiquant si la ressource est recommandéee.
        /// </summary>
        public bool IsRecommandee { get; set; }
        /// <summary>
        /// Obtient ou définit l'identifiant du ci
        /// </summary>
        public int? SpecifiqueCiId { get; set; }

        /// <summary>
        /// Obtient ou définit le ci
        /// </summary>
        public CIEnt SpecifiqueCi { get; set; }

        /// <summary>
        /// Obtient ou définit les Keywords
        /// </summary>
        [Column("Keywords")]
        public string Keywords { get; set; }


        /// <summary>
        ///   Obtient ou définit la liste des ressources enfants
        /// </summary>
        public ICollection<RessourceEnt> RessourcesEnfants { get; set; }

        /// <summary>
        /// Obtient ou définit le référentiel étendu associé
        /// </summary>
        public ICollection<ReferentielEtenduEnt> ReferentielEtendus { get; set; }

        /// <summary>
        ///   Obtient ou définit la liste des associations avec la liste des tâches
        /// </summary>
        public ICollection<RessourceTacheEnt> RessourceTaches { get; set; }

        /// <summary>
        ///   Obtient ou définit la liste des associations avec les CI
        /// </summary>
        public ICollection<CIRessourceEnt> CIRessources { get; set; }

        /// <summary>
        ///   Obtient ou définit la liste des ressources rattachées enfants
        /// </summary>
        public ICollection<RessourceEnt> RessourcesRattachementsEnfants { get; set; }

        ///////////////////////////////////////////////////////////////////////////
        // AJOUT LORS DE LE MIGRATION CODE FIRST 
        ///////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Child CommandeLignes where [FRED_COMMANDE_LIGNE].[RessourceId] point to this entity (FK_COMMANDE_LIGNE_RESSOURCE)
        /// </summary>
        public virtual ICollection<CommandeLigneEnt> CommandeLignes { get; set; } // FRED_COMMANDE_LIGNE.FK_COMMANDE_LIGNE_RESSOURCE

        /// <summary>
        /// Child Depenses where [FRED_DEPENSE].[RessourceId] point to this entity (FK_DEPENSE_RESSOURCE)
        /// </summary>
        public virtual ICollection<DepenseAchatEnt> Depenses { get; set; } // FRED_DEPENSE.FK_DEPENSE_RESSOURCE

        /// <summary>
        /// Child DepenseTemporaires where [FRED_DEPENSE_TEMPORAIRE].[RessourceId] point to this entity (FK_DEPENSE_TEMPORAIRE_RESSOURCE)
        /// </summary>
        public virtual ICollection<DepenseTemporaireEnt> DepenseTemporaires { get; set; } // FRED_DEPENSE_TEMPORAIRE.FK_DEPENSE_TEMPORAIRE_RESSOURCE

        /// <summary>
        /// Child Materiels where [FRED_MATERIEL].[RessourceId] point to this entity (FK_FRED_MATERIEL_RESSOURCE)
        /// </summary>
        public virtual ICollection<MaterielEnt> Materiels { get; set; } // FRED_MATERIEL.FK_FRED_MATERIEL_RESSOURCE

        /// <summary>
        /// Child Personnels where [FRED_PERSONNEL].[RessourceId] point to this entity (FK_PERSONNEL_RESSOURCE)
        /// </summary>
        public virtual ICollection<PersonnelEnt> Personnels { get; set; } // FRED_PERSONNEL.FK_PERSONNEL_RESSOURCE

        /// <summary>
        /// Parent Utilisateur pointed by [FRED_RESSOURCE].([AuteurCreationId]) (FK_FRED_RESSOURCE_AUTEUR_CREATION_UTILISATEUR)
        /// </summary>
        [ForeignKey("AuteurCreationId")]
        public virtual UtilisateurEnt AuteurCreation { get; set; } // FK_FRED_RESSOURCE_AUTEUR_CREATION_UTILISATEUR

        /// <summary>
        /// Parent Utilisateur pointed by [FRED_RESSOURCE].([AuteurModificationId]) (FK_FRED_RESSOURCE_AUTEUR_MODIFICATION_UTILISATEUR)
        /// </summary>
        [ForeignKey("AuteurModificationId")]
        public virtual UtilisateurEnt AuteurModification { get; set; } // FK_FRED_RESSOURCE_AUTEUR_MODIFICATION_UTILISATEUR

        /// <summary>
        /// Parent Utilisateur pointed by [FRED_RESSOURCE].([AuteurSuppressionId]) (FK_FRED_RESSOURCE_AUTEUR_SUPPRESSION_UTILISATEUR)
        /// </summary>    
        [ForeignKey("AuteurSuppressionId")]
        public virtual UtilisateurEnt AuteurSuppression { get; set; } // FK_FRED_RESSOURCE_AUTEUR_SUPPRESSION_UTILISATEUR

        // CECI EST DU CODE METIER CELA N A RIEN A FAIRE ICI : FAIRE UNE METHODE D EXTENSION OU CLASSE PARTIELLE
        /// <summary>
        /// Clone
        /// </summary>
        /// <returns>Nouvelle référence</returns>
        public object Clone()
        {
            RessourceEnt newRess = (RessourceEnt)this.MemberwiseClone();

            if (this.ReferentielEtendus.Any())
            {
                newRess.ReferentielEtendus = new List<ReferentielEtenduEnt>();

                foreach (var refEt in this.ReferentielEtendus)
                {
                    newRess.ReferentielEtendus.Add((ReferentielEtenduEnt)refEt.Clone());
                }
            }

            return newRess;
        }

        // CECI EST DU CODE METIER CELA N A RIEN A FAIRE ICI : FAIRE UNE METHODE D EXTENSION OU CLASSE PARTIELLE
        /// <summary>
        /// Clean - retire toutes les dépendances pour insertion en base
        /// </summary>
        public void Clean()
        {
            this.CIRessources = null;
            this.RessourcesEnfants = null;
            this.RessourceTaches = null;
            this.RessourceId = 0;
            this.SousChapitre = null;
            this.TypeRessource = null;

            if (this.ReferentielEtendus.Any())
            {
                foreach (var refEt in this.ReferentielEtendus)
                {
                    refEt.Clean();
                }
            }
        }
    }
}
