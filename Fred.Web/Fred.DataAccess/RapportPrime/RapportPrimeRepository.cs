using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fred.DataAccess.Common;
using Fred.DataAccess.Interfaces;
using Fred.Entities.RapportPrime;
using Fred.EntityFramework;
using Fred.Framework;
using Fred.Web.Shared.Models.RapportPrime;
using Fred.Web.Shared.Models.RapportPrime.Get;
using Microsoft.EntityFrameworkCore;

namespace Fred.DataAccess.RapportPrime
{
    public class RapportPrimeRepository : FredRepository<RapportPrimeEnt>, IRapportPrimeRepository
    {
        private readonly FredDbContext context;

        public RapportPrimeRepository(FredDbContext context)
          : base(context)
        {
            this.context = context;
        }

        public RapportPrimeEnt GetRapportPrimeByDate(DateTime dateRapport, List<int> listCiId)
        {
            RapportPrimeEnt rapportPrime = context.RapportPrime
                                               .Include(rap => rap.ListLignes).ThenInclude(rl => rl.Personnel.Societe)
                                               .Include(rap => rap.ListLignes).ThenInclude(rl => rl.AuteurCreation.Personnel)
                                               .Include(rap => rap.ListLignes).ThenInclude(rl => rl.AuteurValidation.Personnel)
                                               .Include(rap => rap.ListLignes).ThenInclude(rl => rl.AuteurVerrou.Personnel)
                                               .Include(rap => rap.ListLignes).ThenInclude(rl => rl.Ci)
                                               .Include(rap => rap.ListLignes).ThenInclude(rl => rl.ListPrimes).ThenInclude(p => p.Prime)
                                               .Include(rap => rap.ListLignes).ThenInclude(rl => rl.ListAstreintes).ThenInclude(a => a.Astreinte)
                                               .Where(rap => rap.DateRapportPrime.Year == dateRapport.Year
                                                       && rap.DateRapportPrime.Month == dateRapport.Month
                                                       && !rap.DateSuppression.HasValue)
                                               .FirstOrDefault();

            if (rapportPrime != null)
            {
                // On ne récupère pas les lignes avec une date de suppression
                rapportPrime.ListLignes = rapportPrime.ListLignes.Where(o => !o.DateSuppression.HasValue && (o.CiId == null || listCiId.Contains(o.CiId.Value)))
                                                                 .OrderBy(o => o.RapportPrimeLigneId).ToList();
            }

            return rapportPrime;
        }

        public async Task<RapportPrimeGetModel> GetRapportPrimeByDateAsync(DateTime dateRapport, List<int> listCiId)
        {
            RapportPrimeGetModel rapportPrime = await context.RapportPrime
                .Where(rp => rp.DateRapportPrime.Year == dateRapport.Year
                             && rp.DateRapportPrime.Month == dateRapport.Month
                             && !rp.DateSuppression.HasValue)
                .Select(rp => new RapportPrimeGetModel
                {
                    RapportPrimeId = rp.RapportPrimeId,
                    DateRapportPrime = rp.DateRapportPrime
                })
                .FirstOrDefaultAsync()
                .ConfigureAwait(false);

            if (rapportPrime != null)
            {
                rapportPrime.ListLignes = await context.RapportPrimeLigne
                    .Where(rpl => rpl.RapportPrimeId == rapportPrime.RapportPrimeId
                                  && !rpl.DateSuppression.HasValue
                                  && (!rpl.CiId.HasValue || listCiId.Contains(rpl.CiId.Value)))
                    .Select(rpl => new RapportPrimeLigneGetModel
                    {
                        RapportPrimeLigneId = rpl.RapportPrimeLigneId,
                        DateValidation = rpl.DateValidation,
                        IsCreated = rpl.IsCreated,
                        IsUpdated = rpl.IsUpdated,
                        IsDeleted = rpl.IsDeleted,
                        IsValidated = rpl.IsValidated,
                        PersonnelId = rpl.PersonnelId,
                        Personnel = new PersonnelGetModel
                        {
                            PersonnelId = rpl.PersonnelId,
                            CodeSocieteMatriculePrenomNom = rpl.Personnel.SocieteId != null
                                ? rpl.Personnel.Societe.Code + " - " + rpl.Personnel.Matricule + " - " + rpl.Personnel.PrenomNom
                                : $"{rpl.Personnel.Matricule} - {rpl.Personnel.PrenomNom}"
                        },
                        CiId = rpl.CiId,
                        Ci = new CiGetModel
                        {
                            CodeLibelle = rpl.Ci.CodeLibelle
                        },
                        ListAstreintes = rpl.ListAstreintes
                            .Select(a => a.Astreinte.DateAstreinte)
                            .ToList(),
                        ListPrimes = rpl.ListPrimes
                            .Select(rplp => new RapportPrimeLignePrimeGetModel
                            {
                                RapportPrimeLignePrimeId = rplp.RapportPrimeLignePrimeId,
                                PrimeId = rplp.PrimeId,
                                Prime = new PrimeGetModel
                                {
                                    PrimeId = rplp.PrimeId,
                                    Code = rplp.Prime.Code,
                                    Libelle = rplp.Prime.Libelle,
                                    PrimeType = rplp.Prime.PrimeType,
                                    SeuilMensuel = rplp.Prime.SeuilMensuel
                                },
                                RapportPrimeLigneId = rpl.RapportPrimeLigneId,
                                Montant = rplp.Montant,
                                IsCreated = rplp.IsCreated,
                                IsDeleted = rplp.IsDeleted
                            })
                            .ToList(),
                        AuteurCreationId = rpl.AuteurCreationId,
                        AuteurCreation = new UtilisateurGetModel
                        {
                            Nom = rpl.AuteurCreation.Personnel.Nom,
                            Prenom = rpl.AuteurCreation.Personnel.Prenom,
                        },
                        DateCreation = rpl.DateCreation,
                        AuteurModificationId = rpl.AuteurCreationId,
                        AuteurModification = new UtilisateurGetModel
                        {
                            Nom = rpl.AuteurModification.Personnel.Nom,
                            Prenom = rpl.AuteurModification.Personnel.Prenom,
                        },
                        DateModification = rpl.DateModification,
                        AuteurValidationId = rpl.AuteurCreationId,
                        AuteurValidation = new UtilisateurGetModel
                        {
                            Nom = rpl.AuteurValidation.Personnel.Nom,
                            Prenom = rpl.AuteurValidation.Personnel.Prenom,
                        },
                    })
                    .ToListAsync()
                    .ConfigureAwait(false);

                rapportPrime.ListPrimesHeader = rapportPrime.ListLignes
                    .SelectMany(rpl => rpl.ListPrimes
                        .Select(p => p.Prime))
                    .GroupBy(p => p.PrimeId)
                    .Select(p => p.First())
                    .ToList();
            }

            return rapportPrime;
        }

        public async Task<bool> RapportPrimeExistsAsync(DateTime date)
        {
            return await context.RapportPrime
                .Where(r => r.DateRapportPrime.Year == date.Year
                            && r.DateRapportPrime.Month == date.Month)
                .AnyAsync();
        }

        public async Task AddAsync(RapportPrimeEnt rapportPrime)
        {
            await context.RapportPrime.AddAsync(rapportPrime);
        }
    }
}
