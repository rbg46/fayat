using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Fred.Web.Models
{
  public class CoreModel
  {
    /// <summary>
    /// Nom de l'application
    /// </summary>
    public string AppName { get; set; }

    /// <summary>
    /// Version de l'application
    /// </summary>
    public string AppVersion { get; set; }

    /// <summary>
    /// Nom de l'utilisateur
    /// </summary>
    public string UserName { get; set; }

    /// <summary>
    /// Langage sélectionné par l'utilisateur
    /// </summary>
    public string LanguageSetting { get; set; }
  }
}