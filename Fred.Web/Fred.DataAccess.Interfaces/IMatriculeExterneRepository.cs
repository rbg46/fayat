using Fred.Entities.Personnel.Interimaire;
using System.Collections.Generic;

namespace Fred.DataAccess.Interfaces
{
    /// <summary>
    ///   Représente un référentiel de données des matricule externe
    /// </summary>
    public interface IMatriculeExterneRepository : IRepository<MatriculeExterneEnt>
    {
        /// <summary>
        /// Permet de récupérer un matricule externe en fonction de son identifiant unique
        /// </summary>
        /// <param name="matriculeExterneId">Identifiant unique du matricule externe</param>
        /// <returns>Le matricule externe</returns>
        MatriculeExterneEnt GetMatriculeExterneById(int matriculeExterneId);

        /// <summary>
        /// Permet de récupérer une liste de matricule externe en fonction d'un personnel id
        /// </summary>
        /// <param name="personnelId">Identifiant unique du personnel</param>
        /// <returns>Liste des matricule externe appartenant au personnel id</returns>
        List<MatriculeExterneEnt> GetMatriculeExterneByPersonnelId(int personnelId);

        /// <summary>
        /// Permet de récupérer une liste de matricule externe en fonction d'une liste de personnel id
        /// </summary>
        /// <param name="personnelIds">Liste d'identifiant unique de personnel</param>
        /// <returns>Liste des matricules externes appartenant aux personnels</returns>
        List<MatriculeExterneEnt> GetMatriculeExterneByPersonnelIds(List<int> personnelIds);

        /// <summary>
        /// Permet d'ajouter un matricule externe
        /// </summary>
        /// <param name="matriculeExterneEnt">Matricule Externe</param>
        /// <returns>Le matricule externe enregistré</returns>
        MatriculeExterneEnt AddMatriculeExterne(MatriculeExterneEnt matriculeExterneEnt);

        /// <summary>
        /// Permet de mettre à jour un matricule externe
        /// </summary>
        /// <param name="matriculeExterneEnt">Matricule Externe</param>
        /// <returns>Le matricule externe enregistré</returns>
        MatriculeExterneEnt UpdateMatriculeExterne(MatriculeExterneEnt matriculeExterneEnt);

        /// <summary>
        /// Permet de vérifier si un matricule externe existe ou non 
        /// </summary>
        /// <param name="matriculeExterneEnt">matricule externe</param>
        /// <returns>Le matricule externe</returns>
        MatriculeExterneEnt GetMatriculeExterneExist(MatriculeExterneEnt matriculeExterneEnt);

        /// <summary>
        /// Ajouter une list des matricules externe
        /// </summary>
        /// <param name="matriculeExterneEntList">Matricule externe list</param>
        void AddListMatriculeExterneSapForFtp(List<MatriculeExterneEnt> matriculeExterneEntList);

        /// <summary>
        /// Modifier une list des matricules externe
        /// </summary>
        /// <param name="matriculeExterneEntList">Matricule externe list</param>
        void UpdateListMatriculeExterneSapForFtp(IEnumerable<MatriculeExterneEnt> matriculeExterneEntList);
    }
}
