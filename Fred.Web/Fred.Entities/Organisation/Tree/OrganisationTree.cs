using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Fred.Entities.Organisation.Tree
{
    /// <summary>
    ///  Represente la liste des organisations en noeud (N ARY TREE) 
    /// </summary>
    [DebuggerDisplay("Nombre de Nodes = {Nodes.Count}")]

    public class OrganisationTree
    {

        /// <summary>
        /// ctor. On créer seulement si on a les données
        /// </summary>
        private OrganisationTree()
        {

        }

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="organisations">Liste d'organisationBase</param>
        public OrganisationTree(List<OrganisationBase> organisations)
        {
            foreach (var organisationBase in organisations)
            {
                var node = new Node<OrganisationBase>(organisationBase);
                this.Nodes.Add(node);
                this.DictionaryNodes.Add(organisationBase.OrganisationId, node);
            }

            foreach (var nodeGroupByParentId in this.Nodes.GroupBy(n => n.Data.PereId))
            {
                if (nodeGroupByParentId.Key.HasValue)
                {
                    var containKeyParent = this.DictionaryNodes.ContainsKey(nodeGroupByParentId.Key.Value);
                    if (containKeyParent)
                    {
                        this.DictionaryNodes[nodeGroupByParentId.Key.Value].AddAllChildren(nodeGroupByParentId.ToList());
                    }
                }
            }
        }

        /// <summary>
        /// Represente la liste des noeuds 
        /// </summary>
        public List<Node<OrganisationBase>> Nodes { get; set; } = new List<Node<OrganisationBase>>();

        /// <summary>
        /// Creation du dictionniare orgnisation id et noeud. Rend la recherche par organisationId bq plus rapide
        /// </summary>
        public Dictionary<int, Node<OrganisationBase>> DictionaryNodes { get; set; } = new Dictionary<int, Node<OrganisationBase>>();

        /// <summary>
        /// Retourne l'arbre pour un utilisateur donné
        /// </summary>       
        /// <param name="userId">userId</param>
        /// <returns>L'arbre des organisations de fred pour un utilisateur donné</returns>
        public OrganisationTree GetOrganisationTreeForUser(int userId)
        {
            var userNodes = this.Nodes.Where(n => n.Data.Affectations.Any(a => a.UtilisateurId == userId));

            var allChildren = userNodes.SelectMany(un => un.LevelOrder()).Distinct().ToList();

            return new OrganisationTree(allChildren);
        }

        /// <summary>
        /// Retourne le node correspondant eyant comme Data une OrganisationBase qui a l' organisationId passé en parametre
        /// </summary>       
        /// <param name="organisationId">organisationId</param>
        /// <returns>Un node d'organisationBase, Delclenche une InvalidOperationException si l'organisationId ne correspond a aucun Node</returns>
        public Node<OrganisationBase> GetNode(int organisationId)
        {
            return this.DictionaryNodes[organisationId];
        }

        /// <summary>
        /// Retourne la liste des OrganisationBase parents. Prend l'element courant correspondANT a l'organisationId passé en parametre
        /// </summary>     
        /// <param name="organisationId">organisationId</param>
        /// <returns>La liste des organitionBase parents</returns>
        public List<OrganisationBase> GetParentsWithCurrent(int organisationId)
        {
            var result = new List<OrganisationBase>();

            var node = this.GetNode(organisationId);

            while (node != null)
            {
                result.Add(node.Data);

                node = node.Parent;
            }
            return result;
        }

        /// <summary>
        /// Retourne les OrganisationBase parents jusqu'au groupe
        /// Si le type de l'organisation est est superieur au groupe alors rien n'est retourné
        /// Le groupe est inclu dans le resultat
        /// </summary>
        /// <param name="organisationId">organisationId</param>
        /// <returns>Les parent jusqu'au groupe</returns>
        public List<OrganisationBase> GetParentsWithCurrentUntilGroupe(int organisationId)
        {
            var result = new List<OrganisationBase>();

            var node = this.GetNode(organisationId);

            while (node != null)
            {
                if (node.Data.IsHolding() || node.Data.IsPole())
                {
                    break;
                }

                result.Add(node.Data);

                node = node.Parent;
            }
            return result;
        }

        /// <summary>
        /// Retourne les OrganisationBase parents jusqu'a la societe
        /// Si le type de l'organisation est est superieur a la societe alors rien n'est retourné
        /// La societe est inclu dans le resultat
        /// </summary>
        /// <param name="organisationIdOfSociete">organisationId de la societe</param>
        /// <returns>Les parent jusqu'au groupe</returns>
        public List<OrganisationBase> GetParentsWithCurrentUntilSociete(int organisationIdOfSociete)
        {
            var result = new List<OrganisationBase>();

            var node = this.GetNode(organisationIdOfSociete);

            while (node != null)
            {
                if (node.Data.IsHolding() || node.Data.IsPole() || node.Data.IsGroupe())
                {
                    break;
                }

                result.Add(node.Data);

                node = node.Parent;
            }
            return result;
        }

        /// <summary>
        /// Retourne la liste des OrganisationBase parents. Ne prend pas l'element courant correspondANT a l'organisationId passé en parametre
        /// </summary>     
        /// <param name="organisationId">organisationId</param>
        /// <returns>La liste des organitionBase parents</returns>
        public OrganisationBase GetFirstParent(int organisationId)
        {
            var node = this.GetNode(organisationId);

            return node?.Parent?.Data;
        }

        /// <summary>
        /// Retourne la liste des OrganisationBase parents. Ne prend pas l'element courant correspondant a l'organisationId passé en parametre
        /// </summary>     
        /// <param name="organisationId">organisationId</param>
        /// <returns>La liste des organitionBase parents</returns>
        public List<OrganisationBase> GetParents(int organisationId)
        {
            var result = new List<OrganisationBase>();

            var node = this.GetNode(organisationId);

            while (node.Parent != null)
            {
                result.Add(node.Parent.Data);

                node = node.Parent;
            }
            return result;
        }

        /// <summary>
        /// Retourne le parent
        /// </summary>
        /// <param name="selectionAction">Selection si Predicate respecté</param>
        /// <param name="untilAction">Recherche effectue jusqu'a Predicate respecté</param>
        /// <returns>OrganisationBase</returns>
        private OrganisationBase GetParent(Predicate<OrganisationBase> selectionAction, Predicate<OrganisationBase> untilAction)
        {
            OrganisationBase result = null;

            var node = this.Nodes.FirstOrDefault(x => selectionAction(x.Data));

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
        /// Recupere les parents et l'element courant
        /// </summary>
        /// <param name="selectionAction">Action de selection du premier element</param>
        /// <returns>Tous les parents ainsi que l'element correspondnat a la selection</returns>
        private List<OrganisationBase> GetParents(Predicate<OrganisationBase> selectionAction)
        {
            var result = new List<OrganisationBase>();

            var node = this.Nodes.FirstOrDefault(x => selectionAction(x.Data));

            while (node != null)
            {
                result.Add(node.Data);

                node = node.Parent;
            }
            return result;
        }

        /// <summary>
        /// Recupere le pole a partir de la societe
        /// </summary>
        /// <param name="societeId">Le societeId</param>
        /// <returns>Le pole</returns>
        public OrganisationBase GetPoleParentOfSociete(int societeId)
        {
            var poleOrganisation = GetParent(x => x.Id == societeId && x.IsSociete(), x => x.IsPole());

            return poleOrganisation;
        }

        /// <summary>
        /// Retourne les parents du ci et le ci courrant
        /// </summary>         
        /// <param name="ciId">le ciId</param>
        /// <returns>Liste des parent du ci et le ci courrant</returns>
        public List<OrganisationBase> GetAllParentsOfCi(int ciId)
        {
            return GetParents(x => x.Id == ciId && (x.IsCi() || x.IsSousCi()));
        }

        /// <summary>
        /// Retourne la societe parent a partir d'un ciId
        /// </summary>         
        /// <param name="ciId">ciId</param>
        /// <returns>OrganisationBase</returns>
        public OrganisationBase GetSocieteParentOfCi(int ciId)
        {
            return GetParent(x => x.Id == ciId && (x.IsCi() || x.IsSousCi()), x => x.IsSociete());
        }

        /// <summary>
        /// Retourne la societe parent a partir d'un organisationId
        /// </summary>         
        /// <param name="organisationId">organisationId</param>
        /// <returns>OrganisationBase</returns>
        public OrganisationBase GetSocieteParent(int organisationId)
        {
            var parentsWithCurrent = GetParentsWithCurrent(organisationId);

            return parentsWithCurrent.FirstOrDefault(x => x.IsSociete());

        }

        /// <summary>
        /// Retourne le groupe parent a partir d'un ciId
        /// </summary>         
        /// <param name="ciId">ciId</param>
        /// <returns>OrganisationBase</returns>
        public OrganisationBase GetGroupeParentOfCi(int ciId)
        {
            return GetParent(x => x.Id == ciId && (x.IsCi() || x.IsSousCi()), x => x.IsGroupe());
        }

        /// <summary>
        /// Recupere le groupe a partir du groupeId
        /// </summary>
        /// <param name="groupeId">Le groupe Id</param>
        /// <returns>Le groupe</returns>
        public OrganisationBase GetGroupe(int groupeId)
        {
            var groupe = this.Nodes.FirstOrDefault(n => n.Data.IsGroupe() && n.Data.Id == groupeId);

            return groupe?.Data;
        }

        /// <summary>
        /// Retourne la societe parent de EtablissementComptable (OrganisationBase)
        /// </summary>         
        /// <param name="etablissementComptableId">l'etablissementComptableId</param>
        /// <returns>L'OrganisationBase de type societe</returns>
        public OrganisationBase GetSocieteParentOfEtablissementComptable(int etablissementComptableId)
        {
            return GetParent(x => x.Id == etablissementComptableId && x.IsEtablissement(), x => x.IsSociete());
        }


        /// <summary>
        /// Retourne le groupe parent de la societe (OrganisationBase)
        /// </summary>         
        /// <param name="societeId">le societeId</param>
        /// <returns>L'OrganisationBase de type groupe</returns>
        public OrganisationBase GetGroupeParentOfSociete(int societeId)
        {
            return GetParent(x => x.Id == societeId && x.IsSociete(), x => x.IsGroupe());
        }

        /// <summary>
        /// Retourne les ci acessible par un utilisateur
        /// </summary>
        /// <param name="userId">userId</param>
        /// <returns>Liste de ci IDs</returns>
        public List<int> GetAllCiForUser(int userId)
        {
            return GetAllCiOrganisationBaseForUser(userId).Select(o => o.Id).ToList();
        }

        /// <summary>
        /// Retourne les ci acessible par un utilisateur
        /// </summary>
        /// <param name="userId">userId</param>
        /// <returns>Liste de ci IDs</returns>
        public List<OrganisationBase> GetAllCiOrganisationBaseForUser(int userId)
        {
            var userOrganisationTree = this.GetOrganisationTreeForUser(userId);

            var organisationOfTypeCi = userOrganisationTree.Nodes.SelectMany(n => n.LevelOrder(o => o.IsCi() || o.IsSousCi())).OrderBy(n => n.OrganisationId).Distinct().ToList();

            return organisationOfTypeCi.ToList();
        }

        /// <summary>
        /// Recupere toutes les OrganisationBase de type ci pour une societeId donnée
        /// </summary>
        /// <param name="societeId">societeId</param>
        /// <returns>Liste de ci OrganisationBase</returns>
        public List<OrganisationBase> GetAllCisOfSociete(int societeId)
        {
            var organisationGroupeNode = this.Nodes.FirstOrDefault(n => n.Data.IsSociete() && n.Data.Id == societeId);

            var organisationOfTypeCi = organisationGroupeNode.LevelOrder(o => o.IsCi() || o.IsSousCi());

            return organisationOfTypeCi.ToList();
        }

        /// <summary>
        /// Recupere un ci en fonction du groupeId, de code de la societe et du code du CI.
        /// </summary>
        /// <param name="groupeId">groupeId</param>
        /// <param name="codeSociete"> code de la societe</param>
        /// <param name="codeCi">code du CI</param>
        /// <returns>OrganisationBase de type CI</returns>
        public OrganisationBase GetCi(int groupeId, string codeSociete, string codeCi)
        {
            var groupe = this.Nodes.FirstOrDefault(n => n.Data.IsGroupe() && n.Data.Id == groupeId);

            if (groupe == null)
            {
                return null;
            }

            var societes = groupe.LevelOrder(o => o.IsSociete());

            var societe = societes.FirstOrDefault(n => string.Equals(n.Code, codeSociete, StringComparison.OrdinalIgnoreCase));

            if (societe == null)
            {
                return null;
            }

            var societeNode = this.GetNode(societe.OrganisationId);

            var cis = societeNode.LevelOrder(o => o.IsCi() || o.IsSousCi());

            return cis.FirstOrDefault(x => string.Equals(x.Code, codeCi, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Retourne le ci (OrganisationBase)
        /// </summary>
        /// <param name="ciId">le ciId</param>
        /// <returns>Les OrganisationBase de type CI ou Sous CI</returns>
        public OrganisationBase GetCi(int ciId)
        {
            var ci = this.Nodes.FirstOrDefault(n => (n.Data.IsCi() || n.Data.IsSousCi()) && n.Data.Id == ciId);

            return ci?.Data;
        }


        /// <summary>
        /// Retourne le ci (OrganisationBase)
        /// </summary>     
        /// <param name="ciId">le ciId</param>
        /// <returns>Les OrganisationBase de type CI ou Sous CI</returns>
        public OrganisationBase GetCiStrict(int ciId)
        {
            var ci = this.Nodes.FirstOrDefault(n => n.Data.IsCi() && n.Data.Id == ciId);

            return ci?.Data;
        }

        /// <summary>
        /// Retourne le ci (OrganisationBase)
        /// </summary>       
        /// <param name="ciId">le ciId</param>
        /// <returns>Les OrganisationBase de type CI ou Sous CI</returns>
        public OrganisationBase GetSousCi(int ciId)
        {
            var sousCi = this.Nodes.FirstOrDefault(n => n.Data.IsSousCi() && n.Data.Id == ciId);

            return sousCi?.Data;
        }



        /// <summary>
        /// Retourne l'EtablissementComptable (OrganisationBase)
        /// </summary>       
        /// <param name="etablissementComptableId">l'etablissementComptableId</param>
        /// <returns>Les OrganisationBase de type EtablissementComptable</returns>
        public OrganisationBase GetEtablissementComptable(int etablissementComptableId)
        {
            var etablissementComptable = this.Nodes.FirstOrDefault(n => n.Data.IsEtablissement() && n.Data.Id == etablissementComptableId);

            return etablissementComptable?.Data;
        }

        /// <summary>
        /// Retourne l'etablissement comptable parent a partir d'un ciId
        /// </summary>         
        /// <param name="ciId">ciId</param>
        /// <returns>OrganisationBase</returns>
        public OrganisationBase GetEtablissementComptableOfCi(int ciId)
        {
            return GetParent(x => x.Id == ciId && (x.IsCi() || x.IsSousCi()), x => x.IsEtablissement());
        }

        /// <summary>
        /// Retourne les établissements comptable accessibles par un utilisateur
        /// </summary>      
        /// <param name="userId">userId</param>
        /// <returns>Liste d'établissements comptables IDs</returns>
        public List<int> GetAllEtablissementComptableForUser(int userId)
        {
            return GetAllEtablissementOrganisationBaseForUser(userId).Select(o => o.OrganisationId).ToList();
        }

        /// <summary>
        /// Retourne les établissements comptable accessibles par un utilisateur
        /// </summary>       
        /// <param name="userId">userId</param>
        /// <returns>Liste d'établissement comptable</returns>
        private List<OrganisationBase> GetAllEtablissementOrganisationBaseForUser(int userId)
        {
            var userOrganisationTree = this.GetOrganisationTreeForUser(userId);

            var organisationOfTypeEtablissement = userOrganisationTree.Nodes.SelectMany(n => n.LevelOrder(o => o.IsEtablissement())).OrderBy(n => n.OrganisationId).Distinct().ToList();

            return organisationOfTypeEtablissement.ToList();
        }

        /// <summary>
        /// Retourne les societes d'un groupe
        /// </summary>       
        /// <param name="groupeId">le groupeId</param>
        /// <returns>Les OrganisationBase de type Societe</returns>
        public List<OrganisationBase> GetAllSocietesForGroupe(int groupeId)
        {
            var organisationOfTypeCi = this.Nodes.Where(n => n.Data.IsGroupe() && n.Data.Id == groupeId).SelectMany(n => n.LevelOrder(o => o.IsSociete())).OrderBy(n => n.OrganisationId).Distinct().ToList();

            return organisationOfTypeCi.ToList();
        }

        /// <summary>
        /// Recuere toutes les societes
        /// </summary>
        /// <returns>Toutes les societes</returns>
        public List<OrganisationBase> GetAllSocietes()
        {
            var societes = this.Nodes.Where(n => n.Data.IsSociete()).Select(x => x.Data).ToList();

            return societes;
        }

        /// <summary>
        /// Retourne la societe (OrganisationBase)
        /// </summary>       
        /// <param name="societeId">le societeId</param>
        /// <returns>Les OrganisationBase de type societe</returns>
        public OrganisationBase GetSociete(int societeId)
        {
            var etablissementComptable = this.Nodes.FirstOrDefault(n => n.Data.IsSociete() && n.Data.Id == societeId);

            return etablissementComptable?.Data;
        }

        /// <summary>
        /// Visualisation de l'arbre a partir d'une organisation
        /// </summary>
        /// <param name="root">Lorganisation a partir de laquelle on veux afficher l'arbre</param>
        public void PrintDebug(OrganisationBase root)
        {
            var node = this.GetNode(root.OrganisationId);

            node.PreOrder((organisationBase) =>
            {

                var prefix = string.Format("[{0,-5}] - {1,-20} ", organisationBase.OrganisationId, organisationBase.GetOrganisationTypeLabel());

                var indent = string.Join(string.Empty, Enumerable.Repeat("   ", organisationBase.TypeOrganisationId));

                var suffix = string.Format("{0}{1}(Id:{2})", indent, organisationBase.Code, organisationBase.Id);

                var suffix2 = string.Format("(Parent:{0})", organisationBase.PereId);

                var result = string.Format("{0,-28} {3,-50} {1,-15} {2,-30}  ",
                   prefix,
                   suffix2,
                   organisationBase.Libelle,
                   suffix);

                Debug.WriteLine(result);
            });
        }

    }
}
