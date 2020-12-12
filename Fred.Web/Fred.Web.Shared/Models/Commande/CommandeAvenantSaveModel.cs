using Fred.Entities.Commande;
using System;
using System.Collections.Generic;
using Fred.Web.Shared.Extentions;

namespace Fred.Web.Shared.Models.Commande
{
    /// <summary>
    /// Modèles d'enregistrement d'un avenant de commande.
    /// </summary>
    public class CommandeAvenantSave
    {
        // Front -> back
        #region Model

        /// <summary>
        /// Modèle d'enregistrement d'un avenant de commande.
        /// </summary>
        public class Model
        {
            /// <summary>
            /// Date de modification
            /// </summary>
            private DateTime? dateModification;

            /// <summary>
            /// Date de modification de la commande
            /// </summary>
            public DateTime? DateModification
            {
                get
                {
                    return dateModification;
                }
                set
                {
                    dateModification = (value.HasValue ? value.Value.Truncate(TimeSpan.TicksPerSecond) : (DateTime?)null);
                }
            }

            /// <summary>
            /// Identifiant de la commande.
            /// </summary>
            public int CommandeId { get; set; }

            /// <summary>
            /// Lignes de l'avenant créées.
            /// </summary>
            public List<LigneModel> CreatedLignes { get; set; }

            /// <summary>
            /// Lignes de l'avenant changées.
            /// </summary>
            public List<LigneModel> UpdatedLignes { get; set; }

            /// <summary>
            /// Identifiants des lignes d'avenant supprimées.
            /// </summary>
            public List<int> DeletedLigneIds { get; set; }

            /// <summary>
            /// Le commentaire fournisseur.
            /// </summary>
            public string CommentaireFournisseur { get; set; }

            /// <summary>
            /// Le commentaire interne.
            /// </summary>
            public string CommentaireInterne { get; set; }

            /// <summary>
            /// Les informations d'abonnement.
            /// </summary>
            public AbonnementModel Abonnement { get; set; }

            /// <summary>
            /// Le dlai de livraison.
            /// </summary>
            public string DelaiLivraison { get; set; }

            /// <summary>
            /// Les informations du fournisseur.
            /// </summary>
            public FournisseurModel Fournisseur { get; set; }
        }

        #endregion
        #region LigneModel

        /// <summary>
        /// Modèle d'enregistrement d'une ligne d'avenant de commande.
        /// </summary>
        public class LigneModel
        {
            /// <summary>
            /// Identifiant Numero de la ligne d'avenant.
            /// </summary>
            public int? NumeroLigne { get; set; }

            /// <summary>
            /// Identifiant de la ligne d'avenant.
            /// </summary>
            public int CommandeLigneId { get; set; }

            /// <summary>
            /// Identifiant de la tâche.
            /// </summary>
            public int? TacheId { get; set; }

            /// <summary>
            /// Identifiant de la ressource.
            /// </summary>
            public int? RessourceId { get; set; }

            /// <summary>
            /// Identifiant de l'unité.
            /// </summary>
            public int? UniteId { get; set; }

            /// <summary>
            /// Libellé.
            /// </summary>
            public string Libelle { get; set; }

            /// <summary>
            /// Quantité.
            /// </summary>
            public decimal Quantite { get; set; }

            /// <summary>
            /// Prix unitaire HT.
            /// </summary>
            public decimal PUHT { get; set; }

            /// <summary>
            /// Indique s'il s'agit d'une diminution.
            /// </summary>
            public bool IsDiminution { get; set; }

            /// <summary>
            /// Identifiant de la ligne dans la vue.
            /// </summary>
            public int ViewId { get; set; }
        }

        #endregion
        #region AbonnementModel

        /// <summary>
        /// Modèle d'enregistrement d'un abonnement.
        /// </summary>
        public class AbonnementModel
        {
            /// <summary>
            /// Indique s'il s'agit d'un abonnement.
            /// </summary>
            public bool IsAbonnement { get; set; }

            /// <summary>
            /// La fréquence de l'abonnement.
            /// </summary>
            public int? Frequence { get; set; }

            /// <summary>
            /// La durée de l'abonnement.
            /// </summary>
            public int? Duree { get; set; }

            /// <summary>
            /// La date de la prochaine génération d'une réception.
            /// </summary>
            public DateTime? DateProchaineReception { get; set; }

            /// <summary>
            /// La  date de la première génération d'une réception.
            /// </summary>
            public DateTime? DatePremiereReception { get; set; }
        }

        #endregion
        #region FournisseurModel

        /// <summary>
        /// Modèle d'enregistrement d'un fournisseur.
        /// </summary>
        public class FournisseurModel
        {
            /// <summary>
            /// L'adresse du fournisseur.
            /// </summary>
            public string Adresse { get; set; }

            /// <summary>
            /// Le code postal du fournisseur.
            /// </summary>
            public string CodePostal { get; set; }

            /// <summary>
            /// La ville du fournisseur.
            /// </summary>
            public string Ville { get; set; }

            /// <summary>
            /// L'identifiant du pays du fournisseur.
            /// </summary>
            public int? PaysId { get; set; }
        }

        #endregion

        // Back -> front
        #region ResultModel

        public class ResultModel
        {
            /// <summary>
            /// Date de modification
            /// </summary>
            private DateTime? dateModification;

            /// <summary>
            /// Date de modification de la commande
            /// </summary>
            public DateTime? DateModification
            {
                get
                {
                    return dateModification;
                }
                set
                {
                    dateModification = (value.HasValue ? value.Value.Truncate(TimeSpan.TicksPerSecond) : (DateTime?)null);
                }
            }

            public AvenantModel Avenant { get; private set; }

            /// <summary>
            /// Résultat de création des items de sous-détail.
            /// </summary>
            public List<ItemCreatedModel> ItemsCreated { get; private set; }

            /// <summary>
            /// Constructeur.
            /// </summary>
            /// <param name="avenantEnt">L'avenant.</param>
            /// <param name="dateModification">La date de modification de la commande.</param>
            public ResultModel(CommandeAvenantEnt avenantEnt, DateTime? dateModification)
            {
                Avenant = new AvenantModel(avenantEnt);
                ItemsCreated = new List<ItemCreatedModel>();
                DateModification = dateModification;
            }
        }

        #endregion
        #region AvenantModel

        /// <summary>
        /// Modèle de chargement d'un avenant.
        /// </summary>
        public class AvenantModel
        {
            /// <summary>
            /// Le numéro de l'avenant.
            /// </summary>
            public int NumeroAvenant { get; private set; }

            /// <summary>
            /// La date de validation de l'avenant.
            /// </summary>
            public DateTime? DateValidation { get; private set; }

            /// <summary>
            /// L'id de l'avenant
            /// </summary>
            public int AvenantId { get; private set; }

            /// <summary>
            /// Constructeur.
            /// </summary>
            /// <param name="avenantEnt">Entité concernée.</param>
            public AvenantModel(CommandeAvenantEnt avenantEnt)
            {
                Update(avenantEnt);
            }

            /// <summary>
            /// Met à jour les propriétés.
            /// </summary>
            /// <param name="avenantEnt">Entité concernée.</param>
            public void Update(CommandeAvenantEnt avenantEnt)
            {
                NumeroAvenant = avenantEnt.NumeroAvenant;
                DateValidation = avenantEnt.DateValidation;
                AvenantId = avenantEnt.CommandeAvenantId;
            }
        }

        #endregion
        #region ItemCreatedModel

        /// <summary>
        /// Représente le résultat de création d'un item de sous-détail.
        /// </summary>
        public class ItemCreatedModel
        {
            /// <summary>
            /// Identifiant dans la vue.
            /// </summary>
            public int ViewId { get; private set; }

            /// <summary>
            /// Identifiant de la ligne d'avenant créée.
            /// </summary>
            public int CommandeLigneId { get; private set; }

            /// <summary>
            /// Constructeur.
            /// </summary>
            /// <param name="viewId">Identifiant dans la vue.</param>
            /// <param name="CommandeLigneId">Identifiant de la ligne d'avenant créée.</param>
            public ItemCreatedModel(int viewId, int commandeLigneId)
            {
                ViewId = viewId;
                CommandeLigneId = commandeLigneId;
            }
        }

        #endregion
    }
}
