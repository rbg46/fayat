using Fred.Web.Models.Referential;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Fred.Web.Models.Rapport
{
  public class RapportLignePrimeModel
  {
    /// <summary>
    /// Obtient ou définit l'identifiant unique de la ligne prime du rapport
    /// </summary>
    public int RapportLignePrimeId { get; set; }

    /// <summary>
    /// Obtient ou définit l'id de la ligne de rapport de rattachement
    /// </summary>
    public int RapportLigneId { get; set; }

    /// <summary>
    /// Obtient ou définit l'entité Rapport
    /// </summary>
    public RapportLigneModel RapportLigne { get; set; }

    /// <summary>
    /// Obtient ou définit l'id de la prime
    /// </summary>
    public int PrimeId { get; set; }

    /// <summary>
    /// Obtient ou définit l'entité Prime
    /// </summary>
    public PrimeModel Prime { get; set; }

    /// <summary>
    /// Obtient ou définit le fait que la prime soit checkée
    /// </summary>
    public bool IsChecked { get; set; }

    /// <summary>
    /// Obtient ou définit l'heure de la prime uniquement si TypeHoraire est en heure
    /// </summary>
    public double? HeurePrime { get; set; }

    /// <summary>
    /// Obtient ou définit le fait que la ligne soit en création
    /// </summary>
    public bool IsCreated { get; set; } = false;

    /// <summary>
    /// Obtient ou définit le fait que la ligne soit en modification
    /// </summary>
    public bool IsUpdated { get; set; } = false;

    /// <summary>
    /// Obtient ou définit le fait que la ligne soit à supprimer
    /// </summary>
    public bool IsDeleted { get; set; } = false;
  }
}