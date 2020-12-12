using Fred.DataAccess.Common;
using Fred.DataAccess.Interfaces;
using Fred.Entities.ReferentielEtendu;
using Fred.EntityFramework;

namespace Fred.DataAccess
{
    /// <summary>
    ///    Représente le repo de l'entité ParametrageReferentielEtendu
    /// </summary>
    public class ParametrageReferentielEtenduRepository : FredRepository<ParametrageReferentielEtenduEnt>, IParametrageReferentielEtenduRepository
    {

        public ParametrageReferentielEtenduRepository(FredDbContext context)
          : base(context)
        {
        }

        /// <summary>
        /// Creation d'un parametrageReferentielEtenduEnt
        /// </summary>
        /// <param name="parametrageReferentielEtenduEnt">parametrageReferentielEtenduEnt</param>
        /// <returns>parametrageReferentielEtendu </returns>
        public ParametrageReferentielEtenduEnt InsertParametrageReferentielEtendu(ParametrageReferentielEtenduEnt parametrageReferentielEtenduEnt)
        {
            this.ClearDependencies(parametrageReferentielEtenduEnt);

            this.Insert(parametrageReferentielEtenduEnt);

            return parametrageReferentielEtenduEnt;
        }

        /// <summary>
        /// Mise a jour d'un parametrageReferentielEtenduEnt
        /// </summary>
        /// <param name="parametrageReferentielEtenduEnt">parametrageReferentielEtenduEnt</param>
        /// <returns>parametrageReferentielEtendu </returns>
        public ParametrageReferentielEtenduEnt UpdateParametrageReferentielEtendu(ParametrageReferentielEtenduEnt parametrageReferentielEtenduEnt)
        {

            ClearDependencies(parametrageReferentielEtenduEnt);

            this.Update(parametrageReferentielEtenduEnt);

            return parametrageReferentielEtenduEnt;
        }

        private void ClearDependencies(ParametrageReferentielEtenduEnt parametrageReferentielEtenduEnt)
        {
            parametrageReferentielEtenduEnt.Devise = null;
            parametrageReferentielEtenduEnt.Organisation = null;
            parametrageReferentielEtenduEnt.ReferentielEtendu = null;
            parametrageReferentielEtenduEnt.Unite = null;
        }
    }
}