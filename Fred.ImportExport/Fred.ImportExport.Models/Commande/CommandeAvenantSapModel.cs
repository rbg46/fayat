using System;
using System.Collections.Generic;

namespace Fred.ImportExport.Models.Commande
{
    /// <summary>
    /// Représente le modèle d'un avenant de commande à envoyer vers SAP.
    /// </summary>
    public class CommandeAvenantSapModel : CommandeAdresseModel
    {
        /// <summary>
        /// Obtient ou définit l'identifiant unique d'une commande.
        /// </summary>
        public int CommandeId { get; set; }

        /// <summary>
        /// Obtient ou définit le numéro d'une commande.
        /// </summary>
        public string Numero { get; set; }

        /// <summary>
        ///  Obtient ou définit le numéro de commande externe
        /// </summary>   
        public string NumeroCommandeExterne { get; set; }

        /// <summary>
        ///  Obtient ou définit le numéro de contrat pour les commandes externes
        /// </summary>   
        public string NumeroContratExterne { get; set; }

        /// <summary>
        /// Obtient ou définit le libellé de la commande.
        /// </summary>
        public string Libelle { get; set; }

        /// <summary>
        /// Obtient ou définit la date de la commande.
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Obtient ou définit le délai de livraison de la commande.
        /// </summary>
        public string DelaiLivraison { get; set; }

        /// <summary>
        /// Obtient ou définit le date de mise à disposition de la commande.
        /// </summary>
        public DateTime? DateMiseADispo { get; set; }

        /// <summary>
        /// Obtient ou définit une valeur indiquant si la commande contient la conduite main d'œuvre.
        /// </summary>
        public bool MOConduite { get; set; }

        /// <summary>
        /// Obtient ou définit une valeur indiquant si la commande contient l'entretien mécanique.
        /// </summary>
        public bool EntretienMecanique { get; set; }

        /// <summary>
        /// Obtient ou définit une valeur indiquant si la commande contient l'entretien journalier.
        /// </summary>
        public bool EntretienJournalier { get; set; }

        /// <summary>
        /// Obtient ou définit une valeur indiquant si la commande contient le carburant.
        /// </summary>
        public bool Carburant { get; set; }

        /// <summary>
        /// Obtient ou définit une valeur indiquant si la commande contient le lubrifiant.
        /// </summary>
        public bool Lubrifiant { get; set; }

        /// <summary>
        /// Obtient ou définit une valeur indiquant si la commande contient les frais d'amortissement.
        /// </summary>
        public bool FraisAmortissement { get; set; }

        /// <summary>
        /// Obtient ou définit une valeur indiquant si la commande contient les frais d'amortissement.
        /// </summary>
        public bool FraisAssurance { get; set; }

        /// <summary>
        /// Obtient ou définit les conditions sociétés de la commande.
        /// </summary>
        public string ConditionSociete { get; set; }

        /// <summary>
        /// Obtient ou définit les conditions de prestation de la commande.
        /// </summary>
        public string ConditionPrestation { get; set; }

        /// <summary>
        /// Obtient ou définit le numéro de contact pour la commande.
        /// </summary>
        public string ContactTel { get; set; }

        /// <summary>
        /// Obtient ou définit l'Entete de livraison de la commande.
        /// </summary>
        public string LivraisonEntete { get; set; }

        /// <summary>
        /// Obtient ou définit l'Adresse de livraison de la commande.
        /// </summary>
        public string LivraisonAdresse { get; set; }

        /// <summary>
        /// Obtient ou définit la ville de livraison de la commande.
        /// </summary>
        public string LivraisonVille { get; set; }

        /// <summary>
        /// Obtient ou définit le code postale de livraison de la commande.
        /// </summary>
        public string LivraisonCPostale { get; set; }

        /// <summary>
        /// Obtient ou définit l'Adresse de facturation de la commande.
        /// </summary>
        public string FacturationAdresse { get; set; }

        /// <summary>
        /// Obtient ou définit la ville de facturation de la commande.
        /// </summary>
        public string FacturationVille { get; set; }

        /// <summary>
        /// Obtient ou définit le code postale de facturation de la commande.
        /// </summary>
        public string FacturationCPostale { get; set; }

        /// <summary>
        /// Obtient ou définit la date de validation de la commande.
        /// </summary>
        public DateTime? DateValidation { get; set; }

        /// <summary>
        /// Obtient ou définit la date de clôture de la commande.
        /// </summary>
        public DateTime? DateCloture { get; set; }

        /// <summary>
        /// Obtient ou définit le justificatif de la commande.
        /// </summary>
        public string Justificatif { get; set; }

        /// <summary>
        /// Obtient ou définit les commentaires fournisseur de la commande.
        /// </summary>
        public string CommentaireFournisseur { get; set; }

        /// <summary>
        /// Obtient ou définit les commentaires internes de la commande.
        /// </summary>
        public string CommentaireInterne { get; set; }

        /// <summary>
        /// Obtient ou définit la date de saisie de la commande.
        /// </summary>
        public DateTime? DateCreation { get; set; }

        /// <summary>
        /// Obtient ou définit la dernière date de modification de la commande.
        /// </summary>
        public DateTime? DateModification { get; set; }

        /// <summary>
        /// Obtient ou définit l'accord cadre si la commande peut être visée ou pas
        /// </summary>
        public bool AccordCadre { get; set; }

        /// <summary>
        /// Obtient ou définit l'identifiant d'un type de commande.
        /// CommandeEnt.TypeId => CommandeTypeEnt.Id
        /// </summary>
        public int CommandeTypeId { get; set; }

        /// <summary>
        /// Obtient ou définit le code d'un statut de commande.
        /// CommandeEnt.StatutCommandeId => StatutCommandeEnt.Code
        /// </summary>
        public string StatutCommandeCode { get; set; }

        /// <summary>
        /// Obtient ou définit le prénom et nom du contact de livraison d'une commande.
        /// CommandeEnt.ContactId => PersonnelEnt.Prenom et PersonnelEnt.Nom
        /// </summary>
        public string ContactPersonnel { get; set; }

        /// <summary>
        /// Obtient ou définit le prénom et nom du contact de suivi d'une commande.
        /// CommandeEnt.SuiviId => PersonnelEnt.Prenom et PersonnelEnt.Nom
        /// </summary>
        public string SuiviPersonnel { get; set; }

        /// <summary>
        /// Obtient ou définit le code d'une affaire.
        /// CommandeEnt.CiId => CiEnt.Code
        /// </summary>
        public string CiCode { get; set; }

        /// <summary>
        /// Obtient ou définit le code condensé de la société.
        /// CommandeEnt.SocieteDataId => SocieteEnt.Code
        /// </summary>
        public string SocieteCode { get; set; }

        /// <summary>
        /// Obtient ou définit le code société comptable.
        /// CommandeEnt.SocieteDataId => SocieteEnt.CodeSocieteComptable
        /// </summary>
        public string SocieteComptableCode { get; set; }

        /// <summary>
        /// Obtient ou définit le code établissement comptable.
        /// CommandeEnt.CI.EtablissementComptableId => EtablissementComptableEnt.Code
        /// </summary>
        public string EtablissementComptableCode { get; set; }

        /// <summary>
        /// Obtient ou définit le pays de livraison de la commande.
        /// CommandeEnt.??? => ???
        /// </summary>
        public string LivraisonPaysCode { get; set; }

        /// <summary>
        /// Obtient ou définit le pays de livraison de la commande.
        /// CommandeEnt.??? => ???
        /// </summary>
        public string FacturationPaysCode { get; set; }

        /// <summary>
        /// Obtient ou définit le prénom et nom du validateur de la commande. 
        /// CommandeEnt.ValidateurId => UtilisateurEnt.Prenom et UtilisateurEnt.Nom
        /// </summary>
        public string ValideurUtilisateur { get; set; }

        /// <summary>
        /// Obtient ou définit Code ISO de la devise.
        /// CommandeEnt.DeviseId => DeviseEnt.IsoCode
        /// </summary>
        public string DeviseIsoCode { get; set; }

        /// <summary>
        /// Obtient ou définit l'entité du membre du utilisateur ayant saisi la commande
        /// CommandeEnt.AuteurCreationId => => UtilisateurEnt.Prenom et UtilisateurEnt.Nom
        /// </summary>
        public string AuteurCreation { get; set; }

        /// <summary>
        /// Obtient ou définit l'entité du membre du utilisateur ayant modifier la commande
        /// CommandeEnt.AuteurModificationId => => UtilisateurEnt.Prenom et UtilisateurEnt.Nom
        /// </summary>
        public string AuteurModification { get; set; }

        /// <summary>
        ///   Obtient ou définit la liste des lignes de l'avenant de la commande
        /// </summary>
        public List<CommandeLigneAvenantSapModel> Lignes { get; set; }
    }
}
