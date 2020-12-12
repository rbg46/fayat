using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Fred.Entities.Achat.Calculation.Reception;
using Fred.Entities.Depense;

namespace Fred.DataAccess.Interfaces
{
    public interface IMontantFactureRepository
    {
        List<ReceptionMontantFactureModel> GetMontantFactureForReceptions(Expression<Func<DepenseAchatEnt, bool>> selectionReceptionsExpression, DateTime? dateDebut, DateTime? dateFin);
    }
}
