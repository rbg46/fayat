using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Fred.DataAccess.Common;
using Fred.DataAccess.Interfaces;
using Fred.Entities.Affectation;
using Fred.Entities.CI;
using Fred.Entities.Personnel;
using Fred.Entities.Rapport.Constante;
using Fred.EntityFramework;
using Fred.Web.Models.Affectation;
using Fred.Web.Shared.Comparer;
using Fred.Web.Shared.Extentions;
using Fred.Web.Shared.Models.EtatPaie;
using Microsoft.EntityFrameworkCore;
using static Fred.Entities.Constantes;

namespace Fred.DataAccess.Affectation
{
    /// <summary>
    /// Affectation repository class
    /// </summary>
    public class AffectationRepository : FredRepository<AffectationEnt>, IAffectationRepository
    {
        private readonly FredDbContext context;

        public AffectationRepository(FredDbContext context)
              : base(context)
        {
            this.context = context;
        }

        #region public method

        /// <summary>
        /// Get la liste des personnels affectés a un CI
        /// </summary>
        /// <param name="ciId">Ci identifier</param>
        /// <returns>List des affectations</returns>
        public IEnumerable<AffectationEnt> GetAffectationListByCi(int ciId)
        {

            return Context.Affectation.Where(x => x.CiId == ciId).Include(x => x.CI).Include(x => x.Astreintes).Include(x => x.Personnel)
                                      .ThenInclude(x => x.Societe)
                                      .Include(x => x.Personnel.EquipePersonnels)
                                      .ThenInclude(x => x.Equipe).ToList();
        }

        /// <summary>
        /// Get la liste des personnels affectés a un CI
        /// </summary>
        /// <param name="ciIdList">list des "Ci identifier"</param>
        /// <param name="etablissementPaieIdList">List Etablissement Paie Id </param>
        /// <param name="etatPaieExportModel">Model de l'etat de paie pour l'export</param>
        /// <returns>List des affectations</returns>
        public IEnumerable<PersonnelEnt> GetPersonnelListAffectedCiList(IEnumerable<int> ciIdList, IEnumerable<int?> etablissementPaieIdList, EtatPaieExportModel etatPaieExportModel)
        {
            if (ciIdList.IsNullOrEmpty())
            {
                return new List<PersonnelEnt>();
            }

            return context.Affectation
                                .Include(x => x.Personnel.Societe)
                                .Include(x => x.Personnel.Societe.Organisation)
                                .Where(f => f.Personnel != null && ciIdList.Contains(f.CiId)
                                 && (!f.Personnel.DateSortie.HasValue || (f.Personnel.DateSortie.Value.Month >= etatPaieExportModel.Mois && f.Personnel.DateSortie.Value.Year >= etatPaieExportModel.Annee))
                                 && (!etablissementPaieIdList.Any() || etablissementPaieIdList.Contains(f.Personnel.EtablissementPaieId.Value)))
                                 .AsNoTracking().Select(x => x.Personnel).Include(i => i.Societe).Distinct().ToList();
        }

        /// <summary>
        /// Get la liste des personnels affectés a un  etablissement de paie par l'organisation id
        /// </summary>
        /// <param name="etatPaieExportModel">Model de l'etat de paie pour l'export</param>
        /// <returns>List des affectations</returns>
        public IEnumerable<int> GetPersonnelIdAffectedEtablissementByOrganisationId(EtatPaieExportModel etatPaieExportModel)
        {
            return this.Query().Include(s => s.Personnel.Societe)
                               .Filter(p => p.Personnel != null)
                               .Filter(e => e.Personnel.EtablissementPaie.Societe.Organisation.OrganisationId == etatPaieExportModel.OrganisationId)
                               .Filter(f => !f.Personnel.DateSortie.HasValue || (f.Personnel.DateSortie.Value.Month >= etatPaieExportModel.Mois && f.Personnel.DateSortie.Value.Year >= etatPaieExportModel.Annee))
                               .Get().AsNoTracking().Select(x => x.PersonnelId).Distinct().ToList();

        }

        /// <summary>
        /// Get la liste des personnels affectés a un  etablissement de paie
        /// </summary>
        /// <param name="etablissementPaieIdList">List Etablissement Paie Id </param>
        /// <param name="etatPaieExportModel">Model de l'etat de paie pour l'export</param>
        /// <returns>List des affectations</returns>
        public IEnumerable<int> GetPersonnelIdListAffectedEtablissementList(IEnumerable<int> etablissementPaieIdList, EtatPaieExportModel etatPaieExportModel)
        {
            return this.Query().Include(x => x.Personnel.Societe)
                                    .Filter(f => f.Personnel != null)
                                    .Filter(f => etablissementPaieIdList.Any() && etablissementPaieIdList.Contains(f.Personnel.EtablissementPaieId.Value))
                                    .Filter(f => !f.Personnel.DateSortie.HasValue || (f.Personnel.DateSortie.Value.Month >= etatPaieExportModel.Mois && f.Personnel.DateSortie.Value.Year >= etatPaieExportModel.Annee))
                                    .Get().Select(x => x.PersonnelId).Distinct().ToList();
        }

        /// <summary>
        /// Supprimer une liste des affectations 
        /// </summary>
        /// <param name="affectationList">liste des affectations identifiers</param>
        public void DeleteAffectations(List<AffectationEnt> affectationList)
        {
            if (affectationList != null && affectationList.Count != 0)
            {
                foreach (AffectationEnt affectation in affectationList)
                {
                    if (affectation != null)
                    {
                        Delete(affectation);
                    }
                }
            }
        }

        /// <summary>
        /// Supprimer une liste des affectations 
        /// </summary>
        /// <param name="affectationLisIds">liste des affectations identifiers</param>
        public void DeleteAffectations(List<int> affectationLisIds)
        {
            if (affectationLisIds != null && affectationLisIds.Any())
            {
                foreach (int id in affectationLisIds)
                {
                    AffectationEnt affectation = this.Query().Get().FirstOrDefault(x => x.AffectationId == id);
                    if (affectation != null)
                    {
                        Delete(affectation);
                    }
                }
            }
        }

        /// <summary>
        /// Update affectation
        /// </summary>
        /// <param name="affectation">Affectation</param>
        /// <param name="isDelegate">Is delegate</param>
        public void UpdateAffectation(AffectationEnt affectation, bool isDelegate)
        {
            affectation.IsDelegue = isDelegate;
            Update(affectation);
        }

        /// <summary>
        /// Add or update a personnel to equipe favorite
        /// </summary>
        /// <param name="personnelId">PersonnelId</param>
        /// <param name="currentUserId">Resposable administratif d'une equipe</param>
        /// <param name="isInFavoriteTeam">True si le personnel doit etre dans l'equipe favorite</param>
        public void AddUpdateToFavoriteTeam(int personnelId, int currentUserId, bool isInFavoriteTeam)
        {
            EquipeEnt equipe = context.Equipe.FirstOrDefault(x => x.ProprietaireId == currentUserId);
            if (equipe != null)
            {
                EquipePersonnelEnt equipePersonnel = context.EquipePersonnel.FirstOrDefault(x => x.PersonnelId == personnelId && x.EquipePersoId == equipe.EquipeId);
                if (equipePersonnel == null && isInFavoriteTeam)
                {
                    equipePersonnel = new EquipePersonnelEnt
                    {
                        EquipePersoId = equipe.EquipeId,
                        PersonnelId = personnelId
                    };
                    context.EquipePersonnel.Add(equipePersonnel);
                }

                if (equipePersonnel != null && !isInFavoriteTeam)
                {
                    context.EquipePersonnel.Remove(equipePersonnel);
                }
            }
            if (equipe == null && isInFavoriteTeam)
            {
                // TODO 13841 : Double-check for regression
                equipe = new EquipeEnt { ProprietaireId = currentUserId };
                context.Equipe.Add(equipe);

                EquipePersonnelEnt equipePersonnel = new EquipePersonnelEnt
                {
                    Equipe = equipe,
                    PersonnelId = personnelId
                };
                context.EquipePersonnel.Add(equipePersonnel);
            }
        }

        /// <summary>
        /// Get list des astreintes d'une affectation
        /// </summary>
        /// <param name="affectationId">Affectation identifier</param>
        /// <param name="dateDebut">Date debut</param>
        /// <param name="dateFin">Date fin</param>
        /// <returns>List des astreintes</returns>
        public IEnumerable<AstreinteEnt> GetAstreintesByAffectationIdAndDate(int affectationId, DateTime dateDebut, DateTime dateFin)
        {
            return context.Astreinte.Where(x => x.AffectationId == affectationId && x.DateAstreinte >= dateDebut && x.DateAstreinte <= dateFin).ToList();
        }

        /// <summary>
        /// Get list des astreintes par personnel et ci
        /// </summary>
        /// <param name="personnelId">personnel identifier</param>
        /// <param name="ciId">ci identifier</param>
        /// <param name="dateDebut">Date debut</param>
        /// <param name="dateFin">Date fin</param>
        /// <returns>List des astreintes</returns>
        public IEnumerable<AstreinteEnt> GetAstreintesByPersonnelIdAndCiId(int personnelId, int ciId, DateTime dateDebut, DateTime dateFin)
        {
            return context.Astreinte.Where(x => x.Affectation.CiId == ciId && x.Affectation.PersonnelId == personnelId && x.DateAstreinte >= dateDebut && x.DateAstreinte <= dateFin).ToList();
        }

        /// <summary>
        /// Supprimer une liste des astreintes
        /// </summary>
        /// <param name="astreintesList">List des astreintes</param>
        public void DeleteAstreintes(List<AstreinteEnt> astreintesList)
        {
            if (astreintesList != null && astreintesList.Any())
            {
                this.context.Astreinte.RemoveRange(astreintesList);
            }
        }

        /// <summary>
        /// Récupérer une astreinte d'un personnel dans un CI et une date précise
        /// </summary>
        /// <param name="ciId">L'idenitifiant du CI</param>
        /// <param name="personnelId">L'identifiant du personnel</param>
        /// <param name="astreinteDate">La date de l'astreinte</param>
        /// <returns>L'astreinte</returns>
        public AstreinteEnt GetAstreinte(int ciId, int personnelId, DateTime astreinteDate)
        {
            return context.Astreinte.FirstOrDefault(a => a.Affectation.CiId == ciId && a.Affectation.PersonnelId == personnelId && a.DateAstreinte == astreinteDate);
        }

        /// <summary>
        /// Récupérer l'identifiant d'une astreinte d'un personnel dans un CI et une date précise
        /// </summary>
        /// <param name="ciId">L'idenitifiant du CI</param>
        /// <param name="personnelId">L'identifiant du personnel</param>
        /// <param name="astreinteDate">La date de l'astreinte</param>
        /// <returns>L'astreinte</returns>
        public int? GetAstreinteId(int ciId, int personnelId, DateTime astreinteDate)
        {
            return context.Astreinte.Where(a => a.Affectation.CiId == ciId && a.Affectation.PersonnelId == personnelId && a.DateAstreinte == astreinteDate).Select(a => a.AstreintId).FirstOrDefault();
        }

        /// <summary>
        /// Retourne les astreintes pour un personnel donné et qui appartiennent a la fois a un ci de la liste ciIds et a une DateAstreinte de la liste astreinteDates
        /// </summary>
        /// <param name="ciIds">ciIds</param>
        /// <param name="personnelId">personnelId</param>
        /// <param name="astreinteDates">astreinteDates</param>
        /// <returns>Retourne les astreintes pour un personnel donné</returns>
        public IEnumerable<AstreinteEnt> GetAstreintes(IEnumerable<int> ciIds, int personnelId, IEnumerable<DateTime> astreinteDates)
        {
            return context.Astreinte
                .Include(a => a.Affectation)
                .Where(a => ciIds.Contains(a.Affectation.CiId) && a.Affectation.PersonnelId == personnelId && astreinteDates.Contains(a.DateAstreinte)).AsNoTracking().ToList();
        }

        /// <summary>
        ///  Récupérer une astreinte d'un personnel dans un CI et une date précise a partir d'une liste d'astreinte
        /// </summary>
        /// <param name="astreintes">astreintes</param>
        /// <param name="ciId">L'idenitifiant du CI</param>
        /// <param name="personnelId">L'identifiant du personnel</param>
        /// <param name="astreinteDate">La date de l'astreinte</param>
        /// <returns>L'astreinte</returns>
        public AstreinteEnt GetAstreinte(IEnumerable<AstreinteEnt> astreintes, int ciId, int personnelId, DateTime astreinteDate)
        {
            return astreintes
            .FirstOrDefault(a => a.Affectation.CiId == ciId && a.Affectation.PersonnelId == personnelId && a.DateAstreinte == astreinteDate);
        }

        /// <summary>
        /// Récupérer la liste des CI dans le quel le personnel est affecté
        /// </summary>
        /// <param name="personnelId">L'identifiant du personnel</param>
        /// <returns>La liste des CI</returns>
        public List<CIEnt> GetPersonnelAffectationCiList(int personnelId)
        {
            List<CIEnt> ciList = context.Affectation
              .Where(a => a.PersonnelId == personnelId)
              .Select(a => a.CI)
              .Include(c => c.CIType)
              .ToList();

            return ciList.Distinct(new CiComparer()).ToList();
        }


        /// <summary>
        /// Get identifiers of Etam and Iac affected to a CI
        /// </summary>
        /// <param name="ciIdList">List ci Identifier</param>
        /// <returns>List des personnels Ids</returns>
        public IEnumerable<int> GetEtamAndIacAffectationListByCiList(IEnumerable<int> ciIdList)
        {
            return context.Affectation.Where(x => ciIdList.Contains(x.CiId) &&
                                      (x.Personnel.Statut.Equals(TypePersonnel.ETAM) || x.Personnel.Statut.Equals(TypePersonnel.ETAMArticle36) ||
                                       x.Personnel.Statut.Equals(TypePersonnel.ETAMBureau) || x.Personnel.Statut.Equals(TypePersonnel.Cadre)))
                                      .Select(x => x.PersonnelId)
                                      .Distinct()
                                      .ToList();
        }

        /// <summary>
        /// Get Personnel id list affected to a CI
        /// </summary>
        /// <param name="ciIdList">List ci Identifier</param>
        /// <returns>List des personnels Ids</returns>
        public IEnumerable<int> GetPersonnelsAffectationListByCiList(IEnumerable<int> ciIdList)
        {
            return context.Affectation.Where(x => ciIdList.Contains(x.CiId))
                                     .Select(x => x.PersonnelId)
                                     .Distinct()
                                     .ToList();
        }

        /// <summary>
        /// Récupération ou création d'une Affectation avec besoin de Delegation (exemple : pour FES)
        /// </summary>
        /// <param name="personnelId">Id du personnel</param>
        /// <param name="ciId">Id du CI</param>
        /// <param name="isDelegate">Delegation</param>
        /// <returns>Affectation Entité</returns>
        public AffectationEnt GetOrCreateAffectation(int personnelId, int ciId, bool isDelegate)
        {
            AffectationEnt affectation = context.Affectation.FirstOrDefault(x => x.PersonnelId == personnelId && x.CiId == ciId);
            if (affectation == null)
            {
                affectation = new AffectationEnt
                {
                    CiId = ciId,
                    IsDelegue = isDelegate,
                    PersonnelId = personnelId,
                    IsDelete = false
                };

                context.Affectation.Add(affectation);
            }
            else
            {
                affectation.IsDelegue = isDelegate;
                affectation.IsDelete = false;
                Update(affectation);
            }
            context.SaveChanges();

            return affectation;
        }

        /// <summary>
        /// Récupération ou New d'une Affectation sans besoin de Delegation (exemple : pour RazelBec)
        /// </summary>
        /// <param name="personnelId">Id du personnel</param>
        /// <param name="ciId">Id du CI</param>
        /// <returns>Affectation Entité</returns>
        public async Task<AffectationEnt> GetOrNewAffectationAsync(int personnelId, int ciId)
        {
            AffectationEnt affectation = await context.Affectation.FirstOrDefaultAsync(x => x.PersonnelId == personnelId && x.CiId == ciId);
            if (affectation == null)
            {
                affectation = new AffectationEnt
                {
                    CiId = ciId,
                    IsDelegue = false,
                    PersonnelId = personnelId
                };
            }

            return affectation;
        }

        /// <summary>
        /// Get list des astreintes d'une affectation
        /// </summary>
        /// <param name="affectationId">Affectation identifier</param>
        /// <returns>List des astreintes</returns>
        public List<AstreinteEnt> GetAstreintesByAffectationId(int affectationId)
        {
            return context.Astreinte.Where(x => x.AffectationId == affectationId).ToList();
        }

        /// <summary>
        /// Ajouter une astreinte
        /// </summary>
        /// <param name="affectationId">Affectation identifier</param>
        /// <param name="startDate">Start date</param>
        /// <param name="dayOfWeek">Jour de la semaine</param>
        public void AddAstreinte(int affectationId, DateTime startDate, int dayOfWeek)
        {
            AstreinteEnt astreinte = new AstreinteEnt
            {
                AffectationId = affectationId,
                DateAstreinte = GetAstreinteDate(startDate, dayOfWeek)
            };

            context.Astreinte.Add(astreinte);
        }

        /// <summary>
        /// Update Astreinte
        /// </summary>
        /// <param name="astreinteId">astreinte identifier</param>
        public void UpdateAstreinte(int astreinteId)
        {
            AstreinteEnt astreinte = context.Astreinte.FirstOrDefault(x => x.AstreintId == astreinteId);
            if (astreinte != null)
            {
                context.Astreinte.Remove(astreinte);
            }
        }

        /// <summary>
        /// Récupérer la liste des CI dans le quel le personnel est affecté
        /// </summary>
        /// <typeparam name="TCI">Le type de CI désiré.</typeparam>
        /// <param name="personnelId">L'identifiant du personnel</param>
        /// <param name="selector">Selector permettant de constuire un TCI en fonction d'un CIEnt.</param>
        /// <returns>La liste des CI</returns>
        public List<TCI> GetPersonnelAffectationCiList<TCI>(int personnelId, Expression<Func<CIEnt, TCI>> selector)
        {
            return this.context.Affectation.Include(x => x.CI)
                    .Include(x => x.CI.Organisation)
                    .Where(a => a.PersonnelId == personnelId)
                    .Select(a => a.CI)
                    .GroupBy(c => c.CiId).Select(g => g.FirstOrDefault())
                    .Select(selector)
                    .ToList();
        }

        /// <summary>
        /// Récupérer la liste des CI dans le quel le personnel est affecté
        /// </summary>
        /// <param name="personnelId">L'identifiant du personnel</param>
        /// <returns>La liste des CI</returns>
        public List<CIEnt> GetPersonnelAffectationCiListFiggo(int personnelId)
        {
            List<CIEnt> ciList = context.Affectation
              .Where(a => a.PersonnelId == personnelId)
              .Select(a => a.CI).Where(x => x.Code == FiggoConstante.CodeCiAbsenceFiggo)
              .Include(c => c.CIType)
              .ToList();
            return ciList.Distinct(new CiComparer()).ToList();
        }

        /// <summary>
        /// verifier si le personnel a ci par defaut
        /// </summary>
        /// <param name="personnelId">identifiant du personnel</param>
        /// <returns>true si le personnel a un ci par defaut</returns>
        public bool CheckIfPersonnelHasDefaultCi(int personnelId)
        {
            return context.Affectation.Any(x => x.PersonnelId == personnelId && x.IsDefault);
        }

        /// <summary>
        /// Recuperer list des ouvriers Ids par list des ci Ids
        /// </summary>
        /// <param name="ciList"></param>
        /// <returns>List des ouvriers ids</returns>
        public async Task<IEnumerable<AffectationEnt>> GetOuvriersListIdsByCiListAsync(List<int> ciList)
        {
            return await context.Affectation.Include(a => a.Personnel).Where(p => ciList.Contains(p.CiId) && p.Personnel.Statut == TypePersonnel.Ouvrier)
                                            .AsNoTracking().ToListAsync().ConfigureAwait(false);
        }

        /// <summary>
        /// Récupérer la liste des CI actifs dans le quel le personnel est affecté
        /// </summary>
        /// <param name="personnelId">L'identifiant du personnel</param>
        /// <returns>La liste des CI</returns>
        public IEnumerable<CIEnt> GetPersonnelActifAffectationCiList(int personnelId)
        {
            List<CIEnt> ciList = Context.Affectation
              .Where(a => a.PersonnelId == personnelId)
              .Select(a => a.CI)
              .Include(c => c.CIType)
              .ToList();

            return ciList.Distinct(new CiComparer());
        }

        #endregion

        #region private method

        /// <summary>
        /// Récupérer la date de l'astreinte à partir la date de lundi et le jour de la semaine
        /// </summary>
        /// <param name="mondayDate">Date de lundi</param>
        /// <param name="dayOfWeek">Jour de la semaine</param>
        /// <returns>Date de l'astreinte</returns>
        private DateTime GetAstreinteDate(DateTime mondayDate, int dayOfWeek)
        {
            if (dayOfWeek == 0)
            {
                return mondayDate.AddDays(6);
            }
            else
            {
                return mondayDate.AddDays(dayOfWeek - 1);
            }
        }

        /// <summary>
        /// Get affecation by id
        /// </summary>
        /// <param name="id">Affecation id</param>
        /// <returns>List des affectations</returns>
        public AffectationEnt GetAffectationById(int id)
        {
            return Context.Affectation
                .Include(x => x.Astreintes)
                .Include(x => x.Personnel.Societe.Groupe)
                .FirstOrDefault(x => x.AffectationId == id);
        }

        /// <summary>
        /// Update affectation list
        /// </summary>
        /// <param name="affectationModelList">Affectaion model list</param>
        public void UpdateAffectationList(IEnumerable<AffectationModel> affectationModelList)
        {
            if (affectationModelList.IsNullOrEmpty())
            {
                return;
            }

            affectationModelList.ToList().ForEach(v => UpdateOrCreateAffectation(v));
        }

        /// <summary>
        /// Update or create affectation
        /// </summary>
        /// <param name="model">Affectation model</param>
        private void UpdateOrCreateAffectation(AffectationModel model)
        {
            AffectationEnt affectation = context.Affectation.FirstOrDefault(
                                            x => x.PersonnelId == model.PersonnelId
                                                && x.CiId == model.CiId);

            if (affectation == null)
            {
                affectation = new AffectationEnt
                {
                    CiId = model.CiId,
                    IsDelegue = model.IsDelegue,
                    PersonnelId = model.PersonnelId
                };

                context.Affectation.Add(affectation);
            }
            else
            {
                affectation.IsDelegue = model.IsDelegue;
                affectation.IsDelete = false;
                Update(affectation);
            }
        }

        /// <summary>
        /// Get la liste des personnels affectés a un CI
        /// </summary>
        /// <param name="ciId">Ci identifier</param>
        /// <param name="personnelId">Personnel identifier</param>
        /// <returns>affectations</returns>
        public AffectationEnt GetAffectationByCiAndPersonnel(int ciId, int personnelId)
        {
            return context.Affectation
              .Include(x => x.Personnel)
              .Include(x => x.CI)
              .FirstOrDefault(x => x.CiId == ciId && x.PersonnelId == personnelId);
        }

        /// <summary>
        /// Get la liste des personnels affectés a un CI
        /// </summary>
        /// <param name="personnelId">Personnel identifier</param>
        /// <returns>affectations</returns>
        public List<AffectationEnt> GetAffectationByListPersonnelId(List<int> personnelId)
        {
            return context.Affectation
              .Include(x => x.Personnel)
              .Include(x => x.CI)
              .Include(x => x.CI.EtablissementComptable)
              .Include(x => x.CI.Societe)
              .Include(x => x.CI.CIType)
              .Where(x => personnelId.Contains(x.PersonnelId))
              .ToList();
        }
        #endregion
    }
}
