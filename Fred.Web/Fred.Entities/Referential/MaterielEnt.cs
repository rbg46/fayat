using Fred.Entities.Commande;
using Fred.Entities.Moyen;
using Fred.Entities.Rapport;
using Fred.Entities.ReferentielFixe;
using Fred.Entities.Societe;
using Fred.Entities.Utilisateur;
using Fred.Entities.Valorisation;
using System;
using System.Collections.Generic;

namespace Fred.Entities.Referential
{
    /// <summary>
    ///   Represente un matériel
    /// </summary>
    public class MaterielEnt
    {
        private DateTime? dateSuppression;
        private DateTime? dateModification;
        private DateTime? dateCreation;
        private DateTime? dateDebutLocation;
        private DateTime? dateFinLocation;
        /// <summary>
        ///   Obtient ou définit l'identifiant unique d'un matériel.
        /// </summary>
        public int MaterielId { get; set; }

        /// <summary>
        ///   Obtient ou définit l'Id de la société
        /// </summary>
        public int SocieteId { get; set; }

        /// <summary>
        ///   Obtient ou définit la société du matériel
        /// </summary>
        public SocieteEnt Societe { get; set; }

        /// <summary>
        ///   Obtient ou définit l'Id de la RessourceId
        /// </summary>
        public int RessourceId { get; set; }

        /// <summary>
        ///   Obtient ou définit la Ressource du matériel
        /// </summary>
        public RessourceEnt Ressource { get; set; }

        /// <summary>
        ///   Obtient ou définit le code du matériel
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        ///   Obtient ou définit le libellé
        /// </summary>
        public string Libelle { get; set; }

        /// <summary>
        /// Obtient ou définit le libellé complet
        /// </summary>
        public string LibelleLong
        {
            get
            {
                if (Societe != null)
                {
                    return string.Concat(Societe.Code, " - ", Code, " - ", Libelle);
                }
                else
                {
                    return string.Concat(Code, " - ", Libelle);
                }
            }
        }

        /// <summary>
        ///   Obtient ou définit une valeur indiquant si Actif
        /// </summary>
        public bool Actif { get; set; }

        /// <summary>
        ///   Obtient ou définit l'identifiant de l'auteur de la création
        /// </summary>
        public int? AuteurCreationId { get; set; }

        /// <summary>
        ///   Obtient ou définit l'auteur de la création
        /// </summary>
        public UtilisateurEnt AuteurCreation { get; set; }

        /// <summary>
        ///   Obtient ou définit l'identifiant de l'auteur de la modification
        /// </summary>
        public int? AuteurModificationId { get; set; }

        /// <summary>
        ///   Obtient ou définit l'auteur de la modification
        /// </summary>
        public UtilisateurEnt AuteurModification { get; set; }

        /// <summary>
        ///   Obtient ou définit l'identifiant de l'auteur de la suppression
        /// </summary>
        public int? AuteurSuppressionId { get; set; }

        /// <summary>
        ///   Obtient ou définit l'auteur de la suppression
        /// </summary>
        public UtilisateurEnt AuteurSuppression { get; set; }

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
        ///  Obtient ou définit si le materiel est de type location.
        /// </summary>
        public bool MaterielLocation { get; set; }

        /// <summary>
        ///   Obtient ou définit l'identifiant du Fournisseur.
        /// </summary>
        public int? FournisseurId { get; set; }

        /// <summary>
        ///   Obtient ou définit le Fournisseur.
        /// </summary>
        public FournisseurEnt Fournisseur { get; set; }

        /// <summary>
        ///  Obtient ou définit la date de debut de location 
        /// </summary>
        public DateTime? DateDebutLocation
        {
            get
            {
                return (dateDebutLocation.HasValue) ? DateTime.SpecifyKind(dateDebutLocation.Value, DateTimeKind.Utc) : default(DateTime?);
            }
            set
            {
                dateDebutLocation = (value.HasValue) ? DateTime.SpecifyKind(value.Value, DateTimeKind.Utc) : default(DateTime?);
            }
        }

        /// <summary>
        ///   Obtient ou définit la date de fin de location
        /// </summary>
        public DateTime? DateFinLocation
        {
            get
            {
                return (dateFinLocation.HasValue) ? DateTime.SpecifyKind(dateFinLocation.Value, DateTimeKind.Utc) : default(DateTime?);
            }
            set
            {
                dateFinLocation = (value.HasValue) ? DateTime.SpecifyKind(value.Value, DateTimeKind.Utc) : default(DateTime?);
            }
        }

        /// <summary>
        /// Child RapportLignes where [FRED_RAPPORT_LIGNE].[MaterielId] point to this entity (FK_RAPPORT_LIGNE_MATERIEL)
        /// </summary>
        public virtual ICollection<RapportLigneEnt> RapportLignes { get; set; }

        /// <summary>
        /// Obtient ou définit le code de la classe Equipement STORM
        /// </summary>
        public string ClasseFamilleCode { get; set; }

        /// <summary>
        /// Obtient ou définit le libellé de la classe Equipement STORM
        /// </summary>
        public string ClasseFamilleLibelle { get; set; }

        /// <summary>
        /// Obtient ou définit si le materiel est STORM.
        /// </summary>
        public bool IsStorm { get; set; }

        /// <summary>
        /// Obtient ou définit le partenaire fabriquant machine 
        /// </summary>
        public string Fabriquant { get; set; }

        /// <summary>
        /// Obtient ou définit l'identification véhicule 
        /// </summary>
        public string VIN { get; set; }

        /// <summary>
        /// Obtient ou définit la date mise en service  
        /// </summary>
        public DateTime? DateMiseEnService { get; set; }

        /// <summary>
        /// Obtient ou définit l'immatriculation machine 
        /// </summary>
        public string Immatriculation { get; set; }

        /// <summary>
        /// Obtient ou définit la hauteur machine 
        /// </summary>
        public decimal DimensionH { get; set; }

        /// <summary>
        /// Obtient ou définit largeur machine 
        /// </summary>
        public decimal DimensionL { get; set; }

        /// <summary>
        /// Obtient ou définit longueur machine  
        /// </summary>
        public decimal Dimensiionl { get; set; }

        /// <summary>
        /// Obtient ou définit la puissance machine 
        /// </summary>
        public decimal Puissance { get; set; }

        /// <summary>
        /// Obtient ou définit l'unité de la puissance
        /// </summary>
        public string UnitePuissance { get; set; }

        /// <summary>
        /// Obtient ou définit l'unité de la dimention
        /// </summary>
        public string UniteDimension { get; set; }

        /// <summary>
        /// Obtient ou définit le le site où a été restitué le matériel.
        /// </summary>
        public string SiteRestitution { get; set; }

        /// <summary>
        ///   Obtient ou définit l'identifiant unique d'un Role.
        /// </summary>
        public int? EtablissementComptableId { get; set; }

        /// <summary>
        ///   Obtient ou définit le groupe associé
        /// </summary>
        public EtablissementComptableEnt EtablissementComptable { get; set; }

        /// <summary>
        /// Obtient ou définit le commentaire sur le material
        /// </summary>
        public string Commentaire { get; set; }

        /// <summary>
        /// Obtient ou définit l'identiant du site d'appartenance du material
        /// </summary>
        public int? SiteAppartenanceId { get; set; }

        /// <summary>
        /// Obtient ou définit l'entité du site d'appartenance du material
        /// </summary>
        public SiteEnt SiteAppartenance { get; set; }

        /// <summary>
        /// Obtient ou définit si le moyen est créer en location
        /// </summary>
        public bool IsLocation { get; set; }

        /// <summary>
        /// Obtient ou définit si le moyen est importé
        /// </summary>
        public bool IsImported { get; set; }

        /// <summary>
        /// Collection du materiel locations
        /// </summary>
        public virtual ICollection<MaterielLocationEnt> MaterielLocations { get; set; }

        /// <summary>
        /// Obtient ou définit les lignes de commandes lié au matériel externe
        /// </summary>
        public virtual ICollection<CommandeLigneEnt> CommandeLignes { get; set; }

        /// <summary>
        /// Liste des valorisations du matériel
        /// </summary>
        public virtual ICollection<ValorisationEnt> Valorisations { get; set; }
    }
}
