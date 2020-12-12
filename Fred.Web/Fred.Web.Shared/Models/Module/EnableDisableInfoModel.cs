using System.Collections.Generic;

namespace Fred.Web.Shared.Models.Module
{
  public class EnableDisableInfoModel
  {
    public List<int> OrganisationIdsOfSocietesToEnable { get; set; }

    public List<int> OrganisationIdsOfSocietesToDisable { get; set; }
  }
}
