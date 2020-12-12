using System;
using System.Collections.Generic;
using Fred.Entities.Organisation.Tree;

namespace Fred.Business.Tests.Integration.Organisation.Extensions
{
    /// <summary>
    ///  Methode d'extension d'OrganisationTree centrée sur les Parents
    /// </summary>
    public static class ParentsExtension
    {
        /// <summary>
        /// Retourne la liste des OrganisationBase parents. Ne prend pas l'element courant correspond a l'organisationId passé en parametre
        /// </summary>
        /// <param name="organisationTree">organisationTree</param>
        /// <param name="organisationId">organisationId</param>
        /// <returns>La liste des organitionBase parents</returns>
        public static List<OrganisationBase> GetParents(this OrganisationTree organisationTree, int organisationId)
        {
            var result = new List<OrganisationBase>();

            var node = organisationTree.GetNode(organisationId);

            while (node.Parent != null)
            {
                result.Add(node.Parent.Data);

                node = node.Parent;
            }
            return result;
        }


        /// <summary>
        /// Retourne l'arbre des organisationBase en commencant par organisationBase correspondant a l'organisationId puis suivi par tous ses parents.    
        /// L'untilAction passé en parametre stope la recherche des parents
        /// L'element organisationBase correspondant a l'organisationId est retourné de toutes facons.
        /// </summary>
        /// <param name="organisationTree">L'arbre des organisations</param>
        /// <param name="organisationId">organisationId de depart</param>
        /// <param name="untilAction">Action qui stope la recherche des parents mais pas de l'element courrant</param>
        /// <returns>Une liste d'OrganisationBase</returns>
        public static List<OrganisationBase> GetParentsWithCurrentUntil(this OrganisationTree organisationTree, int organisationId, Predicate<OrganisationBase> untilAction = null)
        {
            var result = new List<OrganisationBase>();

            var node = organisationTree.GetNode(organisationId);

            result.Add(node.Data);

            while (node.Parent != null && untilAction(node.Parent.Data))
            {
                result.Add(node.Parent.Data);

                node = node.Parent;
            }
            return result;
        }

        /// <summary>
        /// Retourne l'arbre des organisationBase en commencant par organisationBase correspondant a l'organisationId puis suivi par tous ses parents.   
        /// L'element organisationBase correspondant a l'organisationId est retourné de toutes facons.
        /// </summary>
        /// <param name="organisationTree">L'arbre des organisations</param>
        /// <param name="organisationId">organisationId de depart</param>    
        /// <returns>Une liste d'OrganisationBase</returns>
        public static List<OrganisationBase> GetParentsWithCurrent(this OrganisationTree organisationTree, int organisationId)
        {
            return organisationTree.GetParentsWithCurrentUntil(organisationId, (_) => true);
        }

        /// <summary>
        /// Retourne organisationBase correspondant a l'organisationId
        /// </summary>
        /// <param name="organisationTree">L'arbre des organisations</param>
        /// <param name="organisationId">organisationId</param>
        /// <returns>Le parent</returns>
        public static OrganisationBase GetParent(this OrganisationTree organisationTree, int organisationId)
        {
            var node = organisationTree.GetNode(organisationId);

            return node.Parent?.Data;
        }

        /// <summary>
        /// Récupère un parent d'une organisation
        /// </summary>
        /// <param name="organisationTree">Arbre des organisations</param>
        /// <param name="organisationId">L'Id de l'organisation concernée</param>
        /// <param name="untilAction">Prédicat de sélection</param>
        /// <returns></returns>
        public static OrganisationBase GetParent(this OrganisationTree organisationTree, int organisationId, Predicate<OrganisationBase> untilAction)
        {
            OrganisationBase result = null;

            var node = organisationTree.GetNode(organisationId);

            while (node != null)
            {
                if (untilAction(node.Data))
                {
                    result = node.Data;
                }

                node = node.Parent;
            }
            return result;
        }

        /// <summary>
        /// Récupère la société parente d'une organisation
        /// </summary>
        /// <param name="organisationTree">Arbre des organisation</param>
        /// <param name="organisationId">L'id de l'organisation concernée</param>
        /// <returns>Organisation trouvée</returns>
        public static OrganisationBase GetSocieteParent(this OrganisationTree organisationTree, int organisationId)
        {
            return GetParent(organisationTree, organisationId, x => x.IsSociete());
        }

        /// <summary>
        /// Récupère l'établissement parent d'une organisation
        /// </summary>
        /// <param name="organisationTree">Arbre des organisation</param>
        /// <param name="organisationId">L'id de l'organisation concernée</param>
        /// <returns>Organisation trouvée</returns>
        public static OrganisationBase GetEtablissementComptableParent(this OrganisationTree organisationTree, int organisationId)
        {
            return GetParent(organisationTree, organisationId, x => x.IsEtablissement());
        }

        /// <summary>
        /// Récupère le groupe parent d'une organisation
        /// </summary>
        /// <param name="organisationTree">Arbre des organisation</param>
        /// <param name="organisationId">L'id de l'organisation concernée</param>
        /// <returns>Organisation trouvée</returns>
        public static OrganisationBase GetGroupeParent(this OrganisationTree organisationTree, int organisationId)
        {
            return GetParent(organisationTree, organisationId, x => x.IsGroupe());
        }
    }
}
