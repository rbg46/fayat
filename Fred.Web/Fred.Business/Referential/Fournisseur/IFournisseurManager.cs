using Fred.Business.Referential.Fournisseur.Common;
using Fred.Entities.Personnel;
using Fred.Entities.Referential;
using System.Collections.Generic;

namespace Fred.Business.Referential
{
    /// <summary>
    ///   Gestionnaire des fournisseurs.
    /// </summary>
    public interface IFournisseurManager : IManager<FournisseurEnt>
    {
        /// <summary>
        ///   Retourne la liste des fournisseurs.
        /// </summary>
        /// <param name="groupeId">Identifiant du groupe</param>
        /// <returns>Renvoie la liste des fournisseurs.</returns>
        IEnumerable<FournisseurEnt> GetFournisseurList(int groupeId);

        /// <summary>
        ///   Retourne la liste des fournisseurs.
        /// </summary>
        /// <returns>Renvoie la liste des fournisseurs.</returns>
        IEnumerable<FournisseurEnt> GetFournisseurList();

        /// <summary>
        ///   Retourne le fournisseur portant l'identifiant unique indiqué.
        /// </summary>
        /// <param name="fournisseurId">Identifiant du fournisseur à retrouver.</param>
        /// <param name="groupeId">Identifiant du groupe</param>
        /// <returns>Le fournisseur retrouvé, sinon nulle.</returns>
        FournisseurEnt GetFournisseur(int fournisseurId, int? groupeId);

        /// <summary>
        /// GetAgenceIdByCodeAndGroupe
        /// </summary>
        /// <param name="agenceCode">ByCode</param>
        /// <param name="groupeId">AndGroupe</param>
        /// <returns>GetAgenceId</returns>
        int? GetAgenceIdByCodeAndGroupe(string agenceCode, int groupeId);

        /// <summary>
        ///   Retourne le fournisseur portant le code indiqué.
        /// </summary>
        /// <param name="fournisseurCode">Code du fournisseur dont l'identifiant est à retrouver.</param>
        /// <returns>Fournisseur retrouvé, null sinon</returns>
        int? GetFournisseurIdByCode(string fournisseurCode);

        /// <summary>
        ///   Ajout un nouveau fournisseur.
        /// </summary>
        /// <param name="fournisseurEnt">Fournisseur à ajouter.</param>
        /// <returns>Fournisseur ajouté.</returns>
        FournisseurEnt AddFournisseur(FournisseurEnt fournisseurEnt);

        /// <summary>
        ///   Sauvegarde les modifications d'un fournisseur.
        /// </summary>
        /// <param name="fournisseurEnt">Fournisseur à modifier</param>
        /// <returns>Fournisseur mis à jour.</returns>
        FournisseurEnt UpdateFournisseur(FournisseurEnt fournisseurEnt);

        /// <summary>
        ///   Supprime un fournisseur.
        /// </summary>
        /// <param name="id">L'identifiant du fournisseur à supprimer.</param>
        void DeleteFournisseurById(int id);

        /// <summary>
        ///   Cherche une liste de fournisseurs.
        /// </summary>
        /// <param name="text">Le texte a chercher dans les propriétés des fournisseurs.</param>
        /// <param name="groupeId">Identifiant du groupe</param>
        /// <returns>Une liste de fournisseur.</returns>
        IEnumerable<FournisseurEnt> SearchFournisseurs(string text, int groupeId);

        /// <summary>
        ///   Vérifie la validité d'un fournisseur importé depuis ANAËL Finances
        /// </summary>
        /// <param name="fournisseurEnt">Entité dont il faut vérifier la validité</param>    
        /// <returns>Vrai si le fournisseur est valide, faux sinon</returns>
        bool CheckValidityBeforeImportation(FournisseurEnt fournisseurEnt);

        /// <summary>
        ///   Vérifie la validité et enregistre les fournisseurs importés depuis ANAËL Finances
        /// </summary>
        /// <param name="fournisseursAnael">Liste des entités dont il faut vérifier la validité</param>
        /// <param name="societeCode">Le code de la société.</param>
        /// <param name="groupeId">Identifiant du groupe</param>
        /// <returns>la liste des fournisseurs ajoute</returns>
        IEnumerable<FournisseurEnt> ManageImportedFournisseurs(IEnumerable<FournisseurEnt> fournisseursAnael, string societeCode, int? groupeId);

        /// <summary>
        ///   Moteur de recherche des fournisseurs pour picklist
        /// </summary>
        /// <param name="recherche">Texte de recherche sur le libellé du fournisseur</param>
        /// <param name="page">page index</param>
        /// <param name="pageSize">page size</param>
        /// <param name="groupeId">L'identifiant du groupe</param>
        /// <param name="recherche2">Autres information de Recherche (Adresse, Code, SIREN)</param>
        /// <param name="ciId">Id Ci selectionner</param>
        /// <param name="withCommandValide">Fournisseur avec commande Valider</param>
        /// <returns>Retourne une liste d'items de référentiel</returns>
        IEnumerable<FournisseurEnt> SearchLight(string recherche, int page, int pageSize, int? groupeId, string recherche2, int? ciId, bool? withCommandValide = false);

        /// <summary>
        /// GetActifRentersForMateriel
        /// </summary>
        /// <param name="text">Texte de recherche</param>
        /// <param name="page">page index</param>
        /// <param name="pageSize">page size</param>
        /// <param name="groupeId">L'identifiant du groupe</param>
        /// <returns>Retourne une liste d'items de référentiel</returns>
        /// <returns>l'ensemble des fournisseurs actifs (= la date du jour doit être comprise entre le date d'ouverture et la date de clôture du Fournisseur), de type Locatier (TypeTiers=L) et associé au groupe de la société de rattachement du Matériel.</returns>
        IEnumerable<FournisseurEnt> GetActiveRentersForMateriel(string text, int page, int pageSize, int groupeId);

        /// <summary>
        ///   Récupère la liste du personnel intérimaire lié au fournisseur
        /// </summary>
        /// <param name="fournisseurId">Identifiant du fournisseur</param>
        /// <returns>Liste du personnel intérimaire</returns>
        IEnumerable<PersonnelEnt> GetPersonnelInterimaireList(int fournisseurId);

        /// <summary>
        ///   Récupère le nombre de personnel intérimaire lié au fournisseur
        /// </summary>
        /// <param name="fournisseurId">Identifiant du fournisseur</param>
        /// <returns>Nombre de personnels intérimaire</returns>
        int GetCountPersonnelList(int fournisseurId);

        /// <summary>
        ///   Permet de récupérer la liste des fournisseurs en fonction des critères de recherche.
        /// </summary>
        /// <param name="filters">Filtres de recherche</param>
        /// <param name="page">Page actuelle</param>
        /// <param name="pageSize">Taille d'une page</param>
        /// <returns>Retourne la liste filtré des fournisseurs</returns>
        IEnumerable<FournisseurEnt> SearchFournisseurWithFilters(SearchFournisseurEnt filters, int page, int pageSize);

        /// <summary>
        /// Récupère un nouveau filtre (SearchFournisseurEnt)
        /// </summary>
        /// <returns>Filtre fournisseur</returns>
        SearchFournisseurEnt GetFilter();

        /// <summary>
        ///   obtient une liste de fournisseur de type ETT
        /// </summary>
        /// <param name="groupeId">Identifiant du groupe</param>
        /// <returns>Une liste de fournisseur.</returns>
        List<FournisseurEnt> GetFournisseurETT(int groupeId);

        /// <summary>
        /// Return A List Of FournisseurIdCodeModel By List code 
        /// </summary>
        /// <param name="listOfCode">List Of Code</param>
        /// <returns>List Of FournisseurIdCodeModel</returns>
        List<FournisseurIdCodeModel> GetAllIdFournisseurForListOfCode(List<string> listOfCode);

        /// <summary>
        /// Retourne le fournisseur par fournisseur SIREN et code groupe.
        /// </summary>
        /// <param name="fournisseurSIREN">SIREN Fournisseur.</param>
        /// <param name="groupeCode">Code groupe.</param>
        /// <returns>Le fournisseur retrouvé, sinon nulle.</returns>
        List<FournisseurEnt> GetBySirenAndGroupeCode(string fournisseurSIREN, string groupeCode);

        /// <summary>
        /// Retourne le fournisseur par reference systeme interimaire SIREN et code groupe.
        /// </summary>
        /// <param name="fournisseurSIREN">SIREN Fournisseur.</param>
        /// <param name="groupeCode">Code groupe.</param>
        /// <returns>Le fournisseur retrouvé, sinon nulle.</returns>
        List<FournisseurEnt> GetByReferenceSystemInterimaireAndGroupeCode(string fournisseurSIREN, string groupeCode);
    }
}
