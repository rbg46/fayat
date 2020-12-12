using System.Collections.Generic;
using System.Linq;
using Fred.Entities.Organisation.Tree;

namespace Fred.Business.Tests.Integration.Organisation.Extensions
{
    /// <summary>
    /// Methode d'extension d'OrganisationTree centrée sur les Nodes
    /// </summary>
    public static class NodesExtension
    {
        /// <summary>
        /// Retourne le node correspondant eyant comme Data une OrganisationBase qui a l' organisationId passé en parametre
        /// </summary>
        /// <param name="organisationTree">Une organisationTree</param>
        /// <param name="organisationId">organisationId</param>
        /// <returns>Un node d'organisationBase, Delclenche une InvalidOperationException si l'organisationId ne correspond a aucun Node</returns>
        public static Node<OrganisationBase> GetNode(this OrganisationTree organisationTree, int organisationId)
        {
            return organisationTree.Nodes.First(x => x.Data.OrganisationId == organisationId);
        }

        /// <summary>
        /// Retourne les nodes eyant comme une OrganisationBase qui a l'organisationId contenu dans la liste passée en parametre
        /// </summary>
        /// <param name="organisationTree">Une organisationTree</param>
        /// <param name="organisationIds">liste d'organisationId</param>
        /// <returns>Liste de node</returns>
        public static List<Node<OrganisationBase>> GetNodes(this OrganisationTree organisationTree, List<int> organisationIds)
        {
            return organisationTree.Nodes.Where(x => organisationIds.Contains(x.Data.OrganisationId)).ToList();
        }

        /// <summary>
        /// Récupérer l'organisation correspondante au CiId passé en paramètre 
        /// </summary>
        /// <param name="organisationTree">Arbre des organisations</param>
        /// <param name="ciId">CiId recherché</param>
        /// <returns>Noeud de l'organisation correspondante</returns>
        public static Node<OrganisationBase> GetCiNode(this OrganisationTree organisationTree, int ciId)
        {
            return organisationTree.Nodes.FirstOrDefault(x => x.Data.Id == ciId && (x.Data.IsCi() || x.Data.IsSousCi()));
        }

        /// <summary>
        /// Récupérer l'organisation correspondante au etablissementComptableId passé en paramètre 
        /// </summary>
        /// <param name="organisationTree">Arbre des organisations</param>
        /// <param name="etablissementComptableId">etablissementComptableId recherché</param>
        /// <returns>Noeud de l'organisation correspondante</returns>
        public static Node<OrganisationBase> GetEtablissementComptableNode(this OrganisationTree organisationTree, int etablissementComptableId)
        {
            return organisationTree.Nodes.FirstOrDefault(x => x.Data.Id == etablissementComptableId && x.Data.IsEtablissement());
        }

        /// <summary>
        /// Récupérer l'organisation correspondante au societeId passé en paramètre 
        /// </summary>
        /// <param name="organisationTree">Arbre des organisations</param>
        /// <param name="societeId">societeId recherché</param>
        /// <returns>Noeud de l'organisation correspondante</returns>
        public static Node<OrganisationBase> GetSocieteNode(this OrganisationTree organisationTree, int societeId)
        {
            return organisationTree.Nodes.FirstOrDefault(x => x.Data.Id == societeId && x.Data.IsSociete());
        }

        /// <summary>
        /// Récupérer l'organisation correspondante au groupeId passé en paramètre 
        /// </summary>
        /// <param name="organisationTree">Arbre des organisations</param>
        /// <param name="groupeId">groupeId recherché</param>
        /// <returns>Noeud de l'organisation correspondante</returns>
        public static Node<OrganisationBase> GetGroupeNode(this OrganisationTree organisationTree, int groupeId)
        {
            return organisationTree.Nodes.FirstOrDefault(x => x.Data.Id == groupeId && x.Data.IsGroupe());
        }
    }
}
