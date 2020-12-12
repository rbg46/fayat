using System.Collections.Generic;
using System.Linq;
using Fred.Business.Referential.Tache;
using Fred.DataAccess.Referential.Common;
using Fred.Entities;
using Fred.Framework.Comparers;
using Fred.Framework.Exceptions;

namespace Fred.Business.Budget.Helpers
{
    /// <summary>
    /// Permet de construire un arbre gérant des taches et des ressources vu dans un certain sens 
    /// avec un axe principal et un axe secondaire
    /// Améliorer ça en s'inspirant de LINQ et et de AutoMapper pour que ce soit fortement typé. (Dès que j'ai le temps)
    /// </summary>
    public class AxeTreeBuilder
    {
        private readonly AxeTreeDataSource source;
        private readonly AxePrincipal axePrincipal;
        private readonly ITacheManager tacheManager;
        private readonly ITacheSearchHelper taskSearchHelper;

        /// <summary>
        /// Décrit l'ordre qui sera supporté dans ce cas la le premier niveau est T1 puis le deuxième niveau T2 etc...
        /// Cet ordre est utilisé lorsque l'axe principal choisi est l'axe Tache
        /// </summary>
        private static AxeTypes[] detailAxeTacheRessource = { AxeTypes.T1, AxeTypes.T2, AxeTypes.T3, AxeTypes.Chapitre, AxeTypes.SousChapitre, AxeTypes.Ressource };

        /// <summary>
        /// Décrit l'ordre qui sera supporté dans ce cas la le premier niveau est Chapitre puis le deuxième niveau SousChapitre etc...
        /// Cet ordre est utilisé lorsque l'axe principal choisi est l'axe Ressource
        /// </summary>
        private static AxeTypes[] detailAxeRessourceTache = { AxeTypes.Chapitre, AxeTypes.SousChapitre, AxeTypes.Ressource, AxeTypes.T1, AxeTypes.T2, AxeTypes.T3 };

        /// <summary>
        /// Cet ordre est utilisé lorsque l'axe principale choisi est TacheOnly
        /// </summary>
        private static AxeTypes[] detailAxeTacheOnly = { AxeTypes.T1, AxeTypes.T2, AxeTypes.T3 };


        /// <summary>
        /// Cet ordre est utilisé lorsque l'axe principale choisi est RessourceOnly
        /// </summary>
        private static AxeTypes[] detailAxeRessourceOnly = { AxeTypes.Chapitre, AxeTypes.SousChapitre, AxeTypes.Ressource };

        /// <summary>
        /// Ordre custom choisi par l'utilisateur
        /// </summary>
        private readonly AxeTypes[] detailsAxeCustom;

        /// <summary>
        /// Instancie le builder 
        /// </summary>
        /// <param name="source">la collection d'objet source qui servia a construire l'arbre</param>
        /// <param name="axe">Permet de connaitre le "sens" de l'abre voir l'énumération pour plus d'info</param>
        /// <param name="detailsAxeCustom">Ce paramètre contiendra le détail des axes anaytique choisi
        /// Si aucun paramètre n'est choisi alors le détail par défaut pour l'axe analytique donné est choisi</param>
        public AxeTreeBuilder(AxeTreeDataSource source, AxePrincipal axe, ITacheSearchHelper taskSearchHelper, AxeTypes[] detailsAxeCustom = null)
        {
            this.axePrincipal = axe;
            this.detailsAxeCustom = detailsAxeCustom;
            this.source = source;
            this.taskSearchHelper = taskSearchHelper;
        }

        /// <summary>
        /// Cette fonction renvoie true si l'axe donnée est considéré comme un axe supérieur avec l'axe principal choisi
        /// Un axe est dit supérieur lorsqu'il représente la "frontière" entre son type et le type suivant.
        /// La fonction utilise le détail de l'axe principal par défaut et pas celui fourni par l'utilisateur.
        /// </summary>
        /// <example>
        /// Par exemple l'axe T3 est dit supérieur pour l'axe principal Taches->Ressource car il est le dernier axe 
        /// de type tache dans l'arbre
        /// </example>
        /// <param name="axe">axe qu'on doit analyser (T1-T2-T3...)</param>
        /// <param name="axePrincipal">axe principal utilisé</param>
        /// <returns>true si l'axe est dit supérieur false sinon</returns>
        public static bool IsNiveauSuperieurForDefaultDetails(AxeTypes axe, AxePrincipal axePrincipal)
        {
            switch (axePrincipal)
            {
                case AxePrincipal.RessourceTache:
                    return axe == AxeTypes.Ressource;
                default:
                    //Il n'y a pas de notions de niveau supérieur dans les autres axes principaux
                    return false;
            }
        }

        /// <summary>
        /// Permet de determiner si l'axe est de niveau Chapitre MO Production
        /// Toutes les lignes doivent etre associées au meme chapitre
        /// </summary>
        /// <param name="axe">axe qu'on doit analyser</param>
        /// <param name="lines">lignes associées à l'axe</param>
        /// <returns>true si l'axe est de Chapitre MO Production</returns>
        public static bool IsNiveauChapitreMoProductionOrUnder(AxeTypes axe, IEnumerable<AxeTreeDataSourceRow> lines)
        {
            return (axe == AxeTypes.Chapitre || axe == AxeTypes.SousChapitre)
                        && lines.Any()
                        && lines.All(x => x?.Ressource?.SousChapitre?.Chapitre.Code == Constantes.ChapitreCode.MoProduction);
        }
        /// <summary>
        /// Retourne true si l'axe passé en paramètre corespond à une tache (T1, T2, T3)
        /// </summary>
        /// <param name="axe">Axe à analyser</param>
        /// <returns>un booléen</returns>
        public static bool IsAxeTache(AxeTypes axe)
        {
            return axe == AxeTypes.T1 || axe == AxeTypes.T2 || axe == AxeTypes.T3;
        }

        /// <summary>
        /// Retourne true si l'axe passé en paramètre correspond à une ressource (Chapitre, SousChapitre, Ressource)
        /// </summary>
        /// <param name="axe">Axe à analyser</param>
        /// <returns>un booléen</returns>
        public static bool IsAxeRessource(AxeTypes axe)
        {
            return axe == AxeTypes.Chapitre || axe == AxeTypes.SousChapitre || axe == AxeTypes.Ressource;
        }

        /// <summary>
        /// Renvoi true si l'axe donnée sera une feuille de l'arbre construit avec l'axe principal donné 
        /// et son détail par défaut. Si un détail par défaut à été utiliser, appeler cette fonction ne renverra pas le résultat attendu
        /// Si l'axe principal donné est inconnu la fonction renvoi une exception.
        /// </summary>
        /// <param name="axe">axe dont on veut la position dans l'abre</param>
        /// <param name="axePrincipal">axe principal choisi pour l'arbre</param>
        /// <exception cref="FredBusinessException">Cette exception est renvoyé si la fonction ne reconnait pas l'axe principal passé en paramètre</exception>
        /// <returns>un booléan</returns>
        public static bool IsLeafForDefaultDetails(AxeTypes axe, AxePrincipal axePrincipal)
        {
            switch (axePrincipal)
            {
                case AxePrincipal.RessourceOnly:
                case AxePrincipal.TacheRessource:
                    return axe == AxeTypes.Ressource;
                case AxePrincipal.RessourceTache:
                case AxePrincipal.TacheOnly:
                    return axe == AxeTypes.T3;
                default:
                    throw new FredBusinessException($"l'axe principal {axePrincipal} est inconnu.");
            }
        }


        /// <summary>
        /// Construit l'arbre
        /// </summary>
        /// <returns>l'arbre construit</returns>
        public IEnumerable<AxeTreeModel> GenerateTree()
        {
            if (detailAxeRessourceTache != null && !CheckCoherenceEntreAxePrincipalEtDetailAxeRessourceTache())
            {
                throw new FredBusinessException("Impossible de construire l'arbre : l'axe principal choisi ne fait pas partie des axes à générer");
            }

            switch (axePrincipal)
            {
                case AxePrincipal.RessourceTache:
                    return GenerateTreeForAxeLevel(source, detailsAxeCustom ?? detailAxeRessourceTache);
                case AxePrincipal.TacheRessource:
                    return GenerateTreeForAxeLevel(source, detailsAxeCustom ?? detailAxeTacheRessource);
                case AxePrincipal.TacheOnly:
                    return GenerateTreeForAxeLevel(source, detailsAxeCustom ?? detailAxeTacheOnly);
                case AxePrincipal.RessourceOnly:
                    return GenerateTreeForAxeLevel(source, detailsAxeCustom ?? detailAxeRessourceOnly);
                default:
                    throw new FredBusinessException("Impossible de construire l'arbre, axe est inconnu");
            }
        }

        /// <summary>
        /// Cette fonction vérifie si une structure personnalisée est choisi alors l'axe principal est compatable avec cette strucutre
        /// </summary>
        /// <example>
        /// Si l'utilisateur demande un arbre T1-T2-T3 et choisit l'axe principal Ressource alors il y a une incohérence car aucune ressource n'est présente la structure
        /// Même chose s'il demande un arbre Chapitre-SousChapitre-Ressource et l'axe principal Taches
        /// </example>
        /// <returns>Un booléen : True si tout est bon, false sinon</returns>
        private bool CheckCoherenceEntreAxePrincipalEtDetailAxeRessourceTache()
        {
            if (detailsAxeCustom == null)
            {
                return true;
            }

            switch (axePrincipal)
            {
                case AxePrincipal.TacheRessource:
                case AxePrincipal.TacheOnly:
                    return detailsAxeCustom.Any(axe => axe == AxeTypes.T1 || axe == AxeTypes.T2 || axe == AxeTypes.T3);
                case AxePrincipal.RessourceTache:
                case AxePrincipal.RessourceOnly:
                    return detailsAxeCustom.Any(axe => axe == AxeTypes.Chapitre || axe == AxeTypes.SousChapitre || axe == AxeTypes.Ressource);
                default:
                    throw new FredBusinessException("Impossible de construire l'arbre, axe est inconnu");

            }

        }

        private IEnumerable<AxeTreeModel> GenerateTreeForAxeLevel(AxeTreeDataSource subSource, AxeTypes[] detailAxe, int i = 0)
        {
            var listAxe = new List<AxeTreeModel>();

            if (i < detailAxe.Length)
            {
                listAxe = Grouping(subSource, detailAxe[i]).ToList();

                foreach (var d in listAxe)
                {
                    if (i + 1 < detailAxe.Length)
                    {
                        var newDataSource = new AxeTreeDataSource();
                        newDataSource.Valeurs = d.Source.ToList();
                        d.SousAxe = GenerateTreeForAxeLevel(newDataSource, detailAxe, i + 1).ToList();
                    }
                }
            }

            return listAxe.OrderBy(x => x.Code, new CustomAlphanumericComparer()).ToList();
        }

        private IEnumerable<AxeTreeModel> Grouping(AxeTreeDataSource subSource, AxeTypes currentAxe)
        {
            var grouper = new AxeTreeGrouper(this, axePrincipal, taskSearchHelper);
            switch (currentAxe)
            {
                case AxeTypes.T1:
                    return grouper.GroupByT1(subSource);
                case AxeTypes.T2:
                    return grouper.GroupByT2(subSource);
                case AxeTypes.T3:
                    return grouper.GroupByT3(subSource);
                case AxeTypes.Chapitre:
                    return grouper.GroupByChapitre(subSource);
                case AxeTypes.SousChapitre:
                    return grouper.GroupBySousChapitre(subSource);
                case AxeTypes.Ressource:
                    return grouper.GroupByRessource(subSource);
                default:
                    throw new FredBusinessException("Axe analytique inconnu");
            }
        }

        /// <summary>
        /// Calcul la valeur des colonnes pour la ligne
        /// </summary>
        /// <param name="subSource">Ensemble de lignes qui doivent être résumé par la cellule</param>
        /// <param name="currentAxe">axe analytique de la ligne</param>
        /// <returns>un dictionnaire contenant le nom de la colonne et sa valeur</returns>
        public Dictionary<string, dynamic> ComputeCellValues(IEnumerable<AxeTreeDataSourceRow> subSource, AxeTypes currentAxe)
        {
            var result = new Dictionary<string, dynamic>();
            foreach (var column in source.Columns)
            {
                if (column.ComputeCellValueFunc != null)
                {
                    var parameters = new ComputeColumnValueForAxeParameters
                    {
                        AllSubLines = subSource,
                        ColumnName = column.Title,
                        CurrentAxe = currentAxe,
                        AxePrincipal = axePrincipal,
                        DetailAxeCustom = detailsAxeCustom
                    };

                    result[column.Title] = column.ComputeCellValueFunc(parameters, column.AdditionnalParams.ToArray());
                }
                else
                {
                    result[column.Title] = null;
                }
            }
            return result;
        }
    }
}
