using System.Collections.Generic;
using Fred.Common.Tests.EntityFramework;
using Fred.Entities.Societe;
using Fred.Entities.Societe.Classification;

namespace Fred.Common.Tests.Data.Societe.Classification.Builder
{
    public class SocieteClassificationBuilder : ModelDataTestBuilder<SocieteClassificationEnt>
    {

        public SocieteClassificationEnt Prototype()
        {
            Model.SocieteClassificationId = 0;
            Model.Code = "03";
            Model.Libelle = "Eiffage Ajout";
            Model.Statut = true;

            return Model;
        }

        public SocieteClassificationBuilder SocieteClassificationId(int id)
        {
            Model.SocieteClassificationId = id;
            return this;
        }

        public SocieteClassificationBuilder Code(string code)
        {
            Model.Code = code;
            return this;
        }

        public SocieteClassificationBuilder Libelle(string libelle)
        {
            Model.Libelle = libelle;
            return this;
        }

        public SocieteClassificationBuilder GroupeId(int id)
        {
            Model.GroupeId = id;
            return this;
        }

        public SocieteClassificationBuilder Statut(bool active)
        {
            Model.Statut = active;
            return this;
        }

        public SocieteClassificationBuilder Societes(List<SocieteEnt> societes)
        {
            Model.Societes = societes;
            return this;
        }
    }
}
