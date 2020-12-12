using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Fred.Business.CI;
using Fred.DataAccess.Interfaces;
using Fred.Entities.Referential;
using static Fred.Entities.Constantes;

namespace Fred.Business.Referential.CodeZoneDeplacement
{
    public class CodeZoneDeplacementManager : Manager<CodeZoneDeplacementEnt, ICodeZoneDeplacementRepository>, ICodeZoneDeplacementManager
    {
        private readonly ICodeZoneDeplacementRepository codeZoneDeplacementRepository;
        private readonly ICIManager ciManager;

        public CodeZoneDeplacementManager(IUnitOfWork uow, ICodeZoneDeplacementRepository codeZoneDeplacementRepository, ICIManager ciManager)
          : base(uow, codeZoneDeplacementRepository)
        {
            this.codeZoneDeplacementRepository = codeZoneDeplacementRepository;
            this.ciManager = ciManager;
        }

        /// <summary>
        ///   Vérifie s'il est possible de supprimer un CodeDeplacement
        /// </summary>
        /// <param name="codeZoneDeplacement">Code zone deplacement à supprimer</param>
        /// <returns>True = suppression ok</returns>
        public bool IsDeletable(CodeZoneDeplacementEnt codeZoneDeplacement)
        {
            return Repository.IsDeletable(codeZoneDeplacement);
        }

        /// <summary>
        ///   Permet de récupérer la liste de tous les codes zones deplacements en fonction des critères de recherche.
        /// </summary>
        /// <param name="text">Texte recherché dans tous les codes zones deplacements</param>
        /// <param name="filters">Filtres de recherche</param>
        /// <param name="societeId">Id société</param>
        /// <returns>Retourne la liste filtré de tous les codes zones deplacements</returns>
        public IEnumerable<CodeZoneDeplacementEnt> SearchCodeZoneDeplacementAllBySocieteIdWithFilters(string text, SearchCodeZoneDeplacementEnt filters, int societeId)
        {
            return Repository.SearchCodeZoneDeplacementAllWithFilters(GetPredicate(text, filters));
        }

        /// <summary>
        ///   Permet de récupérer le prédicat de recherche d'un code zone deplacement.
        /// </summary>
        /// <param name="text">Texte recherché dans les codes zones deplacements</param>
        /// <param name="filters">Filtres pour la recherche</param>
        /// <returns>Retourne la condition de recherche des codes zones deplacements</returns>
        private Expression<Func<CodeZoneDeplacementEnt, bool>> GetPredicate(string text, SearchCodeZoneDeplacementEnt filters)
        {
            return p => (string.IsNullOrEmpty(text)
                         || filters.Code && p.Code.ToLower().Contains(text.ToLower())
                         || filters.Libelle && p.Libelle.ToLower().Contains(text.ToLower()))
                        && !filters.IsActif;
        }

        /// <summary>
        ///   Recherche code zone deplacement sur le champ code uniquement
        /// </summary>
        /// <param name="text">Texte à rechercher dans les champs code-libelle</param>
        /// <param name="page">Page actuelle</param>
        /// <param name="pageSize">Taille d'une page</param>
        /// <param name="ciId">Identifiant du CI</param>
        /// <returns>Retourne une liste de code zone deplacement</returns>
        public IEnumerable<CodeZoneDeplacementEnt> SearchLight(string text, int page, int pageSize, int? ciId, bool? isOuvrier = null, bool? isEtam = null, bool? isCadre = null)
        {
            int? societeId = null;
            if (ciId.HasValue)
            {
                var ci = ciManager.GetCiById(ciId.Value, true);
                if (ci.Societe.TypeSociete.Code == TypeSociete.Sep)
                {
                    var societeGerante = ci.Societe.AssocieSeps.SingleOrDefault(a => a.TypeParticipationSep.Code == TypeParticipationSep.Gerant && a.AssocieSepParentId == null);
                    societeId = societeGerante.SocieteAssocieeId;
                }
                else
                {
                    societeId = ci.SocieteId;
                }
            }

            return Repository
                       .Query()
                       .Filter(c => c.IsActif)
                       .Filter(c => !societeId.HasValue || c.SocieteId == societeId.Value)
                       .Filter(p => !isCadre.HasValue || (isCadre.HasValue && p.IsCadre == isCadre.HasValue))
                       .Filter(p => !isOuvrier.HasValue || (isOuvrier.HasValue && p.IsOuvrier == isOuvrier.HasValue))
                       .Filter(p => !isEtam.HasValue || (isEtam.HasValue && p.IsETAM == isEtam.HasValue))
                       .Filter(c => (string.IsNullOrEmpty(text) || c.Code.Contains(text) || c.Libelle.Contains(text)))
                       .OrderBy(list => list.OrderBy(c => c.Code))
                       .GetPage(page, pageSize);
        }

        /// <summary>
        ///   Ajout d'une code zone deplacement
        /// </summary>
        /// <param name="codeAbs">Code zone deplacement à ajouter</param>
        /// <returns>L'identifiant du code zone deplacement ajouté</returns>
        public int AddCodeZoneDeplacement(CodeZoneDeplacementEnt codeAbs)
        {
            codeZoneDeplacementRepository.Insert(codeAbs);
            Save();
            return codeAbs.CodeZoneDeplacementId;
        }

        /// <summary>
        ///   Supprime un code zone deplacement
        /// </summary>
        /// <param name="codeZoneDeplacement">Le code zone deplacement à supprimer</param>
        /// <returns>Retourne vrai si l'entité a bien été supprimé sinon faux</returns>
        public bool DeleteCodeZoneDeplacementById(CodeZoneDeplacementEnt codeZoneDeplacement)
        {
            if (IsDeletable(codeZoneDeplacement))
            {
                codeZoneDeplacementRepository.Delete(codeZoneDeplacement);
                Save();
                return true;
            }

            return false;
        }

        /// <summary>
        ///   La liste de tous les codes zone deplacement.
        /// </summary>
        /// <returns>Renvoie la liste de des codes zone deplacement active</returns>
        public IEnumerable<CodeZoneDeplacementEnt> GetCodeZoneDeplacementList()
        {
            return Repository.GetCodeZoneDeplacementList();
        }

        /// <summary>
        ///   Retourne la liste de tous les codes zone deplacement.
        /// </summary>
        /// <returns>List de toutes les sociétés</returns>
        public IEnumerable<CodeZoneDeplacementEnt> GetCodeZoneDeplacementListAll()
        {
            return Repository.GetCodeZoneDeplacementListAll();
        }

        /// <summary>
        ///   Sauvegarde les modifications d'un CodeZoneDeplacement
        /// </summary>
        /// <param name="codeAbs">Code Zone Deplacement à modifier</param>
        public void UpdateCodeZoneDeplacement(CodeZoneDeplacementEnt codeAbs)
        {
            codeZoneDeplacementRepository.UpdateCodeZoneDeplacement(codeAbs);
        }

        /// <summary>
        ///   Code zone deplacement via l'id
        /// </summary>
        /// <param name="id">Id du code zone deplacement</param>
        /// <returns>Renvoie un code zone deplacement</returns>
        public CodeZoneDeplacementEnt GetCodeZoneDeplacementById(int id)
        {
            return Repository.GetCodeZoneDeplacementById(id);
        }

        /// <summary>
        ///   Code zone deplacement via societeId
        /// </summary>
        /// <param name="societeId">idSociete de la société</param>
        /// <param name="actif">True pour les actifs, false pour les inactifs, null pour tous.</param>
        /// <returns>Renvoie un code zone deplacement</returns>
        public IEnumerable<CodeZoneDeplacementEnt> GetCodeZoneDeplacementBySocieteId(int societeId, bool? actif = null)
        {
            return Repository.GetCodeZoneDeplacementBySocieteId(societeId, actif);
        }

        /// <summary>
        ///   Import des codes zones deplacements automatiques depuis la Holding
        /// </summary>
        /// <param name="holdingId"> Id du Holding</param>
        /// <returns>Renvoie un int</returns>
        public int ImportCodeZoneDeplacementFromHolding(int holdingId)
        {
            return Repository.ImportCodeZoneDeplacementFromHolding(holdingId);
        }

        /// <summary>
        ///   Permet l'initialisation d'une nouvelle instance de code zone deplacement.
        /// </summary>
        /// <param name="societeId">Identifiant de la societe</param>
        /// <returns>Nouvelle instance de code zone deplacement intialisée</returns>
        public CodeZoneDeplacementEnt GetNewCodeZoneDeplacement(int societeId)
        {
            return new CodeZoneDeplacementEnt { IsActif = true, SocieteId = societeId };
        }

        /// <summary>
        ///   Permet de récupérer la liste des codes zone deplacement en fonction des critères de recherche.
        /// </summary>
        /// <param name="text">Texte recherché dans les sociétés</param>
        /// <param name="filters">Filtres de recherche</param>
        /// <returns>Retourne la liste filtré des sociétés</returns>
        public IEnumerable<CodeZoneDeplacementEnt> SearchCodeZoneDeplacementWithFilters(string text, SearchCodeZoneDeplacementEnt filters)
        {
            return Repository.SearchCodeZoneDeplacementWithFilters(GetPredicate(text, filters));
        }

        /// <summary>
        ///   Permet de récupérer la liste de tous les codes zones deplacements en fonction des critères de recherche par société.
        /// </summary>
        /// <param name="filters">Filtres de recherche sur tous les codes zones deplacements</param>
        /// <param name="societeId">Id de la societe</param>
        /// <param name="text">Le text de recherche</param>
        /// <returns>Retourne la liste filtrée de tous les codes zones deplacements</returns>
        public IEnumerable<CodeZoneDeplacementEnt> SearchCodeZoneDeplacementAllBySocieteIdWithFilters(
          SearchCodeZoneDeplacementEnt filters,
          int societeId,
          string text)
        {
            return Repository.SearchCodeZoneDeplacementAllBySocieteIdWithFilters(societeId, GetPredicate(text, filters));
        }

        /// <summary>
        ///   Permet de récupérer la liste de tous les codes zones deplacements en fonction des critères de recherche.
        /// </summary>
        /// <param name="text">Texte recherché dans tous les codes zones deplacements</param>
        /// <param name="filters">Filtres de recherche</param>
        /// <returns>Retourne la liste filtré de tous les codes zones deplacements</returns>
        public IEnumerable<CodeZoneDeplacementEnt> SearchCodeZoneDeplacementAllWithFilters(string text, SearchCodeZoneDeplacementEnt filters)
        {
            return Repository.SearchCodeZoneDeplacementAllWithFilters(GetPredicate(text, filters));
        }

        /// <summary>
        ///   Permet de connaître l'existence d'une société depuis son code.
        /// </summary>
        /// <param name="idCourant">L'Id courant</param>
        /// <param name="codeZoneDeplacement">Le code de codeZoneDeplacement</param>
        /// <param name="societeId">Le code de la société</param>
        /// <returns>Retourne vrai si le code passé en paramètre existe déjà, faux sinon</returns>
        public bool IsCodeZoneDeplacementExistsByCode(int idCourant, string codeZoneDeplacement, int societeId)
        {
            return Repository.IsCodeZoneDeplacementExistsByCode(idCourant, codeZoneDeplacement, societeId);
        }

        /// <summary>
        ///   Permet de savoir si le code zone deplacement existe déjà pour une societe
        /// </summary>
        /// <param name="codeZoneDeplacement">code CodeZoneDeplacement</param>
        /// <param name="societeId">Id societe</param>
        /// <returns>Retourne vrai si le code passé en paramètre existe déjà, faux sinon</returns>
        public bool IsCodeZoneDeplacementExistsBySoc(string codeZoneDeplacement, int societeId)
        {
            return Repository.IsCodeZoneDeplacementExistsBySoc(codeZoneDeplacement, societeId);
        }

        /// <summary>
        ///   Permet de récupérer les champs de recherche lié à un code zone deplacement.
        /// </summary>
        /// <returns>Retourne la liste des champs de recherche par défaut d'un code zone deplacement</returns>
        public SearchCodeZoneDeplacementEnt GetDefaultFilter()
        {
            var recherche = new SearchCodeZoneDeplacementEnt();
            recherche.Code = true;
            recherche.Libelle = true;
            recherche.IsActif = false;
            return recherche;
        }

        /// <summary>
        ///  Vérifie s'il l'entité est déja utilisée
        /// </summary>
        /// <param name="id">Id de l'entité à vérifié</param>
        /// <returns>True = déjà Utilisée</returns>
        public bool IsAlreadyUsed(int id)
        {
            return Repository.IsAlreadyUsed(id);
        }



        /// <summary>
        ///   Permet la récupération d'une zone de déplacement en fonction de l'identifiant de sa société de paramétrage et de son
        ///   code.
        /// </summary>
        /// <param name="societeId">Identifiant de la société de paramétrage de la zone de déplacement</param>
        /// <param name="codeZone">Code de la zone de déplacement</param>
        /// <returns>Retourne la zone de déplacement en fonction du code passé en paramètre, null si inexistant.</returns>
        public CodeZoneDeplacementEnt GetZoneBySocieteIdAndCode(int societeId, string codeZone)
        {
            return Repository.GetZoneBySocieteIdAndCode(societeId, codeZone);
        }

        /// <summary>
        ///  Récupère le code zone le plus avantageux 
        /// </summary>
        /// <param name="codeZone1">Premier code zone à comparer</param>
        /// <param name="codeZone2">Deuxième code zone à comparer</param>
        /// <param name="societeId">Identifiant de la société</param>
        /// <returns>Le code le plus avantageux</returns>
        public string GetCompareBiggestCodeZondeDeplacement(string codeZone1, string codeZone2, int societeId)
        {
            return Repository.GetCompareBiggestCodeZondeDeplacement(codeZone1, codeZone2, societeId);
        }

        /// <summary>
        /// Permet de récupérer le code zone deplacement en fonction du kilométrage
        /// </summary>
        /// <param name="societeId">société concerné</param>
        /// <param name="km">kilomètre</param>
        /// <returns>code zone déplacement</returns>
        public CodeZoneDeplacementEnt GetCodeZoneDeplacementByKm(int societeId, double km)
        {
            return Repository.GetCodeZoneDeplacementByKm(societeId, km);
        }
    }
}
