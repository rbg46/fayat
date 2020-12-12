using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Fred.Entities.Achat.Calculation.Commande;
using Fred.Entities.Achat.Calculation.Reception;
using Fred.Entities.Commande;
using Fred.Entities.Depense;

namespace Fred.DataAccess.Interfaces
{
    public interface IMontantHtRepository
    {
        List<CommandeMontantHtModel> GetMontantHT(Expression<Func<CommandeEnt, bool>> selectionCommandeFilter);
        List<DepenseAchatMontantHtModel> GetMontantHT(Expression<Func<DepenseAchatEnt, bool>> selectionReceptionsFilter);
    }
}
