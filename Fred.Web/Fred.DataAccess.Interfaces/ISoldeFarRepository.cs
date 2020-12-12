using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Fred.Entities.Achat.Calculation.Reception;
using Fred.Entities.Depense;

namespace Fred.DataAccess.Interfaces
{
    public interface ISoldeFarRepository
    {
        List<ReceptionSoldeFarModel> GetSoldeFar(Expression<Func<DepenseAchatEnt, bool>> selectionReceptionsFilter, DateTime? dateDebut, DateTime? dateFin);
    }
}
