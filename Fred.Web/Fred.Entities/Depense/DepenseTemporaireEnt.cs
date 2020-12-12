using Fred.Entities.CI;
using Fred.Entities.Commande;
using Fred.Entities.Facture;
using Fred.Entities.Referential;
using Fred.Entities.ReferentielFixe;
using Fred.Entities.Utilisateur;
using System;
using System.Collections.Generic;

namespace Fred.Entities.Depense
{
    /// <summary>
    ///   Représente une dépense.
    /// </summary>
    public class DepenseTemporaireEnt
    {
        private DateTime? date;
        private DateTime? dateCreation;

        // REMARQUE LORS DE LE MIGRATION CODE FIRST 
        // DENORMALISATION : L'ATTRIBUT '[Column]  EST DIFFERENT DU NOM DE LA PROPRIETE.
        /// <summary>
        ///   Obtient ou définit l'identifiant unique d'une dépense temporaire.
        /// </summary>
        public int DepenseId { get; set; }

        /// <summary>
        ///   Obtient ou définit la ligne de commande dont dépend la dépense.
        /// </summary>
        public CommandeLigneEnt CommandeLigne { get; set; }

        /// <summary>
        ///   Obtient ou définit l'identifiant unique de la ligne de commande d'une dépense.
        /// </summary>
        public int? CommandeLigneId { get; set; }

        /// <summary>
        ///   Obtient ou définit l'identifiant unique de l'affaire d'une dépense.
        /// </summary>
        public CIEnt CI { get; set; }

        /// <summary>
        ///   Obtient ou définit l'affaire d'une dépense.
        /// </summary>
        public int? CiId { get; set; }

        /// <summary>
        ///   Obtient ou définit l'identifiant unique du fournisseur d'une dépense.
        /// </summary>
        public FournisseurEnt Fournisseur { get; set; }

        /// <summary>
        ///   Obtient ou définit le fournisseur d'une dépense.
        /// </summary>
        public int? FournisseurId { get; set; }

        /// <summary>
        ///   Obtient ou définit le libellé d'une dépense.
        /// </summary>
        public string Libelle { get; set; }

        /// <summary>
        ///   Obtient ou définit l'Id de la tâche d'une dépense.
        /// </summary>
        public int? TacheId { get; set; }

        /// <summary>
        ///   Obtient ou définit l'objet tache
        /// </summary>
        public TacheEnt Tache { get; set; }

        /// <summary>
        ///   Obtient ou définit l'Id de la tâche d'une dépense.
        /// </summary>
        public int? RessourceId { get; set; }

        /// <summary>
        ///   Obtient ou définit l'objet ressource
        /// </summary>
        public RessourceEnt Ressource { get; set; }

        /// <summary>
        ///   Obtient ou définit l'identifiant unique de la ligne de commande d'une dépense.
        /// </summary>
        public string Commentaire { get; set; }

        /// <summary>
        ///   Obtient ou définit la quantité d'une dépense.
        /// </summary>
        public decimal Quantite { get; set; }

        /// <summary>
        ///   Obtient ou définit le prix unitaire d'une dépense.
        /// </summary>
        public decimal PUHT { get; set; }

        /// <summary>
        ///   Obtient ou définit l'identifiant de l'unité
        /// </summary>
        public int? UniteId { get; set; }

        /// <summary>
        ///   Obtient ou définit l'unité
        /// </summary>
        public UniteEnt Unite { get; set; }

        /// <summary>
        ///   Obtient ou définit la date de la dépense.
        /// </summary>
        public DateTime? Date
        {
            get
            {
                return (date.HasValue) ? DateTime.SpecifyKind(date.Value, DateTimeKind.Utc) : default(DateTime?);
            }
            set
            {
                date = (value.HasValue) ? DateTime.SpecifyKind(value.Value, DateTimeKind.Utc) : default(DateTime?);
            }
        }

        /// <summary>
        ///   Obtient ou définit la date de création de la dépense.
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
        ///   Obtient ou définit l'ID de la personne ayant créé la dépense.
        /// </summary>
        public int? AuteurCreationId { get; set; }

        /// <summary>
        ///   Obtient ou définit l'entité du membre du personnel ayant créé la dépense
        /// </summary>
        public UtilisateurEnt AuteurCreation { get; set; } = null;

        /// <summary>
        ///   Obtient ou définit l'identifiant de la devise associée à une dépense.
        /// </summary>
        public int? DeviseId { get; set; }

        /// <summary>
        ///   Obtient ou définit la devise associée à une dépense.
        /// </summary>
        public DeviseEnt Devise { get; set; }

        /// <summary>
        ///   Obtient ou définit le numéro du bon de livraison associé à une dépense.
        /// </summary>
        public string NumeroBL { get; set; }

        /// <summary>
        ///   Obtient ou définit l'identifiant unique de la dépense d'origine
        /// </summary>
        public int? DepenseOrigineId { get; set; }

        /// <summary>
        ///   Obtient ou définit la dépense d'origine
        /// </summary>
        public DepenseAchatEnt DepenseOrigine { get; set; }

        /// <summary>
        ///   Obtient ou définit l'identifiant unique de la dépense parent
        /// </summary>
        public int? DepenseParentId { get; set; }

        /// <summary>
        ///   Obtient ou définit la dépense parent
        /// </summary>
        public DepenseAchatEnt DepenseParent { get; set; }

        /// <summary>
        ///   Obtient ou définit le type de dépense
        /// </summary>
        public string TypeDepense { get; set; }

        /// <summary>
        ///   Obtient ou définit l'identifiant unique de la ligne de facture
        /// </summary>
        public int? FactureLigneId { get; set; }

        /// <summary>
        ///   Obtient ou définit la ligne de facture
        /// </summary>
        public FactureLigneEnt FactureLigne { get; set; }

        /// <summary>
        ///   Obtient ou définit l'identifiant unique de la facture
        /// </summary>
        public int? FactureId { get; set; }

        /// <summary>
        ///   Obtient ou définit la facture
        /// </summary>
        public FactureEnt Facture { get; set; }

        /// <summary>
        ///   Obtient ou définit la dépense temporaire si oui ou non elle peut être rapprochée par l'utilisateur courant
        /// </summary>
        public bool RapprochableParUserCourant { get; set; }

        /// <summary>
        ///   Obtient le montant HT de la dépense
        /// </summary>
        public decimal MontantHT => Quantite * PUHT;

        /// <summary>
        ///   Obtient ou définit la liste des erreurs de la dépense détectées dans le manager
        /// </summary>
        public ICollection<string> ListErreurs { get; set; }

        // LE CHAMP EST UNIQUEMENT DANS LA BASE DE DONNE, AVANT LA MIGRATION,
        // JE LE RAJOUTE CAR IL PEUT ETRE PEUPLE PAR LE JS.
        /// <summary>
        /// Date de reception
        /// </summary>
        public DateTime? DateReception { get; set; }
    }
}