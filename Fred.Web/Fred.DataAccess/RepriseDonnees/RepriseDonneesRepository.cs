using System.Collections.Generic;
using System.Linq;
using Fred.DataAccess.Interfaces;
using Fred.Entities.CI;
using Fred.Entities.Commande;
using Fred.Entities.Groupe;
using Fred.Entities.IndemniteDeplacement;
using Fred.Entities.Personnel;
using Fred.Entities.Referential;
using Fred.Entities.Societe;
using Fred.EntityFramework;
using Microsoft.EntityFrameworkCore;

namespace Fred.DataAccess.RepriseDonnees
{
    /// <summary>
    /// Repository des Reprises des données
    /// </summary>
    public class RepriseDonneesRepository : IRepriseDonneesRepository
    {
        private readonly FredDbContext context;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="unitOfWork">unitOfWork</param>       
        public RepriseDonneesRepository(IUnitOfWork unitOfWork, FredDbContext context)
        {
            this.context = context;
            this.UnitOfWork = unitOfWork;
        }

        /// <summary>
        /// UnitOfWork
        /// </summary>
        public IUnitOfWork UnitOfWork { get; set; }

        /// <summary>
        /// Retourne tous les groupes
        /// </summary>
        /// <returns>Liste de groupes</returns>
        public List<GroupeEnt> GetAllGroupes()
        {
            return context.Groupes.AsNoTracking().ToList();
        }

        /// <summary>
        /// Retourne les ci dont le code est contenu dans la liste ciCodes
        /// </summary>
        /// <param name="ciCodes">liste de code ci</param>
        /// <returns>Les ci qui ont le code contenu dans ciCodes </returns>
        public List<CIEnt> GetCisByCodes(List<string> ciCodes)
        {
            return context.CIs.Where(ci => ciCodes.Contains(ci.Code)).AsNoTracking().ToList();
        }

        /// <summary>
        /// Retourne les ci avec l'organisation et la societe dont le code est contenu dans la liste ciCodes
        /// </summary>
        /// <param name="ciCodes">liste de code ci</param>
        /// <returns>Les ci qui ont le code contenu dans ciCodes </returns>
        public List<CIEnt> GetCisByCodesWithSocieteAndOrganisation(List<string> ciCodes)
        {
            return context.CIs
                    .Where(x => ciCodes.Contains(x.Code))
                    .Include(x => x.Organisation)
                    .Include(x => x.Societe)
                    .AsNoTracking()
                    .ToList();
        }

        /// <summary>
        /// Retourne le Statut commande Validée
        /// </summary>
        /// <returns>le Statut commande Validée</returns>
        public StatutCommandeEnt GetStatusCommandeValidee()
        {
            return context.StatutsCommande
                .Where(x => x.Code == StatutCommandeEnt.CommandeStatutVA)
                .AsNoTracking()
                .FirstOrDefault();
        }

        /// <summary>
        /// Retourne les Taches dont le code est contenu dans la liste tachesCodes
        /// </summary>
        /// <param name="tachesCodes">Liste des codes des taches</param>
        /// <returns>Les Taches qui ont le code contenu dans tachesCodes</returns>
        public List<TacheEnt> GetTachesByCodes(List<string> tachesCodes)
        {
            return context.Taches
                .Where(x => tachesCodes.Contains(x.Code))
                .AsNoTracking()
                .ToList();
        }

        /// <summary>
        /// Retourne les Pays dont le code est contenu dans la liste paysCodes
        /// </summary>
        /// <param name="paysCodes">Liste des codes des pays</param>
        /// <returns>Les Pays qui ont le code contenu dans paysCodes</returns>
        public List<PaysEnt> GetPaysByCodes(List<string> paysCodes)
        {
            return context.Pays
                .Where(x => paysCodes.Contains(x.Code))
                .AsNoTracking()
                .ToList();
        }

        /// <summary>
        /// Retourne les Personnels dont le code et la societé sont contenu dans les listes personnelMatricules et societeCodes
        /// </summary>
        /// <param name="personnelMatricules">Liste des matricules</param>
        /// <param name="societeCodes">Liste des codes sociétés</param>
        /// <returns>Les Personnels qui ont le code et la société contenu dans personnelMatricules et societeCodes</returns>
        public List<PersonnelEnt> GetPersonnelsByCodesAndBySocietes(List<string> personnelMatricules, List<string> societeCodes)
        {
            return context.Personnels
                .Where(p => personnelMatricules.Contains(p.Matricule)
                    && societeCodes.Contains(p.Societe.Code))
                .Include(p => p.Societe)
                .AsNoTracking()
                .ToList();
        }

        /// <summary>
        /// Retourne les Societés pour le groupeId donné et dont les codes sont contenu dans la liste societeCodes
        /// </summary>
        /// <param name="groupeId">ID du groupe</param>
        /// <param name="societeCodes">Liste des codes sociétés</param>
        /// <returns>Les Sociétés qui ont le code contenu dans societesCodes et qui appartiennent au groupe groupeId</returns>
        public List<SocieteEnt> GetListSocieteByGroupAndCodes(int groupeId, List<string> societeCodes)
        {
            return context.Societes
                .Where(s => s.GroupeId == groupeId
                    && societeCodes.Contains(s.Code))
                .AsNoTracking()
                .ToList();
        }

        /// <summary>
        /// Retourne les Matériels pour le groupeId donné et dont les codes sont contenus dans la liste materielCodes
        /// </summary>
        /// <param name="societeCodes">Liste des codes sociétés</param>
        /// <param name="materielCodes">Liste des codes matériels</param>
        /// <returns>Les Matériels qui ont le code contenu dans matrielCodes et qui appartiennent au groupe groupeId</returns>
        public List<MaterielEnt> GetListMaterielBySocieteAndCodes(List<string> societeCodes, List<string> materielCodes)
        {
            return context.Materiels
                .Where(m => societeCodes.Contains(m.Societe.Code)
                    && materielCodes.Contains(m.Code))
                .Include(m => m.Societe)
                .AsNoTracking()
                .ToList();
        }

        /// <summary>
        /// Retourne les Code Deplacemeent dont le code est contenu dans la liste codeDeplacements
        /// </summary>
        /// <param name="societeCodesDep">Liste des codes société des Codes Dep</param>
        /// <param name="codeDeplacements">Liste des codes Déplacments</param>
        /// <returns>Les Codes Deplacement qui ont le code contenu dans codeDeplacements</returns>
        public List<CodeDeplacementEnt> GetListCodeDeplacementBySocietesAndCodes(List<string> societeCodesDep, List<string> codeDeplacements)
        {
            return context.CodesDeplacement
                .Where(c => societeCodesDep.Contains(c.Societe.Code)
                    && codeDeplacements.Contains(c.Code))
                .Include(c => c.Societe)
                .AsNoTracking()
                .ToList();
        }

        /// <summary>
        /// Retourne les Code Zone Deplacemeent dont le code est contenu dans la liste codeZoneDeplacements et la société dans societeCodesDep
        /// </summary>
        /// <param name="societeCodesDep">Liste des codes société des Codes Dep</param>
        /// <param name="codeZoneDeplacements">Liste des codes zone Déplacement</param>
        /// <returns>Les Codes Zone Deplacement qui ont le code contneu dans codeZoneDeplacements et la societe dans societeCodesDep</returns>
        public List<CodeZoneDeplacementEnt> GetListCodeZoneDeplacementBySocietesAndCodes(List<string> societeCodesDep, List<string> codeZoneDeplacements)
        {
            return context.CodeZoneDeplacement
                .Where(c => societeCodesDep.Contains(c.Societe.Code)
                    && codeZoneDeplacements.Contains(c.Code))
                .Include(c => c.Societe)
                .AsNoTracking()
                .ToList();
        }


        /// <summary>
        /// Retourne les Indemnité Déplacement ayant un Ci et un Personnel (matricule) dans les listes ciCodes et personnelMatricules
        /// </summary>
        /// <param name="ciCodes">Liste des CI</param>
        /// <param name="personnelMatricules">Liste des Matricules Personnel</param>
        /// <returns>Les Indemnités Déplacement qui ont le Ci contenu dans ciCodes et le Personnel dans personnelMatricules</returns>
        public List<IndemniteDeplacementEnt> GetIndemniteDeplacementsByCIAndPersonnel(List<string> ciCodes, List<string> personnelMatricules)
        {
            return context.IndemniteDeplacement
                .Where(i => ciCodes.Contains(i.CI.Code)
                    && personnelMatricules.Contains(i.Personnel.Matricule))
                .Include(i => i.CI)
                .Include(i => i.Personnel)
                .AsNoTracking()
                .ToList();
        }

        /// <summary>
        /// Retourne les Personnels ayant une adresse Mail dans la liste adressesEmail
        /// </summary>
        /// <param name="adressesEmail">Liste des Emails</param>
        /// <returns>Les Personnel qui ont l'Email contenu dans adressesEmail</returns>
        public List<PersonnelEnt> GetPersonnelsByEmails(List<string> adressesEmail)
        {
            return context.Personnels
                .Where(p => adressesEmail.Contains(p.Email))
                .AsNoTracking()
                .ToList();
        }
    }
}
