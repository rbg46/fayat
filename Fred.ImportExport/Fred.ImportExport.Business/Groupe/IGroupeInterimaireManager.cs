using System.Collections.Generic;
using Fred.ImportExport.Models.Groupe;

namespace Fred.ImportExport.Business.Groupe
{
    public interface IGroupeInterimaireManager
    {
        List<GroupeInterimaireModel> GetGroupeInterimaire();
    }
}