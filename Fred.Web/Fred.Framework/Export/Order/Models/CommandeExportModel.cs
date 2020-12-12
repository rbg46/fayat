using System.Collections.Generic;
using System.Drawing;

namespace Fred.Framework.Export.Order.Models
{
    public class CommandeExportModel
    {
        public int? SocieteGeranteId { get; set; }

        public string FacturationText { get; set; }

        public string PaiementText  { get; set; }
        
        public string Numero { get; set; }

        public string SocieteGerante { get; set; }

        public string FacturationAdresse { get; set; }

        public string FacturationCPostal { get; set; }

        public string FacturationVille { get; set; }

        public string FacturationPays { get; set; }

        public string Suivi { get; set; }

        public string FournisseurLibelle { get; set; }

        public string FournisseurCode { get; set; }

        public int? FournisseurId { get; set; }

        public string FournisseurAdresse { get; set; }

        public string FournisseurCPostal { get; set; }

        public string FournisseurVille { get; set; }

        public string FournisseurPays { get; set; }

        public string CiCode { get; set; }

        public string PathCGA { get; set; }

        public string CiLibelle { get; set; }

        public string Date { get; set; }

        public string Libelle { get; set; }

        public string DeviseSymbole { get; set; }

        public string CommentaireFournisseur { get; set; }

        public string DelaiLivraison { get; set; }

        public string LivraisonEntete { get; set; }

        public string LivraisonAdresse { get; set; }

        public string LivraisonCPostal { get; set; }

        public string LivraisonVille { get; set; }

        public string LivraisonPays { get; set; }

        public string Contact { get; set; }

        public string ContactTel { get; set; }

        public string Valideur { get; set; }

        public string MontantHT { get; set; }

        /// <summary>
        ///   Obtient ou définit Montant HT par bloc (= montant initiale commande / montant avenant 1 / ...)
        /// </summary>
        public string MontantBlocHT { get; set; }

        /// <summary>
        /// Informations de la société à afficher en pieds de page
        /// </summary>
        public string InfosSociete { get; set; }

        public byte[] SignatureByteArray { get; set; }

        public Image Signature { get; set; }

        public string LogoPath { get; set; }

        public Image Logo { get; set; }

        public List<CommandeLigneExportModel> Lignes { get; set; }

        #region Location

        public string MOConduiteOui { get; set; }

        public string MOConduiteNon { get; set; }

        public string EntretienMecaniqueOui { get; set; }

        public string EntretienMecaniqueNon { get; set; }

        public string EntretienJournalierOui { get; set; }

        public string EntretienJournalierNon { get; set; }

        public string CarburantOui { get; set; }

        public string CarburantNon { get; set; }

        public string LubrifiantOui { get; set; }

        public string LubrifiantNon { get; set; }

        public string FraisAmortissementOui { get; set; }

        public string FraisAmortissementNon { get; set; }

        public string FraisAssuranceOui { get; set; }

        public string FraisAssuranceNon { get; set; }

        #endregion

        #region Prestation

        public string ConditionSociete { get; set; }

        public string ConditionPrestation { get; set; }

        #endregion

        public string CommandeType { get; set; }

        public string LibelleCommande { get; set; }

        /// <summary>
        /// Le numéro d'avenant ou null s'il ne s'agit pas d'un avenant.
        /// </summary>
        public string AvenantNumero { get; set; }

        public string EntetePrestat { get; set; }
    }
}
