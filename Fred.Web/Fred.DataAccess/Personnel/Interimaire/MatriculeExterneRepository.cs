using System.Collections.Generic;
using System.Linq;
using Fred.DataAccess.Common;
using Fred.DataAccess.Interfaces;
using Fred.Entities.Personnel.Interimaire;
using Fred.EntityFramework;
using Microsoft.EntityFrameworkCore;

namespace Fred.DataAccess.Personnel.Interimaire
{
    /// <summary>
    ///   Référentiel de données pour les matricules externes
    /// </summary>
    public class MatriculeExterneRepository : FredRepository<MatriculeExterneEnt>, IMatriculeExterneRepository
    {
        /// <summary>
        ///   Initialise une nouvelle instance de la classe <see cref="MatriculeExterneRepository" />.
        /// </summary>
        /// <param name="logMgr"> Log Manager</param>
        /// <param name="uow">Unit of work</param>
        public MatriculeExterneRepository(FredDbContext context)
          : base(context) { }


        /// <summary>
        /// Permet de récupérer un matricule externe en fonction de son identifiant unique
        /// </summary>
        /// <param name="matriculeExterneId">Identifiant unique du matricule externe</param>
        /// <returns>Le matricule externe</returns>
        public MatriculeExterneEnt GetMatriculeExterneById(int matriculeExterneId)
        {
            return Query()
              .Filter(m => m.MatriculeExterneId == matriculeExterneId)
              .Get()
              .AsNoTracking()
              .FirstOrDefault();
        }


        /// <summary>
        /// Permet de récupérer une liste de matricule externe en fonction d'un personnel id
        /// </summary>
        /// <param name="personnelId">Identifiant unique du personnel</param>
        /// <returns>Liste des matricules externes appartenant au personnel id</returns>
        public List<MatriculeExterneEnt> GetMatriculeExterneByPersonnelId(int personnelId)
        {

            return Query()
              .Filter(d => d.PersonnelId == personnelId)
              .Get()
              .AsNoTracking()
              .ToList();
        }

        /// <summary>
        /// Permet de récupérer une liste de matricule externe en fonction d'une liste de personnel id
        /// </summary>
        /// <param name="personnelIds">Liste d'identifiant unique de personnel</param>
        /// <returns>Liste des matricules externes appartenant aux personnels</returns>
        public List<MatriculeExterneEnt> GetMatriculeExterneByPersonnelIds(List<int> personnelIds)
        {
            return Query().Include(x => x.Personnel)
              .Filter(d => personnelIds.Contains(d.PersonnelId))
              .Get()
              .ToList();
        }

        /// <summary>
        /// Permet d'ajouter un matricule externe
        /// </summary>
        /// <param name="matriculeExterneEnt">Matricule Externe</param>
        /// <returns>Le matricule externe enregistré</returns>
        public MatriculeExterneEnt AddMatriculeExterne(MatriculeExterneEnt matriculeExterneEnt)
        {
            Insert(matriculeExterneEnt);

            return matriculeExterneEnt;
        }

        /// <summary>
        /// Permet de mettre à jour un matricule externe
        /// </summary>
        /// <param name="matriculeExterneEnt">Matricule Externe</param>
        /// <returns>Le matricule externe enregistré</returns>
        public MatriculeExterneEnt UpdateMatriculeExterne(MatriculeExterneEnt matriculeExterneEnt)
        {
            Update(matriculeExterneEnt);

            return matriculeExterneEnt;
        }

        /// <summary>
        /// Permet de vérifier si un matricule externe existe ou non 
        /// </summary>
        /// <param name="matriculeExterneEnt">matricule externe</param>
        /// <returns>Le matricule externe</returns>
        public MatriculeExterneEnt GetMatriculeExterneExist(MatriculeExterneEnt matriculeExterneEnt)
        {
            return Query()
              .Filter(m => m.Matricule == matriculeExterneEnt.Matricule)
              .Filter(m => m.Source == matriculeExterneEnt.Source)
              .Filter(m => m.MatriculeExterneId != matriculeExterneEnt.MatriculeExterneId)
              .Get()
              .AsNoTracking()
              .FirstOrDefault();
        }

        /// <summary>
        /// Ajouter une list des matricules externes pour la societe FTP
        /// </summary>
        /// <param name="matriculeExterneEntList">Matricule externe list</param>
        public void AddListMatriculeExterneSapForFtp(List<MatriculeExterneEnt> matriculeExterneEntList)
        {
            Context.MatriculeExterne.AddRange(matriculeExterneEntList);
            Context.SaveChanges();
        }

        /// <summary>
        /// Modifier une list des matricules externes pour la societe FTP
        /// </summary>
        /// <param name="matriculeExterneEntList">Matricule externe list</param>
        public void UpdateListMatriculeExterneSapForFtp(IEnumerable<MatriculeExterneEnt> matriculeExterneEntList)
        {
            Context.MatriculeExterne.UpdateRange(matriculeExterneEntList);
            Context.SaveChanges();
        }
    }
}
