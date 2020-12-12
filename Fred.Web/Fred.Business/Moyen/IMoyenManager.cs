using System;
using System.Collections.Generic;
using System.IO;
using Fred.Business.Moyen.Common;
using Fred.Entities.Moyen;
using Fred.Entities.Rapport;
using Fred.Entities.Referential;
using Fred.Entities.ReferentielFixe;
using Fred.Entities.Societe;
using Fred.Web.Shared.Models.Moyen;

namespace Fred.Business.Moyen
{
    /// <summary>
    /// Interface des gestionnaires des moyens
    /// </summary>
    public interface IMoyenManager : IManager<MaterielEnt>
    {
        /// <summary>
        /// Retourne un moyen par son code.
        /// </summary>
        /// <param name="code">Le code du  moyen.</param>
        /// <returns>Un moyen.</returns>
        MaterielEnt GetMoyen(string code);

        /// <summary>
        /// Permet d'ajouter ou de mettre à jour un moyen.
        /// Si le moyen n'a pas affection, on en ajoute une par défaut.
        /// </summary>
        /// <param name="materiel">Un moyen.</param>
        /// <returns>Le moyen ajouté ou mis à jour.</returns>
        MaterielEnt AddOrUpdateMoyen(MaterielEnt materiel);

        /// <summary>
        /// Chercher des moyens en fonction des critéres fournies en paramètres
        /// </summary>
        /// <param name="filters">Les filtres de recherche</param>
        /// <param name="page">Numéro de page</param>
        /// <param name="pageSize">Taille de la page</param>
        /// <returns>List des moyens</returns>
        IEnumerable<MaterielEnt> SearchLightForMoyen(SearchMoyenEnt filters, int page = 1, int pageSize = 20);

        /// <summary>
        /// Chercher des immatriculations des moyens en fonction des critéres fournies en paramètres 
        /// </summary>
        /// <param name="filters">Les filtres de recherche</param>
        /// <param name="page">Numéro de page</param>
        /// <param name="pageSize">Taille de la page</param>
        /// <returns>List des immatriculations</returns>
        IEnumerable<MoyenImmatriculationModel> SearchLightForImmatriculation(SearchImmatriculationMoyenEnt filters, int page = 1, int pageSize = 30);

        /// <summary>
        /// Chercher les types d'affectations des moyens en fonction des critéres fournies en paramètres 
        /// </summary>
        /// <param name="page">Numéro de la page</param>
        /// <param name="pageSize">Taille de la page</param>
        /// <param name="text">Mot clé de la recherche</param>
        /// <returns>Liste des types des affectations</returns>
        IEnumerable<AffectationMoyenTypeEnt> SearchLightForAffectationMoyenType(int page = 1, int pageSize = 20, string text = null);

        /// <summary>
        /// Chercher les types des moyens 
        /// </summary>
        /// <param name="page">Numéro de la page</param>
        /// <param name="pageSize">Taille de la page</param>
        /// <param name="text">Mot clé de la recherche</param>
        /// <returns>Liste des types des moyens</returns>
        IEnumerable<ChapitreEnt> SearchLightForTypeMoyen(int page = 1, int pageSize = 20, string text = null);

        /// <summary>
        /// Chercher les sous types des moyens 
        /// </summary>
        /// <param name="page">Numéro de la page</param>
        /// <param name="pageSize">Taille de la page</param>
        /// <param name="text">Mot clé de la recherche</param>
        /// <param name="typeMoyen">Type d'un moyen</param>
        /// <returns>Liste des sous types des moyens</returns>
        IEnumerable<SousChapitreEnt> SearchLightForSousTypeMoyen(int page = 1, int pageSize = 20, string text = null, string typeMoyen = null);

        /// <summary>
        /// Chercher les modèles des moyens 
        /// </summary>
        /// <param name="page">Numéro de la page</param>
        /// <param name="pageSize">Taille de la page</param>
        /// <param name="text">Mot clé de la recherche</param>
        /// <param name="typeMoyen">Type d'un moyen</param>
        /// <param name="sousTypeMoyen">Sous type d'un moyen</param>
        /// <returns>Liste des modèles des moyens</returns>
        IEnumerable<RessourceEnt> SearchLightForModelMoyen(int page = 1, int pageSize = 20, string text = null, string typeMoyen = null, string sousTypeMoyen = null);

        /// <summary>
        /// Search light for fiche generique
        /// </summary>
        /// <param name="page">Page</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="text">text to use in search</param>
        /// <returns>IEnumerable fo FicheGeneriqueModel</returns>
        IEnumerable<FicheGeneriqueModel> SearchLightForFicheGenerique(int page = 1, int pageSize = 30, string text = null);

        /// <summary>
        /// Create moyen en location
        /// </summary>
        /// <param name="materiel">Moyen</param>
        void CreateMoyenEnLocation(MaterielEnt materiel);

        /// <summary>
        /// Update moyen en location
        /// </summary>
        /// <param name="materielLocation">MaterieLocation</param>
        /// <returns>Return the Id of Materiel Updated</returns>
        int UpdateMaterielLocation(MaterielLocationEnt materielLocation);

        /// <summary>
        /// Supprimer un materiel de type location en fonction d'un id 
        /// </summary>
        /// <param name="materielLocationId">L'id du materiel en location a supprimer</param>
        void DeleteMaterielLocation(int materielLocationId);

        /// <summary>
        ///  Chercher des sociétés de moyen en fonction des critéres fournis en paramètres
        /// </summary>
        /// <param name="filters">Les filtres de recherche</param>
        /// <param name="page">Numéro de page</param>
        /// <param name="pageSize">Taille de la page</param>
        /// <returns>List des moyens</returns>
        IEnumerable<SocieteEnt> SearchLightForSociete(SearchSocieteMoyenEnt filters, int page = 1, int pageSize = 30);

        /// <summary>
        ///  Chercher des établissements comptables de moyen en fonction des critéres fournis en paramètres
        /// </summary>
        /// <param name="filters">Les filtres de recherche</param>
        /// <param name="page">Numéro de page</param>
        /// <param name="pageSize">Taille de la page</param>
        /// <returns>List des moyens</returns>
        IEnumerable<EtablissementComptableEnt> SearchLightForEtablissementComptable(SearchEtablissementMoyenEnt filters, int page = 1, int pageSize = 30);

        /// <summary>
        /// Generate Excel Moyen
        /// </summary>
        /// <param name="listAffectationMoyen">list Affectation Moyen</param>
        /// <param name="userName">user Name</param>
        /// <param name="periode">periode</param>
        /// <returns>MemoryStream</returns>
        MemoryStream GenerateExcelMoyen(List<RapportLigneEnt> listAffectationMoyen, string userName, string periode, string templateFolderPath);

        /// <summary>
        /// Mise à jour des pointages matériel 
        /// </summary>
        /// <param name="startDate">Date de début de la mise à jour</param>
        /// <param name="endDate">Date de fin de la mise à jour</param>
        /// <returns>Pointage moyen response</returns>
        PointageMoyenResponse UpdatePointageMoyen(DateTime startDate, DateTime endDate);

        /// <summary>
        /// Récupération du moyen en fonction de son code et l'id de la société
        /// </summary>
        /// <param name="code">Code du moyen</param>
        /// <param name="societeId">Id de la société</param>
        /// <returns>Un objet materiel Ent qui corresponds au code et l'id envoyés</returns>
        MaterielEnt GetMoyen(string code, int societeId);

        /// <summary>
        /// Récupération du moyen en fonction de son code et l'id de la société
        /// </summary>
        /// <param name="code">Code du moyen</param>
        /// <param name="societeId">Id de la société</param>
        /// <param name="etablissementComptableId">Id de l'établiessement comptable</param>
        /// <returns>Un objet materiel Ent qui corresponds au code et l'id envoyés</returns>
        MaterielEnt GetMoyen(string code, int societeId, int? etablissementComptableId);
    }
}
