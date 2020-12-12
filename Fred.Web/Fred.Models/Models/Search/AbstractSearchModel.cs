using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Fred.Web.Models.Search
{
  public abstract class AbstractSearchModel
  {
    /// <summary>
    /// Valeur recherchée
    /// </summary>
    public abstract string ValueText { get; set; }

  }
}