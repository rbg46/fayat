using System;
using System.Collections.Generic;
using System.Linq;
using Fred.Common.Tests.Data.CI.Builder;
using Fred.Common.Tests.EntityFramework;
using Fred.Entities.CI;
using Fred.Entities.Personnel;
using Fred.Entities.Personnel.Interimaire;
using Fred.Entities.Rapport;
using Fred.Entities.Referential;

namespace Fred.Common.Tests.Data.Rapport.Builder
{
    public class RapportLigneBuilder : ModelDataTestBuilder<RapportLigneEnt>
    {
        public RapportLigneEnt Prototype()
        {
            New();
            Model.Ci = new CiBuilder().Prototype();
            Model.CiId = 1;
            Model.RapportId = 1;
            Model.Rapport = new RapportBuilder().Prototype();
            Model.ListErreurs = new List<string>();
            Model.DatePointage = new DateTime(2019, 01, 01);
            Model.RapportLigneStatutId = 5;
            return Model;
        }

        public RapportLigneBuilder RapportLigneStatutId (int statut)
        {
            Model.RapportLigneStatutId = statut;
            return this;
        }

        public RapportLigneBuilder RapportId(int id)
        {
            Model.RapportId = id;
            return this;
        }
        public RapportLigneBuilder ListErreurs(IEnumerable<string> errors)
        {
            Model.ListErreurs = errors?.ToList();
            return this;
        }

        public RapportLigneBuilder AddError(string error)
        {
            if (Model.ListErreurs == null)
            {
                Model.ListErreurs = new List<string>();
            }

            Model.ListErreurs.Add(error);
            return this;
        }

        public RapportLigneBuilder ListRapportLignePrimes(IEnumerable<RapportLignePrimeEnt> primes)
        {
            Model.ListRapportLignePrimes = primes?.ToList();
            return this;
        }

        public RapportLigneBuilder ListRapportLigneTaches(IEnumerable<RapportLigneTacheEnt> taches)
        {
            Model.ListRapportLigneTaches = taches?.ToList();
            return this;
        }
        public RapportLigneBuilder ListRapportLigneAstreintes(IEnumerable<RapportLigneAstreinteEnt> astreintes)
        {
            Model.ListRapportLigneAstreintes = astreintes?.ToList();
            return this;
        }

        public RapportLigneBuilder ListRapportLigneMajorations(IEnumerable<RapportLigneMajorationEnt> majorations)
        {
            Model.ListRapportLigneMajorations = majorations?.ToList();
            return this;
        }

        public RapportLigneBuilder Rapport(RapportEnt rapport)
        {
            Model.Rapport = rapport;
            return this;
        }

        public RapportLigneBuilder Personnel(PersonnelEnt personnel)
        {
            Model.Personnel = personnel;
            Model.PersonnelId = personnel.PersonnelId;
            return this;
        }

        public RapportLigneBuilder CiId(int id)
        {
            Model.CiId = id;
            return this;
        }
        public RapportLigneBuilder Ci(CIEnt ci)
        {
            Model.Ci = ci;
            return this;
        }

        public RapportLigneBuilder PersonnelId(int personnelId)
        {
            Model.PersonnelId = personnelId;
            return this;
        }

        public RapportLigneBuilder HeureNormal(int heure)
        {
            Model.HeureNormale = heure;
            return this;
        }

        public RapportLigneBuilder DateSuppression(DateTime date)
        {
            Model.DateSuppression = date;
            return this;
        }

        public RapportLigneBuilder DatePointage(DateTime date)
        {
            Model.DatePointage = date;
            return this;
        }

        public RapportLigneBuilder ContratId(int contratId)
        {
            Model.ContratId = contratId;
            return this;
        }


        public RapportLigneBuilder Contrat(ContratInterimaireEnt contrat)
        {
            Model.Contrat = contrat;
            return this;
        }

        public RapportLigneBuilder ReceptionInterimaire()
        {
            Model.ReceptionInterimaire = true;
            return this;
        }

        public RapportLigneBuilder CodeAbsence(CodeAbsenceEnt codeAbsence)
        {
            Model.CodeAbsence = codeAbsence;
            Model.CodeAbsenceId = codeAbsence.CodeAbsenceId;
            return this;
        }

    }
}
