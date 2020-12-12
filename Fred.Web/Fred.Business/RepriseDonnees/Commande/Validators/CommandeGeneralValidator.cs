using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Fred.Business.RepriseDonnees.Commande.Models;
using Fred.Business.RepriseDonnees.Commande.Validators.Results;
using Fred.Entities.RepriseDonnees.Excel;

namespace Fred.Business.RepriseDonnees.Commande.Validators
{
    /// <summary>
    /// verifie les rg globales
    /// </summary>
    public class CommandeGeneralValidator
    {

        /// <summary>
        /// RG_5634_003 : 1er outil « Chargement des commandes et réceptions » - Vérification de la cohérence des informations d’en-tête de chaque commande (= contrôle n°1)
        /// n lignes du 1er fichier d’entrée peuvent donner lieu à la création d’une seule commande contenant n lignes de commandes.
        /// Le critère pour rassembler ces n lignes dans une même commande est le « N° Commande Externe » (clé d’identification de la commande).
        /// 
        /// Or une même commande doit nécessairement contenir les mêmes informations communes d’en-tête, à savoir les champs suivants :
        /// -   Code Société(nécessaire pour identifier le CI, le code CI n’étant unique que par société)
        /// -   Code CI
        /// -   Code Fournisseur
        /// -   Type Commande
        /// -   Libellé En-tête Commande
        /// -   Code Devise
        /// -   Date Commande
        /// => Avant tout chargement, faire un contrôle global sur l’ensemble des lignes du fichier pour s’assurer que pour chaque valeur distincte de N° Commande Externe,
        /// on a bien les mêmes informations communes d’en-tête(y compris le même CI, car les commandes multi-CI ne sont pas autorisées).
        /// 
        /// </summary>      
        /// <param name="numerocommandeExterne">numero de commande Externe</param>
        /// <param name="repriseExcelCommandes">liste de lignes eyant le meme numero de commande Externe</param>
        /// <returns>Le resultat de la validation de la coherence des donnees d'entete</returns>
        public CommandeImportRuleResult VerifyCoherenceInformationEnteteRule(string numerocommandeExterne, List<RepriseExcelCommande> repriseExcelCommandes)
        {
            var result = new CommandeImportRuleResult();

            result.ImportRuleType = CommandeImportRuleType.CoherenceInformationEntete;

            var codeSociete = repriseExcelCommandes.First().CodeSociete;
            var codeCi = repriseExcelCommandes.First().CodeCi;
            var codeFournisseur = repriseExcelCommandes.First().CodeFournisseur;
            var typeCommande = repriseExcelCommandes.First().TypeCommande;
            var libelleEnteteCommande = repriseExcelCommandes.First().LibelleEnteteCommande;
            var codeDevise = repriseExcelCommandes.First().CodeDevise;
            var dateCommande = repriseExcelCommandes.First().DateCommande;


            var codeSocieteEquals = repriseExcelCommandes.All(x => x.CodeSociete == codeSociete);
            var codeCiEquals = repriseExcelCommandes.All(x => x.CodeCi == codeCi);
            var codeFournisseurEquals = repriseExcelCommandes.All(x => x.CodeFournisseur == codeFournisseur);
            var typeCommandeEquals = repriseExcelCommandes.All(x => x.TypeCommande == typeCommande);
            var libelleEnteteCommandeEquals = repriseExcelCommandes.All(x => x.LibelleEnteteCommande == libelleEnteteCommande);
            var codeDeviseEquals = repriseExcelCommandes.All(x => x.CodeDevise == codeDevise);
            var dateCommandeEquals = repriseExcelCommandes.All(x => x.DateCommande == dateCommande);


            result.IsValid = codeSocieteEquals &&
                codeCiEquals &&
                codeFournisseurEquals &&
                typeCommandeEquals &&
                libelleEnteteCommandeEquals &&
                codeDeviseEquals &&
                dateCommandeEquals;
            result.ErrorMessage = result.IsValid ? string.Empty : string.Format(BusinessResources.ImportCommandeCoherenceInformationEnteteError, numerocommandeExterne);
            result.SetNumeroDeLigne(0);
            return result;
        }

        /// <summary>
        /// RG_5634_004 : 1er outil « Chargement des commandes et réceptions » - Vérification de l’unicité des numéros externes des commandes (= contrôle n°2)
        /// Une nouvelle commande est créée dans la base FRED pour chaque valeur de « N° Commande Externe » différente dans le fichier.
        /// Avant de créer la commande, vérifier que chaque valeur de « N° Commande Externe » contenue dans le fichier n’existe pas déjà dans la base FRED
        /// (à vérifier dans les colonnes [Numero] et[NumeroCommandeExterne] de la table).
        /// NB – Cela permettra notamment de contrôler que le même fichier n’est pas chargé 2 fois par erreur.
        /// De plus il est important que ce numéro de commande soit unique pour le rapprochement des factures dans SAP.
        /// 
        /// Si un ou plusieurs N° Commande Externe est(sont) en anomalie, remonter la liste de ces numéros en erreur, avec les messages suivants :
        /// « Rejet N° Commande Externe[valeur du n° commande externe 1 en anomalie] : le numéro existe déjà dans FRED. ».
        /// « Rejet N° Commande Externe [valeur du n° commande externe 2 en anomalie] : le numéro existe déjà dans FRED. ».
        /// « Rejet N° Commande Externe [valeur du n° commande externe 3 en anomalie] : le numéro existe déjà dans FRED. ».
        /// </summary>
        /// <param name="numerocommandeExterne">numerocommandeExterne</param>
        /// <param name="context">context de l'import</param>       
        /// <returns>CommandeImportRuleResult</returns>
        public CommandeImportRuleResult VerifyUniciteNumeroExterneRule(string numerocommandeExterne, ContextForImportCommande context)
        {
            var result = new CommandeImportRuleResult();

            result.ImportRuleType = CommandeImportRuleType.UniciteNumeroExterne;

            result.IsValid = !context.CommandesUsedInExcel.Any(x => x.Numero == numerocommandeExterne || x.NumeroCommandeExterne == numerocommandeExterne);

            result.ErrorMessage = result.IsValid ? string.Empty : string.Format(BusinessResources.ImportCommandeUniciteNumeroExterneError, numerocommandeExterne);

            result.SetNumeroDeLigne(0);

            return result;
        }

        /// <summary>
        /// Verifie le pattern des numero de commande externe, il ne doit pas commencer par F et etre suivi par 9 chiffres.
        /// </summary>
        /// <param name="numeroCommandeExterne">numeroCommandeExterne</param>
        /// <returns>Le resultat de la validation de la coherence des donnees d'entete</returns>
        public CommandeImportRuleResult VerifyPatternNumeroCommandeExterneRule(string numeroCommandeExterne)
        {
            var result = new CommandeImportRuleResult();

            result.ImportRuleType = CommandeImportRuleType.PatternNumeroCommandeExterne;

            Regex myRegex = new Regex(@"^F[0-9]{9}$");

            result.IsValid = !myRegex.IsMatch(numeroCommandeExterne);

            result.ErrorMessage = result.IsValid ? string.Empty : string.Format(BusinessResources.ImportCommandePatternNumeroCommandeExterneError, numeroCommandeExterne);

            result.SetNumeroDeLigne(0);

            return result;
        }
    }
}
