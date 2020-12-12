using Fred.Web.Models.Groupe;
using System;
using System.Collections.Generic;

namespace Fred.Web.Models.Referential
{
    /// <summary>
    /// Représente un fournisseur
    /// </summary>
    public class FournisseurModel
    {
        /// <summary>
        /// Obtient ou définit l'identifiant unique d'un fournisseur.
        /// </summary>
        public int FournisseurId { get; set; }

        /// <summary>
        /// Obtient ou définit l'identifiant unique d'un groupe.
        /// </summary>

        public int GroupeId { get; set; }

        /// <summary>
        /// Obtient ou définit l'objet groupe attaché à un groupe
        /// </summary>
        public GroupeModel Groupe { get; set; }

        /// <summary>
        /// Obtient ou définit le code d'un fournisseur.
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Obtient ou définit le libellé d'un fournisseur.
        /// </summary>
        public string Libelle { get; set; }

        /// <summary>
        /// Obtient ou définit l'Adresse du fournisseur cad
        ///  c'est à dire l’Adresse de facturation du compte tiers importé d'Anaël (TADR1, TADR2 et TADR3).
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
        /// Obtient ou définit le téléphone du fournisseur
        ///  c'est à dire le numéro de téléphone du compte tiers importé d'Anaël (TTEL).
        /// </summary>
        public string Telephone { get; set; }

        /// <summary>
        /// Obtient ou définit le fax du fournisseur
        /// c'est à dire le numéro de fax du compte tiers importé d'Anaël (TFAX).
        /// </summary>
        public string Fax { get; set; }

        /// <summary>
        /// Obtient ou définit le code pays du fournisseur
        ///  c'est à dire le code du pays du compte tiers importé d'Anaël (TPAYS).
        /// </summary>
        public string CodePays => Pays?.Code;

        /// <summary>
        ///   Obtient ou définit l'identifiant du pays du fournisseur
        /// </summary>
        public int? PaysId { get; set; }

        /// <summary>
        ///   Obtient ou définit le pays du fournisseur
        /// </summary>
        public PaysModel Pays { get; set; }

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
        /// c'est à dire la date d’ouverture définie dans la règle de gestion du compte tiers importé d'Anaël (TOUVJ, TOUVJ et TOUVJ). 
        /// Si non renseigné, c’est la date du jour.
        /// </summary>
        public DateTime? DateOuverture { get; set; }

        /// <summary>
        /// Obtient ou définit la date de clôture du fournisseur
        /// c'est à dire la date de clôture définie dans la règle de gestion du compte tiers importé d'Anaël (TOUVJ, TOUVJ et TOUVJ). 
        /// Si non renseigné, c’est la date du jour.
        /// </summary>
        public DateTime? DateCloture { get; set; }

        /// <summary>
        /// Obtient ou définit le type de séquence du fournisseur
        /// c'est à dire le code fournisseur du compte tiers importé d'Anaël (RSEQ)
        /// </summary>
        public string TypeSequence { get; set; }

        /// <summary>
        /// Obtient ou définit le type du tiers importé d'Anaël du fournisseur
        /// c'est à dire  le code « F, C, I » de la règle de gestion du compte tiers importé d'Anaël (TKLEPS).
        /// </summary>
        public string TypeTiers { get; set; }

        /// <summary>
        ///   Obtient ou définit la règle de gestion du fournisseur (F: fournisseur, C: client, C1:? ,I: individuel) 
        /// </summary>
        public string RegleGestion { get; set; }

        /// <summary>
        ///   Obtient ou définit la liste des agences
        /// </summary>
        public List<AgenceModel> Agences { get; set; }

        /// <summary>
        /// Obtient une concaténation du code et du libellé
        /// </summary>
        public string CodeLibelle => !string.IsNullOrEmpty(this.Code) ? this.Code + " - " + this.Libelle : string.Empty;

        /// <summary>
        /// Obtient une concaténation du code et du libellé
        /// </summary>
        public string IdRef => this.FournisseurId.ToString();

        /// <summary>
        /// Obtient une concaténation du code et du libellé
        /// </summary>
        public string LibelleRef => !string.IsNullOrEmpty(this.Code) ? this.Code + " (" + this.TypeSequence + ") " + this.Libelle : string.Empty;

        /// <summary>
        /// Obtient une concaténation du code et du libellé
        /// </summary>
        public string CodeRef => !string.IsNullOrEmpty(this.Code) ? this.Code + " (" + this.TypeSequence + ")" : string.Empty;

        /// <summary>
        /// Obtient une concaténation du code et du libellé
        /// </summary>
        public string InfoFournisseur => this.Code + (!string.IsNullOrEmpty(TypeSequence) ? "-(" + TypeSequence + ") " : string.Empty) + this.SIREN;

        /// <summary>
        /// Obtient une concaténation du code et du libellé
        /// </summary>
        public string InfoAdresseFournisseur => Adresse + (!string.IsNullOrEmpty(Ville) ? "-(" + Ville + ") " : string.Empty) + CodePostal;
    }
}

