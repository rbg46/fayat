﻿using Fred.Web.Models.Personnel;
using Fred.Web.Models.Referential;
using Fred.Web.Models.Societe;
using System;

namespace Fred.Web.Shared.Models
{
  /// <summary>
  ///   Model RemontéeVracErreur
  /// </summary>  
  public class RemonteeVracErreurModel
  {
    private DateTime dateDebut;
    private DateTime? dateFin;

    /// <summary>
    ///   Obtient ou définit l'identifiant unique de la remontéeVracErreur
    /// </summary>
    public int RemonteeVracErreurId { get; set; }

    /// <summary>
    ///   Obtient ou définit l'identifiant de la remontée vrac
    /// </summary>    
    public int RemonteeVracId { get; set; }

    /// <summary>
    ///   Obtient ou définti la remontée vrac
    /// </summary>
    public RemonteeVracModel RemonteeVrac { get; set; }

    /// <summary>
    ///   Obtient ou définit l'identifiant de la Société
    /// </summary>    
    public int SocieteId { get; set; }

    /// <summary>
    ///   Obtient ou définit la Société
    /// </summary>    
    public SocieteModel Societe { get; set; }

    /// <summary>
    ///   Obtient ou définit l'identifiant de l'établissement de paie
    /// </summary>    
    public int EtablissementPaieId { get; set; }

    /// <summary>
    ///   Obtient ou définit l'établissement de Paie
    /// </summary>    
    public EtablissementPaieModel EtablissementPaie { get; set; }

    /// <summary>
    ///   Obtient ou définit l'identifiant du personnel
    /// </summary>
    public int PersonnelId { get; set; }

    /// <summary>
    ///   Obtient ou définit le Personnel
    /// </summary>
    public PersonnelModel Personnel { get; set; }

    /// <summary>
    ///   Obtient ou définit la date de début
    /// </summary>
    public DateTime DateDebut
    {
      get { return DateTime.SpecifyKind(dateDebut, DateTimeKind.Utc); }
      set { dateDebut = DateTime.SpecifyKind(value, DateTimeKind.Utc); }
    }

    /// <summary>
    ///   Obtient ou définit la date de fin
    /// </summary>
    public DateTime? DateFin
    {
      get { return (dateFin.HasValue) ? DateTime.SpecifyKind(dateFin.Value, DateTimeKind.Utc) : default(DateTime?); }
      set { dateFin = (value.HasValue) ? DateTime.SpecifyKind(value.Value, DateTimeKind.Utc) : default(DateTime?); }
    }

    /// <summary>
    ///   Obtient ou définit le code absence (Fred)
    /// </summary>    
    public string CodeAbsenceFred { get; set; }

    /// <summary>
    ///   Obtient ou définit l'identifiant du code absence (Anael)
    /// </summary>    
    public string CodeAbsenceAnael { get; set; }
  }
}