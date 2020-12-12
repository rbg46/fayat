using Fred.Entities.Rapport;
using Fred.Entities.Referential;
using Fred.Web.Models.CI;
using Fred.Web.Models.CodeAbsence;
using Fred.Web.Models.Personnel;
using Fred.Web.Models.Referential;
using Fred.Web.Models.Utilisateur;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Fred.Web.Models.Rapport
{
  public class EtatPaieListeIGDModel
  {
    /// <summary>
    /// Obtient ou définit le Etablissement
    /// </summary>
    public string Etablissement { get; set; }

    /// <summary>
    /// Obtient ou définit le Personnel
    /// </summary>
    public string Personnel { get; set; }

    /// <summary>
    /// Obtient ou définit le Affaire
    /// </summary>
    public string Affaire { get; set; }

    /// <summary>
    /// Obtient ou définit le Code
    /// </summary>
    public string Code { get; set; }

    /// <summary>
    /// Obtient ou définit le Quantite
    /// </summary>
    public double Quantite { get; set; }

    /// <summary>
    /// Obtient ou définit le type
    /// </summary>
    public string Type { get; set; }

    /// <summary>
    /// Obtient ou définit le Nom
    /// </summary>
    public string Nom { get; set; }

    /// <summary>
    /// Obtient ou définit le Matricule
    /// </summary>
    public string Matricule { get; set; }

  }
}