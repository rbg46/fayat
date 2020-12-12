using Fred.Entities.Commande;
using Fred.Entities.Depense;
using Fred.Entities.Facture;
using Fred.Entities.Groupe;
using Fred.Entities.Personnel.Interimaire;
using System;
using System.Collections.Generic;

namespace Fred.Entities.Referential
{
    /// <summary>
    /// Représente un fournisseur
    /// </summary>
    public class FournisseurEnt : IEquatable<FournisseurEnt>
    {
        private DateTime? dateCloture;
        private DateTime? dateOuverture;

        /// <summary>
        /// Obtient ou définit l'identifiant unique d'un fournisseur.
        /// </summary>
        public int FournisseurId { get; set; }

        /// <summary>
        /// Obtient ou définit l'identifiant unique d'un groupe.
        /// </summary>
        public int GroupeId { get; set; }

        /// <summary>
        /// Obtient ou définit l'otheret groupe attaché à un groupe
        /// </summary>
        public GroupeEnt Groupe { get; set; }

        /// <summary>
        /// Obtient ou définit le code d'un fournisseur.
        /// c'est à dire le code RAUX du compte tiers.
        /// Il n’est pas unique.
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Obtient ou définit le libellé d'un fournisseur.
        /// c'est à dire le nom du compte tiers importé d'Anaël (TNOM).
        /// </summary>
        public string Libelle { get; set; }

        /// <summary>
        /// Obtient ou définit l'Adresse du fournisseur cad
        /// c'est à dire l’Adresse de facturation du compte tiers importé d'Anaël (TADR1, TADR2 et TADR3).
        /// </summary>
        public string Adresse { get; set; }

        /// <summary>
        /// Obtient ou définit le code postal du fournisseur
        /// c'est à dire le code postal du compte tiers importé d'Anaël (TPOST).
        /// </summary>
        public string CodePostal { get; set; }

        /// <summary>
        /// Obtient ou définit la ville du fournisseur
        /// c'est à dire le nom de la ville du compte tiers importé d'Anaël (TVILLE).
        /// </summary>
        public string Ville { get; set; }

        /// <summary>
        /// Obtient ou définit le SIRET du fournisseur
        /// c'est à dire le code SIRET du compte tiers importé d'Anaël (TSIRET).
        /// </summary>
        public string SIRET { get; set; }

        /// <summary>
        /// Obtient ou définit le SIREN du fournisseur
        /// c'est à dire le code SIREN du compte tiers importé d'Anaël (TSIREN).
        /// </summary>
        public string SIREN { get; set; }

        /// <summary>
        /// Obtient ou définit le code TVA du fournisseur
        /// c'est à dire le code TVA importé d'Anaël
        /// </summary>
        public string CodeTVA { get; set; }

        /// <summary>
        /// Obtient ou définit le téléphone du fournisseur
        /// c'est à dire le numéro de téléphone du compte tiers importé d'Anaël (TTEL).
        /// </summary>
        public string Telephone { get; set; }

        /// <summary>
        /// Obtient ou définit le fax du fournisseur
        /// c'est à dire le numéro de fax du compte tiers importé d'Anaël (TFAX).
        /// </summary>
        public string Fax { get; set; }

        /// <summary>
        /// Obtient ou définit l'identifiant du pays du fournisseur  
        /// </summary>
        public int? PaysId { get; set; }

        /// <summary>
        /// Obtient ou définit le pays du fournisseur  
        /// </summary>
        public PaysEnt Pays { get; set; }

        /// <summary>
        /// Obtient ou définit l'email du fournisseur
        /// c'est à dire l’Adresse mail du compte tiers importé d'Anaël (TEMAIL).
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Obtient ou définit le mode de règlement du fournisseur
        /// c'est à dire le mode de règlement par défaut de la règle de gestion du compte tiers importé d'Anaël (TREGLT).
        /// </summary>
        public string ModeReglement { get; set; }

        /// <summary>
        /// Obtient ou définit la date d'ouverture du fournisseur
        /// c'est à dire la date d’ouverture définie dans la règle de gestion du compte tiers importé d'Anaël (TOUVJ, TOUVJ et
        /// TOUVJ).
        /// Si non renseigné, c’est la date du jour.
        /// </summary>
        public DateTime? DateOuverture
        {
            get
            {
                return (dateOuverture.HasValue) ? DateTime.SpecifyKind(dateOuverture.Value, DateTimeKind.Utc) : default(DateTime?);
            }
            set
            {
                dateOuverture = (value.HasValue) ? DateTime.SpecifyKind(value.Value, DateTimeKind.Utc) : default(DateTime?);
            }
        }

        /// <summary>
        /// Obtient ou définit la date de clôture du fournisseur
        /// c'est à dire la date de clôture définie dans la règle de gestion du compte tiers importé d'Anaël (TOUVJ, TOUVJ et
        /// TOUVJ).
        /// Si non renseigné, c’est la date du jour.
        /// </summary>
        public DateTime? DateCloture
        {
            get
            {
                return (dateCloture.HasValue) ? DateTime.SpecifyKind(dateCloture.Value, DateTimeKind.Utc) : default(DateTime?);
            }
            set
            {
                dateCloture = (value.HasValue) ? DateTime.SpecifyKind(value.Value, DateTimeKind.Utc) : default(DateTime?);
            }
        }

        /// <summary>
        /// Obtient ou définit le type de séquence du fournisseur
        /// c'est à dire le code fournisseur du compte tiers importé d'Anaël (RSEQ)
        /// </summary>
        public string TypeSequence { get; set; }

        /// <summary>
        /// Obtient ou définit le type du tiers importé d'Anaël du fournisseur
        /// - I pour intérim
        /// - L pour locatier
        /// </summary>
        public string TypeTiers { get; set; }

        /// <summary>
        /// Obtient ou définit le type du tiers importé d'Anaël du fournisseur
        /// c'est à dire  le code « F, C, C1, I » de la règle de gestion du compte tiers importé d'Anaël (TKLEPS).
        /// </summary>
        public string RegleGestion { get; set; }

        /// <summary>
        /// Obtient ou définit si le fournisseur est une profession libérale 
        /// </summary>
        public bool IsProfessionLiberale { get; set; }

        /// <summary>
        /// Obtient ou définit si un blocage manuel du contrat intérimaire
        /// </summary>
        public bool BlocageContratInterimaireManuel { get; set; }

        /// <summary>
        /// Obtient ou définit la réference du systeme intérimaire.
        /// </summary>
        public string ReferenceSystemeInterimaire { get; set; }

        /// <summary>
        /// Child AffectationInterimaires where [FRED_PERSONNEL_FOURNISSEUR_SOCIETE].[FournisseurId] point to this entity (FK_FRED_PERSONNEL_FOURNISSEUR_SOCIETE_FOURNISSEUR)
        /// </summary>
        public virtual ICollection<ContratInterimaireEnt> ContratInterimaires { get; set; }

        /// <summary>
        /// Child Commandes where [FRED_COMMANDE].[FournisseurId] point to this entity (FK_COMMANDE_FOURNISSEUR)
        /// </summary>
        public virtual ICollection<CommandeEnt> Commandes { get; set; }

        /// <summary>
        /// Child Depenses where [FRED_DEPENSE].[FournisseurId] point to this entity (FK_DEPENSE_FOURNISSEUR)
        /// </summary>
        public virtual ICollection<DepenseAchatEnt> Depenses { get; set; }

        /// <summary>
        /// Child DepenseTemporaires where [FRED_DEPENSE_TEMPORAIRE].[FournisseurId] point to this entity (FK_DEPENSE_TEMPORAIRE_FOURNISSEUR)
        /// </summary>
        public virtual ICollection<DepenseTemporaireEnt> DepenseTemporaires { get; set; }

        /// <summary>
        /// Child Factures where [FRED_FACTURE].[FournisseurId] point to this entity (FK_FACTURE_AR_FOURNISSEUR)
        /// </summary>
        public virtual ICollection<FactureEnt> Factures { get; set; }

        /// <summary>
        /// Obtient ou définit les agences associées
        /// </summary>
        public virtual List<AgenceEnt> Agences { get; set; }

        /// <summary>
        /// Determine si deux FournisseurEnt sont égaux
        /// </summary>
        /// <param name="other">le FournisseurEnt a comparé</param>
        /// <returns>true si égaux</returns>
        public bool Equals(FournisseurEnt other)
        {
            return Libelle == other.Libelle
               && Adresse == other.Adresse
               && CodePostal == other.CodePostal
               && Ville == other.Ville
               && SIRET == other.SIRET
               && SIREN == other.SIREN
               && Telephone == other.Telephone
               && Fax == other.Fax
               && Email == other.Email
               && TypeTiers?.Trim() == other.TypeTiers?.Trim()
               && DateCloture == other.DateCloture
               && DateOuverture == other.DateOuverture
               && ModeReglement == other.ModeReglement
               && RegleGestion == other.RegleGestion
               && PaysId == other.PaysId
               && IsProfessionLiberale == other.IsProfessionLiberale
               && CodeTVA == other.CodeTVA;
        }
    }
}
