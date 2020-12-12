using System;
using System.Collections.Generic;
using System.Linq;
using Fred.DataAccess.Interfaces;
using Fred.Entities;
using Fred.Entities.Personnel;
using Fred.Entities.Personnel.Interimaire;
using Fred.Entities.Societe;
using Fred.Framework.Exceptions;
using Fred.Web.Shared.Extentions;

namespace Fred.Business.Personnel.Interimaire
{
    /// <summary>
    ///   Gestionnaire des matricules externes
    /// </summary>
    public class MatriculeExterneManager : Manager<MatriculeExterneEnt, IMatriculeExterneRepository>, IMatriculeExterneManager
    {
        public MatriculeExterneManager(IUnitOfWork uow, IMatriculeExterneRepository matriculeExterneRepository)
         : base(uow, matriculeExterneRepository)
        { }

        /// <summary>
        /// Permet de récupérer un matricule externe en fonction de son identifiant unique
        /// </summary>
        /// <param name="matriculeExterneId">Identifiant unique du matricule externe</param>
        /// <returns>Le matricule externe</returns>
        public MatriculeExterneEnt GetMatriculeExterneById(int matriculeExterneId)
        {
            try
            {
                return Repository.GetMatriculeExterneById(matriculeExterneId);
            }
            catch (Exception e)
            {
                throw new FredBusinessException(e.Message, e);
            }
        }

        /// <summary>
        /// Permet de récupérer une liste de contrat intérimaire en fonction d'un personnel id
        /// </summary>
        /// <param name="personnelId">Identifiant unique du personnel</param>
        /// <returns>Liste des contrats intérimaire appartenant au personnel id</returns>
        public List<MatriculeExterneEnt> GetMatriculeExterneByPersonnelId(int personnelId)
        {
            try
            {
                return Repository.GetMatriculeExterneByPersonnelId(personnelId);
            }
            catch (Exception e)
            {
                throw new FredBusinessException(e.Message, e);
            }
        }

        /// <summary>
        /// Permet de récupérer une liste de matricule externe en fonction d'une liste de personnel id
        /// </summary>
        /// <param name="personnelIds">Liste d'identifiant unique de personnel</param>
        /// <returns>Liste des matricules externes appartenant aux personnels</returns>
        public List<MatriculeExterneEnt> GetMatriculeExterneByPersonnelIds(List<int> personnelIds)
        {
            try
            {
                return Repository.GetMatriculeExterneByPersonnelIds(personnelIds);
            }
            catch (Exception e)
            {
                throw new FredBusinessException(e.Message, e);
            }
        }


        /// <summary>
        /// Permet d'ajouter un matricule externe
        /// </summary>
        /// <param name="matriculeExterneEnt">Matricule Externe</param>
        /// <returns>Le matricule externe enregistré</returns>
        public MatriculeExterneEnt AddMatriculeExterne(MatriculeExterneEnt matriculeExterneEnt)
        {
            try
            {
                Repository.AddMatriculeExterne(matriculeExterneEnt);
                Save();

                return GetMatriculeExterneById(matriculeExterneEnt.MatriculeExterneId);
            }
            catch (Exception e)
            {
                throw new FredBusinessException(e.Message, e);
            }
        }


        /// <summary>
        /// Permet de metre à jour un matricule externe
        /// </summary>
        /// <param name="matriculeExterneEnt">Matricule Externe</param>
        /// <returns>Le matricule externe enregistré</returns>
        public MatriculeExterneEnt UpdateMatriculeExterne(MatriculeExterneEnt matriculeExterneEnt)
        {
            try
            {
                Repository.UpdateMatriculeExterne(matriculeExterneEnt);
                Save();

                return Repository.GetMatriculeExterneById(matriculeExterneEnt.MatriculeExterneId);
            }
            catch (Exception e)
            {
                throw new FredBusinessException(e.Message, e);
            }
        }

        /// <summary>
        /// Permet de vérifier si un matricule externe existe
        /// </summary>
        /// <param name="matriculeExterneEnt">Matricule</param>
        /// <returns>Retourne true ou false si un matricule existe ou non</returns>
        public MatriculeExterneEnt GetMatriculeExterneExist(MatriculeExterneEnt matriculeExterneEnt)
        {
            try
            {
                var response = Repository.GetMatriculeExterneExist(matriculeExterneEnt);
                return response;
            }
            catch (Exception e)
            {
                throw new FredBusinessException(e.Message, e);
            }
        }

        /// <summary>
        /// Add or update Matricule Sap for societe FTP
        /// </summary>
        /// <param name="personnelList">Personnel id list</param>
        /// <param name="societe">societe</param>
        public void AddOrUpdateMatriculeSapListFTP(List<PersonnelEnt> personnelList, SocieteEnt societe)
        {
            if (societe.CodeSocietePaye == Constantes.CodeSocietePayeFTP && personnelList.Count > 0)
            {
                List<int> personnelIdsList = personnelList.Select(x => x.PersonnelId).ToList();
                List<MatriculeExterneEnt> matriculeExterneList = GetMatriculeExterneByPersonnelIds(personnelIdsList);
                if (matriculeExterneList.IsNullOrEmpty())
                {
                    AddMatriculeExterneSapListFtp(personnelList);
                }
                else
                {
                    // Gestion des matricules externes à ajouter
                    List<int> matriculeExternePersonnelIdsList = matriculeExterneList.Select(x => x.PersonnelId).ToList();
                    IEnumerable<int> personnelIdToAddMatriculeSap = personnelIdsList.Except(matriculeExternePersonnelIdsList);
                    AddMatriculeExterneSapListFtp(personnelList.Where(x => personnelIdToAddMatriculeSap.Contains(x.PersonnelId)));

                    // Gestion des matricules externes à modifier
                    IEnumerable<MatriculeExterneEnt> matriculeExterneToUpdateMatriculeSap = matriculeExterneList.Where(x => personnelIdsList.Contains(x.PersonnelId))?.Distinct();
                    UpdateMatriculeExterneSapListFtp(matriculeExterneToUpdateMatriculeSap);
                }
            }
        }

        /// <summary>
        /// Ajouter une list des matricules externe pour la societe FTP
        /// </summary>
        /// <param name="personnelIdToAddMatriculeSap">List des personnels a ajoute</param>
        private void AddMatriculeExterneSapListFtp(IEnumerable<PersonnelEnt> personnelIdToAddMatriculeSap)
        {
            if (personnelIdToAddMatriculeSap.Any())
            {
                List<MatriculeExterneEnt> matriculeExterneSapToAdd = new List<MatriculeExterneEnt>();
                foreach (PersonnelEnt personnel in personnelIdToAddMatriculeSap)
                {
                    string personnelMatricule = personnel.Matricule.Substring(Math.Max(0, personnel.Matricule.Length - 5));
                    matriculeExterneSapToAdd.Add(new MatriculeExterneEnt
                    {
                        PersonnelId = personnel.PersonnelId,
                        Matricule = string.Concat("001", personnelMatricule),
                        Source = Constantes.MatriculeExterneSapResource
                    });
                }

                Repository.AddListMatriculeExterneSapForFtp(matriculeExterneSapToAdd);
            }
        }

        /// <summary>
        /// Modifier une list des matricules externe pour la societe FTP
        /// </summary>
        /// <param name="matriculeExterneToUpdateMatriculeSap">List des matricules externes a modifier</param>
        private void UpdateMatriculeExterneSapListFtp(IEnumerable<MatriculeExterneEnt> matriculeExterneToUpdateMatriculeSap)
        {
            if (!matriculeExterneToUpdateMatriculeSap.IsNullOrEmpty())
            {
                List<MatriculeExterneEnt> matriculeExterneSapToUpdate = new List<MatriculeExterneEnt>();
                foreach (MatriculeExterneEnt matriculeExterneEnt in matriculeExterneToUpdateMatriculeSap)
                {
                    string personnelMatricule = matriculeExterneEnt.Personnel.Matricule.Substring(Math.Max(0, matriculeExterneEnt.Personnel.Matricule.Length - 5));
                    if (!matriculeExterneEnt.Matricule.Equals(string.Concat("001", personnelMatricule)))
                    {
                        matriculeExterneEnt.Matricule = string.Concat("001", personnelMatricule);
                        matriculeExterneSapToUpdate.Add(matriculeExterneEnt);
                    }
                }

                if (matriculeExterneSapToUpdate.Any())
                {
                    Repository.UpdateListMatriculeExterneSapForFtp(matriculeExterneSapToUpdate);
                }
            }
        }
    }
}
