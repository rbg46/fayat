using Fred.Entities.Referential;
using System.Collections.Generic;

namespace Fred.Business.Referential.CodeAbsence
{
  /// <summary>
  ///   Interface des gestionnaires des codes d'absence
  /// </summary>
  public interface ICodeAbsenceManager : IManager<CodeAbsenceEnt>
  {
    /// <summary>
    ///   Retourne la liste de tous les codes d'absence.
    /// </summary>
    /// <returns>List de toutes les sociétés</returns>
    IEnumerable<CodeAbsenceEnt> GetCodeAbsListAll();

    /// <summary>
    ///   Retourne la liste de tous les codes d'absence pour la synchronisation mobille.
    /// </summary>
    /// <returns>List de toutes les sociétés</returns>
    IEnumerable<CodeAbsenceEnt> GetCodeAbsListAllSync();

    /// <summary>
    ///   La liste de tous les codes d'absence.
    /// </summary>
    /// <returns>Renvoie la liste de des codes d'absence active</returns>
    IEnumerable<CodeAbsenceEnt> GetCodeAbsList();

    /// <summary>
    ///   Sauvegarde les modifications d'un CodeAbsence
    /// </summary>
    /// <param name="codeAbs">Code absence à modifier</param>
    void UpdateCodeAbsence(CodeAbsenceEnt codeAbs);

    /// <summary>
    ///   Ajout d'une code d'absence
    /// </summary>
    /// <param name="codeAbs">Code d'absence à ajouter</param>
    /// <returns>L'identifiant du code d'absence ajouté</returns>
    int AddCodeAbsence(CodeAbsenceEnt codeAbs);

    /// <summary>
    ///   Supprime un code d'absence
    /// </summary>
    /// <param name="codeAbsenceEnt">Code d'absence à supprimer</param>
    void DeleteCodeAbsenceById(CodeAbsenceEnt codeAbsenceEnt);

    /// <summary>
    ///   Code d'absence via l'id
    /// </summary>
    /// <param name="id">Id du code d'absence</param>
    /// <returns>Renvoie un code d'absence</returns>
    CodeAbsenceEnt GetCodeAbsenceById(int id);

    /// <summary>
    ///   Retourne le codeAbsence correspondant au code
    /// </summary>
    /// <param name="code">Le code de l'absence</param>
    /// <returns>Renvoie un code d'absence</returns>
    CodeAbsenceEnt GetCodeAbsenceByCode(string code);

    /// <summary>
    ///   Code d'absence via societeId
    /// </summary>
    /// <param name="societeId">idSociete de la société</param>
    /// <returns>Renvoie un code d'absence</returns>
    IEnumerable<CodeAbsenceEnt> GetCodeAbsenceBySocieteId(int societeId);

    /// <summary>
    ///   Import des codes absences automatiques depuis la Holding
    /// </summary>
    /// <param name="holdingId"> Id du Holding</param>
    /// <param name="idNewSociete"> Id de la nouvelle société</param>
    /// <returns>Renvoie un int</returns>
    int ImportCodeAbsFromHolding(int holdingId, int idNewSociete);

    /// <summary>
    ///   Permet de récupérer la liste des codes d'absence en fonction des critères de recherche.
    /// </summary>
    /// <param name="text">Texte recherché dans les sociétés</param>
    /// <param name="filters">Filtres de recherche</param>
    /// <returns>Retourne la liste filtré des sociétés</returns>
    IEnumerable<CodeAbsenceEnt> SearchCodeAbsenceWithFilters(string text, SearchCodeAbsenceEnt filters);

    /// <summary>
    ///   Permet de récupérer la liste de tous les codes absences en fonction des critères de recherche.
    /// </summary>
    /// <param name="text">Texte recherché dans tous les codes absences</param>
    /// <param name="filters">Filtres de recherche</param>
    /// <returns>Retourne la liste filtré de tous les codes absences</returns>
    IEnumerable<CodeAbsenceEnt> SearchCodeAbsenceAllWithFilters(string text, SearchCodeAbsenceEnt filters);

        /// <summary>
        ///   Méthode de recherche d'un item de référentiel via son LibelleRef ou son codeRef
        /// </summary>
        /// <param name="text">Le texte recherché</param>
        /// <param name="page">Page courante</param>
        /// <param name="pageSize">Taille de la page</param>
        /// <param name="ciId">Identifiant du Ci</param>
        /// <param name="societeId">Societe identifier</param>
        /// <returns>Une liste d' items de référentiel</returns>
        IEnumerable<CodeAbsenceEnt> SearchLight(string text, int page, int pageSize, int ciId = 0, int? societeId = 0 ,  bool? isOuvrier = null,bool? isEtam = null,bool? isCadre = null);

    /// <summary>
    ///   Permet l'initialisation d'une nouvelle instance de code absence.
    /// </summary>
    /// <param name="societeId">Id de la societe</param>
    /// <returns>Nouvelle instance de code absence intialisée</returns>
    CodeAbsenceEnt GetNewCodeAbsence(int societeId);

    /// <summary>
    ///   Permet de récupérer les champs de recherche lié à un code d'absence.
    /// </summary>
    /// <returns>Retourne la liste des champs de recherche par défaut d'uncode d'absence</returns>
    SearchCodeAbsenceEnt GetDefaultFilter();

    /// <summary>
    ///   Permet de récupérer la liste de tous les codes absences en fonction des critères de recherche par société.
    /// </summary>
    /// <param name="filters">Filtres de recherche sur tous les codes absences</param>
    /// <param name="societeId">Id de la societe</param>
    /// <param name="text">text de recherche</param>
    /// <returns>Retourne la liste filtrée de tous les codes absences</returns>
    IEnumerable<CodeAbsenceEnt> SearchCodeAbsenceAllBySocieteIdWithFilters(SearchCodeAbsenceEnt filters, int societeId, string text);

    /// <summary>
    ///   Permet de connaître l'existence d'une société depuis son code.
    /// </summary>
    /// <param name="idCourant">L'Id courant</param>
    /// <param name="codeCodeAbsence">Le code de codeCodeAbsence</param>
    /// <param name="societeId">Le code de la société</param>
    /// <returns>Retourne vrai si le code passé en paramètre existe déjà, faux sinon</returns>
    bool IsCodeAbsenceExistsByCode(int idCourant, string codeCodeAbsence, int societeId);


    /// <summary>
    ///  Vérifie s'il l'entité est déja utilisée
    /// </summary>
    /// <param name="id">Id de l'entité à vérifié</param>
    /// <returns>True = déjà Utilisée</returns>
    bool IsAlreadyUsed(int id);
  }
}
