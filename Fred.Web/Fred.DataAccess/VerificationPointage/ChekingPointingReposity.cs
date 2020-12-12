using System.Collections.Generic;
using System.Linq;
using Fred.DataAccess.Interfaces;
using Fred.Entities.VerificationPointage;
using Fred.EntityFramework;
using Microsoft.EntityFrameworkCore;

namespace Fred.DataAccess.VerificationPointage
{
    /// <summary>
    /// Interface de Cheking pointing 
    /// </summary>
    public class ChekingPointingReposity : IChekingPointingReposity
    {
        private readonly IUnitOfWork uow;
        private readonly FredDbContext context;

        /// <summary>
        ///   Initialise une nouvelle instance de la classe <see cref="ChekingPointingReposity" />.
        /// </summary>
        /// <param name="uow">Unit of Work</param>
        public ChekingPointingReposity(IUnitOfWork uow, FredDbContext context)
        {
            this.uow = uow;
            this.context = context;
        }

        /// <summary>
        /// Récupération de liste des rapports Pointage
        /// </summary>
        /// <param name="filtre">Filtre de recherche</param>    
        /// <returns>Liste de Pointage</returns>
        public IEnumerable<ChekingPointing> GetChekingPointing(FilterChekingPointing filtre)
        {
            var query = context.RapportLignes
                         .Include(rl => rl.Rapport)
                         .Include(rl => rl.Materiel)
                         .Include(rl => rl.Personnel)
                         .Include(rl => rl.Ci)
                         .Where(filtre.GetPredicateWhere())
                         .Select(rl => new ChekingPointing
                         {
                             DateChantier = rl.Rapport.DateChantier,
                             CodeSociete = rl.Ci.Societe.Code,
                             CiCode = rl.Ci.Code,
                             LibelleCi = rl.Ci.Libelle,
                             Matricule = rl.Personnel.Matricule,
                             Nom = rl.Personnel.Nom,
                             Prenom = rl.Personnel.Prenom,
                             CodeMateriel = rl.Materiel.Code,
                             LibelleMateriel = rl.Materiel.Libelle,
                             MaterielArret = rl.MaterielArret,
                             MaterielMarche = rl.MaterielMarche,
                             MaterielPanne = rl.MaterielPanne,
                             MaterielIntemperie = rl.MaterielIntemperie,
                             HeureTravail = (rl.HeureNormale + rl.HeureMajoration)
                         })
                         .ToList()
                         ;
            return query;
        }
    }
}
