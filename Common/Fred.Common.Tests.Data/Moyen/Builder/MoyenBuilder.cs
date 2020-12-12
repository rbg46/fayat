using System;
using System.Collections.Generic;
using Fred.Common.Tests.EntityFramework;
using Fred.Entities.Moyen;
using Fred.Entities.Referential;
using Fred.Entities.ReferentielFixe;
using Fred.Entities.Societe;

namespace Fred.Common.Tests.Data.Moyen.Builder
{
    public class MoyenBuilder : ModelDataTestBuilder<MaterielEnt>
    {
        public override MaterielEnt New()
        {
            Model = new MaterielEnt
            {
                Actif = true,
                Code = "FAM4682",
                Libelle = "Moyen test3",
                DateCreation = new DateTime(2019, 1, 28),
                AuteurCreationId = 1
            };
            return Model;
        }

        public SearchEtablissementMoyenEnt BuildSearchEtablissementMoyenFilter()
        {
            return new SearchEtablissementMoyenEnt();
        }


        public MoyenBuilder Libelle(string libelle)
        {
            Model.Libelle = libelle;
            return this;
        }

        public MoyenBuilder MaterielLocations(List<MaterielLocationEnt> materielLocations)
        {
            Model.MaterielLocations = materielLocations;
            return this;
        }

        public MoyenBuilder Ressource(RessourceEnt ressource)
        {
            Model.Ressource = ressource;
            return this;
        }

        public MoyenBuilder Societe(SocieteEnt societe)
        {
            Model.Societe = societe;
            return this;
        }

        public MoyenBuilder MaterielId(int materielId)
        {
            Model.MaterielId = materielId;
            return this;
        }
    }
}
