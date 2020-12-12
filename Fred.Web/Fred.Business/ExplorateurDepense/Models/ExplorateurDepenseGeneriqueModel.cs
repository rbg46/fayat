using System;
using System.Collections.Generic;
using System.Diagnostics;
using Fred.Entities.CI;
using Fred.Entities.Depense;
using Fred.Entities.OperationDiverse;
using Fred.Entities.Personnel;
using Fred.Entities.Referential;
using Fred.Entities.ReferentielFixe;

namespace Fred.Business.ExplorateurDepense
{
    /// <summary>
    /// Model Explorateur depense
    /// </summary>
    [DebuggerDisplay("Id={Id} RessourceId={RessourceId} TacheId={TacheId} MontantHT={MontantHT} Code={Code} ")]
    public class ExplorateurDepenseGeneriqueModel
    {
        /// <summary>
        /// Obtient ou définit le CI Code CI + Libelle
        /// </summary>
        public string InfoCI => (Ci != null) ? Ci.Code + " - " + Ci.Libelle : string.Empty;

        /// <summary>
        /// Obtient ou définit le CI Code CI + Libelle
        /// </summary>
        public CIEnt Ci { get; set; }

        /// <summary>
        /// Obtient ou définit l'identifiant unique de la dépense
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Obtient ou définit l'identifiant de la ressource
        /// </summary>
        public int RessourceId { get; set; }

        /// <summary>
        /// Obtient ou définit la ressource
        /// </summary>
        public RessourceEnt Ressource { get; set; }

        /// <summary>
        /// Obtient ou définit l'identifiant de la tâche
        /// </summary>
        public int TacheId { get; set; }

        /// <summary>
        /// Obtient ou définit la tache
        /// </summary>
        public TacheEnt Tache { get; set; }

        /// <summary>
        /// Obtient ou définit le libelle1
        /// </summary>
        public string Libelle1 { get; set; }

        /// <summary>
        /// Obtient ou définit l'identifiant de l'unité
        /// </summary>
        public int UniteId { get; set; }

        /// <summary>
        /// Obtient ou définit l'unité
        /// </summary>
        public UniteEnt Unite { get; set; }

        /// <summary>
        /// Obtient ou définit la quantité
        /// </summary>
        public decimal? Quantite { get; set; }

        /// <summary>
        /// Obtient ou définit le prix unitaire hors taxe
        /// </summary>
        public decimal? PUHT { get; set; }

        /// <summary>
        /// Obtient ou définit le montant hors taxe
        /// </summary>
        public decimal MontantHT { get; set; }

        /// <summary>
        /// Obtient ou définit l'identifiant de la devise
        /// </summary>
        public int DeviseId { get; set; }

        /// <summary>
        /// Obtient ou définit la devise
        /// </summary>
        public DeviseEnt Devise { get; set; }

        /// <summary>
        /// Obtient ou définit le code
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Obtient ou définit le libellé 2
        /// </summary>
        public string Libelle2 { get; set; }

        /// <summary>
        /// Obtient ou définit le commentaire
        /// </summary>
        public string Commentaire { get; set; }

        /// <summary>
        /// Obtient ou définit la date comptable de remplacement
        /// </summary>
        public DateTime DateComptableRemplacement { get; set; }

        /// <summary>
        /// Obtient ou définit la date de la dépense
        /// </summary>
        public DateTime DateDepense { get; set; }

        /// <summary>
        /// Obtient ou définit la période
        /// </summary>
        public DateTime Periode { get; set; }

        /// <summary>
        /// Obtient ou définit l'identifiant de la nature
        /// </summary>
        public int NatureId { get; set; }

        /// <summary>
        /// Obtient ou définit la nature
        /// </summary>
        public NatureEnt Nature { get; set; }

        /// <summary>
        /// Obtient ou définit le type de dépense ["OD", "Valorisation", "Reception", "Facture"]
        /// </summary>
        public string TypeDepense { get; set; }

        /// <summary>
        /// Obtient ou définit le type d'OD (d'après le LibelleCourt de FRED_FAMILLE_OPERATION_DIVERSE)
        /// </summary>
        public string TypeOd { get; set; }

        /// <summary>
        /// Obtient ou définit le type de sous dépense
        /// </summary>
        public string SousTypeDepense { get; set; }

        /// <summary>
        /// Obtient ou définit la date rapprochement
        /// </summary>
        public DateTime? DateRapprochement { get; set; }

        /// <summary>
        /// Obtient ou définit la date facture (ou date opération pour une Réception)
        /// </summary>
        public DateTime? DateFacture { get; set; }

        /// <summary>
        /// Obtient ou définit le numéro de facture
        /// </summary>
        public string NumeroFacture { get; set; }

        /// <summary>
        /// Obtient ou définit le montant de la facture
        /// </summary>
        public decimal? MontantFacture { get; set; }

        /// <summary>
        /// Obtient ou définit le montant hors taxe initial
        /// </summary>
        public decimal? MontantHtInitial { get; set; }

        /// <summary>
        /// Obtient ou définit l'identifiant de la commande
        /// </summary>
        public int? CommandeId { get; set; }

        /// <summary>
        /// Indique si la commande est une commande Energie
        /// </summary>
        public bool IsEnergie { get; set; }

        /// <summary>
        /// Obtient ou définit si la tâche est remplaçable
        /// </summary>
        public bool TacheRemplacable { get; set; }

        /// <summary>
        /// Obtient ou définit si la dépense a été visée
        /// </summary>
        public bool DepenseVisee { get; set; }

        /// <summary>
        /// Obtient ou définit l'identifiant de la dépense
        /// </summary>
        public int DepenseId { get; set; }

        /// <summary>
        /// Obtient ou définit l'identifiant du groupe de remplacement
        /// </summary>
        public int GroupeRemplacementTacheId { get; set; }

        /// <summary>
        /// Obtient ou définit le code et libellé de la Tâche d'origine si elle a été remplacée dans la dépense
        /// </summary>
        public string TacheOrigineCodeLibelle { get; set; }

        /// <summary>
        /// Obtient ou définit l'identifiant de la Tâche d'origine si elle a été remplacée dans la dépense
        /// </summary>
        public int? TacheOrigineId { get; set; }

        /// <summary>
        /// Obtient ou définit la Tâche d'origine si elle a été remplacée dans la dépense
        /// </summary>
        public TacheEnt TacheOrigine { get; set; }

        /// <summary>
        /// Obtient ou définit le personnel
        /// </summary>
        public PersonnelEnt Personnel { get; set; }

        public int FamilleOperationDiverseId { get; set; }

        public FamilleOperationDiverseEnt FamilleOperationDiverse { get; set; }

        #region Champs utilisés uniquement pour filtrage

        /// <summary>
        /// Obtient ou définit l'identifiant du fournisseur
        /// </summary>
        public int? FournisseurId { get; set; }

        /// <summary>
        /// Obtient ou définit l'identifiant de l'agence
        /// </summary>
        public int? AgenceId { get; set; }

        /// <summary>
        /// Obtient ou définit l'identifiant du type de la ressource
        /// </summary>
        public int? TypeRessourceId { get; set; }

        #endregion

        #region Spécifique à l'export Excel

        /// <summary>
        /// Obtient ou définit la liste des tâches remplacées sur la dépense
        /// </summary>
        public IEnumerable<RemplacementTacheEnt> RemplacementTaches { get; set; }

        /// <summary>
        /// Montant du solde FAR 
        /// Champ utilisé pour l'export excel des Comptes d'Exploitation
        /// </summary>
        public decimal SoldeFar { get; set; }

        #endregion

        /// <summary>
        /// Clone l'objet
        /// </summary>
        /// <returns>ExplorateurDepenseGenerique</returns>
        public virtual ExplorateurDepenseGeneriqueModel Clone()
        {
            return new ExplorateurDepenseGeneriqueModel
            {
                Ci = Ci,
                Code = Code,
                CommandeId = CommandeId,
                Commentaire = Commentaire,
                RessourceId = RessourceId,
                Ressource = Ressource,
                TacheId = TacheId,
                Tache = Tache,
                NatureId = NatureId,
                Nature = Nature,
                UniteId = UniteId,
                Unite = Unite,
                DeviseId = DeviseId,
                Devise = Devise,
                PUHT = PUHT,
                MontantHT = MontantHT,
                Libelle1 = Libelle1,
                Libelle2 = Libelle2,
                DateDepense = DateDepense,
                Periode = Periode,
                FournisseurId = FournisseurId,
                Id = Id,
                DateRapprochement = DateRapprochement,
                Quantite = Quantite,
                DepenseVisee = DepenseVisee,
                DepenseId = DepenseId,
                GroupeRemplacementTacheId = GroupeRemplacementTacheId,
                TypeDepense = TypeDepense,
                SousTypeDepense = SousTypeDepense,
                NumeroFacture = NumeroFacture,
                DateComptableRemplacement = DateComptableRemplacement,
                RemplacementTaches = RemplacementTaches,
                DateFacture = DateFacture,
                MontantFacture = MontantFacture,
                TacheOrigine = TacheOrigine,
                TacheOrigineId = TacheOrigineId,
                TacheOrigineCodeLibelle = TacheOrigineCodeLibelle,
                TacheRemplacable = TacheRemplacable,
                SoldeFar = SoldeFar,
                Personnel = Personnel,
                IsEnergie = IsEnergie,
                FamilleOperationDiverse = FamilleOperationDiverse,
                FamilleOperationDiverseId = FamilleOperationDiverseId
            };
        }

        /// <summary>
        /// Conversion d'un ExplorateurDepenseGenerique vers un ExplorateurDepenseFayatTP
        /// </summary>
        /// <returns><see cref="ExplorateurDepenseFayatTPModel"/></returns>
        public ExplorateurDepenseFayatTPModel ConvertToExplorateurDepenseFayatTP()
        {
            return new ExplorateurDepenseFayatTPModel
            {
                AgenceId = AgenceId,
                Ci = Ci,
                Code = Code,
                CommandeId = CommandeId,
                Commentaire = Commentaire,
                DateComptableRemplacement = DateComptableRemplacement,
                DateDepense = DateDepense,
                DateFacture = DateFacture,
                DateRapprochement = DateRapprochement,
                DepenseId = DepenseId,
                DepenseVisee = DepenseVisee,
                Devise = Devise,
                DeviseId = DeviseId,
                FournisseurId = FournisseurId,
                GroupeRemplacementTacheId = GroupeRemplacementTacheId,
                Id = Id,
                IsEnergie = IsEnergie,
                Libelle1 = Libelle1,
                Libelle2 = Libelle2,
                MontantFacture = MontantFacture,
                MontantHT = MontantHT,
                MontantHtInitial = MontantHtInitial,
                Nature = Nature,
                NatureId = NatureId,
                NumeroFacture = NumeroFacture,
                Periode = Periode,
                Personnel = Personnel,
                PUHT = PUHT,
                Quantite = Quantite,
                RemplacementTaches = RemplacementTaches,
                Ressource = Ressource,
                RessourceId = RessourceId,
                SoldeFar = SoldeFar,
                SousTypeDepense = SousTypeDepense,
                Tache = Tache,
                TacheId = TacheId,
                TacheOrigine = TacheOrigine,
                TacheOrigineCodeLibelle = TacheOrigineCodeLibelle,
                TacheOrigineId = TacheOrigineId,
                TacheRemplacable = TacheRemplacable,
                TypeDepense = TypeDepense,
                Unite = Unite,
                UniteId = UniteId,
                FamilleOperationDiverse = FamilleOperationDiverse,
                FamilleOperationDiverseId = FamilleOperationDiverseId
            };
        }
    }
}
