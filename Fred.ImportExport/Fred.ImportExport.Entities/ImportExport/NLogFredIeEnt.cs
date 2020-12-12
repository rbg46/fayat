using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fred.ImportExport.Entities
{
  /// <summary>
  /// Représente un log pour gestionnaire de log 'NLog'
  /// </summary>
  [Table("NLogs", Schema = "nlog")]
  public class NLogFredIeEnt
  {
    private DateTime logged;

    /// <summary>
    /// Obtient ou définit l'identifiant unique
    /// </summary>
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    /// <summary>
    /// Obtient ou définit le nom de l'application
    /// </summary>
    [MaxLength(50)]
    public string Application { get; set; }

    /// <summary>
    /// Obtient ou définit la date 
    /// </summary>
    public DateTime Logged
    {
      get
      {
        return DateTime.SpecifyKind(logged, DateTimeKind.Utc);
      }
      set
      {
        logged = DateTime.SpecifyKind(value, DateTimeKind.Utc);
      }
    }

    /// <summary>
    /// Obtient ou définit le niveau 
    /// </summary>
    [MaxLength(50)]
    public string Level { get; set; }

    /// <summary>
    /// Obtient ou définit le message 
    /// </summary>
    public string Message { get; set; }

    /// <summary>
    /// Obtient ou définit le nom de l'utilisateur
    /// </summary>
    [MaxLength(250)]
    public string UserName { get; set; }

    /// <summary>
    /// Obtient ou définit le nom du serveur
    /// </summary>
    public string ServerName { get; set; }

    /// <summary>
    /// Obtient ou définit le numero de port
    /// </summary>
    public string Port { get; set; }

    /// <summary>
    /// Obtient ou définit l'url
    /// </summary>
    public string Url { get; set; }

    /// <summary>
    /// Obtient ou définit une valeur indiquant si la commande contient si l'url est en HTTPS
    /// </summary>
    public bool Https { get; set; }

    /// <summary>
    /// Obtient ou définit l'adresse du serveur
    /// </summary>
    [MaxLength(100)]
    public string ServerAddress { get; set; }

    /// <summary>
    /// Obtient ou définit l'adresse du serveur distant
    /// </summary>
    [MaxLength(100)]
    public string RemoteAddress { get; set; }

    /// <summary>
    /// Obtient ou définit le nom du logger
    /// </summary>
    [MaxLength(250)]
    public string Logger { get; set; }

    /// <summary>
    /// Obtient ou définit le Callsite
    /// </summary>
    public string Callsite { get; set; }

    /// <summary>
    /// Obtient ou définit l'exception
    /// </summary>
    public string Exception { get; set; }
  }
}
