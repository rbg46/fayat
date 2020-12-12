using System.Collections.Generic;
using Fred.Entities.CI;
using Fred.Entities.Commande;
using Fred.Entities.Groupe;
using Fred.Entities.IndemniteDeplacement;
using Fred.Entities.Personnel;
using Fred.Entities.Referential;
using Fred.Entities.Societe;

namespace Fred.DataAccess.Interfaces

{
    /// <summary>
    /// Repository des Reprises des données
    /// </summary>
    public interface IRepriseDonneesRepository : IMultipleRepository
    {
        /// <summary>
        /// Retourne tous les groupes
        /// </summary>
        /// <returns>Liste de groupes</returns>
        List<GroupeEnt> GetAllGroupes();

        /// <summary>
        /// Retourne les ci dont le code est contenu dans la liste ciCodes
        /// </summary>
        /// <param name="ciCodes">liste de code ci</param>
        /// <returns>Les ci qui ont le code contenu dans ciCodes </returns>
        List<CIEnt> GetCisByCodes(List<string> ciCodes);

        /// <summary>
        /// Retourne les ci avec l'organisation et la societe dont le code est contenu dans la liste ciCodes
        /// </summary>
        /// <param name="ciCodes">liste de code ci</param>
        /// <returns>Les ci qui ont le code contenu dans ciCodes </returns>
        List<CIEnt> GetCisByCodesWithSocieteAndOrganisation(List<string> ciCodes);

        /// <summary>
        /// Retourne le StatusCommandeEnt Validee
        /// </summary>
        /// <returns>StatutCommandeEnt validé</returns>
        StatutCommandeEnt GetStatusCommandeValidee();

        /// <summary>
        /// Retourne les Taches dont le code est contenu dans la liste tachesCodes
        /// </summary>
        /// <param name="tachesCodes">Liste des codes des taches</param>
        /// <returns>Les Taches qui ont le code contenu dans tachesCodes</returns>
        List<TacheEnt> GetTachesByCodes(List<string> tachesCodes);

        /// <summary>
        /// Retourne les Pays dont le code est contenu dans la liste paysCodes
        /// </summary>
        /// <param name="paysCodes">Liste des codes des pays</param>
        /// <returns>Les Pays qui ont le code contenu dans paysCodes</returns>
        List<PaysEnt> GetPaysByCodes(List<string> paysCodes);

        /// <summary>
        /// Retourne les Personnels dont le code et la societé sont contenu dans les listes personnelMatricules et societeCodes
        /// </summary>
        /// <param name="personnelMatricules">Liste des matricules</param>
        /// <param name="societeCodes">Liste des codes sociétés</param>
        /// <returns>Les Personnels qui ont le code et la société contenu dans personnelMatricules et societeCodes</returns>
        List<PersonnelEnt> GetPersonnelsByCodesAndBySocietes(List<string> personnelMatricules, List<string> societeCodes);

        /// <summary>
        /// Retourne les Societés pour le groupeId donné et dont les codes sont contenu dans la liste societeCodes
        /// </summary>
        /// <param name="groupeId">ID du groupe</param>
        /// <param name="societeCodes">Liste des codes sociétés</param>
        /// <returns>Les Sociétés qui ont le code contenu dans societesCodes et qui appartiennent au groupe groupeId</returns>
        List<SocieteEnt> GetListSocieteByGroupAndCodes(int groupeId, List<string> societeCodes);

        /// <summary>
        /// Retourne les Matériels pour le groupeId donné et dont les codes sont contenus dans la liste materielCodes
        /// </summary>
        /// <param name="societeCodes">Liste des codes sociétés</param>
        /// <param name="materielCodes">Liste des codes matériels</param>
        /// <returns>Les Matériels qui ont le code contenu dans matrielCodes et qui appartiennent au groupe groupeId</returns>
        List<MaterielEnt> GetListMaterielBySocieteAndCodes(List<string> societeCodes, List<string> materielCodes);

        /// <summary>
        /// Retourne les Code Deplacemeent dont le code est contenu dans la liste codeDeplacements
        /// </summary>
        /// <param name="societeCodesDep">Liste des codes société des Codes Dep</param>
        /// <param name="codeDeplacements">Liste des codes Déplacements</param>
        /// <returns>Les Codes Deplacement qui ont le code contenu dans codeDeplacements et la societe dans societesCodesDep</returns>
        List<CodeDeplacementEnt> GetListCodeDeplacementBySocietesAndCodes(List<string> societeCodesDep, List<string> codeDeplacements);

        /// <summary>
        /// Retourne les Code Zone Deplacemeent dont le code est contenu dans la liste codeZoneDeplacements et la société dans societeCodesDep
        /// </summary>
        /// <param name="societeCodesDep">Liste des codes société des Codes Dep</param>
        /// <param name="codeZoneDeplacements">Liste des codes zone Déplacement</param>
        /// <returns>Les Codes Zone Deplacement qui ont le code contenu dans codeZoneDeplacements et la societe dans societeCodesDep</returns>
        List<CodeZoneDeplacementEnt> GetListCodeZoneDeplacementBySocietesAndCodes(List<string> societeCodesDep, List<string> codeZoneDeplacements);

        /// <summary>
        /// Retourne les Indemnité Déplacement ayant un Ci et un Personnel (matricule) dans les listes ciCodes et personnelMatricules
        /// </summary>
        /// <param name="ciCodes">Liste des CI</param>
        /// <param name="personnelMatricules">Liste des Matricules Personnel</param>
        /// <returns>Les Indemnités Déplacement qui ont le Ci contenu dans ciCodes et le Personnel dans personnelMatricules</returns>
        List<IndemniteDeplacementEnt> GetIndemniteDeplacementsByCIAndPersonnel(List<string> ciCodes, List<string> personnelMatricules);

        /// <summary>
        /// Retourne les Personnels ayant une adresse Mail dans la liste adressesEmail
        /// </summary>
        /// <param name="adressesEmail">Liste des Emails</param>
        /// <returns>Les Personnel qui ont l'Email contenu dans adressesEmail</returns>
        List<PersonnelEnt> GetPersonnelsByEmails(List<string> adressesEmail);
    }
}
