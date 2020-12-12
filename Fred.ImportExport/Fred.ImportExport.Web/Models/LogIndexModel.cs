using Fred.ImportExport.Entities;
using System.Collections.Generic;

namespace Fred.ImportExport.Web.Models
{
  public class LogIndexModel
  {
    public string SearchText { get; set; }
    public string Level { get; set; }
    public List<string> Levels { get; set; }
    public List<NLogFredIeEnt> Logs { get; set; }
  }
}