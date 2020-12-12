using System;

namespace Fred.ImportExport.Models.Depense
{
    /// <summary>
    /// Représente une réception SAP (Appelé "dépense" pour FRED).
    /// </summary>
    public class ReceptionSapModel
    {
        /// <summary>
        /// Obtient ou définit l'identifiant unique d'une réception.
        /// </summary>
        public int ReceptionId { get; set; }

        /// <summary>
        /// Obtient ou définit l'identifiant unique de la ligne de commande.
        /// </summary>
        public int? CommandeLigneId { get; set; }

        /// <summary>
        /// Obtient ou définit le libellé d'une dépense.
        /// </summary>
        public string Libelle { get; set; }

        /// <summary>
        /// Obtient ou définit la quantité d'une dépense.
        /// </summary>
        public decimal Quantite { get; set; }

        /// <summary>
        /// Obtient ou définit le numéro du bon de livraison associé à une dépense.
        /// </summary>
        public string NumeroBL { get; set; }

        /// <summary>
        /// Obtient ou définit le commentaire.
        /// </summary>
        public string Commentaire { get; set; }

        /// <summary>
        /// Obtient ou définit la date de saisie de la commande.
        /// </summary>
        public DateTime? DateCreation { get; set; }

        /// <summary>
        /// Obtient ou définit la dernière date de modification de la commande.
        /// </summary>
        public DateTime? DateModification { get; set; }

        /// <summary>
        /// Obtient ou définit la dernière date de récéption.
        /// </summary>
        public DateTime? DateReception { get; set; }

        /// <summary>
        /// Obtient ou définit la dernière date comptable.
        /// </summary>
        public DateTime? DateComptable { get; set; }

        /// <summary>
        /// Obtient ou définit la date de verouillage de la ligne de commande.
        /// </summary>
        public DateTime? DateVerouillage { get; set; }

        /// <summary>
        ///     Obtient ou définit le mouvement FAR Hors Taxe
        /// </summary>
        public decimal? MouvementFarHt { get; set; }

        /// <summary>
        ///     Obtient ou définit le total FAR Hors Taxe
        /// </summary>
        public decimal? TotalFarHt { get; set; }

        #region ForeignKey

        /// <summary>
        /// Obtient ou définit le libellé d'une ressource.
        /// DepenseEnt.RessourceId => RessourceEnt.Libelle
        /// </summary>
        public string RessourceLibelle { get; set; }

        /// <summary>
        /// Obtient ou définit le code d'une ressource.
        /// DepenseEnt.RessourceId => RessourceEnt.Code
        /// </summary>
        public string RessourceCode { get; set; }

        /// <summary>
        /// Obtient ou définit le libellé d'une tâche.
        /// DepenseEnt.TacheId =>TacheEnt.Libelle
        /// </summary>
        public string TacheLibelle { get; set; }

        /// <summary>
        /// Obtient ou définit le code d'une tâche.
        /// DepenseEnt.TacheId => TacheEnt.Code
        /// </summary>
        public string TacheCode { get; set; }

        /// <summary>
        /// Obtient ou définit l'entité du membre du utilisateur ayant saisi la commande.
        /// DepenseEnt.AuteurCreationId => => UtilisateurEnt.Prenom et UtilisateurEnt.Nom
        /// </summary>
        public string AuteurCreation { get; set; }

        /// <summary>
        /// Obtient ou définit l'entité du membre du utilisateur ayant modifier la commande.
        /// DepenseEnt.AuteurModificationId => => UtilisateurEnt.Prenom et UtilisateurEnt.Nom
        /// </summary>
        public string AuteurModification { get; set; }

        /// <summary>
        /// Obtient ou définit le numéro d'une commande.
        /// DepenseEnt.CommandeLigneId => CommandeLigneEnt.CommandeId => CommandeEnt.Numero 
        /// </summary>
        public string Numero { get; set; }

        /// <summary>
        /// Obtient ou définit l'identifiant SAP de la ligne de commande.
        /// A prendre en compte pour le futur dev des imports des commandes.
        /// DepenseEnt.CommandeLigneId => CommandeLigneEnt.PostSap
        /// </summary>
        public string CommandeLigneSap { get; set; }

        /// <summary>
        /// Obtient ou définit l'identifiant le code société comptable.
        /// </summary>
        public string SocieteComptableCode { get; set; }

        /// <summary>
        /// Obtient ou définit l'identifiant le code du ci.
        /// /// DepenseEnt.CiId => CIEnt.Code
        /// </summary>
        public string CiCode { get; set; }

        /// <summary>
        /// Obtient ou définit l'identifiant le code du ci.
        /// /// DepenseEnt.CommandeLigneId => CommandeLigneEnt.CommandeId => CommandeEnt.CommandeContratInterimaire => CommandeContratInterimaire.InterimaireId => InterimaireEnt.MatriculeExterne => MatriculeExterne.Source = SAP
        /// </summary>
        public string MatriculeSap { get; set; }

        /// <summary>
        /// Obtient ou définit l'identifiant le code du ci.
        /// /// DepenseEnt.CommandeLigneId => CommandeLigneEnt.CommandeId => CommandeEnt.CommandeContratInterimaire => CommandeContratInterimaire.InterimaireId => InterimaireEnt.MatriculeExterne => MatriculeExterne.Source = PIXID
        /// </summary>
        public string MatriculePixid { get; set; }

        /// <summary>
        /// Obtient ou définit l'identifiant le code du ci.
        /// /// DepenseEnt.CommandeLigneId => CommandeLigneEnt.CommandeId => CommandeEnt.CommandeContratInterimaire => CommandeContratInterimaire.InterimaireId => InterimaireEnt.MatriculeExterne => MatriculeExterne.Source = DIRECTSKILLS
        /// </summary>
        public string MatriculeDirectSkills { get; set; }

        #endregion Jointure

        public bool Diminution { get; set; }
    }
}
