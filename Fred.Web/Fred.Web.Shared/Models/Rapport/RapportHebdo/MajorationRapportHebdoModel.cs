using System;
using System.Collections.Generic;

namespace Fred.Web.Shared.Models.Rapport.RapportHebdo
{
  public class MajorationRapportHebdoModel
  {
    public DateTime DateChantier { get; set; }

    public IList<MajorationPersonnelCiModel> MajorationPersonnelCiModelList { get; set; }
  }
}
