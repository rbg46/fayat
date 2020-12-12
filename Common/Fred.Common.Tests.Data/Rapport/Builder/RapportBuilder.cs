using System;
using System.Collections.Generic;
using System.Linq;
using Fred.Common.Tests.Data.CI.Builder;
using Fred.Common.Tests.EntityFramework;
using Fred.Entities.Rapport;

namespace Fred.Common.Tests.Data.Rapport.Builder
{
    public class RapportBuilder : ModelDataTestBuilder<RapportEnt>
    {

        public RapportEnt Prototype()
        {
            New();
            Model.CI = new CiBuilder().Prototype();
            Model.CiId = 1;
            Model.RapportId = 1;
            Model.RapportStatutId = 1;
            Model.TypeRapport = (int)Entities.Rapport.TypeRapport.Journee;
            Model.TypeRapportEnum = Entities.Rapport.TypeRapport.Journee;
            Model.IsStatutVerrouille = true;
            Model.ValidationSuperieur = true;
            Model.DateChantier = DateTime.UtcNow;
            Model.ListErreurs = new List<string>();
            Model.ListLignes = new List<RapportLigneEnt>();
            return Model;
        }

        public RapportBuilder Prerequi()
        {
            New();
            Prototype();
            return this;
        }

        public RapportBuilder CI(Entities.CI.CIEnt ci)
        {
            Model.CI = ci;
            return this;
        }

        public RapportBuilder CiId(int id)
        {
            Model.CiId = id;
            return this;
        }

        public RapportBuilder RapportId(int id)
        {
            Model.RapportId = id;
            return this;
        }

        public RapportBuilder RapportStatutId(int statutId)
        {
            Model.RapportStatutId = statutId;
            return this;
        }

        public RapportBuilder TypeRapport(int typeRapport)
        {
            Model.TypeRapport = typeRapport;
            return this;
        }

        public RapportBuilder ValidationSuperieur(bool validationSuperieur)
        {
            Model.ValidationSuperieur = validationSuperieur;
            return this;
        }

        public RapportBuilder DateChantier(DateTime dateChantier)
        {
            Model.DateChantier = dateChantier;
            return this;
        }

        public RapportBuilder HoraireDebutS(DateTime horaireDebutS)
        {
            Model.HoraireDebutS = horaireDebutS;
            return this;
        }

        public RapportBuilder HoraireFinS(DateTime horaireFinS)
        {
            Model.HoraireFinS = horaireFinS;
            return this;
        }

        public RapportBuilder InitRapportLignes()
        {
            if (Model.ListLignes == null)
            {
                Model.ListLignes = new List<RapportLigneEnt>();
            }

            return this;
        }

        public RapportBuilder AddLigne(RapportLigneEnt ligne)
        {
            if (Model.ListLignes == null)
            {
                Model.ListLignes = new List<RapportLigneEnt>();
            }

            Model.ListLignes.Add(ligne);
            return this;
        }

        public RapportBuilder AddLignes(ICollection<RapportLigneEnt> lignes)
        {
            if (Model.ListLignes == null)
            {
                Model.ListLignes = new List<RapportLigneEnt>();
            }

            foreach (var item in lignes)
            {
                Model.ListLignes.Add(item);
            }

            return this;
        }

        public RapportBuilder ListLignes(ICollection<RapportLigneEnt> rapportLigneList)
        {
            Model.ListLignes = rapportLigneList;
            return this;
        }

        public RapportBuilder ListErreurs(IEnumerable<string> errors)
        {
            Model.ListErreurs = errors?.ToList();
            return this;
        }

        public RapportBuilder NePasVerrouiller()
        {
            Model.IsStatutVerrouille = false;
            return this;
        }
        public RapportBuilder Verrouiller()
        {
            Model.IsStatutVerrouille = true;
            return this;
        }
    }
}
