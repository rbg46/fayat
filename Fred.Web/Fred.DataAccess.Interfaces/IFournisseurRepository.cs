using System.Collections.Generic;
using Fred.Entities.Referential;

namespace Fred.DataAccess.Interfaces
{
    /// <summary>
    ///   Représente un référentiel de données pour les fournisseurs.
    /// </summary>
    public interface IFournisseurRepository : IFredRepository<FournisseurEnt>
    {
        /// <summary>
        ///   Retourne la liste des fournisseurs.
        /// </summary>
        /// <param name="groupeId">Identifiant du groupe</param>
        /// <returns>La liste des fournisseurs.</returns>
        IEnumerable<FournisseurEnt> GetFournisseurList(int groupeId);

        /// <summary>
        ///   Retourne la liste des fournisseurs.
        /// </summary>
        /// <returns>La liste des fournisseurs.</returns>
        IEnumerable<FournisseurEnt> GetFournisseurList();

        /// <summary>
        /// GetAgenceIdByCodeAndGroupe
        /// </summary>
        /// <param name="agenceCode">ByCode</param>
        /// <param name="groupeId">AndGroupe</param>
        /// <returns>GetAgenceId</returns>
        int? GetAgenceIdByCodeAndGroupe(string agenceCode, int groupeId);

        /// <summary>
        ///   Retourne le fournisseur portant l'identifiant unique indiqué et de son groupe ID
        /// </summary>
        /// <param name="fournisseurId">Identifiant du fournisseur à retrouver.</param>
        /// <param name="groupeId">Identifiant du groupe</param>
        /// <returns>Le fournisseur retrouvé, sinon nulle.</returns>
        FournisseurEnt GetFournisseur(int fournisseurId, int groupeId);

        /// <summary>
        ///   Retourne le fournisseur portant le code indiqué.
        /// </summary>
        /// <param name="fournisseurCode">Code du fournisseur dont l'identifiant est à retrouver.</param>
        /// <returns>Fournisseur retrouvé, null sinon</returns>
        int? GetFournisseurIdByCode(string fournisseurCode);

        /// <summary>
        ///   Ajout un nouveau fournisseur
        /// </summary>
        /// <param name="fournisseurEnt"> fournisseur à ajouter</param>
        /// <returns> Fournisseur ajouté</returns>
        FournisseurEnt AddFournisseur(FournisseurEnt fournisseurEnt);

        /// <summary>
        ///   Sauvegarde les modifications d'un fournisseur
        /// </summary>
        /// <param name="fournisseurEnt">fournisseur à modifier</param>
        /// <returns>Fournisseur mis à jour.</returns>
        FournisseurEnt UpdateFournisseur(FournisseurEnt fournisseurEnt);

        /// <summary>
        ///   Supprime un fournisseur
        /// </summary>
        /// <param name="id">L'identifiant du fournisseur à supprimer</param>
        void DeleteFournisseurById(int id);

        /// <summary>
        ///   Cherche une liste de fournisseur.
        /// </summary>
        /// <param name="text">Le texte a chercher dans les propriétés des fournisseurs.</param>
        /// <param name="groupeId">Identifiant du groupe</param>
        /// <returns>Une liste de fournisseur.</returns>
        IEnumerable<FournisseurEnt> SearchFournisseurs(string text, int groupeId);

        /// <summary>
        ///   Récupère le fournisseur portant le triplet code, typeSequence, groupe.
        /// </summary>
        /// <param name="code">Code du fournisseur</param>
        /// <param name="typeSequence">Type séquence du fournisseur</param>
        /// <param name="groupeId">Identifiant du groupe"</param>
        /// <returns>Le fournisseur trouvé, null sinon</returns>
        FournisseurEnt GetFournisseurLight(string code, string typeSequence, int groupeId);

        /// <summary>
        ///   Récupère la liste des fournisseurs d'un groupe (version light sans include)
        /// </summary>
        /// <param name="groupeId">Identifiant du groupe</param>
        /// <returns>Liste de fournisseurs</returns>
        IEnumerable<FournisseurEnt> GetFournisseurLight(int groupeId);

        /// <summary>
        ///   Récupère un fournisseur en fonction du groupe id et du code
        /// </summary>
        /// <param name="groupeId">Identifiant du groupe</param>
        /// <param name="code">le code fournisseur</param>
        /// <returns>l'entite fournisseur</returns>
        FournisseurEnt GetFournisseurByCodeAndGroupeId(int groupeId, string code);

        /// <summary>
        ///   Ajoute ou met à jour des Fournisseurs selon la liste en paramètre
        /// </summary>
        /// <param name="fournisseurs">Liste des Fournisseurs</param>    
        void AddOrUpdateFournisseurList(IEnumerable<FournisseurEnt> fournisseurs);

        /// <summary>
        ///   obtient une liste de fournisseur de type ETT
        /// </summary>
        /// <param name="groupeId">Identifiant du groupe</param>
        /// <returns>Une liste de fournisseur.</returns>
        List<FournisseurEnt> GetFournisseurETT(int groupeId);

        /// <summary>
        ///   Retourne le fournisseur dont portant l'identifiant unique indiqué.
        /// </summary>
        /// <param name="fournisseurId">Identifiant du fournisseur à retrouver.</param>    
        /// <returns>Le fournisseur retrouvé, sinon nulle.</returns>
        FournisseurEnt GetFournisseur(int fournisseurId);

        /// <summary>
        /// Mise à jour d'un fournisseur
        /// </summary>
        /// <param name="fournisseurId">id fournisseur</param>
        /// <param name="fournisseur">Fournisseur à Mettre à jour</param>
        void UpdateFournisseur(int fournisseurId, FournisseurEnt fournisseur);

        /// <summary>
        /// Return A List Of Fournisseur By List code 
        /// </summary>
        /// <param name="listOfCode">List Of Code</param>
        /// <returns>List Of Fournisseur</returns>
        IEnumerable<FournisseurEnt> GetAllIdFournisseurForListOfCode(List<string> listOfCode);

        /// <summary>
        /// Ajout un nouveau fournisseur sans action save
        /// </summary>
        /// <param name="fournisseur">fournisseur à ajouter</param>
        void AddFournisseurWithoutSaving(FournisseurEnt fournisseur);

        /// <summary>
        /// Retourne le fournisseur par reference systeme interimaire SIREN et code groupe.
        /// </summary>
        /// <param name="fournisseurSIREN">SIREN Fournisseur.</param>
        /// <param name="groupeCode">Code groupe.</param>
        /// <returns>Le fournisseur retrouvé, sinon nulle.</returns>
        List<FournisseurEnt> GetByReferenceSystemInterimaireAndGroupeCode(string fournisseurSIREN, string groupeCode);

        /// <summary>
        /// Retourne le fournisseur par fournisseur SIREN et code groupe.
        /// </summary>
        /// <param name="fournisseurSIREN">SIREN Fournisseur.</param>
        /// <param name="groupeCode">Code groupe.</param>
        /// <returns>Le fournisseur retrouvé, sinon nulle.</returns>
        List<FournisseurEnt> GetBySirenAndGroupeCode(string fournisseurSIREN, string groupeCode);
    }
}
