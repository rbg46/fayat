using System.Collections.Generic;
using Fred.Business.RepriseDonnees.Commande.Models;
using Fred.Entities.Commande;
using Fred.Entities.RepriseDonnees.Excel;

namespace Fred.Business.RepriseDonnees.Commande.Mapper
{
    /// <summary>
    /// Transform les données du fichier excel en commandes
    /// </summary>
    public interface ICommandeDataMapper : IService
    {
        /// <summary>
        /// Creer une commandes avec ses commandes lignes a partir d'une liste de RepriseExcelCommande
        /// </summary>
        /// <param name="context">context contenant les data necessaire a l'import</param>
        /// <param name="repriseExcelCommandes">les commandes/commandeLignes sous la forme excel</param>
        /// <returns>Liste de Commandes</returns>
        CommandeTransformResult Transform(ContextForImportCommande context, List<RepriseExcelCommande> repriseExcelCommandes);
        /// <summary>
        /// Mets a jours les numeros de commande
        /// </summary>
        /// <param name="createdCommandes">createdCommandes</param>
        /// <returns>Les commandes mise a jour</returns>
        List<CommandeEnt> UpdateNumeroDeCommandes(List<CommandeEnt> createdCommandes);
    }
}
