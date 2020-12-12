using System;
using System.Linq;
using System.Linq.Expressions;
using Fred.Entities.Commande;
using Fred.Entities.Depense;

namespace Fred.Entities.Common.CommonSearchExpression
{
    public class SearchExpression
    {
        public Expression<Func<DepenseAchatEnt, bool>> GetIsReceptionNotDeletedExpression()
        {
            return r => !r.DateSuppression.HasValue && r.DepenseType.Code == Constantes.DepenseTypeCode.Reception;
        }

        public Expression<Func<CommandeLigneEnt, decimal>> GetTotalCommandeLigneExpression()
        {
            var isReceptionNotDeleted = this.GetIsReceptionNotDeletedExpression();

            return l => Math.Abs(l.AllDepenses.Any() ? l.AllDepenses.AsQueryable().Where(isReceptionNotDeleted).AsQueryable().Sum(r => r.Quantite * r.PUHT) : 0);
        }

        public Expression<Func<CommandeEnt, bool>> GetCommandeReceivableExpression()
        {
            var totalCommandeLigne = this.GetTotalCommandeLigneExpression();

            var isReceptionNotDeleted = this.GetIsReceptionNotDeletedExpression();

            return c =>   /* MontantHT */            c.Lignes.Sum(l => l.Quantite * l.PUHT)
                        /* MontantHTReceptionne */ - (c.Lignes.SelectMany(l => l.AllDepenses).Any() ? c.Lignes.AsQueryable().Select(totalCommandeLigne).AsQueryable().Sum(r => r) : 0)
                        > 0;
        }
    }
}
