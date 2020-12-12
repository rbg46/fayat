using Fred.Entities.Depense;
using Fred.Entities.Journal;
using Fred.Entities.Referential;
using Fred.Entities.Societe;
using Fred.Entities.Utilisateur;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fred.Entities.Facture
{
    /// <summary>
    ///   Représente une fonctionnalité
    /// </summary>
    public class FactureEnt
    {
        private DateTime? dateComptable;
        private DateTime? dateImport;
        private DateTime? dateSuppression;
        private DateTime? dateGestion;
        private DateTime? dateEcheance;
        private DateTime? dateFacture;
        private DateTime? dateCloture;
        private DateTime? dateCreation;
        private DateTime? dateModification;
        private DateTime? dateRapprochement;

        /// <summary>
        ///   Obtient ou définit l'identifiant unique d'une facture.
        /// </summary>
        public int FactureId { get; set; }

        /// <summary>
        ///   Obtient ou définit l'identifiant de la societe
        /// </summary>
        public int? SocieteId { get; set; }

        /// <summary>
        ///   Obtient ou définit la societe
        /// </summary>
        public SocieteEnt Societe { get; set; }

        /// <summary>
        ///   Obtient ou définit l'identifiant du fournisseur
        /// </summary>
        public int? FournisseurId { get; set; }

        /// <summary>
        ///   Obtient ou définit le fournisseur
        /// </summary>
        public FournisseurEnt Fournisseur { get; set; }

        /// <summary>
        ///   Obtient ou définit l'identifiant de l'établissement
        /// </summary>
        public int? EtablissementId { get; set; }

        /// <summary>
        ///   Obtient ou définit l'établissement
        /// </summary>
        public EtablissementComptableEnt Etablissement { get; set; }

        /// <summary>
        ///   Obtient ou définit le numéro de facture
        /// </summary>
        public string NoFacture { get; set; }

        /// <summary>
        ///   Obtient ou définit l'identifiant du journal
        /// </summary>
        public int? JournalId { get; set; }

        /// <summary>
        ///   Obtient ou définit le journal
        /// </summary>
        public JournalEnt Journal { get; set; }

        /// <summary>
        ///   Obtient ou définit le commentaire
        /// </summary>
        public string Commentaire { get; set; }

        /// <summary>
        ///   Obtient ou définit le Numéro de bon de livraison
        /// </summary>
        public string NoBonlivraison { get; set; }

        /// <summary>
        ///   Obtient ou définit le numéro de bon de commande
        /// </summary>
        public string NoBonCommande { get; set; }

        /// <summary>
        ///   Obtient ou définit le Type de fournisseur
        /// </summary>
        public string Typefournisseur { get; set; }

        /// <summary>
        ///   Obtient ou définit le compte fournisseur
        /// </summary>
        public string CompteFournisseur { get; set; }

        /// <summary>
        ///   Obtient ou définit l'identifiant de la devise
        /// </summary>
        public int? DeviseId { get; set; }

        /// <summary>
        ///   Obtient ou définit la devise
        /// </summary>
        public DeviseEnt Devise { get; set; }

        /// <summary>
        ///   Obtient ou définit la date comptable
        /// </summary>
        public DateTime? DateComptable
        {
            get
            {
                return (dateComptable.HasValue) ? DateTime.SpecifyKind(dateComptable.Value, DateTimeKind.Utc) : default(DateTime?);
            }
            set
            {
                dateComptable = (value.HasValue) ? DateTime.SpecifyKind(value.Value, DateTimeKind.Utc) : default(DateTime?);
            }
        }

        /// <summary>
        ///   Obtient ou définit la date de gestion
        /// </summary>
        public DateTime? DateGestion
        {
            get
            {
                return (dateGestion.HasValue) ? DateTime.SpecifyKind(dateGestion.Value, DateTimeKind.Utc) : default(DateTime?);
            }
            set
            {
                dateGestion = (value.HasValue) ? DateTime.SpecifyKind(value.Value, DateTimeKind.Utc) : default(DateTime?);
            }
        }

        /// <summary>
        ///   Obtient ou définit la date de facturation
        /// </summary>
        public DateTime? DateFacture
        {
            get
            {
                return (dateFacture.HasValue) ? DateTime.SpecifyKind(dateFacture.Value, DateTimeKind.Utc) : default(DateTime?);
            }
            set
            {
                dateFacture = (value.HasValue) ? DateTime.SpecifyKind(value.Value, DateTimeKind.Utc) : default(DateTime?);
            }
        }

        /// <summary>
        ///   Obtient ou définit la date d'échéance
        /// </summary>
        public DateTime? DateEcheance
        {
            get
            {
                return (dateEcheance.HasValue) ? DateTime.SpecifyKind(dateEcheance.Value, DateTimeKind.Utc) : default(DateTime?);
            }
            set
            {
                dateEcheance = (value.HasValue) ? DateTime.SpecifyKind(value.Value, DateTimeKind.Utc) : default(DateTime?);
            }
        }

        /// <summary>
        ///   Obtient ou définit la date de clôture
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
        ///   Obtient ou définit l'identifiant de l'auteur de la clôture
        /// </summary>
        public int? AuteurClotureId { get; set; }

        /// <summary>
        ///   Obtient ou définit l'auteur de la clôture
        /// </summary>
        public UtilisateurEnt AuteurCloture { get; set; }

        /// <summary>
        ///   Obtient ou définit le numéro de facture du fournisseur
        /// </summary>
        public string NoFactureFournisseur { get; set; }

        /// <summary>
        ///   Obtient ou définit le numéro FMFI
        /// </summary>
        public string NoFMFI { get; set; }

        /// <summary>
        ///   Obtient ou définit le mode de règlement
        /// </summary>
        public string ModeReglement { get; set; }

        /// <summary>
        ///   Obtient ou définit le folio ou le trigramme utilisateur
        /// </summary>
        public string Folio { get; set; }

        /// <summary>
        ///   Obtient ou définit le montant HT
        /// </summary>
        public decimal? MontantHT { get; set; }

        /// <summary>
        ///   Obtient ou définit le montant de TVA
        /// </summary>
        public decimal? MontantTVA { get; set; }

        /// <summary>
        ///   Obtient ou définit le montant TTC
        /// </summary>
        public decimal MontantTTC { get; set; }

        /// <summary>
        ///   Obtient ou définit le compte gnénéral
        /// </summary>
        public string CompteGeneral { get; set; }

        /// <summary>
        ///   Obtient ou définit la date d'import
        /// </summary>
        public DateTime? DateImport
        {
            get
            {
                return (dateImport.HasValue) ? DateTime.SpecifyKind(dateImport.Value, DateTimeKind.Utc) : default(DateTime?);
            }
            set
            {
                dateImport = (value.HasValue) ? DateTime.SpecifyKind(value.Value, DateTimeKind.Utc) : default(DateTime?);
            }
        }

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
        ///   Obtient ou définit l'identifiant de l'utilisateur ayant fait la création
        /// </summary>
        public int? UtilisateurCreationId { get; set; }

        /// <summary>
        ///   Obtient ou définit l'utilisateur ayant fait la création
        /// </summary>
        public UtilisateurEnt UtilisateurCreation { get; set; }

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
        ///   Obtient ou définit l'identifiant de l'utilisateur ayant fait la modification
        /// </summary>
        public int? UtilisateurModificationId { get; set; }

        /// <summary>
        ///   Obtient ou définit l'utilisateur ayant fait la modification
        /// </summary>
        public UtilisateurEnt UtilisateurModification { get; set; }

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
        ///   Obtient ou définit l'id de l'utilisateur ayant fait la suppression
        /// </summary>
        public int? UtilisateurSupressionId { get; set; }

        /// <summary>
        ///   Obtient ou définit l'utilisateur ayant fait la suppression
        /// </summary>
        public UtilisateurEnt UtilisateurSupression { get; set; }

        /// <summary>
        ///   Obtient ou définit la date du rapprochement
        /// </summary>
        public DateTime? DateRapprochement
        {
            get
            {
                return (dateRapprochement.HasValue) ? DateTime.SpecifyKind(dateRapprochement.Value, DateTimeKind.Utc) : default(DateTime?);
            }
            set
            {
                dateRapprochement = (value.HasValue) ? DateTime.SpecifyKind(value.Value, DateTimeKind.Utc) : default(DateTime?);
            }
        }

        /// <summary>
        ///   Obtient ou définit l'identifiant unique de l'auteur du rapprochement
        /// </summary>
        public int? AuteurRapprochementId { get; set; }

        /// <summary>
        ///   Obtient ou définit l'utilisateur ayant fait le rapprochement
        /// </summary>
        public UtilisateurEnt AuteurRapprochement { get; set; }

        /// <summary>
        ///   Obtient ou définit la facture si elle est cachée ou non
        /// </summary>
        public bool Cachee { get; set; }

        /// <summary>
        ///   Obtient ou définit la liste des lignes facture AR
        /// </summary>
        public ICollection<FactureLigneEnt> ListLigneFacture { get; set; }

        /// <summary>
        ///   Obtient ou définit la facture si oui ou non peut être rapprochée par l'utilisateur courant
        /// </summary>
        public bool RapprochableParUserCourant { get; set; }

        /// <summary>
        ///   Obtient ou définit le CodeCI de la facture (CodeCI ou MultiCI)
        /// </summary>
        public string CICode { get; set; }

        /// <summary>
        ///   Obtient ou définit la liste des Code-Libelle des CI d'une facture
        /// </summary>
        public ICollection<string> CiCodeLibelles { get; set; }

        /// <summary>
        ///   Obtient ou définit l'Url du scan de la facture
        /// </summary>
        public string ScanUrl { get; set; }

        /// <summary>
        /// Child DepenseTemporaires where [FRED_DEPENSE_TEMPORAIRE].[FactureId] point to this entity (FK_DEPENSE_TEMPORAIRE_FACTURE)
        /// </summary>
        public virtual ICollection<DepenseTemporaireEnt> DepenseTemporaires { get; set; }

        /// <summary>
        ///   Redefinition de la méthode ToString()
        /// </summary>
        /// <returns>Renvois l'objet courant au format texte</returns>
        public override string ToString()
        {
            StringBuilder resu = new StringBuilder()
              .Append("Commentaire - ").AppendLine(Commentaire)
              .Append("CompteFournisseur - ").AppendLine(CompteFournisseur)
              .Append("CompteGeneral - ").AppendLine(CompteGeneral)
              .Append("DateComptable - ").Append(DateComptable).AppendLine()
              .Append("DateCreation - ").Append(DateCreation).AppendLine()
              .Append("DateEcheance - ").Append(DateEcheance).AppendLine()
              .Append("DateFacture - ").Append(DateFacture).AppendLine()
              .Append("DateGestion - ").Append(DateGestion).AppendLine()
              .AppendFormat("DateImport - {0}", DateImport).AppendLine()
              .AppendFormat("DeviseId - {0}", DeviseId).AppendLine()
              .AppendFormat("EtablissementId - {0}", EtablissementId).AppendLine()
              .AppendFormat("FactureId - {0}", FactureId).AppendLine()
              .Append("Folio - ").AppendLine(Folio)
              .AppendFormat("FournisseurId - {0}", FournisseurId).AppendLine()
              .AppendFormat("JournalId - {0}", JournalId).AppendLine()
              .Append("ModeReglement - ").AppendLine(ModeReglement)
              .AppendFormat("MontantHT - {0}", MontantHT).AppendLine()
              .AppendFormat("MontantTTC - {0}", MontantTTC).AppendLine()
              .AppendFormat("MontantTVA - {0}", MontantTVA).AppendLine()
              .Append("NoBonCommande - ").AppendLine(NoBonCommande)
              .Append("NoBonlivraison - ").AppendLine(NoBonlivraison)
              .Append("NoFacture - ").AppendLine(NoFacture)
              .Append("NoFactureFournisseur - ").AppendLine(NoFactureFournisseur)
              .Append("NoFMFI - ").AppendLine(NoFMFI)
              .AppendFormat("SocieteId - {0}", SocieteId).AppendLine()
              .Append("Typefournisseur - ").AppendLine(Typefournisseur);

            return resu.ToString();
        }
    }
}