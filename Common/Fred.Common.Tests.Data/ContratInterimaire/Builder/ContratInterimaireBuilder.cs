using System;
using Fred.Common.Tests.EntityFramework;
using Fred.Entities.Personnel;
using Fred.Entities.Personnel.Interimaire;
using Fred.Entities.Referential;
using Fred.Entities.ReferentielFixe;
using Fred.Entities.Societe;

namespace Fred.Common.Tests.Data.ContratInterimaire.Builder
{
    public class ContratInterimaireBuilder : ModelDataTestBuilder<ContratInterimaireEnt>
    {

        public ContratInterimaireBuilder ci(Entities.CI.CIEnt ci)
        {
            Model.Ci = ci;
            return this;
        }

        public ContratInterimaireBuilder CiId(int id)
        {
            Model.CiId = id;
            return this;
        }

        public ContratInterimaireBuilder ContratInterimaireId(int id)
        {
            Model.ContratInterimaireId = id;
            return this;
        }

        public ContratInterimaireBuilder InterimaireId(int interimaireId)
        {
            Model.InterimaireId = interimaireId;
            return this;
        }


        public ContratInterimaireBuilder Interimaire(PersonnelEnt personnel)
        {
            Model.Interimaire = personnel;
            return this;
        }

        public ContratInterimaireBuilder DateDebut(DateTime date)
        {
            Model.DateDebut = date;
            return this;
        }

        public ContratInterimaireBuilder DateFin(DateTime date)
        {
            Model.DateFin = date;
            return this;
        }

        public ContratInterimaireBuilder Energie(bool energie)
        {
            Model.Energie = energie;
            return this;
        }

        public ContratInterimaireBuilder DateChantier(UniteEnt unite)
        {
            Model.Unite = unite;
            return this;
        }

        public ContratInterimaireBuilder UniteId(int uniteid)
        {
            Model.UniteId = uniteid;
            return this;
        }

        public ContratInterimaireBuilder Unite(UniteEnt unite)
        {
            Model.Unite = unite;
            return this;
        }
        public ContratInterimaireBuilder Ressource(RessourceEnt ressource)
        {
            Model.Ressource = ressource;
            return this;
        }

        public ContratInterimaireBuilder RessourceId(int ressourceId)
        {
            Model.RessourceId = ressourceId;
            return this;
        }

        public ContratInterimaireBuilder Societe(SocieteEnt societe)
        {
            Model.Societe = societe;
            return this;
        }

        public ContratInterimaireBuilder SocieteId(int societeId)
        {
            Model.SocieteId = societeId;
            return this;
        }

        public ContratInterimaireBuilder NumContrat(string numContrat)
        {
            Model.NumContrat = numContrat;
            return this;
        }

        public ContratInterimaireBuilder Valorisation(decimal valorisation)
        {
            Model.Valorisation = valorisation;
            return this;
        }
    }

}
