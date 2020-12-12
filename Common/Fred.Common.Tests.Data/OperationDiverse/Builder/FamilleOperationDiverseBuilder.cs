using Fred.Common.Tests.EntityFramework;
using Fred.Entities.OperationDiverse;
using Fred.Web.Shared.Models.OperationDiverse;

namespace Fred.Common.Tests.Data.OperationDiverse.Builder
{
    /// <summary>
    /// Builder de <see cref="FamilleOperationDiverseEnt"/>
    /// </summary>
    public class FamilleOperationDiverseBuilder : ModelDataTestBuilder<FamilleOperationDiverseEnt>
    {
        public override FamilleOperationDiverseEnt New()
        {
            base.New();
            Model.FamilleOperationDiverseId = 1;
            Model.Code = "MO";
            Model.Libelle = "MO POINTEE (Hors Interim)";
            Model.LibelleCourt = "Déboursé MO";
            Model.SocieteId = 1;
            return Model;
        }

        public FamilleOperationDiverseBuilder Code(string code)
        {
            Model.Code = code;
            return this;
        }

        public FamilleOperationDiverseBuilder Libelle(string libelle)
        {
            Model.Libelle = libelle;
            return this;
        }

        public FamilleOperationDiverseBuilder LibelleCourt(string libelleCourt)
        {
            Model.LibelleCourt = libelleCourt;
            return this;
        }

        public FamilleOperationDiverseBuilder SocieteId(int societeId)
        {
            Model.SocieteId = societeId;
            return this;
        }

        public FamilleOperationDiverseModel NewModel()
        {
            FamilleOperationDiverseModel model = new FamilleOperationDiverseModel();
            model.Code = "MO";
            model.FamilleOperationDiverseId = 1;
            model.Libelle = "MO POINTEE (Hors Interim)";
            model.LibelleCourt = "Déboursé MO";
            model.SocieteCode = "50338";
            return model;
        }

        public FamilleOperationDiverseEnt NewMoFamily()
        {
            return new FamilleOperationDiverseEnt
            {
                FamilleOperationDiverseId = 1,
                Code = "MO",
                SocieteId = 1,
                IsAccrued = true,
                MustHaveOrder = false,
                IsValued = true,
                TacheId = 3466,
                RessourceId = 972,
                CategoryValorisationId = 0
            };
        }

        public FamilleOperationDiverseEnt NewAchFamily()
        {
            return new FamilleOperationDiverseEnt
            {
                FamilleOperationDiverseId = 2,
                Code = "ACH",
                SocieteId = 1,
                IsAccrued = true,
                MustHaveOrder = true,
                IsValued = false,
                TacheId = 3469,
                RessourceId = 974,
                CategoryValorisationId = null
            };
        }

        public FamilleOperationDiverseEnt NewMitFamily()
        {
            return new FamilleOperationDiverseEnt
            {
                FamilleOperationDiverseId = 3,
                Code = "MIT",
                SocieteId = 1,
                IsAccrued = true,
                MustHaveOrder = false,
                IsValued = false,
                TacheId = 3468,
                RessourceId = 973,
                CategoryValorisationId = 1
            };
        }

        public FamilleOperationDiverseEnt NewMiFamily()
        {
            return new FamilleOperationDiverseEnt
            {
                FamilleOperationDiverseId = 4,
                Code = "MI",
                SocieteId = 1,
                IsAccrued = false,
                MustHaveOrder = false,
                IsValued = true,
                TacheId = 18790,
                RessourceId = 974,
                CategoryValorisationId = null
            };
        }

        public FamilleOperationDiverseEnt NewOthFamily()
        {
            return new FamilleOperationDiverseEnt
            {
                FamilleOperationDiverseId = 5,
                Code = "OTH",
                SocieteId = 1,
                IsAccrued = false,
                MustHaveOrder = false,
                IsValued = false,
                TacheId = 3470,
                RessourceId = 975,
                CategoryValorisationId = null
            };
        }
    }
}
