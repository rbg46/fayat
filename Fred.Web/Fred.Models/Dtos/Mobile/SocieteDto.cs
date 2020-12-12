using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Fred.Web.Dtos.Mobile
{
  /// <summary>
  /// DTO Société
  /// </summary>
  public class SocieteDto
  {
    /// <summary>
    /// Obtient ou définit l'identifiant unique d'une société.
    /// </summary>
    public int SocieteId { get; set; }

    /// <summary>
    /// Obtient ou définit le code condensé de la société
    /// </summary>
    public string Code { get; set; }

    /// <summary>
    /// Obtient ou définit le code société paye
    /// </summary>
    public string CodeSocietePaye { get; set; }

    /// <summary>
    /// Obtient ou définit le code société comptable
    /// </summary>
    public string CodeSocieteComptable { get; set; }

    /// <summary>
    /// Obtient ou définit le libellé de la société
    /// </summary>
    public string Libelle { get; set; }

    /// <summary>
    /// Obtient ou définit l'Adresse d'une société
    /// </summary>
    public string Adresse { get; set; }

    /// <summary>
    /// Obtient ou définit la ville d'une société
    /// </summary>
    public string Ville { get; set; }

    /// <summary>
    /// Obtient ou définit le code postal d'une société
    /// </summary>
    public string CodePostal { get; set; }

    /// <summary>
    /// Obtient ou définit le numéro SIRET d'une société
    /// </summary>
    public string SIRET { get; set; }

    /// <summary>
    /// Obtient ou définit une valeur indiquant si une société est externe au groupe ou non.
    /// </summary>
    public bool Externe { get; set; }
  }
}