using System;
using System.Collections.Generic;
using System.Linq;
using Fred.Business.Utilisateur;
using Fred.DataAccess.Interfaces;
using Fred.Entities.CI;
using Fred.Entities.Referential;
using Fred.Entities.Utilisateur;

namespace Fred.Business.Referential
{
    /// <summary>
    ///   Référentiel de données pour les codes majoration.
    /// </summary>
    public class CodeMajorationManager : Manager<CodeMajorationEnt, ICodeMajorationRepository>, ICodeMajorationManager
    {
        private readonly IRepository<CICodeMajorationEnt> repository;
        private readonly IUtilisateurManager utilisateurMgr;

        public CodeMajorationManager(
            IUnitOfWork uow,
            ICodeMajorationRepository codeMajRepo,
            IUtilisateurManager utilisateurMgr,
            IRepository<CICodeMajorationEnt> repository)
            : base(uow, codeMajRepo)
        {
            this.repository = repository;
            this.utilisateurMgr = utilisateurMgr;
        }

        /// <summary>
        ///   Vérifie s'il est possible de supprimer un CodeDeplacement
        /// </summary>
        /// <param name="codeMajorationEnt">Code majoration à supprimer</param>
        /// <returns>True = suppression ok</returns>
        public bool IsDeletable(CodeMajorationEnt codeMajorationEnt)
        {
            return this.Repository.IsDeletable(codeMajorationEnt);
        }

        /// <summary>
        ///   Retourne la liste des codesMajoration.
        /// </summary>
        /// <param name="utilisateur">utilisater pour lequel récupérer le code Majoration</param>
        /// <param name="recherche">critère textuel de recherche</param>
        /// <returns>La liste des codesMajoration.</returns>
        public IEnumerable<CodeMajorationEnt> GetCodeMajorationList(UtilisateurEnt utilisateur = null, string recherche = null)
        {
            var retour = this.Repository.GetCodeMajorationList(recherche);
            if (utilisateur != null)
            {
                retour = retour.Where(cm => cm.GroupeId == utilisateur.Personnel.Societe.GroupeId);
            }
            return retour;
        }

        /// <summary>
        ///   Retourne la liste des codesMajoration pour la synchronisation mobile.
        /// </summary>
        /// <param name="utilisateur">utilisater pour lequel récupérer le code Majoration</param>
        /// <param name="recherche">critère textuel de recherche</param>
        /// <returns>La liste des codesMajoration.</returns>
        public IEnumerable<CodeMajorationEnt> GetCodeMajorationListSync(UtilisateurEnt utilisateur = null, string recherche = null)
        {
            return this.Repository.GetCodeMajorationListSync(recherche);
        }

        /// <summary>
        ///   Retourne le CodeMajoration portant l'identifiant unique indiqué.
        /// </summary>
        /// <param name="codeMajorationID">Identifiant du CodeMajoration à retrouver.</param>
        /// <returns>Le CodeMajoration retrouvé, sinon nulle.</returns>
        public CodeMajorationEnt GetCodeMajorationById(int codeMajorationID)
        {
            return this.Repository.GetCodeMajorationById(codeMajorationID);
        }

        /// <summary>
        ///   Retourne la liste de codes majoration associés à la société + les publics
        /// </summary>
        /// <param name="groupeId">Identifiant de la société associée aux code majoration à retourner</param>
        /// <returns>La liste des codes majoration associés à la société</returns>
        public IEnumerable<CodeMajorationEnt> GetCodeMajorationListByGroupeId(int groupeId)
        {
            return this.Repository.GetCodeMajorationListByGroupeId(groupeId);
        }

        /// <summary>
        ///   Retourne la liste de codes majoration actifs associés à la société + les publics
        /// </summary>
        /// <param name="societeId"> Identifiant du groupe associé aux code majoration à retourner </param>
        /// <param name="ciId">Identifiant du CI</param>
        /// <returns> La liste des codes majoration actifs associés à la société </returns>
        public IEnumerable<CodeMajorationEnt> GetActifsCodeMajorationListBySocieteId(int societeId, int ciId)
        {
            return this.Repository.GetActifsCodeMajorationListByGroupeId(societeId, ciId);
        }

        /// <summary>
        ///   Ajout un nouveau CodeMajoration
        /// </summary>
        /// <param name="codeMajorationEnt"> CodeMajoration à ajouter</param>
        /// <param name="createur">createur du code majoration</param>
        /// <returns> L'identifiant du CodeMajoration ajouté</returns>
        public int AddCodeMajoration(CodeMajorationEnt codeMajorationEnt, UtilisateurEnt createur)
        {
            codeMajorationEnt.GroupeId = createur.Personnel.Societe.GroupeId;
            this.Repository.Insert(codeMajorationEnt);
            Save();
            return codeMajorationEnt.CodeMajorationId;
        }

        /// <summary>
        ///   Sauvegarde les modifications d'un CodeMajoration
        /// </summary>
        /// <param name="codeMajorationEnt">CodeMajoration à modifier</param>
        public void UpdateCodeMajoration(CodeMajorationEnt codeMajorationEnt)
        {
            this.Repository.UpdateCodeMajoration(codeMajorationEnt);
        }

        /// <summary>
        ///   Supprime un CodeMajoration
        /// </summary>
        /// <param name="codeMajorationEnt">CodeMajoration à supprimer</param>
        /// <returns>Retourne vrai si l'entité a bien été supprimé sinon faux</returns>
        public bool DeleteCodeMajorationById(CodeMajorationEnt codeMajorationEnt)
        {
            if (IsDeletable(codeMajorationEnt))
            {
                this.Repository.Delete(codeMajorationEnt);
                Save();
                return true;
            }

            return false;
        }

        /// <summary>
        ///   Permet l'initialisation d'une nouvelle instance de code majoration.
        /// </summary>
        /// <returns>Nouvelle instance de code majoration intialisée</returns>
        public CodeMajorationEnt GetNewCodeMajoration()
        {
            return new CodeMajorationEnt
            {
                IsActif = true
            };
        }

        /// <summary>
        ///   Méthode vérifiant l'existence d'un code majoration via son code.
        /// </summary>
        /// <param name="idCourant"> id courant</param>
        /// <param name="code">Code du code majoration</param>
        /// <param name="groupeId">Identifiant du groupe</param>
        /// <returns>Retourne vrai si la nature existe, faux sinon</returns>
        public bool IsCodeMajorationExistsByCodeInGroupe(int idCourant, string code, int groupeId)
        {
            return this.Repository.IsCodeMajorationExistsByCodeInGroupe(idCourant, code, groupeId);
        }



        #region Gestion codes majoration avec CI

        /// <summary>
        ///   Retourne la liste complète de CICodeMajorations
        /// </summary>
        /// <returns>Une liste de CICodeMajorations</returns>
        public IEnumerable<CICodeMajorationEnt> GetCiCodeMajorationList()
        {
            return this.Repository.GetCiCodeMajorationList();
        }

        /// <summary>
        ///   Retourne la liste complète de CICodeMajorations pour la synchronisation mobile.
        /// </summary>
        /// <returns>Une liste de CICodeMajorations</returns>
        public IEnumerable<CICodeMajorationEnt> GetCiCodeMajorationListSync()
        {
            return this.Repository.GetCiCodeMajorationListSync();
        }

        /// <summary>
        ///   Ajout ou mise à jour d'une liste d'associations CI/Code majoration
        /// </summary>
        /// <param name="ciId">Identifiant du CI</param>
        /// <param name="ciCodeMajorationList">Liste des relations CI/CodeMajoration à ajouter ou mettre à jour</param> 
        /// <returns>Liste des CICodeMajoration crée et/ou mis à jour</returns>
        public IEnumerable<CICodeMajorationEnt> ManageCICodeMajoration(int ciId, IEnumerable<CICodeMajorationEnt> ciCodeMajorationList)
        {
            List<int> existingCICodeMajorationList = GetCodeMajorationIdsByCiId(ciId);
            List<int> ciCodeMajorationIdList = ciCodeMajorationList.Select(x => x.CodeMajorationId).ToList();

            // Suppresion des relations CICodeMajorations
            if (existingCICodeMajorationList.Count > 0)
            {
                foreach (int cmId in existingCICodeMajorationList)
                {
                    if (!ciCodeMajorationIdList.Contains(cmId))
                    {
                        DeleteCICodeMajorationById(cmId, ciId);
                    }
                }
            }

            Repository.AddOrUpdateCICodeMajorationList(ciCodeMajorationList);
            Save();

            return ciCodeMajorationList;
        }

        /// <summary>
        ///   Cherche une liste d'ID de code majoration en fonction d'un ID de CI
        /// </summary>
        /// <param name="ciId">ID du CI pour lequel on recherche les IDs de codes majoration correspndants</param>
        /// <returns>Une liste d'ID de CodeMajoration.</returns>
        public List<int> GetCodeMajorationIdsByCiId(int ciId)
        {
            return Repository.GetCodeMajorationIdsByCiId(ciId);
        }

        /// <summary>
        ///   Ajout un nouvelle association CI/Code majoration
        /// </summary>
        /// <param name="codesMajorationIds"> Codes majoration à associer</param>
        /// <param name="ciId"> CI à associer</param>
        public void AddCiCodesMajoration(int codesMajorationIds, int ciId)
        {
            var ciCodeMaj = new CICodeMajorationEnt();
            ciCodeMaj.CiId = ciId;
            ciCodeMaj.CodeMajorationId = codesMajorationIds;
            Repository.AddOrUpdateCICodeMajoration(ciCodeMaj);
            Save();
        }

        /// <summary>
        ///   Supprime un CICodeMajoration à partir des ses IDs Code Majoration et CI
        /// </summary>
        /// <param name="codeMajId">ID du code majoration référencé</param>
        /// <param name="ciId">ID du CI référence</param>
        public void DeleteCICodeMajorationById(int codeMajId, int ciId)
        {
            this.Repository.DelteCICodeMajoration(codeMajId, ciId);
            Save();
        }

        #endregion

        /// <inheritdoc />
        public IEnumerable<CodeMajorationEnt> GetSyncCodeMajorations(DateTime lastModification = default(DateTime))
        {
            //On récupère l'utilisateur courant.
            var currentUser = this.utilisateurMgr.GetContextUtilisateur();

            return Repository.GetCodeMajorations(currentUser.Personnel.SocieteId, lastModification);
        }


        /// <summary>
        ///  Vérifie s'il l'entité est déja utilisée
        /// </summary>
        /// <param name="id">Id de l'entité à vérifié</param>
        /// <returns>True = déjà Utilisée</returns>
        public bool IsAlreadyUsed(int id)
        {
            return this.Repository.IsAlreadyUsed(id);
        }

        /// <summary>
        ///   Moteur de recherche des codes majoration pour picklist
        /// </summary>
        /// <param name="text">Texte de recherche</param>
        /// <param name="page">Page courante</param>
        /// <param name="pageSize">Taille de la page</param>
        /// <param name="groupeId">identifiant du groupe</param>
        /// <param name="ciId">Identifiant CI</param>
        /// <param name="isHeureNuit">Filter pour ETAM/IAC ou ouvrier horarire codes</param>
        /// <param name="isOuvrier">Filter pour ouvrier </param>
        /// <param name="isETAM">Filter pour ETAM </param>
        /// <param name="isCadre">Filter pour cadre </param>
        /// <returns>Retourne une liste d'items de référentiel</returns>
        public IEnumerable<CodeMajorationEnt> SearchLight(string text, int page, int pageSize, int? groupeId, int? ciId, bool? isHeureNuit, bool? isOuvrier = null, bool? isETAM = null, bool? isCadre = null)
        {
            if (groupeId.HasValue && isHeureNuit.HasValue)
            {
                return SearchLightbyGroupeId(text, page, pageSize, groupeId, isHeureNuit, isOuvrier, isETAM, isCadre);
            }

            var ciCodeMajorationQuery = repository.Get();
            var query = from cm in this.Repository.Query().Get()
                        join ciCm in ciCodeMajorationQuery on cm.CodeMajorationId equals ciCm.CodeMajorationId into gj
                        from subCiCm in gj.DefaultIfEmpty()
                        where cm.IsActif
                        where !groupeId.HasValue || groupeId.HasValue && cm.GroupeId == groupeId
                        where !isOuvrier.HasValue || isOuvrier.HasValue && cm.IsOuvrier == isOuvrier
                        where !isETAM.HasValue || isETAM.HasValue && cm.IsETAM == isETAM
                        where !isCadre.HasValue || isCadre.HasValue && cm.IsCadre == isCadre
                        where cm.EtatPublic || ciId.HasValue && subCiCm.CiId == ciId
                        where string.IsNullOrEmpty(text) || cm.Code.ToLower().Contains(text.ToLower()) || cm.Libelle.ToLower().Contains(text.ToLower())
                        select cm;

            query = query.Distinct();

            return query.OrderBy(p => p.Code).Skip((page - 1) * pageSize).Take(pageSize);
        }

        /// <summary>
        ///   Retourne la liste de codes majoration associés à la société et qui sont heures de nuit
        /// </summary>
        /// <param name="groupeId">Identifiant du groupe associé aux code majoration à retourner</param>
        /// <returns>La liste des codes majoration associés à la société</returns>
        public IEnumerable<CodeMajorationEnt> GetCodeMajorationListByGroupeIdAndIsHeurNuit(int groupeId)
        {
            return this.Repository.GetCodeMajorationListByGroupeIdAndIsHeurNuit(groupeId);
        }

        /// <summary>
        /// Search code majoration by groupe identifier
        /// </summary>
        /// <param name="text">Texte de recherche</param>
        /// <param name="page">Page courante</param>
        /// <param name="pageSize">Taille de la page</param>
        /// <param name="groupeId">identifiant du groupe</param>
        /// <param name="isHeureNuit">Filter pour ETAM/IAC ou ouvrier horarire codes</param>
        /// <returns>Retourne une liste d'items de référentiel</returns>
        private IEnumerable<CodeMajorationEnt> SearchLightbyGroupeId(string text, int page, int pageSize, int? groupeId, bool? isHeureNuit, bool? isOuvrier = null, bool? isETAM = null, bool? isCadre = null)
        {

            if (isHeureNuit.HasValue && isHeureNuit.Value == true)
            {
                return this.Repository.Query().Filter(x => x.GroupeId == groupeId && x.IsHeureNuit == isHeureNuit.Value)
                                              .Filter(x => x.IsActif)
                                              .Filter(x => !isOuvrier.HasValue || (isOuvrier.HasValue && x.IsOuvrier == isOuvrier.Value))
                                              .Filter(x => !isETAM.HasValue || (isETAM.HasValue && x.IsETAM == isETAM.Value))
                                              .Filter(x => !isCadre.HasValue || (isCadre.HasValue && x.IsCadre == isCadre.Value))
                                              .Filter(x => string.IsNullOrEmpty(text) || x.Code.ToLower().Contains(text.ToLower()) || x.Libelle.ToLower().Contains(text.ToLower()))
                                              .OrderBy(y => y.OrderBy(x => x.Code))
                                              .GetPage(page, pageSize).ToList();

            }

            return this.Repository.Query().Filter(x => x.GroupeId == groupeId)
                                         .Filter(x => x.IsActif)
                                         .Filter(x => !isOuvrier.HasValue || (isOuvrier.HasValue && x.IsOuvrier == isOuvrier.Value))
                                         .Filter(x => !isETAM.HasValue || (isETAM.HasValue && x.IsETAM == isETAM.Value))
                                         .Filter(x => !isCadre.HasValue || (isCadre.HasValue && x.IsCadre == isCadre.Value))
                                         .Filter(x => string.IsNullOrEmpty(text) || x.Code.ToLower().Contains(text.ToLower()) || x.Libelle.ToLower().Contains(text.ToLower()))
                                         .OrderBy(y => y.OrderBy(x => x.Code))
                                         .GetPage(page, pageSize).ToList();
        }
    }
}
